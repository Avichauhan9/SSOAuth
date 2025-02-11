using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using System.Data;
using SSO_Backend.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SSO_Backend.Context
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Models.Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Models.RolePermission> RolePermissions { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseModel).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType, entity =>
                {
                    entity.Property<DateTimeOffset>(nameof(BaseModel.CreatedAt))
                        .HasDefaultValueSql("SYSUTCDATETIME()")
                        .ValueGeneratedOnAdd();

                    entity.Property<DateTimeOffset>(nameof(BaseModel.LastUpdatedAt))
                        .HasDefaultValueSql("SYSUTCDATETIME()")
                        .ValueGeneratedOnAddOrUpdate();
                });
            }

            modelBuilder.Entity<UserRole>()
              .HasIndex(ur => new { ur.UserId, ur.RoleId })
              .IsUnique();

            modelBuilder.Entity<Models.RolePermission>()
              .HasIndex(ur => new { ur.RoleId, ur.PermissionId })
              .IsUnique();

            ConfigureRelationships(modelBuilder);
            SeedInitialData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>(entity =>
            {
                entity.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(r => r.RolePermissions)
                    .WithOne(rp => rp.Role)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            var adminUser = new Models.User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@example.com",
                IsActive = true,
                CreatedById = null,
                UpdatedById = null
            };

            
            modelBuilder.Entity<Models.User>().HasData(adminUser);
        }

        public override int SaveChanges()
        {
            SetAuditTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseModel>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.LastUpdatedAt = DateTimeOffset.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                }
            }
        }
    }
}
