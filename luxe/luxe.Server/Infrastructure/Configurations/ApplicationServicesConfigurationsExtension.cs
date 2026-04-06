using luxe.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace luxe.Server.Infrastructure.Configurations
{
    public static class ApplicationServicesConfigurationsExtension
    {
        public static IServiceCollection AddApplicationServicesConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            // Add application services configurations here
            // Configure services, options, etc. based on the application's needs
            //Congigure EF Core with SQL Server
            var connectionString = configuration.GetConnectionString("luxeDB") ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            
                
            return services;
        }
    }
}
