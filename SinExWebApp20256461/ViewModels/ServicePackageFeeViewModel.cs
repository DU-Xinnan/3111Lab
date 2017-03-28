using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SinExWebApp20256461.Models;
namespace SinExWebApp20256461.ViewModels
{
    public class ServicePackageFeeViewModel
    {
        public virtual int ServicePackageFeeID { get; set; }
        public virtual decimal Fee { get; set; }
        public virtual decimal HKDFee { get; set; }
        public virtual decimal MOPFee { get; set; }
        public virtual decimal TYDFee { get; set; }
        public virtual decimal MinimumFee { get; set; }

        public virtual int PackageTypeID { get; set; }
        public virtual int ServiceTypeID { get; set; }
        public virtual PackageType PackageType { get; set; }
        public virtual ServiceType ServiceType { get; set; }
    }
}