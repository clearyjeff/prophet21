using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using Acme.P21.Common;

namespace Acme.P21.Common
{
    public class AppConfiguration
    {
        #region Constructors

        public AppConfiguration()
        {
            var appConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            Application = appConfiguration.AppSettings.Settings["APPLICATION"].Value;
            LogLevel = appConfiguration.AppSettings.Settings["LOGLEVEL"].Value;
            LogFile = appConfiguration.AppSettings.Settings["LOGFILE"].Value;
            LogInternal =Convert.ToBoolean(appConfiguration.AppSettings.Settings["LOGINTERNAL"].Value);
            P21ConnectionString = Encryption.Decrypt(appConfiguration.AppSettings.Settings["P21ConnectionString"].Value);
            LogSqlConnection = Encryption.Decrypt(appConfiguration.AppSettings.Settings["LogSqlConnection"].Value);
            ApiUrl = appConfiguration.AppSettings.Settings["P21Api"].Value;
            ApiConsumerKey = appConfiguration.AppSettings.Settings["ApiConsumerKey"].Value;
            ApiUser = appConfiguration.AppSettings.Settings["ApiUser"].Value;
            SeqUrl = appConfiguration.AppSettings.Settings["SeqUrl"].Value;
            ElasticUrl = appConfiguration.AppSettings.Settings["ElasticUrl"].Value;
        }

        #endregion

        #region Public Properties

        public bool LogInternal
        {
            get;
            set;
        }

        public string Application
        {
            get;
            set;
        }

        public string LogFile 
        {
            get;
            set;
        }
        public string LogLevel
        {
            get;
            set;
        }

        public string P21ConnectionString
        {
            get;
        }

        public string LogSqlConnection
        {
            get;
        }
 
        public string SmtpServer
        {
            get;
        }

        public string ApiUrl
        {
            get;
            set;
        }

        public string ApiUser
        {
            get;
            set;
        }

        public string ApiConsumerKey
        {
            get;
            set;
        }

        public string SeqUrl
        {
            get;
            set;
        }

        public string ElasticUrl
        {
            get;
            set;
        }


        #endregion
    }
}
