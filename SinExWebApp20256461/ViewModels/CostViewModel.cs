using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SinExWebApp20256461.Models;

namespace SinExWebApp20256461.ViewModels
{
    public class CostViewModel
    {
        [Key]
        public virtual int CostId { set; get; }
        public virtual string Origin { set; get; }
        public virtual string Destination { set; get; }
        public virtual string Size { set; get; }
        public virtual string ServiceType { set; get; }
        public virtual string PackageType { set; get; }
        public virtual int weights { set; get; }
        public virtual double CNYcost { set; get; }
        public virtual double HKDcost { set; get; }
        public virtual double MOPcost { set; get; }
        public virtual double TWDcost { set; get; }
        public virtual List<string> PackageTypes { set; get; }
        public virtual List<string> ServiceTypes { set; get; }
        public virtual List<string> Origins { set; get; }
        public virtual List<string> Destinations { set; get; }
        public virtual List<string> Sizes { set; get; }
    }
}