using DialogMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatBotMVC.ViewModels
{
    public class FixedRulesViewModel
    {
        public List<Fixedrule> FixedRules { get; set; }
        public string Query { get; set; }
        public string Response { get; set; }
    }

    public enum ApproveReject
    {
        Approve,
        Reject
    }

    public static class DisableHtmlControlExtension
    {
        public static MvcHtmlString DisableIf(this MvcHtmlString htmlString, Func<bool> expression)
        {
            if (expression.Invoke())
            {
                var html = htmlString.ToString();
                const string disabled = "\"lnk-is-disabled\"";
                html = html.Insert(html.IndexOf(">",
                  StringComparison.Ordinal), "class="+disabled);
                html = html.Insert(html.IndexOf(">",
                  StringComparison.Ordinal), "disabled =" + "\"disabled\"");
                return new MvcHtmlString(html);
            }
            return htmlString;
        }
    }
}