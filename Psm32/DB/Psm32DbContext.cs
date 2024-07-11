using System;
using Microsoft.EntityFrameworkCore;
using Psm32.Models;

namespace Psm32.DB;
public class Psm32DbContext : DbContext
{
    public Psm32DbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .Property(u => u.UserRole)
            .HasConversion(
                v => v.ToString(),
                v => (UserRole)Enum.Parse(typeof(UserRole), v));
    }

    public override void Dispose() => base.Dispose();

    public DbSet<MotorTaskDTO> MotorTasks { get; set; }
    public DbSet<ConfigurationItemDTO> AppConfig { get; set; }
    public DbSet<User> Users { get; set; }
}
