using DialogMVC.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DialogMVC.Business
{
    /// <summary>
    /// This class presents a core domain logic of the chatbot dialog box for retriving data and being responsive
    /// </summary>
    public class SystemRules
    {
        public static List<Rules> Rules;

        /// <summary>
        /// Retrive all rules from Rules table.
        /// </summary>
        public IEnumerable<Rules> AllRules
        {
            get
            {
                using (var context = new UTSDatabaseEntities())
                {
                    var fixRules = (from dr in context.Fixedrules.ToList()
                                    where dr.Status == "Approved"
                                    select new Fixedrule
                                    {
                                        Query = dr.Query,
                                        Response = dr.Response
                                    }).Cast<Rules>().ToList();
                    var dyRules = (from dr in context.DyanamicRules.ToList()
                                   where dr.Status == "Approved"
                                   select new DyanamicRule
                                   {
                                       Query = dr.Query,
                                       Response = dr.Response,
                                       Keyword = dr.Keyword,
                                       Match = dr.Match
                                   }).Cast<Rules>().ToList();
                    Rules.AddRange(fixRules);
                    Rules.AddRange(dyRules);
                    return Rules.ToList();
                }
            }
        }
        public SystemRules()
        {
            Rules = new List<Rules>();
        }

        /// <summary>
        /// Retrives response to a query entered by the user
        /// </summary>
        /// <param name="rules">a reference to SystemRules class to Accesses the Ienum rules from the Rules table</param>
        /// <param name="query">The query enetred by user for finding a response</param>
        /// <returns>Returns a string of response to the query asked by user</returns>
        public string ResponseToQuery(SystemRules rules, string query)
        {
            string xmlfilepath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\DialogMVC.Business\CustomMessage.xml"));
            XElement xelement = XElement.Load(xmlfilepath);
            try
            {
                if (query == "")
                {
                    return (from message in xelement.Elements("Message")
                           where (string)message.Element("key") == "Empty"
                           select message.Element("value").Value).FirstOrDefault();
                }
                else
                {
                    string userInput = Regex.Replace(query, @"[^\w\#\@\$\&\*]", "").ToLower();
                    var matchfixRule = rules.AllRules.OfType<Fixedrule>().ToList().Find(a => (Regex.Replace(a.Query, @"[^\w\#\@\$\&\*]", "").ToLower() == userInput));
                    if (matchfixRule != null)
                    {
                        return matchfixRule.Response;
                    }
                    else
                    {
                        using (var context = new UTSDatabaseEntities())
                        {
                            //string pattern = Regex.Match(userInput.ToLower(), @"(learn)|(week(?=[\w\d]))").Value;
                            string pattern = Regex.Replace(Regex.Match(userInput.ToLower(), @"^(?=.*what)(?=.*week).*$|^(?=.*which)(?=.*week).*$|^(?=.*what)(?=.*topic).*$|^(?=.*when)(?=.*learn).*$").Value, @"(\d+)(?![a-z])", "");
                            var matchDyRule = rules.AllRules.OfType<DyanamicRule>().ToList().
                                Find(a => (pattern.Contains(Regex.Replace
                                (Regex.Replace(a.Query, @"[^\w\#\@\$\&\*]", "").ToLower(), @"(keyword)", ""))));

                            if (matchDyRule != null)
                            {
                                if (matchDyRule.Keyword == "WeekNumber")
                                {
                                    int keywordValue = 0;
                                    bool res = int.TryParse(Regex.Match(userInput, @"(\d+)(?![a-z])").Value, out keywordValue);
                                    string matchValue = (from tr in context.TopicInfo
                                                         where tr.WeekNumber == keywordValue
                                                         select tr.Topic).First();

                                    string testString = matchDyRule.Response;
                                    StringBuilder sb = new StringBuilder(testString);
                                    sb.Replace("[Match]", matchValue);
                                    return sb.ToString();
                                }
                                else if (matchDyRule.Keyword == "Topic")
                                {
                                    string str = (Regex.Replace(Regex.Replace(matchDyRule.Query, @"\[(Keyword)\]", ""), @"[^\w\#\@\$\&\*]", "")).ToLower();
                                    string keywordValue = userInput.Substring(str.Length);
                                    int matchValue = (from tr in context.TopicInfo.AsEnumerable()
                                                      where Regex.Replace(tr.Topic, @"[^\w\#\@\$\&\*]", "").ToLower() == keywordValue
                                                      select tr.WeekNumber).First();

                                    string testString = matchDyRule.Response;
                                    StringBuilder sb = new StringBuilder(testString);
                                    sb.Replace("[Match]", matchValue.ToString());
                                    return sb.ToString();
                                }
                            }
                        }
                    }
                }
                {
                    return (from message in xelement.Elements("Message")
                           where (string)message.Element("key") == "Invalid"
                           select message.Element("value").Value).FirstOrDefault();
                    ;
                }
            }
            //Returns the invalid message from xelement
            catch (InvalidOperationException)
            {
                return (from message in xelement.Elements("Message")
                        where (string)message.Element("key") == "Invalid"
                        select message.Element("value").Value).FirstOrDefault();
            }
            catch (ArgumentNullException)
            {
                return (from message in xelement.Elements("Message")
                        where (string)message.Element("key") == "Empty"
                        select message.Element("value").Value).FirstOrDefault();
            }

            catch (FormatException)
            {
                return "Please enter a week number to search";
            }
        }
        public enum ErrorMessage
        {
            EmptyInput
        }
    }
}
