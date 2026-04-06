using luxe.Server.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace luxe.Server.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Configuring model properties and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<AppUser>(e =>
            {
                // Each User can have many UserClaims
                e.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                e.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                e.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                e.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                // 
                e.ToTable(name: "Users");
            });

            modelBuilder.Entity<AppRole>(e =>
            {
                // Each Role can have many entries in the UserRole join table
                e.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                e.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

                e.ToTable(name: "Roles");
            });

            modelBuilder.Entity<AppUserClaim>(e =>
            {
                e.ToTable(name: "UserClaims");
            });

            modelBuilder.Entity<AppUserLogin>(e =>
            {
                e.ToTable(name: "UserLogins");
            });

            modelBuilder.Entity<AppRoleClaim>(e =>
            {
                e.ToTable(name: "RoleClaims");
            });

            modelBuilder.Entity<AppUserToken>(e =>
            {
                e.ToTable(name: "UserTokens");
            });

            modelBuilder.Entity<AppUserRole>(e =>
            {
                e.ToTable(name: "UserRoles");
            });
        }
    }


}
