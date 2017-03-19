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
    public class BusinessShippingAccountsController : Controller
    {
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();

        // GET: BusinessShippingAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessShippingAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShippingAccountID,BuildingInformation,StreetInformation,City,ProvinceCode,PostalCode,CardType,CardNumber,SecurityNumber,CardHolderName,Month,Year,PhoneNumber,EmailAddress,ContactPersonName,CompanyName,DepartmentName")] BusinessShippingAccount businessShippingAccount)
        {
            if (ModelState.IsValid)
            {
                db.ShippingAccounts.Add(businessShippingAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(businessShippingAccount);
        }

        // GET: BusinessShippingAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = db.BusinessShippingAccounts.SingleOrDefault(c => c.UserName == User.Identity.Name);
            id = query.ShippingAccountId;
            BusinessShippingAccount businessShippingAccount = (BusinessShippingAccount)db.ShippingAccounts.Find(id);
            if (businessShippingAccount == null)
            {
                return HttpNotFound();
            }
            return View(businessShippingAccount);
        }
        // GET: BusinessShippingAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = db.BusinessShippingAccounts.SingleOrDefault(c => c.UserName == User.Identity.Name);
            id = query.ShippingAccountId;
            BusinessShippingAccount businessShippingAccount = (BusinessShippingAccount)db.ShippingAccounts.Find(id);
            if (businessShippingAccount == null)
            {
                return HttpNotFound();
            }
            return View(businessShippingAccount);
        }
        // POST: BusinessShippingAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShippingAccountID,BuildingInformation,StreetInformation,City,ProvinceCode,PostalCode,CardType,CardNumber,SecurityNumber,CardHolderName,Month,Year,PhoneNumber,EmailAddress,ContactPersonName,CompanyName,DepartmentName")] BusinessShippingAccount businessShippingAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessShippingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(businessShippingAccount);
        }
    }
}
