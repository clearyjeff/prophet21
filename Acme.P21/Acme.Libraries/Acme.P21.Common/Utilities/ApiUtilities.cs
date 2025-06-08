using System;
using System.Collections.Generic;
using Acme.P21.Common.Logging;
using Acme.P21.Common.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace Acme.P21.Common.Utilities
{
    public class ApiUtilities : IApiUtilities
    {
        public ApiUtilities(ILoggingService logger, AppConfiguration appConfiguration)
        {
            Logger = logger;
            AppConfiguration = appConfiguration;
        }

        private ILoggingService Logger
        {
            get;
        }

        private AppConfiguration AppConfiguration
        {
            get;
        }


        #region IApiUtilities Members

        public List<T> ExecP21ApiPost<T, TR>(string path, Dictionary<string, string> queryParameters, List<TR> body)
        {
            try
            {
                var token = GetP21Token();

                var restClientOptions = new RestClientOptions
                {
                    BaseUrl = new Uri(AppConfiguration.ApiUrl),
                    Authenticator = new JwtAuthenticator(token),
                    MaxTimeout = 15000,
                };

                var client = new RestClient(restClientOptions);

                var request = new RestRequest(path);

                foreach (var kvp in queryParameters) request.AddQueryParameter(kvp.Key, kvp.Value);
                request.AddJsonBody(body);

                Logger.Information("ExecP21ApiPost Request: {@Request}", request);

                var response = client.Post<List<T>>(request);

                Logger.Information("ExecP21ApiPost Response: {@Response}", response);

                return response;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "ExecP21ApiPost");
            }

            return new List<T>();
        }

        public string GetP21Token()
        {
            try
            {
                var client = new RestClient(AppConfiguration.ApiUrl);
                var request = new RestRequest("api/security/token")
                    .AddHeader("username", AppConfiguration.ApiUser)
                    .AddHeader("consumer_key", AppConfiguration.ApiConsumerKey)
                    .AddHeader("grant_type", "client_credentials");
                var response = client.PostAsync<Token>(request).Result;

                return response.AccessToken;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Error retrieving P21 Token");
            }

            return string.Empty;
        }

        #endregion
    }
}