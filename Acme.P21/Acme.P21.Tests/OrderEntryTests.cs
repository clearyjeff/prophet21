using Acme.P21.Rules.OrderEntry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P21.Extensions.BusinessRule;
using P21.Extensions.DataAccess;
using System;
using System.Configuration;
using System.IO;

namespace Acme.P21.Tests
{
    [TestClass]
    public class OrderEntryTests
    {
        #region Fields

        private static readonly DBCredentials DbCredentials = new DBCredentials(ConfigurationManager.AppSettings["DBUSER"], ConfigurationManager.AppSettings["DBPASSWORD"], ConfigurationManager.AppSettings["DBSERVER"], ConfigurationManager.AppSettings["P21DataBase"]);

        #endregion

        #region Public Methods

        [TestMethod]
        public void HeaderSourceLocChange()
        {
            var headerSourceLocChange = new HeaderSourceLocChange();

            var xml = GetXMLTestData("OrderEntry\\HeaderRule");

            if(!string.IsNullOrEmpty(xml))
            {
                var executeRuleRequest = new ExecuteRuleRequest
                {
                    DBCredentials = DbCredentials,
                    XML = xml
                };

                headerSourceLocChange.Execute(executeRuleRequest);
            }
        }

        [TestMethod]
        public void CallApiRuleTest()
        {
            var callApiRule = new CallApiRule();

            var xml = GetXMLTestData("OrderEntry\\CallAPIData");

            if (!string.IsNullOrEmpty(xml))
            {
                var executeRuleRequest = new ExecuteRuleRequest
                {
                    DBCredentials = DbCredentials,
                    XML = xml
                };

                callApiRule.Execute(executeRuleRequest);
            }
        }

        [TestMethod]
        public void NoInventorySourceChange()
        {
            var noInventorySourceChangeRule = new NoInventorySourceChange();

            var xml = GetXMLTestData("OrderEntry\\NoInventorySourceChange");

            if (!string.IsNullOrEmpty(xml))
            {
                var executeRuleRequest = new ExecuteRuleRequest
                {
                    DBCredentials = DbCredentials,
                    XML = xml
                };

                noInventorySourceChangeRule.Execute(executeRuleRequest);
            }
        }

        #endregion

        #region Private Methods

        private string GetXMLTestData(string file)
        {
            var path = Path.Combine(Environment.CurrentDirectory, $"TestData\\{file}.xml");

            if (File.Exists(path)) {
                return File.ReadAllText(path);
            }

            return string.Empty;
        }

        #endregion
    }
}
