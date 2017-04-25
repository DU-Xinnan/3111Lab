using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    public class Package
    {
        [key]
        public virtual int PackageID { get; set; }
        public virtual  int WaybillId { get; set; }
        public virtual Shipment Shipment { get; set; }
        public virtual string ShippingAccountNumber { get; set; }
        public virtual string Description { get; set; }
        public virtual double Value { get; set; }
        public virtual double WeightEstimated { get; set; }
        public virtual double WeightActual  { get; set; }
        //public virtual int PackageTypeID { get; set; }
        //public virtual PackageType PackageType  { get; set; }
        public virtual string PackageTypeSize { get; set; }    //for easy creation
        //public virtual PakageTypeSize PackageTypeSize { get; set; }
    }
}