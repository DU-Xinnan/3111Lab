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
    public class SavedAddressesController : Controller
    {
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();

        // GET: SavedAddresses
        public ActionResult Index()
        {
            var savedAddresses = db.SavedAddresses.Include(s => s.ShippingAccount);
            return View(savedAddresses.ToList());
        }

        // GET: SavedAddresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavedAddress savedAddress = db.SavedAddresses.Find(id);
            if (savedAddress == null)
            {
                return HttpNotFound();
            }
            return View(savedAddress);
        }

        // GET: SavedAddresses/Create
        public ActionResult Create()
        {
            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "ShippingAccountNumber");
            return View();
        }

        // POST: SavedAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SavedAddressID,NickName,Address,Type,ShippingAccountId")] SavedAddress savedAddress)
        {
            if (ModelState.IsValid)
            {
                db.SavedAddresses.Add(savedAddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "ShippingAccountNumber", savedAddress.ShippingAccountId);
            return View(savedAddress);
        }

        // GET: SavedAddresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavedAddress savedAddress = db.SavedAddresses.Find(id);
            if (savedAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "ShippingAccountNumber", savedAddress.ShippingAccountId);
            return View(savedAddress);
        }

        // POST: SavedAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SavedAddressID,NickName,Address,Type,ShippingAccountId")] SavedAddress savedAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(savedAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShippingAccountId = new SelectList(db.ShippingAccounts, "ShippingAccountId", "ShippingAccountNumber", savedAddress.ShippingAccountId);
            return View(savedAddress);
        }

        // GET: SavedAddresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavedAddress savedAddress = db.SavedAddresses.Find(id);
            if (savedAddress == null)
            {
                return HttpNotFound();
            }
            return View(savedAddress);
        }

        // POST: SavedAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SavedAddress savedAddress = db.SavedAddresses.Find(id);
            db.SavedAddresses.Remove(savedAddress);
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
