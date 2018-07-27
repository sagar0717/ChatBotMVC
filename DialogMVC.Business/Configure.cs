using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DialogMVC.Data;


namespace DialogMVC.Business
{

    /// <summary>
    /// This class represents the core domain logic of the configuration of fixed rules and dynamic rules.
    /// This class provides business logic to perform all types of CRUD activities for both fixed rules and dynamic rules.
    /// </summary>
    public class Configure : Rules
    {


        /// <summary>
        /// This funtion represts the logic to save a dynamic rule which is requested by the user.
        /// </summary>
        /// <param name="dynamic"> Is a refernece of Dynamicrule which helps to access th values stored in the variables of dynamic rule and
        /// stores the value entered by a user and other values.</param>
        /// <param name="loggedinUser">The session email of the logged in user (user.identity.name)</param>
        /// <returns>Returns true when the rule is saved and false when the rule is not saved or already exsits.</returns>
        public bool SaveDynamicRule(DyanamicRule dynamic, string loggedinUser)
        {
            using (var usersDbContext = new UsersEntities())
            {
                string userid = (from row in usersDbContext.AspNetUsers
                                 where row.UserName == loggedinUser
                                 select row.Id).First();

                bool isSaved = false;
                if (dynamic.Query == "" && dynamic.Response == "")
                    return isSaved;
                else
                {
                    using (var context = new UTSDatabaseEntities())
                    {
                        var query = (from row in context.DyanamicRules.ToList()
                                     where row.Query == dynamic.Query
                                     select row.Query).FirstOrDefault();
                        //Checking if the query already exsist
                        if (query == dynamic.Query)
                        {
                            return isSaved;
                        }
                        dynamic.Type = "Dynamic";
                        dynamic.Createdby = loggedinUser;
                        dynamic.LastUpdatedby = loggedinUser;
                        dynamic.UserId = userid;
                        dynamic.Status = RulesStatus.Created.ToString();
                        context.DyanamicRules.Add(dynamic);
                        context.SaveChanges();
                        isSaved = true;

                    }
                }
                return isSaved;
            }
        }
        /// <summary>
        /// This funtion represts the logic to save a fixed rule which is requested by the user.
        /// </summary>
        /// <param name="fixrule"> Is a refernece of Fixedrule which helps to access th values stored in the variables of fixed rule and
        /// stores the value entered by a user and other values.</param>
        /// <param name="loggedinUser">The session email of the logged in user (user.identity.name)</param>
        /// <returns>Returns true when the rule is saved and false when the rule is not saved or already exsits</returns>
        public bool SaveFixedRule(Fixedrule fixrule, string loggedinUser)
        {
            using (var usersDbContext = new UsersEntities())
            {
                string userid = (from row in usersDbContext.AspNetUsers
                                 where row.UserName == loggedinUser
                                 select row.Id).First();

                bool isSaved = false;
                if (fixrule.Query == "" && fixrule.Response == "")
                    return isSaved;
                else if (fixrule.Query == null && fixrule.Response == null)
                    return isSaved;
                else
                {
                    using (var context = new UTSDatabaseEntities())
                    {
                        var query = (from row in context.Fixedrules
                                     where row.Query == fixrule.Query
                                     select row.Query).FirstOrDefault();
                        //checking if the rule already exsists
                        if (query == fixrule.Query)
                        {
                            return isSaved;

                        }
                        fixrule.Type = "Fixed";
                        fixrule.Createdby = loggedinUser;
                        fixrule.LastUpdatedby = loggedinUser;
                        fixrule.UserId = userid;
                        fixrule.Status = RulesStatus.Created.ToString();
                        context.Fixedrules.Add(fixrule);
                        context.SaveChanges();
                        isSaved = true;
                        return isSaved;
                    }
                }
            }
        }

        /// <summary>
        /// This methods searches the fixed rule in db by its status (approved, rejected, created, updated)
        /// </summary>
        /// <param name="rulestatus">This parameter accepts the status of which the rules are to be queried.</param>
        /// <returns>return fixed rules searched by status</returns>
        public List<Fixedrule> SearchFxRuleByStatus(string rulestatus)
        {
            using (var context = new UTSDatabaseEntities())
            {
                var filterRules = (from row in context.Fixedrules
                                   where row.Status == rulestatus
                                   select row).ToList();
                return filterRules;
            }
        }
        /// <summary>
        /// This methods searches the dynamic rule in db by its status (approved, rejected, created, updated)
        /// </summary>
        /// <param name="rulestatus">This parameter accepts the status of which the rules are to be queried.</param>
        /// <returns>return dynamic rules searched by status</returns>
        public List<DyanamicRule> SearchDyRuleByStatus(string rulestatus)
        {
            using (var context = new UTSDatabaseEntities())
            {
                var filterRules = (from row in context.DyanamicRules
                                   where row.Status == rulestatus
                                   select row).ToList();
                return filterRules;
            }
        }

