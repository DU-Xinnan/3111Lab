using System;
using System.Collections.Generic;
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
        public virtual String ServiceType { get; set; }
        public virtual String PackageType { get; set; }
        public virtual String Description { get; set; }
        public virtual String Value { get; set; }
        public virtual String WeightEstimated { get; set; }
        public virtual String ShipmentPayerNumber { get; set; }
        public virtual String taxPayerNumber { get; set; }
        public virtual List<String> ServiceTypes { get; set; }
        public virtual List<String> PackageTypes { get; set; }
    }
}