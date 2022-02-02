using KafkaService.Domain;
using Microsoft.EntityFrameworkCore;

namespace KafkaService.Db;

public class KafkaServiceDbContext : DbContext
{
    public KafkaServiceDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DepartureChangeTime> DepartureChangeTimes { get; set; } = null!;
}