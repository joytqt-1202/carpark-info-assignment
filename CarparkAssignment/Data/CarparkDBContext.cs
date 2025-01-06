using Microsoft.EntityFrameworkCore;
using CarparkAssignment.Models;

namespace CarparkAssignment.Data;

public class CarparkDbContext : DbContext {
    
    public CarparkDbContext(DbContextOptions<CarparkDbContext> options): base(options) {
    }

    public DbSet<Carpark> Carparks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite("Data Source=carpark.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Carpark>().HasKey(c => c.car_park_no);
    }
}
