using Microsoft.AspNet.Identity;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rolory.Controllers
{
    public class HomeController : Controller
    {
        MessageManagement msg;
        ApplicationDbContext db;
        public ActionResult Index()
        {
            db = new ApplicationDbContext();
            msg = new MessageManagement();
            msg.CycleMessages();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}