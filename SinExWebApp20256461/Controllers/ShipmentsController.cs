﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20256461.Models;
using SinExWebApp20256461.ViewModels;
using X.PagedList;
using System.Windows.Forms;

namespace SinExWebApp20256461.Controllers
{
    public class ShipmentsController : BaseController
    {  
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();
        // GET: Shipments/GenerateHistoryReport
        [Authorize(Roles = "Employee, Customer")]
        public ActionResult GenerateHistoryReport(int? ShippingAccountId, string sortOrder, int? page, DateTime? ShippedStartDate, DateTime? ShippedEndDate)
        {
            // Instantiate an instance of the ShipmentsReportViewModel and the ShipmentsSearchViewModel.
            var shipmentSearch = new ShipmentsReportViewModel();
            shipmentSearch.Shipment = new ShipmentsSearchViewModel();
            ViewBag.CurrentSort = sortOrder;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            if (ShippingAccountId == null)
            {
                ShippingAccountId = 0;
            }
            // Populate the ShippingAccountId dropdown list.
            shipmentSearch.Shipment.ShippingAccounts = PopulateShippingAccountsDropdownList().ToList();
            if (User.IsInRole("Customer"))
            {
                shipmentSearch.Shipment.ShippingAccounts = PopulateCustomerShippingAccountsDropdownList().ToList();
                //var currShippingAccount = db.Shipments.Where(a => a.ShippingAccount.UserName == User.Identity.Name).Select(a => a.ShippingAccountId).Distinct().ToList();
                //if (!currShippingAccount.Contains((int)ShippingAccountId))
                //{
                   // shipmentSearch.Shipments = new ShipmentsListViewModel().ToPagedList;
                    //return View(shipmentSearch);
                //}
            }
            ViewBag.CurrentShippingAccountId = ShippingAccountId;
            ViewBag.CurrentShippingStartDate = ShippedStartDate;
            ViewBag.CurrentShippingEndDate = ShippedEndDate;
            // Initialize the query to retrieve shipments using the ShipmentsListViewModel.
            var shipmentQuery = from s in db.Shipments
                                select new ShipmentsListViewModel
                                {
                                    WaybillId = s.WaybillId,
                                    ServiceType = s.ServiceType,
                                    ShippedDate = s.ShippedDate,
                                    DeliveredDate = s.DeliveredDate,
                                    RecipientName = s.RecipientName,
                                    NumberOfPackages = s.NumberOfPackages,
                                    Origin = s.Origin,
                                    Destination = s.Destination,
                                    ShippingAccountId = s.ShippingAccountId
                                };
            if (User.IsInRole("Customer"))
            {
                string userName = System.Web.HttpContext.Current.User.Identity.Name;
                shipmentQuery = from s in db.Shipments
                                where s.ShippingAccount.UserName == userName
                                select new ShipmentsListViewModel
                                {
                                    WaybillId = s.WaybillId,
                                    ServiceType = s.ServiceType,
                                    ShippedDate = s.ShippedDate,
                                    DeliveredDate = s.DeliveredDate,
                                    RecipientName = s.RecipientName,
                                    NumberOfPackages = s.NumberOfPackages,
                                    Origin = s.Origin,
                                    Destination = s.Destination,
                                    ShippingAccountId = s.ShippingAccountId
                                };
            }
            // Add the condition to select a spefic shipping account if shipping account id is not null.
            if (ShippingAccountId != null)
            {
                // TODO: Construct the LINQ query to retrive only the shipments for the specified shipping account id.
                shipmentQuery = from s in shipmentQuery
                                where s.ShippingAccountId == ShippingAccountId
                                select s;
                // shipmentSearch.Shipments = shipmentQuery.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                // Return an empty result if no shipping account id has been selected.
                // shipmentSearch.Shipments = new ShipmentsListViewModel[0].ToPagedList(pageNumber, pageSize);
                shipmentQuery = from s in shipmentQuery
                                where s.ShippingAccountId == 0
                                select s;
                // page = 1;
            }

            if (ShippedStartDate != null && ShippedEndDate != null)
            {
                shipmentQuery = from s in shipmentQuery
                                where s.ShippedDate >= ShippedStartDate && s.ShippedDate <= ShippedEndDate
                                select s;
            }
            ViewBag.ServiceTypeSortParm = string.IsNullOrEmpty(sortOrder) ? "serviceType_desc" : "";
            ViewBag.ShippedDateSortParm = string.IsNullOrEmpty(sortOrder) ? "shippedDate_desc" : "";
            ViewBag.DeliveredDateSortParm = string.IsNullOrEmpty(sortOrder) ? "DeliveredDate_desc" : "";
            ViewBag.RecipientNameSortParm = string.IsNullOrEmpty(sortOrder) ? "RecipientName_desc" : "";
            ViewBag.OriginSortParm = string.IsNullOrEmpty(sortOrder) ? "Origin_desc" : "";
            ViewBag.DestinationSortParm = string.IsNullOrEmpty(sortOrder) ? "Destination_desc" : "";
            ViewBag.ShippingAccountIdSortParm = string.IsNullOrEmpty(sortOrder) ? "ShippingAccountId_desc" : "";
            switch (sortOrder)
            {
                case "serviceType_desc":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ServiceType);
                    break;
                case "shippedDate_desc":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShippedDate);
                    break;
                case "DeliveredDate_desc":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.DeliveredDate);
                    break;
                case "RecipientName_desc":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.RecipientName);
                    break;
                case "Origin_desc":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ServiceType);
                    break;
                case "Destination_desc":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.Destination);
                    break;
                case "ShippingAccountId_desc":
                    shipmentQuery = shipmentQuery.OrderBy(s => s.ShippingAccountId);
                    break;
                default:
                    shipmentQuery = shipmentQuery.OrderBy(s => s.WaybillId);
                    break;
            }
            shipmentSearch.Shipments = shipmentQuery.ToPagedList(pageNumber, pageSize);
            return View(shipmentSearch);
        }
        public ActionResult getCost(string Origin, string Destination, string ServiceType, string PackageType, string Size, int? weights)
        {
            var cost = new CostViewModel();
            // cost.PackageTypes = (new SelectList(db.PackageTypes.Select(a => a.Type).Distinct())).ToList();
            // cost.ServiceTypes = (new SelectList(db.ServiceTypes.Select(a => a.Type).Distinct())).ToList();
            cost.PackageTypes = db.PackageTypes.Select(a => a.Type).Distinct().ToList();
            cost.ServiceTypes = db.ServiceTypes.Select(a => a.Type).Distinct().ToList();
            cost.Origins = db.Destinations.Select(a => a.City).Distinct().ToList();
            cost.Destinations = db.Destinations.Select(a => a.City).Distinct().ToList();
            cost.Sizes = db.PakageTypeSizes.Select(a => a.size).Distinct().ToList();
            // int serviceTypeID = db.ServiceTypes.SingleOrDefault(s => s.Type == ServiceType).ServiceTypeID;
            // int packageTypeID = db.PackageTypes.SingleOrDefault(s => s.Type == PackageType).PackageTypeID;
            IEnumerable<ServicePackageFee> Fees = db.ServicePackageFees.Include(c => c.PackageType).Include(c => c.ServiceType);
            IEnumerable<PakageTypeSize> Sizes = db.PakageTypeSizes.Include(c => PackageType);
            double minimumFee = 0;
            double Fee = 0;
            foreach (var fee in Fees)
            {
                if (fee.ServiceType.Type == ServiceType && fee.PackageType.Type == PackageType)
                {
                    minimumFee = (double)fee.MinimumFee;
                    Fee = (double)fee.Fee;
                }
            }
            double HKDRate = db.Currencies.SingleOrDefault(s => s.CurrencyCode == "HKD").ExchangeRate;
            double MOPRate = db.Currencies.SingleOrDefault(s => s.CurrencyCode == "MOP").ExchangeRate;
            double TWDRate = db.Currencies.SingleOrDefault(s => s.CurrencyCode == "TWD").ExchangeRate;
            cost.Origin = Origin;
            cost.Destination = Destination;
            cost.ServiceType = ServiceType;
            cost.PackageType = PackageType;
            string weightLimit = "kg";
            foreach (var size in Sizes)
            {
                if (size.PackageType.Type == PackageType && size.size == Size)
                {
                    weightLimit = size.weightLimit;
                }
            }

            if (weights == null)
            {
                weights = 0;
            }
            cost.weights = (int)weights;
            if (ServiceType == null || PackageType == null)
            {
                cost.CNYcost = 0;
                cost.HKDcost = 0;
                cost.MOPcost = 0;
                cost.TWDcost = 0;
                return View(cost);
            }
            if (PackageType == "Envelope")
            {
                cost.CNYcost = Fee;
                cost.HKDcost = (double)ConvertCurrency("HKD", (decimal)Fee);
                cost.MOPcost = (double)ConvertCurrency("MOP", (decimal)Fee); ;
                cost.TWDcost = (double)ConvertCurrency("TWD", (decimal)Fee); ;
            }
            else if (PackageType == "Customer")
            {
                double price = (int)weights * Fee;
                if (price < minimumFee)
                {
                    price = minimumFee;
                }
                cost.CNYcost = price;
                cost.HKDcost = (double)ConvertCurrency("HKD", (decimal)price);
                cost.MOPcost = (double)ConvertCurrency("MOP", (decimal)price);
                cost.TWDcost = (double)ConvertCurrency("TWD", (decimal)price);
            }
            else
            {
                int weightLimitLength = weightLimit.Length - 2;
                if (weightLimitLength < 0)
                {
                    weightLimitLength = 0;
                }
                string weightSubString = weightLimit.Substring(0, weightLimitLength);
                int intWeightLimit = 0;
                if (weightSubString.Length > 0)
                {
                    if (weightLimit == "Not applicable")
                    {
                        intWeightLimit = 2147483647;
                    }
                    else
                    {
                        intWeightLimit = Int32.Parse(weightSubString);
                    }
                }
                else
                {
                    cost.CNYcost = 0;
                    cost.HKDcost = 0;
                    cost.MOPcost = 0;
                    cost.TWDcost = 0;
                    return View(cost);
                }
                double price = (int)weights * Fee;
                if (weights > intWeightLimit)
                {
                    price += 500;
                }
                if (price < minimumFee)
                {
                    price = minimumFee;
                }
                cost.CNYcost = price;
                cost.HKDcost = (double)ConvertCurrency("HKD", (decimal)price);
                cost.MOPcost = (double)ConvertCurrency("MOP", (decimal)price);
                cost.TWDcost = (double)ConvertCurrency("TWD", (decimal)price);
            }
            return View(cost);
        }
        private SelectList PopulateShippingAccountsDropdownList()
        {
            // TODO: Construct the LINQ query to retrieve the unique list of shipping account ids.
            var shippingAccountQuery = db.Shipments.Select(a => a.ShippingAccountId).Distinct().OrderBy(c => c);
            return new SelectList(shippingAccountQuery);
        }
        private SelectList PopulateCustomerShippingAccountsDropdownList()
        {
            // TODO: Construct the LINQ query to retrieve the unique list of shipping account ids.
            var shippingAccountQuery = db.Shipments.Where(a => a.ShippingAccount.UserName == User.Identity.Name).Select(a => a.ShippingAccountId).Distinct().OrderBy(c => c);
            return new SelectList(shippingAccountQuery);
        }
        // GET: Shipments
        public ActionResult Index()
        {
            return View(db.Shipments.ToList());
        }

        // GET: Shipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // GET: Shipments/Create
        public ActionResult Create()
        {
            var shipment = new CreateShipmentViewModel();
            shipment.ServiceTypes = db.ServiceTypes.Select(a => a.Type).Distinct().ToList();
            shipment.PackageTypes = db.PackageTypes.Select(a => a.Type).Distinct().ToList();
            return View(shipment);
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "Shipment.ReferenceNumber,Shipment.Origin,Shipment.Destination,ServiceType,Shipment.IfSendEmail,PackageType,Description,Value,WeightEstimated,Recipient.FullName,Recipient.CompanyName,Recipient.DeliveryAddress,Recipient.EmailAddress,Recipient.PhoneNumber,ShipmentPayer,TaxPayer")] Shipment shipment)
        public ActionResult Create(String ReferenceNumber, string Origin, string Destination, string ServiceType, List<string> PackageType, List<string> Description, List<string> Value, List<string> WeightEstimated, string FullName, string CompanyName, string DeliveryAddress, string EmailAddress, string PhoneNumber, string ShipmentPayerNumber, string taxPayerNumber, string IfSendEmail)
        {
            if (ModelState.IsValid)
            {
                var shipment = new Shipment();

                shipment.ReferenceNumber = ReferenceNumber;
                shipment.ServiceType = ServiceType;
                shipment.RecipientName = FullName;
                shipment.NumberOfPackages = 1;
                shipment.Origin = Origin;
                shipment.Destination = Destination;
                shipment.Status = "pending";

                var recipient = new Recipient();
                recipient.FullName = FullName;
                recipient.CompanyName = CompanyName;
                recipient.DeliveryAddress = DeliveryAddress;
                recipient.EmailAddress = EmailAddress;
                recipient.PhoneNumber = PhoneNumber;
                shipment.Recipient = recipient;

                /* to do: no need to instantiate package? */
                var package = new Package();
                package.Description = Description[0];
                package.WeightEstimated = Convert.ToDouble(WeightEstimated[0]);
                package.Value = Convert.ToDouble(Value[0]);
                string tmp = PackageType[0];
                var packageType = (from s in db.PackageTypes
                                   where s.Type == tmp
                                   select s).First();
                package.PackageTypeID = packageType.PackageTypeID;

                var shippingAccount = (from s in db.ShippingAccounts
                                       where s.UserName == User.Identity.Name
                                       select s).First();
                shipment.ShippingAccountId = shippingAccount.ShippingAccountId;
                package.ShippingAccountNumber = shippingAccount.ShippingAccountNumber;

                shipment.Packages = new List<Package>();
                shipment.Packages.Add(package);

                if (IfSendEmail == "Yes")
                {
                    shipment.IfSendEmail = true;
                }
                else
                {
                    shipment.IfSendEmail = false;
                }

                var shipmentPayer = (from s in db.ShippingAccounts
                                     where s.ShippingAccountNumber == ShipmentPayerNumber
                                     select s).FirstOrDefault();
                var taxPayer = (from s in db.ShippingAccounts
                                where s.ShippingAccountNumber == taxPayerNumber
                                select s).FirstOrDefault();
                if (shipmentPayer == null || taxPayer == null)
                {
                    return RedirectToAction("Index"); /* To do */
                }

                shipment.Invoices = new List<Invoice>();
                var shipmentInvoice = new Invoice();
                shipmentInvoice.Type = "shipment";
                shipmentInvoice.ShippingAccountNumber = ShipmentPayerNumber;
                shipment.Invoices.Add(shipmentInvoice);

                var taxInvoice = new Invoice();
                taxInvoice.Type = "tax_duty";
                taxInvoice.ShippingAccountNumber = taxPayerNumber;
                shipment.Invoices.Add(taxInvoice);

                shipment.ShippedDate = DateTime.Now;
                shipment.DeliveredDate = DateTime.Now;

                /* Todo: maybe delete the default pickup */
                var pickup = new Pickup();
                pickup.Location = "";
                pickup.Date = DateTime.Now;
                pickup.Type = "";

                shipment.Pickup = pickup;


                /* Update the database */
                db.Invoices.Add(taxInvoice);
                db.Invoices.Add(shipmentInvoice);
                db.Packages.Add(package);
                db.Recipients.Add(recipient);

                db.Shipments.Add(shipment);
                db.SaveChanges();

                ViewBag.waybillId = shipment.WaybillId;

                return RedirectToAction("Index");
            }

            return View();
        }
        // GET: Shipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);

            var ret = new CreateShipmentViewModel();
            ret.shipment = shipment;

            ret.ServiceTypes = db.ServiceTypes.Select(a => a.Type).Distinct().ToList();
            ret.PackageTypes = db.PackageTypes.Select(a => a.Type).Distinct().ToList();

            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(ret);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string submit, string ReferenceNumber, string Origin, string Destination, string FullName, string CompanyName, string DeliveryAddress, string EmailAddress, string PhoneNumber, string ServiceType, string IfSendEmail, string ShipmentPayerNumber, string taxPayerNumber, List<string> PackageType, List<string> Description, List<string> Value, List<string> WeightEstimated)
        {
            if (ModelState.IsValid)
            {
                Shipment shipment = db.Shipments.Find(id);
                if (submit == "add")
                {
                    // CreateShipmentViewModel newViewModel = new CreateShipmentViewModel();
                    // newViewModel.ReferenceNumber = ReferenceNumber;
                    // newViewModel.Destination = Destination;
                    shipment.NumberOfPackages += 1;
                    var newPackage = new Package();
                    newPackage.WaybillId = shipment.WaybillId;
                    newPackage.Shipment = shipment;
                    return View(shipment);
                }

                if (IfSendEmail == "Yes")
                {
                    shipment.IfSendEmail = true;
                }
                else
                {
                    shipment.IfSendEmail = false;
                }
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Shipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
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
