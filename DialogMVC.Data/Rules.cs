using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogMVC.Data
{
    /// <summary>
    /// Enum defined to state the status of the rules. This is inherited in Configure.cs and SystemRules.cs
    /// </summary>
    public enum RulesStatus
    {
        Created,
        Updated,
        Approved,
        Rejected
    }
    /// <summary>
    /// This class obtains/gets and sets user request and system reponse. This is inherited by SystemRules.cs
    /// </summary>
    public class Rules
    {
        public string UserRequest { get; set; }
        public string SystemResponse { get; set; }
    }
}
