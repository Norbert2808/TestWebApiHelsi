using Microsoft.EntityFrameworkCore;
using TestWebApp.Data.Entities;

namespace TestWebApp.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }
    public DbSet<UserTaskList> UserTaskLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.TaskLists)
            .WithMany(e => e.SharedUsers)
            .UsingEntity<UserTaskList>();

        modelBuilder.Entity<TaskList>()
            .HasOne(tl => tl.Owner)
            .WithMany(u => u.OwnerTaskLists)
            .HasForeignKey(tl => tl.OwnerId);
        
        modelBuilder.Entity<UserTaskList>()
            .HasIndex(utl => new { utl.UserId, utl.TaskListId })
            .IsUnique();
    }
}