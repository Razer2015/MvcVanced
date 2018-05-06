using System.Web.Mvc;

namespace MvcVanced.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Changelogs(CHANGELOG_TYPE changelog = CHANGELOG_TYPE.BUILD) {
            var changelogs = MvcVanced.Changelogs.Changelog.GetChangelogs(changelog);
            
            ViewBag.Type = changelog;
            
            return View(changelogs);
        }

        public ActionResult Contact() {
            ViewBag.Message = "How to reach us?";

            return View();
        }
    }
}