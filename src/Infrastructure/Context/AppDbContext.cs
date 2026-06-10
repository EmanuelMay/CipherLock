using CipherLock.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CipherLock.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; private set; }
    public DbSet<Vault> Vaults { get; private set; }
    public DbSet<Credential> Credentials { get; private set; }
    public DbSet<UserResetPassword> UserResetPasswords { get; private set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>()
            .ToTable("users");

        mb.Entity<User>()
            .HasQueryFilter(x => !x.IsDeleted);

        mb.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();
    
        mb.Entity<User>()
            .Property(x => x.Name)
            .HasColumnType("varchar(150)");
        
        mb.Entity<User>()   
            .Property(x => x.Email)
            .HasColumnType("varchar(255)");
        
        mb.Entity<User>()
            .Property(x => x.PasswordHash)
            .HasColumnType("varchar(255)");
        
        mb.Entity<Vault>()
            .ToTable("vaults");

        mb.Entity<Vault>()
            .HasQueryFilter(x => !x.User.IsDeleted);
        
        mb.Entity<Vault>()
            .Property(x => x.Name)
            .HasColumnType("varchar(150)");
        
        mb.Entity<Credential>()
            .ToTable("credentials");
        
        mb.Entity<Credential>()
            .HasQueryFilter(x => !x.Vault.User.IsDeleted);
        
        mb.Entity<Credential>()
            .Property(x => x.Title)
            .HasColumnType("varchar(150)");
        
        mb.Entity<Credential>()
            .Property(x => x.EncryptedPassword)
            .HasColumnType("varchar(150)");
        
        mb.Entity<Credential>()
            .Property(x => x.IV)
            .HasColumnType("varchar(150)");
        
        mb.Entity<UserResetPassword>()
            .ToTable("user_reset_passwords");
        
        mb.Entity<UserResetPassword>()
            .HasQueryFilter(x => !x.User.IsDeleted);
    }
}
