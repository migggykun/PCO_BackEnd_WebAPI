using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Accounts;
namespace PCO_BackEnd_WebAPI.Models.Seeder
{
    public class ApplicationUserSeeder : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            string email = "admin@admin.com";
            string password = "Admin12345^";
            string phoneNumber = "1234";
            UserInfo userInfo = new UserInfo()
            {
                FirstName = "admin",
                LastName = "admin",
                Organization = "PCO",
                MembershipTypeId = 1,
                Address = "Manila"
            };

            ApplicationUser user = new ApplicationUser()
            {
                Email = email,
                UserName = email,
                PhoneNumber = phoneNumber,
                UserInfo = userInfo
            };
            context.Users.Add(user);
            base.Seed(context);
        }
    }
}