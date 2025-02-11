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
                IsServicePrincipal = false,
                CreatedById = null,
                UpdatedById = null
            };

            var NotificationTemplate = new NotificationTemplate
            {
                Id = 1,
                Name = "Invite User",
                Subject = "SSO App invited you to access {{AppName}} applications",
                Body = "<style> .rps_d108 .x_auto-style40 { border-color: #000000; border-width: 0px; } </style>  <p>Hello {{DisplayName}},</p>  <p>You have been invited to join our {{AppName}}!</p>  <table class=\"x_auto-style40\" bgcolor=\"#F4F4F4\" width=\"700\" align=\"center\" role=\"presentation\" >   <tbody>    <tr aria-hidden=\"true\">     <td class=\"x_auto-style22\" colspan=\"6\"></td>    </tr>    <tr aria-hidden=\"true\">     <td class=\"x_auto-style22\" colspan=\"6\"></td>    </tr>    <tr></tr>    <tr>     <td align=\"center\" class=\"x_auto-style12\" colspan=\"6\" style=\"height: 10px\" > If you visit this invitation, you’ll be redirected to       <a href=\"{{InviteLink}}\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\" data-linkindex=\"0\" >{{InviteLink}}</a >.      </td>    </tr>    <tr aria-hidden=\"true\">     <td class=\"x_auto-style22\" colspan=\"6\" style=\"height: 10px\"></td>    </tr>    <tr>     <td colspan=\"6\" align=\"center\" style=\"padding-top: 5px; padding-bottom: 5px; text-align: center\" >      <a href=\"{{InviteLink}}\" target=\"_blank\" rel=\"noopener noreferrer\" data-auth=\"NotApplicable\"  data-linkindex=\"1\"       style=\"display: inline-block; background-color: #4C66AF; color: white; padding: 10px 20px; text-align: center; text-decoration: none; border-radius: 5px; border: none; cursor: pointer;\" >Visit now </a>      <p>We're excited to have you on board. If you have any questions, feel free to reach out <a href=\"mailto:support@outamatemods.com\">SSO Administrator</a></p>     </td>    </tr>    <tr aria-hidden=\"true\">     <td colspan=\"6\" class=\"x_auto-style22\"></td>    </tr>    <tr aria-hidden=\"true\">     <td colspan=\"6\" class=\"x_auto-style22\"></td>    </tr>   </tbody>  </table>  <p>Best regards,   <br>SSO  Team.   </p>",
                IsBodyHtml = true,
            };

            modelBuilder.Entity<Models.User>().HasData(adminUser);
            modelBuilder.Entity<NotificationTemplate>().HasData(NotificationTemplate);
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
