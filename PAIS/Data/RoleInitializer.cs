using PAIS.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAIS.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string UserAdminEmail = "markiyanse@gmail.com";
            string UserAdminPassword = "_Aa123456UserAdmin";

            string ShopAdminEmail = "ShopAdmin@gmail.com";
            string ShopAdminPassword = "_Aa123456ShopAdmin";

            string SuperAdminEmail = "SuperAdmin@gmail.com";
            string SuperAdminPassword = "SuperAdmin123.";


            string UserAdminRole = "user admin";
            string ShopAdminRole = "shop admin";
            string UserRole = "user";
            if (await roleManager.FindByNameAsync(UserAdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserAdminRole));
            }
            if (await roleManager.FindByNameAsync(ShopAdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(ShopAdminRole));
            }
            if (await roleManager.FindByNameAsync(UserRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole));
            }
            if (await userManager.FindByNameAsync(UserAdminEmail) == null)
            {
                IdentityUser UserAdmin = new IdentityUser { UserName = "MarkUserAdmin", Email = UserAdminEmail };
                IdentityResult result = await userManager.CreateAsync(UserAdmin, UserAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(UserAdmin, UserAdminRole);
                }
            }
            if (await userManager.FindByNameAsync(ShopAdminEmail) == null)
            {
                IdentityUser ShopAdmin = new IdentityUser { UserName = "JackShopAdmin", Email = ShopAdminEmail };
                IdentityResult result = await userManager.CreateAsync(ShopAdmin, ShopAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(ShopAdmin, ShopAdminRole);
                }
            }
            if (await userManager.FindByNameAsync(SuperAdminEmail) == null)
            {
                IdentityUser SuperAdmin = new IdentityUser { UserName = "SuperAdmin", Email = SuperAdminEmail };
                IdentityResult result = await userManager.CreateAsync(SuperAdmin, SuperAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(SuperAdmin, ShopAdminRole);
                    await userManager.AddToRoleAsync(SuperAdmin, UserAdminRole);
                    await userManager.AddToRoleAsync(SuperAdmin, UserRole);
                }
            }
        }
    }
}
