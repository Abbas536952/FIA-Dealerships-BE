using DealershipManagement.Entities.Account;
using DealershipManagement.Entities.Address;
using DealershipManagement.Entities.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Template.Repository.Interfaces;

namespace DealershipManagement.DataAccess
{
    public class DealershipManagementDbContext : IdentityDbContext<UserAccount, SystemRole, string, IdentityUserClaim<string>,
        UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DealershipManagementDbContext(DbContextOptions<DealershipManagementDbContext> options)
        : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<SystemRole> SystemRoles { get; set; }

        public override DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

        public DbSet<Entities.Customer.Customer> Customers { get; set; }

        public DbSet<Entities.Customer.Transaction> Transactions { get; set; }

        public DbSet<Entities.Vehicle.Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<SystemRole>().ToTable("SystemRoles");
            modelBuilder.Entity<UserAccount>().ToTable("UserAccounts");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var parameter = Expression.Parameter(entityType.ClrType);

                var prop = entityType.ClrType.GetProperty(nameof(ISoftDeletableEntity.IsActive));

                if (prop != null)
                {
                    var compareExpression = Expression.MakeMemberAccess(parameter, prop);

                    var lambda = Expression.Lambda(compareExpression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}
