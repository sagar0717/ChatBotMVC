using DialogMVC.Business;
using Microsoft.Owin;
using Owin;
using System;
using System.IO;

[assembly: OwinStartupAttribute(typeof(ChatBotMVC.Startup))]
namespace ChatBotMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\DialogMVC.Data"));
            //string xmlfilepath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\DialogMVC.Business\bin\Debug\Serializedrules.xml"));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            //new SystemRules().RulestoXML(xmlfilepath);
            ConfigureAuth(app);
        }
    }
}
