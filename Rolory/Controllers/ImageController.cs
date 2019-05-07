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
                if (imageModel.ImagePath != null)
                {
                    var image = new Image();
                    string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                    string extension = Path.GetExtension(image.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    imageModel.ImagePath = "~/UserImages/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/UserImages/"), fileName);
                    image.ImageFile.SaveAs(fileName);
                }
            }
           
            return View();
        }
    }
}