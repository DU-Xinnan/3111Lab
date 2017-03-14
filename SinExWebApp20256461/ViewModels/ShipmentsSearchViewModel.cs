using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SinExWebApp20256461.ViewModels
{
    public class ShipmentsSearchViewModel
    {
        public virtual int ShippingAccountId { get; set; }
        public virtual DateTime ShippedStartDate { get; set; }
        public virtual DateTime ShippedEndDate { get; set; }
        public virtual List<SelectListItem> ShippingAccounts { get; set; }
    }
}