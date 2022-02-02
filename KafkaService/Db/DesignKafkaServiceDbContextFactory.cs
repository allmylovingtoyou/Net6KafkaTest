using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KafkaService.Db
{
    public class DesignKafkaServiceDbContextFactory : IDesignTimeDbContextFactory<KafkaServiceDbContext>
    {
        public KafkaServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KafkaServiceDbContext>();
            optionsBuilder
                .UseNpgsql("Server=127.0.0.1;Port=5455;Database=test_kafka_service;User Id=postgres;Password=admin;")
                .UseSnakeCaseNamingConvention();

            return new KafkaServiceDbContext(optionsBuilder.Options);
        }
    }
}