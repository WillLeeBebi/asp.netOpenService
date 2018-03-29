using liemei.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace liemei.Service.Controllers
{
    public class SharedController : BaseController
    {
        // GET: Shared
        public ActionResult Header(string title)
        {
            ViewBag.title = title;
            return View(User);
        }
    }
}