using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

using SinExWebApp20256461.Models;

namespace SinExWebApp20256461.ViewModels
{
    public class CreateShipmentViewModel
    {
        public virtual ShippingAccount ShippingAccount { get; set; }
        public virtual Shipment shipment { get; set; }
        public virtual string ReferenceNumber { get; set; }

        [StringLength(2, MinimumLength = 2)]
        [RegularExpression(@"^BJ|JL|HN|SC|CQ|JX|QH|GD|GZ|HI|NM|ZJ|HL|AH|NM|HK|NM|SD|XJ|YN|GS|XZ|MC|JX|JS|JX|HL|SH|LN|HE|TW|SX|HE|XJ|HB|SN|QH|NX|GS|HA$",
    ErrorMessage = "Please input valid Code")]
        public virtual string Origin { get; set; }

        [StringLength(2, MinimumLength = 2)]
        [RegularExpression(@"^BJ|JL|HN|SC|CQ|JX|QH|GD|GZ|HI|NM|ZJ|HL|AH|NM|HK|NM|SD|XJ|YN|GS|XZ|MC|JX|JS|JX|HL|SH|LN|HE|TW|SX|HE|XJ|HB|SN|QH|NX|GS|HA$",
ErrorMessage = "Please input valid Code")]
        public virtual string Destination { get; set; }
 
        public virtual string IfSendEmail { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual string RecipientCompany { get; set; }
        public virtual string DeliveryAddress { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual Pickup Pickup { get; set; }
        public virtual String ServiceType { get; set; }
        public virtual List<String> PackageType { get; set; }
        public virtual List<String> Description { get; set; }
        public virtual List<String> Value { get; set; }
        public virtual List<String> WeightEstimated { get; set; }
        public virtual String ShipmentPayerNumber { get; set; }
        public virtual String taxPayerNumber { get; set; }
        public virtual List<String> ServiceTypes { get; set; }
        public virtual List<String> PackageTypes { get; set; }
    }
}