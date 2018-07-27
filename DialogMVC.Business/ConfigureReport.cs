using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DialogMVC.Data;

namespace DialogMVC.Business
{
    public class ConfigureReport
    {
        private Configure configure = new DialogMVC.Business.Configure();
        //List of Approved Rules from Fixed and Dynamic Rules
        public IEnumerable<Report> GetIndRules(string loggedInUser)
        {
            List<Report> reports = new List<Report>();
            using (var context = new UTSDatabaseEntities())
            {
                var fixRules = configure.LoadFixedrules.Where(x => x.LastUpdatedby == loggedInUser).Select(s => new Report
                {
                    Query = s.Query,
                    Response = s.Response,
                    Type = s.Type,
                    Status = s.Status

                }).ToList();

                var dyRules = configure.LoadDynamicRules.Where(x => x.LastUpdatedby == loggedInUser).Select(s => new Report
                {
                    Query = s.Query,
                    Response = s.Response,
                    Type = s.Type,
                    Status = s.Status

                }).ToList();

                return fixRules.Concat(dyRules);
            }
        }

        public IEnumerable<Report> GetAllRules()
        {
            List<Report> reports = new List<Report>();
            using (var context = new UTSDatabaseEntities())
            {
                var fixRules = configure.LoadFixedrules.Select(s => new Report
                {
                    Query = s.Query,
                    Response = s.Response,
                    Type = s.Type,
                    Status = s.Status,
                    LastUpdatedBy = s.LastUpdatedby

                }).ToList();

                var dyRules = configure.LoadDynamicRules.Select(s => new Report
                {
                    Query = s.Query,
                    Response = s.Response,
                    Type = s.Type,
                    Status = s.Status,
                    LastUpdatedBy = s.LastUpdatedby

                }).ToList();

                return fixRules.Concat(dyRules);
            }
        }

        public IEnumerable<SystemReport> GetSystemReportData()
        {
            var allRules = GetAllRules();
            var reportDat = from rule in allRules
                            group rule by new
                            {
                                rule.LastUpdatedBy,
                                rule.Status
                            }
                           into rulegroup
                            select new
                            {
                                Editor = rulegroup.Key.LastUpdatedBy,
                                Status = rulegroup.Key.Status,
                                Count = rulegroup.Count()
                            };

            var result = reportDat.GroupBy(cc => cc.Editor).
               Select(dd => new SystemReport
               {
                   Editor = dd.Key,
                   ApprovedRules = dd.Where(ee => ee.Status == RulesStatus.Approved.ToString()).Select(ee => ee.Count).FirstOrDefault(),
                   RejectedRules = dd.Where(ee => ee.Status == RulesStatus.Rejected.ToString()).Select(ee => ee.Count).FirstOrDefault(),
                   TotalRules = dd.Where(ee => ee.Status == RulesStatus.Created.ToString() || ee.Status == RulesStatus.Updated.ToString()).Select(ee => ee.Count).FirstOrDefault()
               });

            return result;
        }




        /*public List<StatusCount> GetIndividualStatusCount()
        {
            using (var context = new UTSDatabaseEntities())
            {

                List<StatusCount> statusCounts = new List<StatusCount>();

                //Approved + Rejected Lists for Individual Report
                var IndividualReportList = GetApproved().Concat(GetRejected()).ToList();

                //Status Count for Approved + Rejected Lists for Individual Report
                var IndividualReportCount = IndividualReportList.GroupBy(a => a.Status).Select(b => new { StatusName = b.Key, Count = b.Count() }).ToList();

                //Success Percentage for Individual Report
                var IndividualSuccess = ConfigureReport.Converter.Calculate((double)int.Parse(GetApproved().Count().ToString()) / (int.Parse(GetApproved().Count().ToString()) + int.Parse(GetRejected().Count().ToString())));

                //List to display Status Count for Individual Report
                List<StatusCount> individualcountlist = new List<StatusCount>();

                for (int i = 0; i < IndividualReportCount.ToList().Count(); i++)
                {
                    individualcountlist.Add(new StatusCount { StatusName = IndividualReportCount[i].StatusName, Count = IndividualReportCount[i].Count, PercentageSuccess = IndividualSuccess });
                }
                return individualcountlist.ToList();
            }
        }

        //List containing the counts of status of Fixed and Dynamic Rules for Overall Report
        public List<StatusCount> GetOverallStatusCount()
        {
            List<StatusCount> statusCounts = new List<StatusCount>();

            //Approved + Rejected + Remaining Lists for Overall Report
            var OverallReportList = GetApproved().Concat(GetRejected()).Concat(GetCreated()).Concat(GetUpdated()).ToList();

            //Status Count for Approved + Rejected + Remaining Lists for Overall Report
            var OverallReportCount = OverallReportList.GroupBy(a => a.Status).Select(b => new { StatusName = b.Key, Count = b.Count() }).ToList();

            //Percentage Success for Overall Report
            var OverallSuccess = ConfigureReport.Converter.Calculate((double)int.Parse(GetApproved().Count().ToString()) / (int.Parse(GetApproved().Count().ToString()) + int.Parse(GetRejected().Count().ToString())));

            //Average Success for Overall Report
            var OverallAverageSuccess = ConfigureReport.Converter.Calculate((double)int.Parse(GetApproved().Count().ToString()) / (int.Parse(GetApproved().Count().ToString()) + int.Parse(GetRejected().Count().ToString()) + int.Parse(GetCreated().Count().ToString()) + int.Parse(GetUpdated().Count().ToString())));

            //List to display Status Count for Individual Report
            List<StatusCount> overallcountlist = new List<StatusCount>();

            for (int i = 0; i < OverallReportCount.ToList().Count(); i++)
            {
                overallcountlist.Add(new StatusCount { StatusName = OverallReportCount[i].StatusName, Count = OverallReportCount[i].Count, PercentageSuccess = OverallSuccess, AverageSuccess = OverallAverageSuccess });
            }
            return overallcountlist.ToList();
        }
        public static class Converter
        {
            /// <summary>
            /// Number converter to percentage.
            /// </summary>
            /// <param name="num">Number in double format.</param>
            /// <param name="ToPercent">Convert to percentage. Default is false.</param>
            public static double Calculate(double num)
            {
                double calc = num * 100;
                return calc;
            }
        }*/
    }
}