        /// <summary>
        /// This funtion represts the logic to edit an exsiting dynamic rule which is requested by the user.
        /// </summary>
        /// <param name="dynamic"> Is a refernece of Dynamicrule which helps to access the values stored in the variables of dynamic rule and
        ///                        stores the value entered by a user and other values.</param>
        /// <param name="loggedinUser">The session email of the logged in user (user.identity.name)</param>
        /// <returns>true if the rule is editted and flase when the rule can not be editted</returns>
        public bool EditDynamicRule(DyanamicRule dynamic, string loggedinUser)
        {
            using (var usersDbContext = new UsersEntities())
            {
                string userid = (from row in usersDbContext.AspNetUsers
                                 where row.UserName == loggedinUser
                                 select row.Id).First();

                bool isSaved = false;
                if (dynamic.Query == "" && dynamic.Response == "")
                    return isSaved;
                else if (dynamic.Query == null && dynamic.Response == null)
                    return isSaved;
                else
                {
                    using (var context = new UTSDatabaseEntities())
                    {
                        var query_search = (from row in context.DyanamicRules.ToList()
                                     where row.Query == dynamic.Query
                                     select row.Query).FirstOrDefault();
                        if (query_search.ToLower() == dynamic.Query.ToLower())
                        {
                            return isSaved;
                        }
                        else
                        {
                            var stored = context.DyanamicRules.Find(dynamic.RuleId);
                            var oldvalue = stored;
                            stored.Status = RulesStatus.Updated.ToString();
                            stored.Query = dynamic.Query;
                            stored.Response = dynamic.Response;
                            stored.Keyword = dynamic.Keyword;
                            stored.Match = dynamic.Match;
                            stored.LastUpdatedby = loggedinUser;
                            context.Entry(oldvalue).CurrentValues.SetValues(stored);
                            context.SaveChanges();
                            isSaved = true;
                            return isSaved;

                        }
                    }
                }
               
            }
        }

        /// <summary>
        /// This funtion represts the logic to edit an exsiting fixed rule which is requested by the user.
        /// </summary>
        /// <param name="fixrule"> Is a refernece of Fixedrule which helps to access the values stored in the variables of fixed rule and
        ///                        stores the value entered by a user and other values.</param>
        /// <param name="loggedinUser">The session email of the logged in user (user.identity.name)</param>
        /// <returns>true if the rule is editted and flase when the rule can not be editted</returns>
        public bool EditFixedRule(Fixedrule fixrule, string loggedinUser)
        {
            using (var usersDbContext = new UsersEntities())
            {
                bool isSaved = false;
                string userid = (from row in usersDbContext.AspNetUsers
                                 where row.UserName == loggedinUser
                                 select row.Id).First();
            
                if (fixrule.Query == "" && fixrule.Response == "")
                    return isSaved;
               /* else if (fixrule.Query == null && fixrule.Response == null)
                    return isSaved;*/
                else
                {
                    using (var context = new UTSDatabaseEntities())
                    {
                        var query_search = (from row in context.Fixedrules
                                     where row.Query == fixrule.Query
                                     select row.Query).FirstOrDefault();
                       

                        if (query_search.ToLower() == fixrule.Query.ToLower() )
                        {
                            return isSaved;

                        }
                        else
                        {
                            var stored = context.Fixedrules.Find(fixrule.RuleId);
                            var oldvalue = stored;
                            stored.Status = RulesStatus.Updated.ToString();
                            stored.Query = fixrule.Query;
                            stored.Response = fixrule.Response;
                            stored.LastUpdatedby = loggedinUser;
                            context.Entry(oldvalue).CurrentValues.SetValues(stored);
                            context.SaveChanges();
                            isSaved = true;
                            return isSaved;
                        }
                       
                    }
                }    
            }
        }

