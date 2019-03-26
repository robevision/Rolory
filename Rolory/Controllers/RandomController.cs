using Microsoft.AspNet.Identity;
using Rolory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rolory.Controllers
{
    public class RandomController : Controller
    {
        ApplicationDbContext db;
        public RandomController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Random
        public ActionResult Index()
        {
            List<Contact> pushedContacts = new List<Contact>();
            string userId = User.Identity.GetUserId();
            var networker = db.Networkers.Where(n => n.UserId == userId).SingleOrDefault();
            var contactList = db.Contacts.Where(c => c.NetworkerId == networker.Id).Where(c=>c.InContact == false).ToList();
            var filteredContactList = contactList.Where(c => c.Perpetual == false).ToList();
            foreach(Contact contact in filteredContactList)
            {
                if()
            }
            return View();
        }
        public ActionResult Index(Contact contact)
        {
            return RedirectToAction("Index", "Random");
        }
        // GET: Random/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Random/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Random/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Random/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Random/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Random/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Random/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
