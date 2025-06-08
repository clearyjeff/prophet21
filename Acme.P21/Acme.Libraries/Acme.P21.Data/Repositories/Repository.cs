using System.Data;
using System.Data.SqlClient;
using Acme.P21.Common;
using Acme.P21.Common.Logging;
using Dapper.Contrib.Extensions;

namespace Acme.P21.Data.Repositories
{
    public abstract class Repository : IRepository
    {
        internal readonly AppConfiguration AppConfiguration;
        internal readonly ILoggingService Logger;

        internal IDbConnection P21SqlConnection => new SqlConnection(AppConfiguration.P21ConnectionString);

        protected Repository(AppConfiguration appConfiguration, ILoggingService loggingService)
        {
            AppConfiguration = appConfiguration;
            Logger = loggingService;
        }

        public T Get<T>(int id) where T : class
        {
            using (var connection = P21SqlConnection)
            {
                return connection.Get<T>(id);
            }
        }
    }
}