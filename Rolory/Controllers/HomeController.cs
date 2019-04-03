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
        public ActionResult Index()
        {
            ViewBag.Alert = null;
            msg = new MessageManagement();
            msg.GenerateAllUserEmails();
            if (msg.CycleMessages() == true)
            {
                ViewBag.Alert = "You have a new message!";
            }
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