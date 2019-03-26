using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Rolory.Models;

[assembly: OwinStartupAttribute(typeof(Rolory.Startup))]
namespace Rolory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }
        private void CreateRolesAndUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                var user = new ApplicationUser();
                user.UserName = "example";
                user.Email = "example@example.com";
                string userPassword = "Hi1234!";
                var checkUser = userManager.Create(user, userPassword);

                if (checkUser.Succeeded)
                {
                    var primaryResult = userManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Networker"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Networker";
                roleManager.Create(role);
                var user = new ApplicationUser();
                var primaryResult = userManager.AddToRole(user.Id, "Networker");
            }
        }
    }
}
