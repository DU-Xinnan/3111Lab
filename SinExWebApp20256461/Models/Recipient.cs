using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    public class Recipient
    {
        public virtual int RecipientID { get; set; }
        public virtual string FullName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string DepartmentName { get; set; }

        public virtual string DeliveryAddress { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string EmailAddress { get; set; }
        // Princeple end Problem
        // public virtual int WaybillId { get; set; }
        // public virtual Shipment Shipment { get; set; }
    }
}