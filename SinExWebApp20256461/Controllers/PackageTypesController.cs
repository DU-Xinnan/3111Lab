﻿using System;
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
    public class PackageTypesController : Controller
    {
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();

        // GET: PackageTypes
        public ActionResult Index()
        {
            return View(db.PackageTypes.ToList());
        }

        // GET: PackageTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageType packageType = db.PackageTypes.Find(id);
            if (packageType == null)
            {
                return HttpNotFound();
            }
            return View(packageType);
        }

        // GET: PackageTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PackageTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackageTypeID,Type,Description")] PackageType packageType)
        {
            if (ModelState.IsValid)
            {
                db.PackageTypes.Add(packageType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packageType);
        }

        // GET: PackageTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageType packageType = db.PackageTypes.Find(id);
            if (packageType == null)
            {
                return HttpNotFound();
            }
            return View(packageType);
        }

        // POST: PackageTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackageTypeID,Type,Description")] PackageType packageType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packageType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packageType);
        }

        // GET: PackageTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageType packageType = db.PackageTypes.Find(id);
            if (packageType == null)
            {
                return HttpNotFound();
            }
            return View(packageType);
        }

        // POST: PackageTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackageType packageType = db.PackageTypes.Find(id);
            db.PackageTypes.Remove(packageType);
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
