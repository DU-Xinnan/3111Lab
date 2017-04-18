using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    public class Pickup
    {
        public virtual int PickupID { get; set; }
        public virtual string Location { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Type { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}