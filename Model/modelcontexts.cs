// Data/WebinarDbContext.cs
using Microsoft.EntityFrameworkCore;

public class WebinarDbContext : DbContext
{
    public WebinarDbContext(DbContextOptions<WebinarDbContext> options) : base(options)
    {
    }

    public DbSet<Webinar> Webinars { get; set; }
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed an admin user (for demo purposes only)
        modelBuilder.Entity<AdminUser>().HasData(
            new AdminUser {Id=1, Username = "admin", Password = "admin123" }
        );
    }
}