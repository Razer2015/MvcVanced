﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MvcVanced.Models;
using Owin;
using System.Web;

[assembly: OwinStartup(typeof(MvcVanced.Startup))]

namespace MvcVanced
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            createRolesandUsers();

            Global.GoogleClient = new GoogleDrive.Client(HttpContext.Current.Server.MapPath("~/App_Data/"));
            Global.GoogleClient.FetchedFiles = Global.GoogleClient.FetchAllFiles();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers() {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin")) {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "xfileFIN";
                user.Email = "teamvanced@gmail.com";

                string userPWD = "r!8UV5476v1A";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded) {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager")) {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Tester role    
            if (!roleManager.RoleExists("Tester")) {
                var role = new IdentityRole();
                role.Name = "Tester";
                roleManager.Create(role);

            }
        }
    }
}
