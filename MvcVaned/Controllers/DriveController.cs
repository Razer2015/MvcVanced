using GoogleDrive.Models;
using MvcVanced.Helpers;
using MvcVanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcVanced.Controllers
{
    public class DriveController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private APKDBContext db = new APKDBContext();

        // GET: Drive
        public ActionResult Index() {
            var changes = Global.GoogleClient.GetChanges();

            // Remove the IDs that are already listed in the site but have been deleted from the Google Drive
            foreach (var change in changes) {
                if (db.APKs.Any(x => x.FileID.Equals(change))) {
                    APK aPK = db.APKs.FirstOrDefault(x => x.FileID.Equals(change));
                    db.APKs.Remove(aPK);
                }
            }
            db.SaveChanges();

            var dbFiles = db.APKs.ToList();
            var files = Global.GoogleClient.FetchAllFiles();

            var Ids = new HashSet<string>(dbFiles.Select(x => x.FileID));
            var Versions = new HashSet<string>(dbFiles.Select(x => x.Version));
            var model = new DriveModel();
            model.NonRoot = new List<DriveFile>(files["NONROOT"]);
            model.NonRoot_Beta = new List<DriveFile>(files["NONROOT_BETA"]);
            model.Root = new List<DriveFile>(files["ROOT"]);
            model.Root_Beta = new List<DriveFile>(files["ROOT_BETA"]);
            model.Magisk = new List<DriveFile>(files["MAGISK"]);
            model.Magisk_Beta = new List<DriveFile>(files["MAGISK_BETA"]);

            model.NonRoot.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.NonRoot_Beta.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Root.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Root_Beta.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Magisk.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Magisk_Beta.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));

            return View(model);
        }

        // GET: APKs/Create
        public ActionResult Create(string title, string version, long? size, string fileId, string type) {
            ViewBag.Data = new GoogleDrive.Models.DriveFile {
                Name = title,
                Version = version,
                Size = size,
                FileID = fileId
            };

            switch (type) {
                default:
                case "nonroot":
                    ViewBag.Arch = "All";
                    ViewBag.API = "API17";
                    ViewBag.Type = APKTYPE.NONROOT;
                    break;
                case "nonroot_beta":
                    ViewBag.Arch = "All";
                    ViewBag.API = "API17";
                    ViewBag.Type = APKTYPE.NONROOT_BETA;
                    break;
                case "root":
                    ViewBag.Arch = "";
                    ViewBag.API = "API21";
                    ViewBag.Type = APKTYPE.ROOT;
                    break;
                case "root_beta":
                    ViewBag.Arch = "";
                    ViewBag.API = "API21";
                    ViewBag.Type = APKTYPE.ROOT_BETA;
                    break;
                case "magisk":
                    ViewBag.Arch = "";
                    ViewBag.API = "API21";
                    ViewBag.Type = APKTYPE.MAGISK;
                    break;
                case "magisk_beta":
                    ViewBag.Arch = "";
                    ViewBag.API = "API21";
                    ViewBag.Type = APKTYPE.MAGISK_BETA;
                    break;
            }

            return View();
        }
    }
}