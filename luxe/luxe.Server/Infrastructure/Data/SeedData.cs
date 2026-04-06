using luxe.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace luxe.Server.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            List<AppRole> appRoles = new List<AppRole> 
            {
                new AppRole{Id = Guid.NewGuid(), Name = "Super Admin", Description = "", NormalizedName = "SUPER ADMIN", DateCreated = DateTime.UtcNow, IsActive = true},
                new AppRole{Id = Guid.NewGuid(), Name = "Admin", Description = "", NormalizedName = "ADMIN", DateCreated = DateTime.UtcNow, IsActive = true},
                new AppRole{Id = Guid.NewGuid(), Name = "Manager", Description = "", NormalizedName = "MANAGER", DateCreated = DateTime.UtcNow, IsActive = true},
                new AppRole{Id = Guid.NewGuid(), Name = "Customer Service", Description = "", NormalizedName = "CUSTOMER SERVICE", DateCreated = DateTime.UtcNow, IsActive = true},
                new AppRole{Id = Guid.NewGuid(), Name = "User", Description = "", NormalizedName = "USER", DateCreated = DateTime.UtcNow, IsActive = true},
            };
            modelBuilder.Entity<AppRole>().HasData(appRoles);
        }
        
    }
}
