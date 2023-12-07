using Microsoft.EntityFrameworkCore;
using TubeGram.API.Models;

namespace TubeGram.API.Helpers;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<PictureProfile> PictureProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username, u.Email })
            .IsUnique();
        
        modelBuilder.Entity<Image>()
            .HasOne(i => i.User)
            .WithMany(u => u.Images)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        modelBuilder.Entity<Video>()
            .HasOne(v => v.User)
            .WithMany(u => u.Videos)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        modelBuilder.Entity<PictureProfile>()
            .HasOne(p => p.User)
            .WithOne(u => u.PictureProfile)
            .HasForeignKey<PictureProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}