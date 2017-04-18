using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    [Table("Destination")]
    public class Destination
    {
        public virtual int DestinationID { get; set; }
        public virtual string City { get; set; }
        public virtual string ProvinceCode { get; set; }
        public virtual string CurrencyCode { get; set; }
        public virtual Currency Currency { get; set; }
    }
}