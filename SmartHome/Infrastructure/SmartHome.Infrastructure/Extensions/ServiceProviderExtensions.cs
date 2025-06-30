using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Domain.Entities.Identity;
using SmartHome.Infrastructure.Data;

namespace SmartHome.Infrastructure.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static async Task SeedRolesAndAdminAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            await DbInitializer.SeedRolesAndAdminAsync(roleManager, userManager);
        }
    }
}
