using Microsoft.EntityFrameworkCore;
using Tutorial11.Models;

namespace Tutorial11.DAL;

public class TutElDbContext : DbContext
{
    public TutElDbContext()
    {
    }

    public TutElDbContext(DbContextOptions<TutElDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
    }
}