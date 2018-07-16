using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MvcVanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcVanced.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext context;

        public AdminController() {
            context = new ApplicationDbContext();
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated) {
                if (!isAdminUser()) {
                    return RedirectToAction("Index", "Home");
                }
            }
            else {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public Boolean isAdminUser() {
            if (User.Identity.IsAuthenticated) {
                var user = User.Identity;
                var UserManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin") {
                    return true;
                }
                else {
                    return false;
                }
            }
            return false;
        }
    }
}