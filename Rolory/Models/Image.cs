using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rolory.Models
{
    public class Image
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}