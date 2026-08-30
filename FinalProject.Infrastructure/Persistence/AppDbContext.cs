using FinalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Accountant> Accountants => Set<Accountant>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.Property(u => u.Salary).HasPrecision(18, 2);
            builder.Property(u => u.IsBlocked).HasDefaultValue(false);
        });

        modelBuilder.Entity<Accountant>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.UserName).IsRequired().HasMaxLength(50);
            builder.HasIndex(a => a.UserName).IsUnique();
        });

        modelBuilder.Entity<Loan>(builder =>
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Amount).HasPrecision(18, 2);
            
            builder.HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}