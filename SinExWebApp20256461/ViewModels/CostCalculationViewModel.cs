using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SinExWebApp20256461.ViewModels
{
    public class CostCalculationViewModel
    {
        public virtual IList<string> OriginList { get; set; }
        public virtual IList<string> DestinationList { get; set; }
        public virtual IList<string> ServiceTypesList { get; set; }
        public virtual IList<string> PackageTypesSizeList { get; set; }
        [Required]
        public virtual string Origin { get; set; }
        [Required]
        public virtual string Destination { get; set; }
        [Required]
        public virtual string ServiceType { get; set; }
        public virtual IList<string> PackagesTypeSizes { get; set; }
        public virtual IList<decimal> Weights { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Please enter valid integer Number")]
        public virtual int NumOfPackages { get; set; }
    }
}