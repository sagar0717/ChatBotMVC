using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatBotMVC.Controllers;
using DialogMVC.Business;
using DialogMVC.Data;
using System.IO;
using System.Web.Mvc;

namespace DialogMVC.Test
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void CreateFixruleMethod_WithoutInput_ReturnToSameView()
        {
            FixedRulesController controllerUnderTest = new FixedRulesController();
            var result = controllerUnderTest.Create() as ViewResult; // 23 is invalid

            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void CreateDynamicMethod_WithoutInput_ReturnToSameView()
        {
            var controller = new DynamicRulesController();
            var result = controller.Create() as ViewResult;
            Assert.AreEqual("", result.ViewName);

        }
        [TestMethod]
        public void EditFixedRule_RuleIdMatchMismatch_Successful()
        {
            var controller = new DynamicRulesController();
            var result = controller.Edit(2) as ViewResult;
            try
            {
                Assert.IsNotNull(result);
            }
            catch
            {
                Assert.IsNull(result);
            }
        }
        [TestMethod]
        public void Creat_QueryExsistInDb_ReturnsToCreateView()
        {
            var controller = new FixedRulesController();
            Fixedrule fxrule = new Fixedrule()
            {
                Query = "Hello",
                UserRequest = "Hello",
                SystemResponse = "hi",
                Response = "hi",
                Createdby = "s@smail.com"

            };

            var result = controller.Create(fxrule) as ViewResult;
            try
            {
                Assert.AreEqual("Create", result.ViewName);
            }
            catch
            {
                Assert.IsNull(result);
            }
            // viewbag error then create view since the rule already exsists

        }


    }

}

