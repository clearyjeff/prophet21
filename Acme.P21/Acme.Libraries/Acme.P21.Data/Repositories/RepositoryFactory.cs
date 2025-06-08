using System;
using System.Linq;
using Acme.P21.Common;
using Acme.P21.Common.Logging;

namespace Acme.P21.Data.Repositories
{
    public class RepositoryFactory
    {
        public static T Create<T>(AppConfiguration appConfiguration, ILoggingService loggingService)
        {
            var typeClass = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Acme")).SelectMany(x => x.GetTypes()).FirstOrDefault(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
      
            //Add something in here for the configuration, add a dependency on the base class for a configuration object.
            if (typeClass != null) return (T) Activator.CreateInstance(typeClass, appConfiguration, loggingService);

            throw new NotImplementedException(
                $"Unable to instantiate type of {typeof(T)}. Interface or class may not be implemented.");

        }
    }
}