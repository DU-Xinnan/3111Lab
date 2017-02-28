using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    public class BusinessShippingAccount: ShippingAccount
    {
        [Required]
        [StringLength(70)]
        public virtual string ContactPersonName { set; get; }
        [Required]
        [StringLength(40)]
        public virtual string CompanyName { set; get; }
        [Required]
        [StringLength(30)]
        public virtual string DepartmentName { set; get; }
    }
}