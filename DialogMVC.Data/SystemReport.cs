using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogMVC.Data
{
    public class SystemReport
    {
        public string Editor { get; set; }
        public string Status { get; set; }
        public double ApprovedRules { get; set; }
        public double RejectedRules { get; set; }
        public double TotalRules { get; set; }
        public double Percentage { get; set; }
        public double SucessRate { get; set; }
    }
}
