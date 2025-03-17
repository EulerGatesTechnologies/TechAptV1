
using Microsoft.EntityFrameworkCore;
using TechAptV1.Client.Models;

namespace TechAptV1.Client.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Number> Numbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Number>().ToTable("Number");

        modelBuilder.Entity<Number>().Property(n => n.IsPrime).HasDefaultValue(0);
    }
}
