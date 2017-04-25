using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
namespace SinExWebApp20256461.ViewModels
{
    public class InvoicesReportViewModel
    {
        public virtual InvoicesSearchViewModel Invoice { get; set; }
        public virtual IPagedList<InvoicesListViewModel> Invoices { get; set; }
    }
}