using Microsoft.EntityFrameworkCore;
using TubeGram.API.Models;

namespace TubeGram.API.Helpers;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username, u.Email })
            .IsUnique();
    }
}