using luxe.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace luxe.Server.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //List<AppRole> appRoles = new List<AppRole>
            //{
                
            //};
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Name = "Super Admin", Description = "", NormalizedName = "SUPER ADMIN", IsActive = true },
                new AppRole { Name = "Admin", Description = "", NormalizedName = "ADMIN", IsActive = true },
                new AppRole { Name = "Manager", Description = "", NormalizedName = "MANAGER", IsActive = true },
                new AppRole { Name = "Customer Service", Description = "", NormalizedName = "CUSTOMER SERVICE", IsActive = true },
                new AppRole { Name = "Customer", Description = "", NormalizedName = "CUSTOMER", IsActive = true }
                );
        }
    }
        
}
