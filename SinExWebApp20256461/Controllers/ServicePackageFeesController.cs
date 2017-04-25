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
        public ActionResult Costcalculation(string Origin, string Destination, string ServiceType, IList<string> PackagesTypeSizes, IList<decimal> Weights, int? NumOfPackages, string submit)
        {
            CostCalculationViewModel CostViewModel = new CostCalculationViewModel();
            CostViewModel.ServiceTypesList = db.ServiceTypes.Select(a => a.Type).Distinct().ToList();
            CostViewModel.DestinationList = db.Destinations.Select(a => a.City).Distinct().ToList();
            CostViewModel.OriginList = db.Destinations.Select(a => a.City).Distinct().ToList();
            CostViewModel.PackageTypesSizeList = db.PakageTypeSizes.Select(a => a.size).Distinct().ToList();
            if (String.IsNullOrEmpty(submit) || NumOfPackages == null || NumOfPackages <= 0)
            {
                if (NumOfPackages <= 0)
                {
                    ViewBag.msg = "Package number can't smaller or equal zero";
                }
                ViewBag.status = "initial";
                return View(CostViewModel);
            }
            else if (submit == "Add Packages")
            {
                CostViewModel.Origin = Origin;
                CostViewModel.Destination = Destination;
                CostViewModel.ServiceType = ServiceType;
                CostViewModel.NumOfPackages = (int)NumOfPackages;
                ViewBag.status = "Add Packages";
                return View(CostViewModel);
            }
            else if (submit == "Calculate")
            {
                CostViewModel.PackagesTypeSizes = PackagesTypeSizes;
                CostViewModel.Weights = Weights;
                CostViewModel.NumOfPackages = (int)NumOfPackages;
                CostViewModel.Origin = Origin;
                CostViewModel.Destination = Destination;
                CostViewModel.ServiceType = ServiceType;
                Dictionary<string, decimal>[] Prices = new Dictionary<string, decimal>[(int)NumOfPackages];
                Dictionary<string, decimal> TotalPrice = new Dictionary<string, decimal>();
                TotalPrice["CNY"] = 0;
                for (var i = 0; i < NumOfPackages; i += 1)
                {
                    if (!PackagesTypeSizes[i].Contains("Envenlope"))
                    {
                        //if (!Regex.IsMatch(Weights[i], "^\\d+(?:\\.\\d)?$"))
                        //{
                        //    ViewBag.status = "Add Packages";
                        //    ViewBag.msg = "Please input valid weight";
                        //    return View(CostViewModel);
                        //}
                        if (Weights[i] <= 0 || Weights[i] > (decimal)5792000000000000000000000.0)
                        {
                            ViewBag.status = "Add Packages";
                            ViewBag.msg = "Please weight can't be smaller or equal to 1, or larger than the weight of the earth ";
                            return View(CostViewModel);
                        }
                    }
                    Prices[i] = Calculate(ServiceType, PackagesTypeSizes[i], Weights[i]);
                    TotalPrice["CNY"] += Prices[i]["CNY"];
                }
                TotalPrice["HKD"] = ConvertCurrency("HKD", TotalPrice["CNY"]);
                TotalPrice["MOP"] = ConvertCurrency("MOP", TotalPrice["CNY"]);
                TotalPrice["TWD"] = ConvertCurrency("TWD", TotalPrice["CNY"]);
                ViewBag.Prices = Prices;
                ViewBag.TotalPrice = TotalPrice;
                ViewBag.status = "Calculate";
                return View(CostViewModel);
            }
            return View();
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
