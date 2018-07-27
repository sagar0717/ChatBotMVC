using DialogMVC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ChatBotMVC.ViewModels
{
    public class TopicInformationViewModel
    {
        public int WeekNumber { get; set; }
        public string Topic { get; set; }
    }
}
