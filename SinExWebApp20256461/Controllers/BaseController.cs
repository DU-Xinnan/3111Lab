using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SinExWebApp20256461.Models;

namespace SinExWebApp20256461.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        private SinExWebApp20256461Context db = new SinExWebApp20256461Context();

        public decimal ConvertCurrency(string currency, decimal value)
        {
            if (Session[currency] == null)
            {
                Session[currency] = db.Currencies.SingleOrDefault(s => s.CurrencyCode == currency).ExchangeRate;

            }
            return decimal.Parse(Session[currency].ToString()) * value;
        }
        public Dictionary<string, decimal> Calculate(string ServiceType, string PackageTypeSize, decimal weight)
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
                decimal actualWeight = Math.Round(weight, 1);
                Price = Fee * actualWeight;
                if (CostPackageTypeSize.PackageType.Type != "Tube" && CostPackageTypeSize.PackageType.Type != "Customer")
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
    }
}