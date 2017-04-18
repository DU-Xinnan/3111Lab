using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    public class Invoice
    {
        public virtual int InvoiceID {get;set;}
        public virtual string AuthenticationCode { get; set; }
        public virtual string Type { get; set; }

        //on the diagram, says Account Number
        // public virtual int ShippingAccountId { get; set; }
        // public virtual ShippingAccount ShippingAccount { get; set; }
        public virtual string ShippingAccountNumber { get; set; }
        public virtual double TotalAmountPayable { get; set; }

        public virtual int WaybillId { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}