using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics;

namespace Rolory.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string UserRole { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            int timeout = 1000;
            var task = manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                return userIdentity;
            }
            else
            {
                userIdentity = null;
                return userIdentity;
            }
           
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Networker> Networkers { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<SharedActivity> SharedActivities { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       
    }
}