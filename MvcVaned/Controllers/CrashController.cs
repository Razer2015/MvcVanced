using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcVanced.Controllers
{
    public class CrashController : Controller
    {
        // GET: Crash
        public ActionResult Index()
        {
            throw new Exception("Error: Just an error");
            return View();
        }
    }
}