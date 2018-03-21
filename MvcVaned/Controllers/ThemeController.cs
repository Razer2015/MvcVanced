using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcVanced.Controllers
{
    public class ThemeController : Controller
    {
        // GET: Theme
        public ActionResult Index()
        {
            return View();
        }

        // GET: Theme
        public ActionResult ChangeTheme(int? id, Uri url) {
            HttpCookie cookie = new HttpCookie("theme", (id ?? 0).ToString()) {
                Expires = DateTime.Now.AddYears(1)
            };
            Response.Cookies.Add(cookie);

            return (Redirect(url.AbsoluteUri));
        }
    }
}