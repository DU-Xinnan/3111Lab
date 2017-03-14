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
        public virtual int ShippingAccountID { get; set; }
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
        [StringLength(2)]
        public virtual string ProvinceCode { set; get; }
        [StringLength(6, MinimumLength = 5)]
        public virtual string PostalCode { get; set; }
        // --------mailing address over -------------

        //--- credit card information ---------------
        [Required]
        public virtual string CardType { get; set; }
        [Required]
        [StringLength(19, MinimumLength = 14)]
        // [RegularExpression(@"^[0,9]*$", ErrorMessage = "It must be a number")]
        public virtual string CardNumber { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3)]
        // [RegularExpression(@"^[0,9]*$", ErrorMessage = "It must be a number")]
        public virtual string SecurityNumber { get; set; }
        [Required]
        [StringLength(70)]
        public virtual string CardHolderName { get; set; }

        [Required]
        // [RegularExpression(@"^[0,9]*$", ErrorMessage = "It must be a number")]
        public virtual string Month { get; set; }

        [Required]
        // [RegularExpression(@"^[0,9]*$", ErrorMessage ="It must be a number")]
        public virtual string Year { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 8)]
        // [RegularExpression(@"^[0,9]*$", ErrorMessage = "It must be a number")]
        public virtual string PhoneNumber { get; set; }
        [Required]
        [StringLength(30)]
        // [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email")]
        public virtual string EmailAddress { get; set; }
    }
}