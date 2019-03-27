using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rolory.Models
{
    public class ImageController : Controller
    {
        // GET: Image
        private ApplicationDbContext db;
        public ActionResult Add()
        {
            db = new ApplicationDbContext();
            return View();
        }
        [HttpPost]
        public ActionResult Add(Contact imageModel)
        {
            if (ModelState.IsValid)
            {
                //string fileName = FilePathResult.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                //string extension = FilePathResult.GetExtension(imageModel.ImageFile.FileName);
                //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //imageModel.image = "~/UserImages/" + fileName;
                //fileName = Path.Combine(Server.MapPath("~/UserImages/"), fileName);
                //imageModel.ImageFile.SaveAs(fileName);
                //db.Contacts.Add(imageModel);
                //db.SaveChanges();
                //ModelState.Clear();
            }
           
            return View();
        }
    }
}