        /// <summary>
        /// This fnuction approves or rejects fixed rules as requested 
        /// </summary>
        /// <param name="fixrule"> Is a refernece of Fixedrule which helps to access the values stored in the variables of fixed rule and
        ///                        stores the value entered by a user and other values.</param>
        /// <param name="selectedValue">The value selected as approved or rejected</param>
        /// <returns>true if the chnages are done nd saved , false otherwise</returns>
        public bool ApproveRejectRule(Fixedrule fixrule, string selectedValue)
        {
            using (var usersDbContext = new UsersEntities())
            {
                bool isSaved = false;
                using (var context = new UTSDatabaseEntities())
                {
                    Fixedrule storedRule = context.Fixedrules.Find(fixrule.RuleId);
                    storedRule.Status = selectedValue == "Approve" ? (RulesStatus.Approved).ToString() : (RulesStatus.Rejected).ToString();
                    context.SaveChanges();
                    isSaved = true;
                }
                return isSaved;
            }
        }
        /// <summary>
        /// This fnuction approves or rejects dynamic rules as requested 
        /// </summary>
        /// <param name="dyrule"> Is a refernece of Dynamic rules table which helps to access the values stored in the variables of dynamic rule and
        ///                        stores the value entered by a user and other values.</param>
        /// <param name="selectedValue">The value selected as approved or rejected</param>
        /// <returns>true if the chnages are done nd saved , false otherwise</returns>
        public bool ApproveRejectRule(DyanamicRule dyrule, string selectedValue)
        {
            using (var usersDbContext = new UsersEntities()) 
            {
                bool isSaved = false;
                using (var context = new UTSDatabaseEntities())
                {
                    DyanamicRule storedRule = context.DyanamicRules.Find(dyrule.RuleId);
                    storedRule.Status = selectedValue == "Approve" ? (RulesStatus.Approved).ToString() : (RulesStatus.Rejected).ToString();
                    context.SaveChanges();
                    isSaved = true;
                }
                return isSaved;
            }
        }

        /// <summary>
        /// Searches for fixed rule in the fixedrule table using the rule id provided
        /// </summary>
        /// <param name="id">the rule id of the rule to be searched</param>
        /// <returns>returns the rule searched. </returns>
        public Fixedrule FixedSearchByRuleId(int id)
        {
            using (var context = new UTSDatabaseEntities())
            {
                return (from c in context.Fixedrules
                        where c.RuleId == id
                        select c).First();
            }
        }
        /// <summary>
        /// Searches for dynamic rule in the Dynamicrule table using the rule id provided
        /// </summary>
        /// <param name="id">the rule id of the rule to be searched</param>
        /// <returns>returns the rule searched. </returns>
        public DyanamicRule DynamicSearchByRuleId(int id)
        {
            using (var context = new UTSDatabaseEntities())
            {
                return (from c in context.DyanamicRules
                        where c.RuleId == id
                        select c).First();
            }
        }
        /// <summary>
        /// Remove a dynamic rule from the Database.
        /// </summary>
        /// <param name="dynamic">The dynamic rule to remove</param>
        /// <returns>true if the rule is successfully deleted and false otherwise.</returns>
        public bool DeleteDynamicRule(DyanamicRule dynamic)
        {
            bool value = false;
            try
            {
                if (dynamic.RuleId != 0)
                {
                    using (var context = new UTSDatabaseEntities())
                    {
                        var saved = context.DyanamicRules.Find(dynamic.RuleId);
                        context.DyanamicRules.Remove(saved);
                        context.SaveChanges();
                        value = true;
                        return value;
                    }
                }
                else
                    return value;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// Remove a fixed rule from the Database.
        /// </summary>
        /// <param name="fixedrule">The fixed rule rule to remove</param>
        /// <returns>true if the rule is successfully deleted and false otherwise.</returns>
        public bool DeleteFixedRule(Fixedrule fixrule)
        {
            bool value = false;
            try
            {
                if (fixrule.RuleId != 0)
                {
                    using (var context = new UTSDatabaseEntities())
                    {
                        var saved = context.Fixedrules.Find(fixrule.RuleId);
                        context.Fixedrules.Remove(saved);
                        context.SaveChanges();
                        value = true;
                        return value;
                    }
                }
                else
                    return value;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
        /// <summary>
        /// Retrive all rules from Fixedrule table.
        /// </summary>
        public IEnumerable<Fixedrule> LoadFixedrules
        {
            get
            {
                using (var context = new UTSDatabaseEntities())
                {
                    // Materialize to a list because the context is closed
                    return context.Fixedrules.ToList();
                }
            }
        }
        /// <summary>
        /// Retrive all rules from Fixedrule table.
        /// </summary>
        public IEnumerable<DyanamicRule> LoadDynamicRules
        {
            get
            {
                using (var context = new UTSDatabaseEntities())
                {
                    // Materialize to a list because the context is closed
                    return context.DyanamicRules.ToList();
                }
            }
        }
     
    }
}
