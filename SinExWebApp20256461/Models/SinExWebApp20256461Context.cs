using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SinExWebApp20256461.Models
{
    public class SinExWebApp20256461Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<SinExWebApp20256461Context>(null);
        //    base.OnModelCreating(modelBuilder);
        //}
        public SinExWebApp20256461Context() : base("name=SinExWebApp20256461Context")
        {
        }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.PackageType> PackageTypes { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.ServicePackageFee> ServicePackageFees { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.ServiceType> ServiceTypes { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.Destination> Destinations { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.Currency> Currencies { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.PakageTypeSize> PakageTypeSizes { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.Shipment> Shipments { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.ShippingAccount> ShippingAccounts { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.PersonalShippingAccount> PersonalShippingAccounts { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.Models.BusinessShippingAccount> BusinessShippingAccounts { get; set; }

        public System.Data.Entity.DbSet<SinExWebApp20256461.ViewModels.CostViewModel> CostViewModels { get; set; }
    }
}
