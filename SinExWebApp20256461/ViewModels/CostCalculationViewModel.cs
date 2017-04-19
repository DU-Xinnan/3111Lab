using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.ViewModels
{
    public class CostCalculationViewModel
    {
        public virtual IList<string> OriginList { get; set; }
        public virtual IList<string> DestinationList { get; set; }
        public virtual IList<string> ServiceTypesList { get; set; }
        public virtual IList<string> PackageTypesSizeList { get; set; }
        public virtual string Origin { get; set; }
        public virtual string Destination { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual IList<string> PackagesTypeSizes { get; set; }
        public virtual IList<string> Weights { get; set; }
        public virtual int NumOfPackages { get; set; }
    }
}