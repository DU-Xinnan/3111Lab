using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20256461.Models;

namespace SinExWebApp20256461.Controllers
{
    public class PakageTypeSizesController : Controller
    {
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();

        // GET: PakageTypeSizes
        public ActionResult Index()
        {
            var pakageTypeSizes = db.PakageTypeSizes.Include(p => p.PackageType);
            return View(pakageTypeSizes.ToList());
        }

        // GET: PakageTypeSizes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PakageTypeSize pakageTypeSize = db.PakageTypeSizes.Find(id);
            if (pakageTypeSize == null)
            {
                return HttpNotFound();
            }
            return View(pakageTypeSize);
        }

        // GET: PakageTypeSizes/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type");
            return View();
        }

        // POST: PakageTypeSizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PakageTypeSizeID,size,weightLimit,PackageTypeID")] PakageTypeSize pakageTypeSize)
        {
            if (ModelState.IsValid)
            {
                db.PakageTypeSizes.Add(pakageTypeSize);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", pakageTypeSize.PackageTypeID);
            return View(pakageTypeSize);
        }

        // GET: PakageTypeSizes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PakageTypeSize pakageTypeSize = db.PakageTypeSizes.Find(id);
            if (pakageTypeSize == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", pakageTypeSize.PackageTypeID);
            return View(pakageTypeSize);
        }

        // POST: PakageTypeSizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PakageTypeSizeID,size,weightLimit,PackageTypeID")] PakageTypeSize pakageTypeSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pakageTypeSize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", pakageTypeSize.PackageTypeID);
            return View(pakageTypeSize);
        }

        // GET: PakageTypeSizes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PakageTypeSize pakageTypeSize = db.PakageTypeSizes.Find(id);
            if (pakageTypeSize == null)
            {
                return HttpNotFound();
            }
            return View(pakageTypeSize);
        }

        // POST: PakageTypeSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PakageTypeSize pakageTypeSize = db.PakageTypeSizes.Find(id);
            db.PakageTypeSizes.Remove(pakageTypeSize);
            db.SaveChanges();
            return RedirectToAction("Index");
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
