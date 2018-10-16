using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCDemoSite1.Models
{
    public class DataModel
    {
        public List<ReportItem> Report { get; set; }
    }

    public class ReportItem
    {
        public string FirstName { get; set; }

        public string Gender { get; set; }

        public string Qoute { get; set; }

        public string Discount { get; set; }
    }
}