using System;
using System.Collections.Generic;
using System.Linq;
using Acme.P21.Common;
using Acme.P21.Common.Logging;
using Acme.P21.Data.Entities;
using Acme.P21.Data.Models;
using Dapper;

namespace Acme.P21.Data.Repositories.Inventory
{
    public class InventoryRepository : Repository, IInventoryRepository
    {
        public InventoryRepository(AppConfiguration appConfiguration, ILoggingService loggingService) : base (appConfiguration, loggingService)
        {
        }


        /// <summary>
        ///     Gets the state from the ship to id.
        /// </summary>
        /// <param name="shipToId">Ship To Id</param>
        /// <param name="p21SqlConnection">P21 Sql connection</param>
        /// <returns>State</returns>
        public string GetShipToState(string shipToId)
        {
            try
            {
                using (var connection = P21SqlConnection)
                {
                    return connection
                        .Query<string>(
                            "SELECT CASE WHEN ISNULL(phys_state, '') = '' THEN mail_state ELSE phys_state END FROM p21_view_address WHERE id = @ShipToId",
                            new { ShipToId = shipToId }).FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "InventoryRepository.GetShipToState ShipToId {ShipToId}", shipToId);
            }

            return string.Empty;
        }

        /// <summary>
        ///     Gets the priority 1 state from the alternate warehouse table.
        /// </summary>
        /// <param name="state">State code</param>
        /// <returns>Alternate State</returns>
        /// <remarks>
        ///     Uses a  custom table: alternate_warehouse
        /// </remarks>
        public string GetThePrimaryLocationForTheState(string state)
        {
            try
            {
                if (string.IsNullOrEmpty(state)) throw new Exception("There was no state entered.");

                using (var connection = P21SqlConnection)
                {
                    var location = connection.Query<string>(@"SELECT TOP 1 Warehouse FROM alternate_warehouse (NOLOCK) WHERE State = @state", new { state }).FirstOrDefault();
                    return location ?? string.Empty;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "InventoryRepository.GetThePrimaryLocationForTheState State {state}", state);
            }
            
            return string.Empty;
        }

        public List<ItemQuantity> GetWarehousesInventory(string itemId)
        {
            try
            {
                using (var connection = P21SqlConnection)
                {
                    return connection
                        .Query<ItemQuantity>(
                            @"SELECT ItemId, Location, QuantityAvailable, Stockable, ProductionProcessing
                                FROM
                                (SELECT 
                                    DISTINCT I.item_id AS ItemId, 
	                                L.location_id AS [Location], 
	                                QuantityAvailable = 
                                    (L.qty_on_hand 
	                                -   CASE 
                                            WHEN L.qty_allocated < 0 
                                            THEN 0 
                                            ELSE L.qty_allocated 
                                        END 
	                                - COALESCE(S.qty_non_pickable, 0) 
	                                - COALESCE(S.qty_quarantined, 0) 
	                                - COALESCE(S.qty_frozen, 0)
	                                - ISNULL(L.qty_reserved_due_in, 0))
	                                / (CASE 
                                            WHEN unit_size < 0 
                                            THEN 0 
                                            ELSE unit_size 
                                        END),
	                                ISNULL(L.stockable,'N') as Stockable,
	                                ISNULL(production_order_processing,'Y') as ProductionProcessing
	                                FROM p21_view_inv_loc L
	                                INNER JOIN	p21_view_inv_mast I 
                                    ON I.inv_mast_uid = L.inv_mast_uid 
	                                LEFT JOIN p21_view_inv_loc_stock_status S 
                                    ON S.inv_mast_uid = L.inv_mast_uid 
                                    AND S.location_id = L.location_id
	                                LEFT JOIN p21_view_item_uom U on I.inv_mast_uid = U.inv_mast_uid
	                                AND I.default_selling_unit = U.unit_of_measure
	                                LEFT JOIN p21_view_assembly_hdr on I.item_id = p21_view_assembly_hdr.item_id 
                                    AND p21_view_assembly_hdr.delete_flag = 'N'
	                                WHERE I.item_id = @itemId )
                                    --AND L.company_id IN (''))
	                                AS RESULTS
	                                GROUP BY ItemId, [Location], QuantityAvailable, Stockable, ProductionProcessing", new { itemId }).ToList();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "InventoryRepository.GetWarehousesInventory itemId {ItemId}", itemId);
            }

            return new List<ItemQuantity>();
        }

        public List<Location> GetLocations(string company)
        {
            try
            {
                using (var connection = P21SqlConnection)
                {
                    return connection.Query<Location>(@"SELECT A.location_id as Id, 
                                                                LEFT(ISNULL(B.phys_postal_code,'00000'),5) as ZipCode 
                                                                from LOCATION A (nolock)
                                                                JOIN address B (nolock) on A.location_id = B.id
                                                                Where company_id = @company AND A.delete_flag = 'N' AND B.delete_flag = 'N'",
                        new { company }).ToList();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "InventoryRepository.GetLocations - company: {company}", company);
            }

            return new List<Location>();

        }

        public GeoSpatial GetGeoLocation(string zipcode)
        {
            try
            {
                using (var connection = P21SqlConnection)
                {
                    return connection.QueryFirstOrDefault<GeoSpatial>(@"SELECT TOP 1 * FROM 
                                                                usa_geospatial (NOLOCK)
                                                                WHERE zipcode = @zipcode",
                        new { zipcode });
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "InventoryRepository.GetGeoLocation - zipcode: {0}", zipcode);
            }

            return null;

        }

        public List<GeoSpatial> GetGeoLocations(IEnumerable<string> zipcodes)
        {
            try
            {
                using (var connection = P21SqlConnection)
                {
                    return connection.Query<GeoSpatial>(@"SELECT * FROM 
                                                                usa_geospatial (NOLOCK)
                                                                WHERE zipcode IN @zipcodes",
                        new { zipcodes = zipcodes.ToArray() }).ToList();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "InventoryRepository.GetGeoLocations - zipcodes: {@ZipCodes}", zipcodes);
            }

            return new List<GeoSpatial>();

        }
    }
}
