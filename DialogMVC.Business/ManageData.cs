using DialogMVC.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogMVC.Business
{
    /// <summary>
    /// this class represents the core domain logic of the datatable and includes crud operation on the datatable of dynamic rules
    /// </summary>
    public class ManageData
    {
        private UTSDatabaseEntities db = new UTSDatabaseEntities();

        /// <summary>
        /// Retrives datarow by the ruleid that is passed
        /// </summary>
        /// <param name="ruleId">The id of the rule which is used to retrive datarow</param>
        /// <returns></returns>
        public TopicInformation FindDataRowById(int ruleId)
        {
            return db.TopicInfo.Find(ruleId);
        }
        public bool SaveData(TopicInformation topicInformation, string loggedinUser)
        {
            using (var context = new UTSDatabaseEntities())
            {
                var key = context.TopicInfo.Where(m => m.WeekNumber == topicInformation.WeekNumber).FirstOrDefault();

                if (key == null)
                {
                    topicInformation.LastUpdatedBy = loggedinUser;
                    db.TopicInfo.Add(topicInformation);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Domain logic of Editting datatable entry 
        /// </summary>
        /// <param name="topicInformation">The refernce to access topic information for editting it</param>
        /// <param name="loggedinUser">The session of user that is logged in.</param>
        /// <returns></returns>
        public bool EditData(TopicInformation topicInformation, string loggedinUser)
        {
            using (var context = new UTSDatabaseEntities())
            {
                var key = context.TopicInfo.Where(m => m.WeekNumber == topicInformation.WeekNumber).FirstOrDefault();

                if (key != null)
                {
                    topicInformation.LastUpdatedBy = loggedinUser;
                    db.Entry(topicInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Domain logic of deleting datatable entry
        /// </summary>
        /// <param name="topicInformation">The refernce to access topic information for deletting it.</param>
        /// <returns></returns>
        public bool DeleData(TopicInformation topicInformation)
        {
            using (var context = new UTSDatabaseEntities())
            {
                var key = context.TopicInfo.Where(m => m.WeekNumber == topicInformation.WeekNumber).FirstOrDefault();

                if (key != null)
                {
                    db.TopicInfo.Remove(topicInformation);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrives all the data from TopicInformation table and lists it.
        /// </summary>
        public IEnumerable<TopicInformation> LoadData
        {
            get
            {
                using (var context = new UTSDatabaseEntities())
                {
                    // Materialize to a list because the context is closed
                    return context.TopicInfo.ToList();
                }
            }
        }
    }
}
