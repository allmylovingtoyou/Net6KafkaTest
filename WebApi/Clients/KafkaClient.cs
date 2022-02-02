using System.Net;
using Confluent.Kafka;
using KafkaServiceApi;
using KafkaServiceApi.Serializers;

namespace WebApi.Clients;

public class KafkaClient : IKafkaClient
{
    private readonly ProducerConfig _producerConfig;

    public KafkaClient(ILogger<KafkaClient> logger)
    {
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = "kafka:9092",
            ClientId = Dns.GetHostName()
        };
    }

    public async Task SendDepartureTimeChangeAsync(DepartureTimeChangeCommand command)
    {
        using var producer = new ProducerBuilder<Null, DepartureTimeChangeCommand>(_producerConfig)
            .SetValueSerializer(new SimpleKafKaSerializer<DepartureTimeChangeCommand>())
            .Build();
        await producer.ProduceAsync(KafkaTopics.DepartureTimeChange, new Message<Null, DepartureTimeChangeCommand> { Value = command });
    }
}