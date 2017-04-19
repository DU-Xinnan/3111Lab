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
using X.PagedList;
using Invoicer.Helpers;
using Invoicer.Models;
using Invoicer.Services.Impl;
using Invoicer.Services;

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

        public void SendInvoice(int? WaybillId)
        {
            var shipment = db.Shipments.SingleOrDefault(s => s.WaybillId == WaybillId);
            if (shipment == null)
            {
                return;
            }

            SinExWebApp20256461.Models.Invoice shipmentInvoice = null;
            SinExWebApp20256461.Models.Invoice dutyAndTaxInvoice = null;
            foreach (var invoice in shipment.Invoices)
            {
                if (invoice.Type.Equals("shipment"))
                    shipmentInvoice = invoice;
                else if (invoice.Type.Equals("tax_duty"))
                    dutyAndTaxInvoice = invoice;
            }

            string[] CompanyAddress = { "Sino Express LLC", "HKUST" };

            // Shipment Invoice
            string shipmentPayerShippingAccountNumber = shipmentInvoice.ShippingAccountNumber;
            var shipmentPayerShippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountNumber.Equals(shipmentPayerShippingAccountNumber));
            string shipmentPayerCurrencyCode = db.Destinations.SingleOrDefault(s => s.ProvinceCode.Equals(shipmentPayerShippingAccount.ProvinceCode)).CurrencyCode + " ";

            // Shipment Payer Info
            string[] shipmentPayerInfo = new string[5];
            if (shipmentPayerShippingAccount is BusinessShippingAccount)
            {
                var shipmentPayerBusinessShippingAccount = (BusinessShippingAccount)shipmentPayerShippingAccount;
                shipmentPayerInfo[0] = shipmentPayerBusinessShippingAccount.CompanyName;
            }
            else
            {
                var shipmentPayerPersonalShippingAccount = (PersonalShippingAccount)shipmentPayerShippingAccount;
                shipmentPayerInfo[0] = shipmentPayerPersonalShippingAccount.FirstName + " " + shipmentPayerPersonalShippingAccount.LastName;
            }
            shipmentPayerInfo[1] = "Shipping Account Number: " + shipmentPayerShippingAccount.ShippingAccountNumber;
            shipmentPayerInfo[2] = "Credit Card Type: " + shipmentPayerShippingAccount.CardType;
            string shipmentPayerCreditCardNumber = shipmentPayerShippingAccount.CardNumber;
            shipmentPayerInfo[3] = "Credit Card Number (Last Four Digits): " + shipmentPayerCreditCardNumber.Substring(shipmentPayerCreditCardNumber.Length - 4);
            shipmentPayerInfo[4] = "Authorization Code: " + shipmentInvoice.AuthenticationCode;

            // Shipment Info
            string waybillNumber = WaybillId.ToString().PadLeft(16, '0');
            string[] shipmentInfo = new string[6];
            shipmentInfo[0] = "Waybill Number: " + waybillNumber;
            shipmentInfo[1] = "Shipped Date: " + shipment.ShippedDate.ToString();
            shipmentInfo[2] = "Service Type: " + shipment.ServiceType;
            shipmentInfo[3] = "Sender's Reference Number: " + shipment.ReferenceNumber;
            var senderShippingAccount = shipment.ShippingAccount;
            if (senderShippingAccount is BusinessShippingAccount)
            {
                var senderBusinessShippingAccount = (BusinessShippingAccount)senderShippingAccount;
                shipmentInfo[4] = "Sender Company: " + senderBusinessShippingAccount.CompanyName
                    + " (Contact Person: " + senderBusinessShippingAccount.ContactPersonName + ")";
            }
            else
            {
                var senderPersonalShippingAccount = (PersonalShippingAccount)senderShippingAccount;
                shipmentInfo[4] = "Sender's Full Name: " + senderPersonalShippingAccount.FirstName + " " + senderPersonalShippingAccount.LastName;
            }
            shipmentInfo[5] = "Sender's Address: " + senderShippingAccount.BuildingInformation + ", "
                + senderShippingAccount.StreetInformation + ", "
                + senderShippingAccount.City + ", "
                + senderShippingAccount.ProvinceCode + ", "
                + senderShippingAccount.PostalCode;

            double dutyAndTaxAmount = dutyAndTaxInvoice.TotalAmountPayable;

            bool seperateInvoice = (shipmentInvoice.ShippingAccountNumber != dutyAndTaxInvoice.ShippingAccountNumber);

            if (seperateInvoice)
            {
                // -------------------------------
                // Shipment Invoice
                // -------------------------------
                int i = 0;
                decimal totalShipmentCost = 0;
                List<ItemRow> shipmentItems = new List<ItemRow>();
                foreach (var package in shipment.Packages)
                {
                    i++;
                    string packageInfo = "Package Type: " + package.PackageType.Type
                        + "\nActual Weight: " + package.WeightActual;
                    decimal cost = 100;
                    totalShipmentCost += cost;
                    shipmentItems.Add(ItemRow.Make("Package " + i.ToString(), packageInfo, (decimal)1, 0, (decimal)cost, (decimal)cost));
                }

                new InvoicerApi(SizeOption.A4, OrientationOption.Landscape, shipmentPayerCurrencyCode)
                    .TextColor("#CC0000")
                    .BackColor("#FFD6CC")
                    .Reference(waybillNumber)
                    //.Image(@"company.jpg", 125, 27)
                    .Company(Address.Make("FROM", CompanyAddress))
                    .Client(Address.Make("BILLING TO", shipmentPayerInfo))
                    .Items(shipmentItems)
                    .Totals(new List<TotalRow> {
                        //TotalRow.Make("Sub Total", (decimal)totalShipmentCost),
                        TotalRow.Make("Total", (decimal)totalShipmentCost, true),
                    })
                    .Details(new List<DetailRow> {
                        DetailRow.Make("SHIPMENT INFORMATION", shipmentInfo)
                    })
                    .Save(Server.MapPath("~/Invoices") + "/" + waybillNumber + "_shipment.pdf");

                // Send Email



                // -------------------------------
                // Duty and Tax Invoice
                // -------------------------------
                string dutyAndTaxPayerShippingAccountNumber = dutyAndTaxInvoice.ShippingAccountNumber;
                var dutyAndTaxPayerShippingAccount = db.ShippingAccounts.SingleOrDefault(s => s.ShippingAccountNumber.Equals(dutyAndTaxPayerShippingAccountNumber));
                string dutyAndTaxPayerCurrencyCode = db.Destinations.SingleOrDefault(s => s.ProvinceCode.Equals(dutyAndTaxPayerShippingAccount.ProvinceCode)).CurrencyCode + " ";

                // Duty and Tax Payer Info
                string[] dutyAndTaxPayerInfo = new string[5];
                if (dutyAndTaxPayerShippingAccount is BusinessShippingAccount)
                {
                    var dutyAndTaxPayerBusinessShippingAccount = (BusinessShippingAccount)dutyAndTaxPayerShippingAccount;
                    dutyAndTaxPayerInfo[0] = dutyAndTaxPayerBusinessShippingAccount.CompanyName;
                }
                else
                {
                    var dutyAndTaxPayerPersonalShippingAccount = (PersonalShippingAccount)dutyAndTaxPayerShippingAccount;
                    dutyAndTaxPayerInfo[0] = dutyAndTaxPayerPersonalShippingAccount.FirstName + " " + dutyAndTaxPayerPersonalShippingAccount.LastName;
                }
                dutyAndTaxPayerInfo[1] = "Shipping Account Number: " + dutyAndTaxPayerShippingAccount.ShippingAccountNumber;
                dutyAndTaxPayerInfo[2] = "Credit Card Type: " + dutyAndTaxPayerShippingAccount.CardType;
                string dutyAndTaxPayerCreditCardNumber = dutyAndTaxPayerShippingAccount.CardNumber;
                dutyAndTaxPayerInfo[3] = "Credit Card Number (Last Four Digits): " + dutyAndTaxPayerCreditCardNumber.Substring(dutyAndTaxPayerCreditCardNumber.Length - 4);
                dutyAndTaxPayerInfo[4] = "Authorization Code: " + dutyAndTaxInvoice.AuthenticationCode;

                // Duty and Tax Invoice
                new InvoicerApi(SizeOption.A4, OrientationOption.Landscape, dutyAndTaxPayerCurrencyCode)
                    .TextColor("#CC0000")
                    .BackColor("#FFD6CC")
                    .Reference(waybillNumber)
                    //.Image(@"company.jpg", 125, 27)
                    .Company(Address.Make("FROM", CompanyAddress))
                    .Client(Address.Make("BILLING TO", dutyAndTaxPayerInfo))
                    .Items(new List<ItemRow> {
                        ItemRow.Make("Duty and Tax", "", (decimal)1, 0, (decimal)dutyAndTaxAmount, (decimal)dutyAndTaxAmount)
                    })
                    .Totals(new List<TotalRow> {
                        //TotalRow.Make("Sub Total", (decimal)dutyAndTaxAmount),
                        TotalRow.Make("Total", (decimal)dutyAndTaxAmount, true),
                    })
                    .Details(new List<DetailRow> {
                        DetailRow.Make("SHIPMENT INFORMATION", shipmentInfo)
                    })
                    .Save(Server.MapPath("~/Invoices") + "/" + waybillNumber + "_duty_and_tax.pdf");

                // Send Email


            }
            else
            {
                // -----------------------------------
                // Shipment and Duty and Tax Invoice
                // -----------------------------------
                int i = 0;
                decimal totalShipmentCost = 0;
                List<ItemRow> shipmentItems = new List<ItemRow>();
                foreach (var package in shipment.Packages)
                {
                    i++;
                    string packageInfo = "Package Type: " + package.PackageType.Type
                        + "\nActual Weight: " + package.WeightActual;
                    decimal cost = 100;
                    totalShipmentCost += cost;
                    shipmentItems.Add(ItemRow.Make("Package " + i.ToString(), packageInfo, (decimal)1, 0, (decimal)cost, (decimal)cost));
                }
                shipmentItems.Add(ItemRow.Make("Duty and Tax", "", (decimal)1, 0, (decimal)dutyAndTaxAmount, (decimal)dutyAndTaxAmount));

                new InvoicerApi(SizeOption.A4, OrientationOption.Landscape, shipmentPayerCurrencyCode)
                    .TextColor("#CC0000")
                    .BackColor("#FFD6CC")
                    .Reference(waybillNumber)
                    //.Image(@"company.jpg", 125, 27)
                    .Company(Address.Make("FROM", CompanyAddress))
                    .Client(Address.Make("BILLING TO", shipmentPayerInfo))
                    .Items(shipmentItems)
                    .Totals(new List<TotalRow> {
                        //TotalRow.Make("Sub Total", (decimal)dutyAndTaxAmount),
                        TotalRow.Make("Total", (decimal)(totalShipmentCost + (decimal)dutyAndTaxAmount), true),
                    })
                    .Details(new List<DetailRow> {
                        DetailRow.Make("SHIPMENT INFORMATION", shipmentInfo)
                    })
                    .Save(Server.MapPath("~/Invoices") + "/" + waybillNumber + "_total.pdf");

                // Send Email


            }
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
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WaybillId,ReferenceNumber,ServiceType,ShippedDate,DeliveredDate,RecipientName,NumberOfPackages,Origin,Destination,Status,ShippingAccountId")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Shipments.Add(shipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipment);
        }

        // GET: Shipments/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WaybillId,ReferenceNumber,ServiceType,ShippedDate,DeliveredDate,RecipientName,NumberOfPackages,Origin,Destination,Status,ShippingAccountId")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipment);
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
