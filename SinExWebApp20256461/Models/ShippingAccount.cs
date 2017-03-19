using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    [Table("ShippingAccount")]
    public abstract class ShippingAccount
    {
        // TODO: how to auto generate 12-digit shipping account
        public virtual int ShippingAccountId { get; set; }
        public virtual string ShippingAccountNumber { get; set; }
        [StringLength(10)]
        public string UserName { get; set; }
        public virtual ICollection<Shipment> Shipments { get;set; }
        // for mailing address----------------------------
        [StringLength(50)]
        public virtual string BuildingInformation { set; get; }
        [Required]
        [StringLength(35)]
        public virtual string StreetInformation { set; get; }
        [Required]
        [StringLength(25)]
        public virtual string City { set; get; }
        [Required]
        [StringLength(2, MinimumLength =2)]
        [RegularExpression(@"^BJ|JL|HN|SC|CQ|JX|QH|GD|GZ|HI|NM|ZJ|HL|AH|NM|HK|NM|SD|XJ|YN|GS|XZ|MC|JX|JS|JX|HL|SH|LN|HE|TW|SX|HE|XJ|HB|SN|QH|NX|GS|HA$",
            ErrorMessage = "Please input valid Code")]
        public virtual string ProvinceCode { set; get; }
        [StringLength(6, MinimumLength = 5)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please input number between 5-6 digits")]
        public virtual string PostalCode { get; set; }
        // --------mailing address over -------------

        //--- credit card information ---------------
        [Required]
        [RegularExpression(@"^American Express|Diners Club|Discover|MasterCard|UnionPay|Visa$", ErrorMessage ="Please input valid cardType")]
        public virtual string CardType { get; set; }
        [Required]
       //  [StringLength(19, MinimumLength = 14)]
        [RegularExpression(@"^[0-9]{14,19}$", ErrorMessage = "Please input number between 14-19 digits")]
        public virtual string CardNumber { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "security # should be number and between 3-4 digits")]
        public virtual string SecurityNumber { get; set; }
        [Required]
        [StringLength(70)]
        public virtual string CardHolderName { get; set; }

        [Required]
        [RegularExpression(@"^(0?[1-9]|1[012])$", ErrorMessage = "Please input valid month")]
        public virtual string Month { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage ="Please input valid year")]
        public virtual string Year { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 8)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must contsains 8-14 digits.")]
        public virtual string PhoneNumber { get; set; }
        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$", ErrorMessage = "Please enter a valid email")]
        public virtual string EmailAddress { get; set; }
    }
}