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
        public ActionResult Costcalculation(string Origin, string Destination, string ServiceType, IList<string> PackagesTypeSizes, IList<string> Weights, int? NumOfPackages, string submit)
        {
            CostCalculationViewModel CostViewModel = new CostCalculationViewModel();
            CostViewModel.ServiceTypesList = db.ServiceTypes.Select(a => a.Type).Distinct().ToList();
            CostViewModel.DestinationList = db.Destinations.Select(a => a.City).Distinct().ToList();
            CostViewModel.OriginList = db.Destinations.Select(a => a.City).Distinct().ToList();
            CostViewModel.PackageTypesSizeList = db.PakageTypeSizes.Select(a => a.size).Distinct().ToList();
            if (String.IsNullOrEmpty(submit))
            {
                ViewBag.status = "initial";
                return View(CostViewModel);
            }
            else if(submit == "Add Packages")
            {
                CostViewModel.Origin = Origin;
                CostViewModel.Destination = Destination;
                CostViewModel.ServiceType = ServiceType;
                CostViewModel.NumOfPackages = (int)NumOfPackages;
                ViewBag.status = "Add Packages";
                return View(CostViewModel);
            }
            else if(submit == "Calculate")
            {
                CostViewModel.PackagesTypeSizes = PackagesTypeSizes;
                CostViewModel.Weights = Weights;
                CostViewModel.NumOfPackages = (int)NumOfPackages;
                CostViewModel.Origin = Origin;
                CostViewModel.Destination = Destination;
                CostViewModel.ServiceType = ServiceType;
                Dictionary<string, decimal>[] Prices = new Dictionary<string, decimal>[(int)NumOfPackages];
                for(var i = 0; i < NumOfPackages; i += 1)
                {
                    Prices[i] = Calculate(ServiceType, PackagesTypeSizes[i], Weights[i]);
                }
                ViewBag.Prices = Prices;
                ViewBag.status = "Calculate";
                return View(CostViewModel);
            }
            return View();
        }
        Dictionary<string, decimal> Calculate(string ServiceType, string PackageTypeSize, string weight)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            ServiceType CostServiceType = db.ServiceTypes.SingleOrDefault(a => a.Type == ServiceType);
            PakageTypeSize CostPackageTypeSize = db.PakageTypeSizes.SingleOrDefault(a => a.size == PackageTypeSize);
            ServicePackageFee CostFee = db.ServicePackageFees.SingleOrDefault(a => a.PackageTypeID == CostPackageTypeSize.PackageType.PackageTypeID && a.ServiceTypeID == CostServiceType.ServiceTypeID);
            string WeightLimit = CostPackageTypeSize.weightLimit;
            decimal Fee = CostFee.Fee;
            decimal MinimumFee = CostFee.MinimumFee;
            decimal Price = Fee;
            if (CostPackageTypeSize.PackageType.Type == "Envelope")
            {
                Price = Fee;
            }
            else
            {
                decimal actualWeight = decimal.Parse(weight);
                Price = Fee * actualWeight;
                if (CostPackageTypeSize.PackageType.Type != "Tube")
                {
                    decimal actualWeightLimit = decimal.Parse(WeightLimit.Replace("kg", ""));
                    if (actualWeight > actualWeightLimit)
                    {
                        Price += 500;
                    }
                }
                if (Price < MinimumFee)
                {
                    Price = MinimumFee;
                }
            }
            result.Add("CNY", Price);
            result.Add("HKD", ConvertCurrency("HKD", Price));
            result.Add("MOP", ConvertCurrency("MOP", Price));
            result.Add("TWD", ConvertCurrency("TWD", Price));
            return result;
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
