using Microsoft.EntityFrameworkCore;
using CarparkAssignment.Models;

namespace CarparkAssignment.Data;

public class CarparkDbContext : DbContext {
    
    public CarparkDbContext(DbContextOptions<CarparkDbContext> options): base(options) {
    }

    public DbSet<Carpark> Carparks { get; set; }
    // TODO: add and link users table
    /* public DbSet<User> Users { get; set; } */
    public DbSet<UserFavourites> UserFavourites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite("Data Source=carpark.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<Carpark>()
                .HasKey(c => c.car_park_no);

        modelBuilder.Entity<UserFavourites>()
                .HasKey(uf => uf.id);
        
        modelBuilder.Entity<UserFavourites>()
                .HasOne(uf => uf.carpark)
                .WithMany()
                .HasForeignKey(uf => uf.car_park_no);

        // TODO: link user favourites table to users table
        /*
        modelBuilder.Entity<UserFavourites>()
                .HasOne(uf => uf.user)
                .WithMany()
                .HasForeignKey(uf => uf.user_id);
        */
    }
}
