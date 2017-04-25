using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    [Table("Shipment")]
    public class Shipment
    {
        [Key]
        public virtual int WaybillId { get; set; }
        public virtual string WaybillNumber { get; set; }
        public virtual string ReferenceNumber { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual DateTime ShippedDate { get; set; }
        public virtual DateTime DeliveredDate { get; set; }
        public virtual string RecipientName { get; set; } // what is dis ???
        public virtual int NumberOfPackages { get; set; }
        public virtual string Origin { get; set; } // why str ???
        public virtual string Destination { get; set; } // why str ???
        public virtual string Status { get; set; }
        public virtual int ShippingAccountId { get; set; }
        public virtual ShippingAccount ShippingAccount { get; set; }
        public virtual int RecipientID { get; set; }
        public virtual Recipient Recipient { get; set; }
        //417
        public virtual bool IfSendEmail { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Package> Packages { get; set; }
        public virtual int PickupID { get; set; }
        public virtual Pickup Pickup { get; set; }
    }
}