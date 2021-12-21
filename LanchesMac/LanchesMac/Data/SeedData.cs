using Microsoft.AspNetCore.Identity;

namespace LanchesMac.Data
{
    public static class SeedData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[]  roleNames = { "Admin", "Member"};
            IdentityResult roleResult;

            foreach(var roleName in roleNames)
            {
                var RoleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!RoleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var poweruser = new IdentityUser()
            {
                UserName = configuration.GetSection("UserSettings")["UserName"],
                Email = configuration.GetSection("UserSettings")["UserEmail"],
            };

            string userPassword = configuration.GetSection("UserSettings")["UserPassword"];
            var user = await UserManager.FindByEmailAsync(configuration.GetSection("UserSettings")["UserEmail"]);

            if(user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if(createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}
