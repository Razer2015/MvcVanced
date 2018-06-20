using GoogleDrive.Models;
using MvcVanced.Helpers;
using MvcVanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MvcVanced.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class DriveController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private APKDBContext db = new APKDBContext();
        List<string> Architectures = new List<string>() {
                "armeabi-v7a",
                "arm64-v8a",
                "x86",
                "x86_64"
            };
        List<string> APIs = new List<string>() {
            "API16",
            "API17",
            "API21",
        };
        List<string> DPIs = new List<string>() {
            "nodpi",
            "160dpi",
            "240dpi",
            "320dpi",
            "480dpi",
        };

        // GET: Drive
        public ActionResult Index() {
            var dbFiles = db.APKs.ToList();
            var files = Global.GoogleClient.FetchedFiles;

            if (files == null) {
                return View();
            }

            var Ids = new HashSet<string>(dbFiles.Select(x => x.FileID));
            var Versions = new HashSet<string>(dbFiles.Select(x => x.Version));
            var model = new DriveModel {
                NonRoot = new List<DriveFile>(files["NONROOT"]),
                NonRoot_Beta = new List<DriveFile>(files["NONROOT_BETA"]),
                Root = new List<DriveFile>(files["ROOT"]),
                Root_Beta = new List<DriveFile>(files["ROOT_BETA"]),
                Magisk = new List<DriveFile>(files["MAGISK"]),
                Magisk_Beta = new List<DriveFile>(files["MAGISK_BETA"])
            };

            model.NonRoot.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.NonRoot_Beta.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Root.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Root_Beta.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Magisk.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            model.Magisk_Beta.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));

            return View(model);
        }

        public ActionResult Refresh() {
            var before = Global.GoogleClient.FetchedFiles;
            var after = Global.GoogleClient.FetchAllFiles();
            var changes = Global.GoogleClient.GetChanges(before, after);

            foreach (var category in changes) {
                foreach (var change in category.Value) {
                    switch (change.Type) {
                        //case CHANGE_TYPE.ADD:
                        //    break;
                        case CHANGE_TYPE.DELETE:
                            if (db.APKs.Any(x => x.FileID.Equals(change.FileID))) {
                                APK aPK = db.APKs.FirstOrDefault(x => x.FileID.Equals(change.FileID));
                                db.APKs.Remove(aPK);
                            }
                            break;
                        case CHANGE_TYPE.CHANGE:
                            if (db.APKs.Any(x => x.FileID.Equals(change.FileID))) {
                                db.APKs.First(x => x.FileID.Equals(change.FileID)).Version = change.Version;
                            }
                            break;
                    }
                }
            }

            // Delete all IDs from database that aren't in the Google Drive
            var AllIDs = after["NONROOT"].Select(x => x);
            AllIDs = AllIDs.Union(after["NONROOT_BETA"].Select(x => x));
            AllIDs = AllIDs.Union(after["ROOT"].Select(x => x));
            AllIDs = AllIDs.Union(after["ROOT_BETA"].Select(x => x));
            AllIDs = AllIDs.Union(after["MAGISK"].Select(x => x));
            AllIDs = AllIDs.Union(after["MAGISK_BETA"].Select(x => x));
            var Ids = new HashSet<string>(AllIDs.Select(x => x.FileID));
            var Versions = new HashSet<string>(AllIDs.Select(x => x.Version));

            var dbFiles = db.APKs.ToList();
            dbFiles.RemoveAll(p => Ids.Contains(p.FileID) && Versions.Contains(p.Version));
            foreach (var item in dbFiles) {
                if (db.APKs.Any(x => x.FileID.Equals(item.FileID) && x.Type != APKTYPE.MICROG)) {
                    APK aPK = db.APKs.FirstOrDefault(x => x.FileID.Equals(item.FileID));
                    db.APKs.Remove(aPK);
                }
            }

            db.SaveChanges();

            Global.GoogleClient.FetchedFiles = after;

            return RedirectToAction("Index");
        }

        // GET: APKs/Create
        public ActionResult Create(string title, string version, long? size, string fileId, string type, bool fast = false) {
            //ID,Title,Version,Architecture,MinimumAPI,DPI,Size,Type,Downloads,FileID,Public

            var apkType = APKTYPE.NONROOT;
            switch (type) {
                default:
                case "nonroot":
                    apkType = APKTYPE.NONROOT;
                    break;
                case "nonroot_beta":
                    apkType = APKTYPE.NONROOT_BETA;
                    break;
                case "root":
                    apkType = APKTYPE.ROOT;
                    break;
                case "root_beta":
                    apkType = APKTYPE.ROOT_BETA;
                    break;
                case "magisk":
                    apkType = APKTYPE.MAGISK;
                    break;
                case "magisk_beta":
                    apkType = APKTYPE.MAGISK_BETA;
                    break;
            }

            if (ModelState.IsValid && fast) {
                var apk = new APK {
                    ID = 0,
                    Title = ParseTheme(title),
                    Version = version,
                    Architecture = ParseArch(title),
                    MinimumAPI = ParseApi(title),
                    DPI = ParseDpi(title),
                    Size = size ?? 0,
                    Type = apkType,
                    Downloads = 0,
                    FileID = fileId,
                    Public = false,
                    Published = DateTime.UtcNow
                };
                db.APKs.Add(apk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Data = new DriveFile {
                Name = ParseTheme(title),
                Version = version,
                Size = size,
                FileID = fileId
            };
            ViewBag.Arch = ParseArch(title);
            ViewBag.API = ParseApi(title);
            ViewBag.Type = apkType;

            return View();
        }

        private string ParseTheme(string title) {
            var name = title;
            if (name.Contains("vDark")) {
                name = "White/Dark";
            }
            else if (name.Contains("vBlack")) {
                name = "White/Black";
            }
            return (name);
        }

        private string ParseArch(string title) {
            var name = "All";
            if (Architectures.Any(x => title.Contains(x))) {
                var matches = Architectures.Where(x => title.Contains(x)).ToList();

                if (matches.Contains("x86_64") && Regex.Matches(title, "x86").Count <= 1) {
                    matches.Remove("x86");
                }

                name = string.Join("/", matches);
            }
            return (name);
        }

        private string ParseApi(string title) {
            var name = "Unknown";
            if (APIs.Any(x => title.Contains(x))) {
                var matches = APIs.Where(x => title.Contains(x)).ToList();
                name = string.Join("/", matches);
            }
            return (name);
        }

        private string ParseDpi(string title) {
            var name = "nodpi";
            if (DPIs.Any(x => title.Contains(x))) {
                var matches = DPIs.Where(x => title.Contains(x)).ToList();
                name = string.Join("/", matches);
            }
            return (name);
        }
    }
}