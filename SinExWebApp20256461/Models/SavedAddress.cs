using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    public class SavedAddress
    {
        public virtual int SavedAddressID { get; set; }
        public virtual string NickName { get; set; }
        public virtual string Address { get; set; }
        public virtual string Type { get; set; }
        public virtual int ShippingAccountId { get; set; }
        public virtual ShippingAccount ShippingAccount { get; set; }

    }
}