using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    [Table("PackageType")]
    public class PackageType
    {
        public virtual int PackageTypeID { get; set; }
        public virtual string Type { get; set; }
        public virtual string Description { get; set; }
        public virtual int PakageTypeSizeID { get; set; }
        public virtual ICollection<PakageTypeSize> PackageTypeSize { get; set; }
        public virtual ICollection<ServicePackageFee> ServicePackageFees { get; set; }
    }
}