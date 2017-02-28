using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    [Table("PakageTypeSize")]
    public class PakageTypeSize
    {
        public virtual int PakageTypeSizeID { get; set; }
        public virtual string size { get; set; }
        public virtual string weightLimit { get; set; }

        public virtual int PackageTypeID { get; set; }
        public virtual PackageType PackageType { get; set; }
    }
}