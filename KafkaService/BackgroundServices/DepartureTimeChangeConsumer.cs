using Confluent.Kafka;
using KafkaService.Db;
using KafkaService.Domain;
using KafkaServiceApi;
using KafkaServiceApi.Serializers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaService.BackgroundServices;

public class DepartureTimeChangeConsumer : IHostedService
{
    private readonly ILogger<DepartureTimeChangeConsumer> _logger;
    private readonly Func<KafkaServiceDbContext> _dbFactory;

    public DepartureTimeChangeConsumer(ILogger<DepartureTimeChangeConsumer> logger, Func<KafkaServiceDbContext> dbFactory)
    {
        _logger = logger;
        _dbFactory = dbFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartAsync() - start");
        var config = new ConsumerConfig
        {
            BootstrapServers = "kafka:9092",
            GroupId = "foo",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeAsync(config, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "StartAsync()");
                await Task.Delay(500, cancellationToken);
            }
        }

        _logger.LogInformation("StartAsync() - end");
    }

    private async Task ConsumeAsync(ConsumerConfig config, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, DepartureTimeChangeCommand>(config)
            .SetValueDeserializer(new SimpleKafKaDeserializer<DepartureTimeChangeCommand>())
            .Build();
        config.AllowAutoCreateTopics = true;
        consumer.Subscribe(KafkaTopics.DepartureTimeChange);
        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(cancellationToken);
            await using var dbContext = _dbFactory();
            var entity = new DepartureChangeTime
            {
                DepartureDate = consumeResult.Message.Value.DepartureDate,
                FlightNumber = consumeResult.Message.Value.FlightNumber
            };
            dbContext.DepartureChangeTimes.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("ConsumeAsync() - new record added with id = {Id}", entity.Id);
        }

        consumer.Close();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}