using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    [Table("ServicePackageFee")]
    public class ServicePackageFee
    {
        public virtual int ServicePackageFeeID { get; set; }
        public virtual decimal Fee { get; set; }
        public virtual decimal MinimumFee { get; set; }

        public virtual int PackageTypeID { get; set; }
        public virtual int ServiceTypeID { get; set; }
        public virtual PackageType PackageType { get; set; }
        public virtual ServiceType ServiceType { get; set; }

    }
}