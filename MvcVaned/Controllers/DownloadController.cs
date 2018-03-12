using System.Web;
using System.Web.Mvc;

namespace MvcVanced.Controllers
{

    public class DownloadController : Controller
    {
        // 
        // GET: /Download/ 

        public ActionResult Index() {
            return View();
        }

        // 
        // GET: /Download/Welcome/ 

        public string Welcome() {
            return "This is the Welcome action method...";
        }
    }
}