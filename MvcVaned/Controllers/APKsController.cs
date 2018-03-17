using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcVanced.Models;

namespace MvcVanced.Controllers
{
    public class APKsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private APKDBContext db = new APKDBContext();

        // GET: APKs
        public ActionResult Index(APKTYPE type = APKTYPE.NONROOT)
        {
            ViewBag.Type = type;
            if (type == APKTYPE.NONROOT) {
                var xd = db.APKs.ToList().Where(x => x.Type.Equals(APKTYPE.MICROG)).ToList();
                ViewBag.MicroG = xd;
            }

            return View(db.APKs.ToList().OrderByDescending(x => x.Version));
        }

        // GET: APKs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APK aPK = db.APKs.Find(id);
            if (aPK == null)
            {
                return HttpNotFound();
            }
            return View(aPK);
        }

        // GET: APKs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create([Bind(Include = "ID,Title,Version,Architecture,MinimumAPI,DPI,Size,Type,Downloads,FileID,Public")] APK aPK)
        {
            if (ModelState.IsValid)
            {
                db.APKs.Add(aPK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aPK);
        }

        // GET: APKs/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APK aPK = db.APKs.Find(id);
            if (aPK == null)
            {
                return HttpNotFound();
            }
            return View(aPK);
        }

        // POST: APKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit([Bind(Include = "ID,Title,Version,Architecture,MinimumAPI,DPI,Size,Type,Downloads,FileID,Public")] APK aPK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aPK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aPK);
        }

        // GET: APKs/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APK aPK = db.APKs.Find(id);
            if (aPK == null)
            {
                return HttpNotFound();
            }
            return View(aPK);
        }

        // POST: APKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            APK aPK = db.APKs.Find(id);
            db.APKs.Remove(aPK);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: APKs/Download/0BxmmnFTeQFuaeC1NSjlQd2s4VFk
        public ActionResult Download(string fileId) {
            if (!db.APKs.Any(x => x.FileID.Equals(fileId))) {
                return HttpNotFound();
            }

            // Add download to counter
            db.APKs.FirstOrDefault(x => x.FileID.Equals(fileId)).Downloads++;
            db.SaveChanges();

            // Download
            return Redirect($"https://drive.google.com/uc?export=download&id={fileId}");
        }

        // GET: APKs/Publicity/0BxmmnFTeQFuaeC1NSjlQd2s4VFk
        [Authorize(Roles = "Admin,Manager")]
        public void Publicity(string fileId) {
            if (fileId == null) {
                return;// new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.APKs.Any(x => x.FileID.Equals(fileId))) {
                return;// HttpNotFound();
            }

            // Reverse the publicity
            db.APKs.FirstOrDefault(x => x.FileID.Equals(fileId)).Public = !db.APKs.FirstOrDefault(x => x.FileID.Equals(fileId)).Public;
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
