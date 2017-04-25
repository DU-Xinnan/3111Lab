using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20256461.Models;
using SinExWebApp20256461.ViewModels;
namespace SinExWebApp20256461.Controllers
{
    public class TrackingsController : Controller
    {
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();

        // GET: Trackings
        public ActionResult Index()
        {
            var trackings = db.Trackings;
            return View(trackings.ToList());
        }

        // GET: Trackings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }
            return View(tracking);
        }

        // GET: Trackings/Create
        [Authorize(Roles = "Employee")]
        public ActionResult Create()
        {
            ViewBag.currTime = DateTime.Now;
            ViewBag.WaybillIds = new SelectList(db.Shipments.Select(a => a.WaybillId).Distinct());
            return View();
        }

        // POST: Trackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TrackingID,WaybillNumber,DateTime,Description,Location,Remarks")] Tracking tracking)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string WaybillNumber = tracking.WaybillNumber;
        //        tracking.WaybillId = db.Shipments.SingleOrDefault(a => a.WaybillNumber == WaybillNumber).WaybillId;
        //        db.Trackings.Add(tracking);
        //        db.SaveChanges();
        //        return RedirectToAction("Create");
        //    }

        //    ViewBag.WaybillId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", tracking.WaybillId);
        //    return View(tracking);
        //}

        // GET: Trackings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }
            ViewBag.WaybillId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", tracking.WaybillId);
            return View(tracking);
        }

        // POST: Trackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrackingID,WaybillId,DateTime,Description,Location,Remarks")] Tracking tracking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tracking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WaybillId = new SelectList(db.Shipments, "WaybillId", "ReferenceNumber", tracking.WaybillId);
            return View(tracking);
        }

        // GET: Trackings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tracking tracking = db.Trackings.Find(id);
            if (tracking == null)
            {
                return HttpNotFound();
            }
            return View(tracking);
        }
        //public ActionResult GetTracking(string WaybillNumber)
        //{
        //    TrackingViewModel TrackingView = new TrackingViewModel();

        //    if (WaybillNumber == null)
        //    {
        //        return View(TrackingView);
        //    }
        //    TrackingView.Trackings = db.Trackings.Where(a => a.Shipment.WaybillNumber == WaybillNumber).OrderBy(a => a.DateTime).ToList();
        //    TrackingView.Shipment = db.Shipments.SingleOrDefault(a => a.WaybillNumber == WaybillNumber);
        //    return View(TrackingView);
        //}
        // POST: Trackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tracking tracking = db.Trackings.Find(id);
            db.Trackings.Remove(tracking);
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
