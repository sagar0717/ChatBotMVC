using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogMVC.Data
{
    public class StatusCount
    {
        public string StatusName { get; set; }

        public int Count { get; set; }

        public double PercentageSuccess { get; set; }

        public double AverageSuccess { get; set; }

        public double Createdby { get; set; }
    }
}