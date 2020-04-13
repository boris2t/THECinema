namespace THECinema.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using THECinema.Common;
    using THECinema.Data.Models;

    public class AdminsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var user = dbContext.Users.Where(u => u.UserName == "admin1").FirstOrDefault();
            var adminRoleId = dbContext.Roles.Select(x => x.Id).FirstOrDefault();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (user == null)
            {
                return;
            }
            else if (user.Roles.Select(x => x.RoleId).FirstOrDefault() == adminRoleId)
            {
                return;
            }

            await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }
    }
}
