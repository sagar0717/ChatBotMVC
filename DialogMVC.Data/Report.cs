using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogMVC.Data
{
    public class Report
    {
        public string Query { get; set; }

        public string Response { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string LastUpdatedBy { get; set; }
    }
}