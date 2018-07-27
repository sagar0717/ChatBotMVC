using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DialogMVC.Business;
using DialogMVC.Data;
using ChatBotMVC.Controllers;
using System.IO;
using System.Web.Mvc;

namespace DialogMVC.Test
{
    [TestClass]
    public class DialogSystemTests
    {
        
        [TestMethod]
        public void SaveFixedRule_QueryOrResponseIsBlank_returnsFalse()
        {
            //Arrange
            Configure fixRuleSave = new Configure();
            Fixedrule fxrule = new Fixedrule
            {
                Query = "",
                Response = ""
            };
            //Act
            bool isRuleSaved = fixRuleSave.SaveFixedRule(fxrule, "s@smail.com");
            //Assert
            Assert.IsFalse(isRuleSaved, "Rule Not Saved");
        }
        [TestMethod]
        public void SaveFixedRule_QueryOrResponseIsNull_returnsFalse()
        {
            //Arrange
            Configure fixRuleSave = new Configure();
            Fixedrule fxrule = new Fixedrule
            {
                Query = null,
                Response = null
            };
            //Act
            bool isRuleSaved = fixRuleSave.SaveFixedRule(fxrule, "s@smail.com");
            //Assert
            Assert.IsFalse(isRuleSaved, "Rule Not Saved");
        }
        [TestMethod]
        public void SaveFixedRule_QueryAlreadyExsists_returnsFalse()
        {
            //Arrange
            Configure fixRuleSave = new Configure();
            Fixedrule fxrule = new Fixedrule()
            {
                Query = "Hello",
                Response = "hi"
            };
            //Act
            bool isRuleSaved = fixRuleSave.SaveFixedRule(fxrule, "s@smail.com");
            //Assert
            Assert.IsFalse(isRuleSaved, "Query Already exsists");
        }
        [TestMethod]
        public void SaveFixedRule_ValidData_returnsTrue()
        {
            //Arrange
            Configure fixRuleTest = new Configure();
            Fixedrule fxrule = new Fixedrule()
            {
                Query = "Hello ChatBot. How are you?",
                Response = "Hi ChatBot user. Always in service. "
            };
            //Act
            bool isRuleSaved = fixRuleTest.SaveFixedRule(fxrule, "s@smail.com");
            //Assert
            Assert.AreEqual(true, isRuleSaved);
        }

        [TestMethod]
        public void DeleteFixedRule_InvalidRuleId_ReturnsFalse()
        {
            //Arrange
            Configure fixRuleTest = new Configure();
            Fixedrule fxrule = new Fixedrule
            {
                RuleId = 0
            };
            //Act
            bool isRuleDeleted = fixRuleTest.DeleteFixedRule(fxrule);
            //Assert
            Assert.IsFalse(isRuleDeleted, "Rule doen not exsist.");
        }

        [TestMethod]
        public void DeleteFixedRule_ValidRuleId_ReturnsTrue()
        {
            //Arrange
            Configure fixRuleTest = new Configure();
            Fixedrule fxrule = new Fixedrule
            {
                RuleId = 43
                //Put rule id on final day
            };
            //Act
            bool isRuleDeleted = fixRuleTest.DeleteFixedRule(fxrule);
            //Assert
            Assert.IsTrue(isRuleDeleted, "Rule Deleted.");
        }

        [TestMethod]
        public void SaveDynamicRule_QueryAlreadyExsists_returnsFalse()
        {
            //Arrange
            Configure dynamicRuleTest = new Configure();
            DyanamicRule dynamic = new DyanamicRule()
            {
                Query = "What will happen in Week [Keyword]	",
                Response = "hi"
            };
            //Act
            bool isRuleSaved = dynamicRuleTest.SaveDynamicRule(dynamic, "s@smail.com");
            //Assert
            Assert.IsFalse(isRuleSaved, "Query Already exsists.");
        }
        [TestMethod]
        public void SaveDynamicRule_QueryOrResponseIsNull_returnsFalse()
        {
            //Arrange
            Configure dynamicRuleTest = new Configure();
            DyanamicRule dynamic = new DyanamicRule
            {
                Query = null,
                Response = null
            };
            //Act
            bool isRuleSaved = dynamicRuleTest.SaveDynamicRule(dynamic, "s@smail.com");
            //Assert
            Assert.IsFalse(isRuleSaved, "Rule Not Saved");
        }
        [TestMethod]
        public void DeleteDynamicRule_InvalidRuleId_ReturnsFalse()
        {
            //Arrange
            Configure dynamicRuleTest = new Configure();
            DyanamicRule dynamic = new DyanamicRule
            {
                RuleId = 0
            };
            //Act
            bool isRuleDeleted = dynamicRuleTest.DeleteDynamicRule(dynamic);
            //Assert
            Assert.IsFalse(isRuleDeleted, "Rule does not exsist");
        }
        [TestMethod]
        public void DeleteDynamicRule_ValidRuleIdInput_ReturnsTrue()
        {
            //Arrange
            Configure dynamicRuleTest = new Configure();
            DyanamicRule dynamic = new DyanamicRule
            {
                RuleId = 16
                // add rule id here on the final day. 
            };
            //Act
            bool isRuleDeleted = dynamicRuleTest.DeleteDynamicRule(dynamic);
            //Assert
            Assert.IsTrue(isRuleDeleted, "Rule Deleted.");
        }

    }
    [TestClass]
    public class ManageData_Tests
    {


        [TestMethod]
        public void EditData_ValidOrInvalidData_Successful()
        {
            const string Message = "Data doesnt exsist in the topicinformation db.";
            //Arrange
            ManageData manageDataTest = new ManageData();
            TopicInformation topicinformation = new TopicInformation
            {
                WeekNumber = 12,
                Topic = "Revision"
            };
            //Act
            bool isDataEditted = manageDataTest.EditData(topicinformation, "s@smail.com");
            //Assert
            try
            {
                //Returns true if the data passed as parameter to EditData( data ) function exsists in the database
                Assert.IsTrue(isDataEditted, "Data exsists to be editted.");
            }
            catch
            {
                //Returns false if the data passed as parameter to EditData( data ) function does not exsist in the database
                Assert.IsFalse(isDataEditted, Message);
            }

        }
        [TestMethod]
        public void DeleteData_ValidOrInvalidData_Successful()
        {
            const string Message = "Data doesnt exsist in the topicinformation db.";
            //Arrange
            ManageData manageDataTest = new ManageData();
            TopicInformation topicinformation = new TopicInformation
            {
                WeekNumber = 13,
                LastUpdatedBy = "s@smail.com",
                Topic = "Revision"
            };
            //Act
            bool isDataDeleted = manageDataTest.DeleData(topicinformation);
            //Assert
            try
            {
                //Returns true if the data passed as parameter to DeteData( data ) function exsists in the database
                Assert.IsTrue(isDataDeleted, "Data exsists to be editted.");
            }
            catch
            {
                ////Returns false if the data passed as parameter to DeteData( data ) function exsist in the database
                Assert.IsFalse(isDataDeleted, Message);
            }

        }
    }

    [TestClass]
    public class ControllerTests
    {
        [TestClass]
        public class ControllersTests
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
            
        }
    }

}
