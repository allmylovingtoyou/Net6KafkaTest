// See https://aka.ms/new-console-template for more information
using EFCore.DbContextFactory.Extensions;
using KafkaService.BackgroundServices;
using KafkaService.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");
var configuration = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServicesInternal)
    .Build();

var dbContext = host.Services.GetRequiredService<KafkaServiceDbContext>();
await dbContext.Database.MigrateAsync();

await host.RunAsync();

void ConfigureServicesInternal(IServiceCollection serviceCollection)
{
    serviceCollection.AddDbContext<KafkaServiceDbContext>(options => AddDbOptions(options, configuration));
    serviceCollection.AddDbContextFactory<KafkaServiceDbContext>(options => AddDbOptions(options, configuration));
    serviceCollection.AddHostedService<DepartureTimeChangeConsumer>();
}

void AddDbOptions(DbContextOptionsBuilder dbContextOptionsBuilder, IConfigurationRoot? configurationRoot)
{
    dbContextOptionsBuilder.UseNpgsql(configurationRoot.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention();
}