using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20256461.ViewModels
{
    public class InvoicesSearchViewModel
    {
        public virtual string ShippingAccountNumber { get; set; }
        public virtual DateTime ShippedStartDate { get; set; }
        public virtual DateTime ShippedEndDate { get; set; }
        public virtual List<SelectListItem> ShippingAccounts { get; set; }
    }
}