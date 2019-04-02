using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Rolory.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: User
        public bool IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext db = new ApplicationDbContext();
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                var s = userManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                //	ApplicationDbContext context = new ApplicationDbContext();
                //	var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                //var s = userManager.GetRoles(user.GetUserId());
                ViewBag.displayMenu = "No";

                if (IsAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }

            return View();


        }
        public ActionResult Manage(int? id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).Select(n=>n.UserId).SingleOrDefault(); 
            if (networker != null)
            {
                return RedirectToAction("Modify", "User");
            }
           return RedirectToAction("CreateAccount", "User");
        }
        //GET
        public ActionResult CreateAccount(ApplicationUser user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            ViewBag.ID = new SelectList(db.Networkers, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(Networker networker)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            networker.UserId = userId;
            db.Networkers.Add(networker);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public ActionResult Modify()
        {
            string userId = User.Identity.GetUserId();
            ApplicationDbContext db = new ApplicationDbContext();
            Networker networker = db.Networkers.Where(n=>n.UserId==userId).SingleOrDefault();
            ViewBag.Email = db.Users.Where(u => u.Id == userId).Select(u => u.Email);
            return View(networker);
        }
        [HttpPost]
        public ActionResult Modify(Networker networker)
        {
            string monday = null;
            string tuesday = null;
            string wednesday = null;
            string thursday = null;
            string friday = null;
            string saturday = null;
            string sunday = null;
            if(networker != null)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                if(networker.ReceiveEmails == false)
                {
                    
                }

                if (Request.Form["Monday"] != null)
                {
                    monday = Request.Form["Monday"].ToString();
                }
                if (Request.Form["Tuesday"] != null)
                {
                    tuesday = Request.Form["Tuesday"].ToString();
                }
                if (Request.Form["Wednesday"] != null)
                {
                    wednesday = (Request.Form["Wednesday"].ToString());
                }
                if (Request.Form["Thursday"] != null)
                {
                    thursday = Request.Form["Thursday"].ToString();
                }
                if (Request.Form["Friday"] != null)
                {
                    friday = Request.Form["Friday"].ToString();
                }
                if (Request.Form["Saturday"] != null)
                {
                    saturday = Request.Form["Saturday"].ToString();
                }
                if (Request.Form["Sunday"] != null)
                {
                    sunday = Request.Form["Sunday"].ToString();
                }

                string availabilityString = db.Networkers.Where(n => n.Id == networker.Id).Select(n => n.Availability).SingleOrDefault();
                string addedAvailability = monday + tuesday + wednesday + thursday + friday + saturday + sunday;
                networker.Availability = addedAvailability;
                //if (!availabilityString.Contains(addedAvailability))
                //{
                //    string updatedAvailabilityString = availabilityString + "," + " " + addedAvailability;
                //    networker.Availability = updatedAvailabilityString;
                //}
                //string activitiesString = networker.UserActivities;
                //string addedActivities = Request.Form["Activities"].ToString();
                //string updatedActivitiesString = activitiesString + "," + " " + addedActivities;
                //networker.UserActivities = updatedActivitiesString;
                if (networker.Goal != null || networker.Goal != 0)
                {
                    networker.GoalActive = true;
                }
                db.Entry(networker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}