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
    public class ServicePackageFeesController : BaseController
    {
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();

        // GET: ServicePackageFees
        public ActionResult Index()
        {
            var servicePackageFees = db.ServicePackageFees.Include(s => s.PackageType).Include(s => s.ServiceType);
            return View(servicePackageFees.ToList());
        }

        // GET: ServicePackageFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type");
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type");
            return View();
        }

        // POST: ServicePackageFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServicePackageFeeID,Fee,MinimumFee,PackageTypeID,ServiceTypeID")] ServicePackageFee servicePackageFee)
        {
            if (ModelState.IsValid)
            {
                db.ServicePackageFees.Add(servicePackageFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // POST: ServicePackageFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServicePackageFeeID,Fee,MinimumFee,PackageTypeID,ServiceTypeID")] ServicePackageFee servicePackageFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicePackageFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeID = new SelectList(db.PackageTypes, "PackageTypeID", "Type", servicePackageFee.PackageTypeID);
            ViewBag.ServiceTypeID = new SelectList(db.ServiceTypes, "ServiceTypeID", "Type", servicePackageFee.ServiceTypeID);
            return View(servicePackageFee);
        }

        // GET: ServicePackageFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            if (servicePackageFee == null)
            {
                return HttpNotFound();
            }
            return View(servicePackageFee);
        }

        // POST: ServicePackageFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServicePackageFee servicePackageFee = db.ServicePackageFees.Find(id);
            db.ServicePackageFees.Remove(servicePackageFee);
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
