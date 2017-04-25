using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SinExWebApp20256461.Models;
namespace SinExWebApp20256461.ViewModels
{
    public class TrackingViewModel
    {
        public virtual Shipment Shipment { set; get; }
        public virtual string WaybillNumber { set; get; }
        public virtual IEnumerable<Tracking> Trackings { set; get; }
    }
}