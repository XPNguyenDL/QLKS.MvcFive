using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.DAL
{
    public class AccountSeeder
    {
        public static void Seed(HotelDbContext context)
        {
            var userManager = new UserManager<Account>(new UserStore<Account>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            const string adminRole = "Admin",
                managerRole = "Manager",
                saleRole = "Salesman",
                customerRole = "Customer",
                userName = "xpnguyen",
                password = "123456",
                email = "xpnguyen240802@gmail.com";

            // Tạo quyền (vai trò của người dùng trong hệ thống)
            if (!roleManager.RoleExists(adminRole))
            {
                roleManager.Create(new IdentityRole(adminRole));
            }

            if (!roleManager.RoleExists(managerRole))
            {
                roleManager.Create(new IdentityRole(managerRole));
            }

            if (!roleManager.RoleExists(customerRole))
            {
                roleManager.Create(new IdentityRole(customerRole));
            }

            //Tạo tài khoản Admin
            var adminUser = new Account()
            {
                UserName = userName,
                Email = email,
                PhoneNumber = "034607648",
                Profile = new UserProfile
                {
                    FirstName = "Nguyễn Xuân",
                    LastName = "Phát",
                    Address = "Đà Lạt",
                    BirthDate = new DateTime(2002, 8, 24),
                    JobPosition = "Quản trị hệ thống",
                    Picture = "~/images/profile_sm.jpg"
                }

            };
            // Gán quyền Admin và Manager cho người dùng vừa tạo
            var result = userManager.Create(adminUser, password);
            // userManager.Update()
            if (result.Succeeded)
            {
                userManager.AddToRole(adminUser.Id, adminRole);
                userManager.AddToRole(adminUser.Id, managerRole);
            }


            // Tạo một tài khoản khách hàng
            var customer = new Account()
            {
                UserName = "bangnv",
                Email = "tranhuubang@gmail.com",
                PhoneNumber = "034607648",
                Profile = new UserProfile
                {
                    FirstName = "Trần Hữu",
                    LastName = "Bằng",
                    Address = "Đà Lạt",
                    BirthDate = new DateTime(2002, 8, 8)
                }
            };

            result = userManager.Create(customer, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(customer.Id, customerRole);
            }
        }
    }
}