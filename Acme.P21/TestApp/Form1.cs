using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Acme.P21.Rules.OrderEntry;
using P21.Extensions.BusinessRule;
using P21.Extensions.DataAccess;

namespace Acme.Distributors.Winforms.Application
{
    public partial class Form1 : Form
    {

        private static readonly DBCredentials DbCredentials = new DBCredentials(ConfigurationManager.AppSettings["DBUSER"], ConfigurationManager.AppSettings["DBPASSWORD"], ConfigurationManager.AppSettings["DBSERVER"], ConfigurationManager.AppSettings["P21DataBase"]);


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var headerTestRule = new HeaderSourceLocChange();

            var xml = GetXMLTestData("OrderEntry\\HeaderRule");

            if (!string.IsNullOrEmpty(xml))
            {
                var executeRuleRequest = new ExecuteRuleRequest
                {
                    DBCredentials = DbCredentials,
                    XML = xml
                };

                headerTestRule.Execute(executeRuleRequest);
            }
        }

        private string GetXMLTestData(string file)
        {
            var path = Path.Combine(Environment.CurrentDirectory, $"TestData\\{file}.xml");

            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var callApiRule = new CallApiRule();

            var xml = GetXMLTestData("OrderEntry\\HeaderRule");

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
    }
}
