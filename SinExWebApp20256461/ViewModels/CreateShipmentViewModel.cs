using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

using SinExWebApp20256461.Models;

namespace SinExWebApp20256461.ViewModels
{
    public class CreateShipmentViewModel
    {
        public virtual ShippingAccount ShippingAccount { get; set; }
        public virtual Shipment Shipment { get; set; }
        public virtual Recipient Recipient { get; set; }
        public virtual Pickup Pickup { get; set; }
        public virtual string Nickname { get; set; }
        public virtual string IfSendEmail { get; set; }
        public virtual string ShipmentPayer { get; set; }
        public virtual string TaxPayer { get; set; }
        public virtual IList<Package> Packages { get; set; }
        public virtual List<String> ServiceTypes { get; set; }
        public virtual List<String> PackageTypeSizes { get; set; }
    }
}