using System;
using System.Collections.Generic;
using System.Linq;
using Acme.P21.Common;
using Acme.P21.Common.Logging;
using Acme.P21.Data.Repositories;
using Acme.P21.Data.Repositories.Inventory;
using P21.Extensions.BusinessRule;

namespace Acme.P21.Rules.OrderEntry
{
    /// <summary>
    ///     Rule attempts to source from another warehouse with inventory when no inventory exists in current location.
    /// </summary>
    /// <remarks>
    ///     Window: Order Entry
    ///     Rule Type: Validator
    ///     Run Type: Synchronous
    ///     Apply Rule On: Quantity
    /// </remarks>
    [RuleDescription("No Inventory Source Change",
        "Rule attempts to source from another warehouse with inventory when no inventory exists in current location.")]
    public class NoInventorySourceChange : Rule
    {
        private static readonly AppConfiguration AppConfiguration = new AppConfiguration();
        private static readonly ILoggingService Logger = new LoggingService<NoInventorySourceChange>(AppConfiguration);

        public override RuleResult Execute()
        {
            Logger.Debug("Executing Rule - {0}", this.Name);
            var ruleResult = new RuleResult();

            try
            {
                var inventoryRepository = RepositoryFactory.Create<IInventoryRepository>(AppConfiguration, Logger);

                var rmaFlag = Data.Fields.GetFieldByAlias("rma_flag").FieldValue;
                var quote = Data.Fields.GetFieldByAlias("quote").FieldValue;
                var customerId = Data.Fields.GetFieldByAlias("customer_id").FieldValue;
                var otherCharge = Data.Fields.GetFieldByAlias("other_charge").FieldValue;
                var quantityAvailable =
                    Convert.ToDecimal(Data.Fields.GetFieldByAlias("quantity_available").FieldValue);
                var quantityOrdered = Convert.ToDecimal(Data.Fields.GetFieldByAlias("unit_quantity").FieldValue);
                var quantityAllocated = Convert.ToDecimal(Data.Fields.GetFieldByAlias("qty_allocated").FieldValue);
                var shipToState = Data.Fields.GetFieldByAlias("phys_state").FieldValue;
                var zipCode = Data.Fields.GetFieldByAlias("zip_code").FieldValue;
                var sourceLocationId = Data.Fields.GetFieldByAlias("line_source_loc_id").FieldValue;
                var itemId = Data.Fields.GetFieldByAlias("oe_order_item_id").FieldValue;
                var quantityLine = quantityAllocated + quantityAvailable;


                Logger.Debug("Data Fields {@Fields}", new
                {
                    quote, rmaFlag, customerId, otherCharge,
                    quantityAvailable,
                    quantityOrdered,
                    quantityAllocated,
                    shipToState,
                    zipCode,
                    sourceLocationId,
                    itemId
                });


                if (rmaFlag == "N" && otherCharge == "N" && quantityOrdered > quantityLine)
                {
                    var alternateWarehouses = GetAlternateWarehouses(zipCode, "ACME", inventoryRepository);
                    Logger.Debug("Sorted list of alternate warehouse by closest distance from ship to zip code: {@AlternateWarehouses}", alternateWarehouses);

                    if (alternateWarehouses.Count > 0)
                    {
                        var warehouseItemInventory = inventoryRepository.GetWarehousesInventory(itemId);

                        Logger.Debug("List of available inventory be each warehouse: {@WarehouseItemInventory}",
                            warehouseItemInventory);

                        var stockableWarehouseId = 0;

                        foreach (var warehouse in alternateWarehouses)
                        {
                            var result =
                                warehouseItemInventory.FirstOrDefault(item => item.Location == warehouse.Key);

                            stockableWarehouseId =
                                result != null && stockableWarehouseId == 0 && result.Stockable.Trim() == "Y"
                                    ? result.Location
                                    : stockableWarehouseId;

                            if (result != null && quantityOrdered <= result.QuantityAvailable)
                            {
                                Data.Fields.GetFieldByAlias("line_source_loc_id").FieldValue =
                                    result.Location.ToString();
                                Logger.Warning("An alternate warehouse was found for item {@ItemId} with quantity of {@QuantityAvailable} at {@Location} ", itemId, result.QuantityAvailable, result.Location);
                                Logger.Debug("An alternate warehouse was found with available quantity, full inventory results: {@InventoryResults}", result);
                                return ruleResult;
                            }
                        }

                        //There was no warehouse with inventory but we have a stockable warehouse so use that.
                        if (stockableWarehouseId > 0)
                        {
                            Data.Fields.GetFieldByAlias("line_source_loc_id").FieldValue =
                                stockableWarehouseId.ToString();
                            Logger.Warning("There was no available inventory at any warehouses for item {0}, setting the location id to the first stockable warehouse of location {@StockableLocation}", itemId, stockableWarehouseId);
                            return ruleResult;
                        }


                        Data.Fields.GetFieldByAlias("disposition").FieldValue = "S";
                        //ruleResult.Message = "No stockable warehouse has enough stock to fulfill this quantity. Part or all of this line will be special order.";
                        Logger.Warning(
                            "No stockable warehouse for item {@ItemId} has enough stock to fulfill this quantity. Part or all of this line will be special order",
                            itemId);
                    }
                }
            }
            catch (Exception exception)
            {
                ruleResult.Success = false;
                ruleResult.Message = "An error occurred while attempting to source from an alternate warehouse, please contact IT.";
                Logger.Error(exception, Name);
            }

            return ruleResult;
        }

        private Dictionary<int, double> GetAlternateWarehouses(string zipCode, string company,
            IInventoryRepository inventoryRepository)
        {
            var shipToLocation = inventoryRepository.GetGeoLocation(zipCode);
            if (shipToLocation == null)
            {
                Logger.Warning("Unable to retrieve geolocation data for the provided zip code: {@ZipCode}", zipCode);
                return new Dictionary<int, double>();
            }

            Logger.Debug("ShipToLocation {@ShipToLocation}", shipToLocation);

            var locations = inventoryRepository.GetLocations(company);

            Logger.Debug("Company warehouse locations: {@Locations}", locations);

            var locationsGeoData = inventoryRepository.GetGeoLocations(locations.Select(x => x.ZipCode));

            Logger.Debug("Geo Date for warehouse locations {@Locations}", locationsGeoData);

            var geoLocations = new Dictionary<int, double>();
            foreach (var location in locations)
            {
                var geoLocation = locationsGeoData.FirstOrDefault(g => g.ZipCode == location.ZipCode);
                if (geoLocation != null)
                    geoLocations.Add(location.Id,
                        geoLocation.GetDistanceTo(shipToLocation.Latitude, shipToLocation.Longitude));
            }

            var ordered = geoLocations.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return ordered;
        }
    }
}