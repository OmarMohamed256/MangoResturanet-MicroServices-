using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (_roleManager.FindByNameAsync(Config.Admin).Result == null)
                {
                    _roleManager.CreateAsync(new IdentityRole(Config.Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Config.Customer)).GetAwaiter().GetResult();
                }
                else
                {
                    return;
                }

                ApplicationUser adminUser = new ApplicationUser()
                {
                    UserName = "admin1@gmail.com",
                    Email = "admin1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "111111111111",
                    FirstName = "Ben",
                    LastName = "Admin"
                };
                _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(adminUser, Config.Admin).GetAwaiter().GetResult();

                var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[]
                {
                new Claim(JwtClaimTypes.Name,adminUser.FirstName+" "+adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
                new Claim(JwtClaimTypes.Role,Config.Admin),
                }).Result;

                ApplicationUser customerUser = new ApplicationUser()
                {
                    UserName = "customer1@gmail.com",
                    Email = "customer1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "111111111111",
                    FirstName = "Ben",
                    LastName = "Cust"
                };
                _userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(customerUser, Config.Customer).GetAwaiter().GetResult();

                var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
                {
                new Claim(JwtClaimTypes.Name,customerUser.FirstName+" "+customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                new Claim(JwtClaimTypes.Role,Config.Customer),
                }).Result;
            }
        }
    }
}