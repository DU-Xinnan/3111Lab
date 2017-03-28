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
    }
}