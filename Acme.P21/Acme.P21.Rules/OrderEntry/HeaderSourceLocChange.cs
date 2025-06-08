using Acme.P21.Common;
using Acme.P21.Common.Logging;
using P21.Extensions.BusinessRule;
using System;
using Acme.P21.Data.Repositories;
using Acme.P21.Data.Repositories.Inventory;

namespace Acme.P21.Rules.OrderEntry
{
    [RuleDescription("HeaderSourceLocChange", "Changes the source location on the header based on the ship to state.")]
    public class HeaderSourceLocChange : Rule
    {
        #region Fields

        private static readonly AppConfiguration AppConfiguration = new AppConfiguration();
        private static readonly ILoggingService Logger = new LoggingService<HeaderSourceLocChange>(AppConfiguration);

        #endregion

        public override RuleResult Execute()
        {
            Logger.Debug("Executing Rule - {0}", this.Name);
            var ruleResult = new RuleResult();

            try
            {
                IInventoryRepository inventoryRepository = RepositoryFactory.Create<IInventoryRepository>(AppConfiguration, Logger);

                var shipToId = Data.Fields.GetFieldByAlias("ship_to_id").FieldValue;
                var rmaFlag = Data.Fields.GetFieldByAlias("rma_flag").FieldValue;
                var orderPhysState = Data.Fields.GetFieldByAlias("phys_state").FieldValue;

                Logger.Debug("Data Fields {@Fields}", new { shipToId, rmaFlag, orderPhysState });

                if (!string.IsNullOrWhiteSpace(shipToId) && rmaFlag == "N" && !string.IsNullOrWhiteSpace(orderPhysState))
                {
                    // get the address physical state
                    var addressPhysicalState = inventoryRepository.GetShipToState(shipToId);
                    Logger.Debug("Physical State: {AddressPhysicalState}", addressPhysicalState);

                    if (orderPhysState != addressPhysicalState)
                    {
                        var sourceLocationId = inventoryRepository.GetThePrimaryLocationForTheState(orderPhysState);
                        if (!string.IsNullOrEmpty(sourceLocationId))
                        {
                            Logger.Debug("Changing sales location and source location to: {SourceLocationId}", sourceLocationId);
                            Data.Fields.GetFieldByAlias("sales_loc_id").FieldValue = sourceLocationId;
                            Data.Fields.GetFieldByAlias("source_loc_id").FieldValue = sourceLocationId;
                        }
                        else
                        {
                            Logger.Warning("Could not obtain a primary source location id for state: {State}.", orderPhysState);
                            
                            //We have a couple options here. We could just return a success and
                            // not log a warning. We could also return a failure message with a friendly message for the user.
                            ruleResult.Success = false;
                            ruleResult.Message = "Unable to change the warehouse for this state.";
                            return ruleResult;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ruleResult.Success = false;
                ruleResult.Message = "An error occurred while changing the state, please contact IT.";
                Logger.Error(exception, Name);
            }

            return ruleResult;
        }
    }
}
