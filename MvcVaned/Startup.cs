using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MvcVanced.Models;
using Owin;
using System.Threading;
using System.Web;

[assembly: OwinStartup(typeof(MvcVanced.Startup))]

namespace MvcVanced
{
    public partial class Startup
    {

        public Startup() {
            Changelogs.Changelog.VersionPath = HttpContext.Current.Server.MapPath("~/App_Data/Changelogs/VersionChanges/");
            Changelogs.Changelog.BuildPath = HttpContext.Current.Server.MapPath("~/App_Data/Changelogs/BuildChanges/");
            Changelogs.Changelog.ThemePath = HttpContext.Current.Server.MapPath("~/App_Data/Changelogs/ThemeChanges/");

            new Watcher().Run(Changelogs.Changelog.VersionPath, CHANGELOG_TYPE.VERSION);
            new Watcher().Run(Changelogs.Changelog.BuildPath, CHANGELOG_TYPE.BUILD);
            new Watcher().Run(Changelogs.Changelog.ThemePath, CHANGELOG_TYPE.THEME);
            //Changelogs.Changelog.GenerateExampleData();

            Global.GoogleClient = new GoogleDrive.Client(HttpContext.Current.Server.MapPath("~/App_Data/"));
            if (Global.GoogleClient.Connect()) {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Global.GoogleClient.FetchedFiles = Global.GoogleClient.FetchAllFiles();
                }).Start();
            }
        }

        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void CreateRolesandUsers() {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User
            if (!roleManager.RoleExists("Admin")) {

                // first we create Admin rool   
                var role = new IdentityRole {
                    Name = "Admin"
                };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website
                var user = new ApplicationUser {
                    UserName = "xfileFIN",
                    Email = "teamvanced@gmail.com"
                };

                string userPWD = "r!8UV5476v1A"; // Has been changed in the release database

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded) {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager")) {
                var role = new IdentityRole {
                    Name = "Manager"
                };
                roleManager.Create(role);
            }

            // creating Creating Tester role    
            if (!roleManager.RoleExists("Tester")) {
                var role = new IdentityRole {
                    Name = "Tester"
                };
                roleManager.Create(role);
            }
        }
    }
}
