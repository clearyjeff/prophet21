using Acme.P21.Common;
using Acme.P21.Common.Logging;
using P21.Extensions.BusinessRule;
using System;
using System.Collections.Generic;
using Acme.P21.Common.Utilities;
using Acme.P21.Data.Models;


namespace Acme.P21.Rules.OrderEntry
{
    [RuleDescription("Call Api Rule", "Example Rule that calls an API.")]
    public class CallApiRule : Rule
    {
        #region Fields

        private static readonly AppConfiguration AppConfiguration = new AppConfiguration();
        private static readonly ILoggingService Logger = new LoggingService<CallApiRule>(AppConfiguration);

        #endregion

        public override RuleResult Execute()
        {
            Logger.Debug("Executing Rule - {0}", this.Name);

            var ruleResult = new RuleResult();

            try
            {
                var apiUtilities = new ApiUtilities(Logger, AppConfiguration);

                var queryParams = new Dictionary<string, string>
                {
                    {"companyId", "ACME"},
                    {"customerId", "123456"},
                    {"salesLocId", "1"},
                    {"shipToId", "123456"}
                };


                var body = new List<PriceRequest>
                {
                    new PriceRequest { ItemId = "SK1234", UnitQuantity = 1 }
                };

                var endpoint = "api/inventory/v2/parts/prices";

                Logger.Information("Request made to api endpoint {ApiEndPoint} with request body {@Request} and query params {@QueryParams}", endpoint, body, queryParams);

                var result = apiUtilities.ExecP21ApiPost<PricedP21Item, PriceRequest>(endpoint, queryParams, body);
                Logger.Information("Price Request API Call Result: {@Result}", result);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, Name);
                ruleResult.Message =
                    "There was an exception in the rule. Please check the logs where ever they may be.";
                ruleResult.Success = false;
            }

            return ruleResult;
        }
    }
}
