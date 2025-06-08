using System.Collections.Generic;
using Acme.P21.Data.Entities;
using Acme.P21.Data.Models;

namespace Acme.P21.Data.Repositories.Inventory
{
    public interface IInventoryRepository
    {
        List<ItemQuantity> GetWarehousesInventory(string itemId);

        List<Location> GetLocations(string company);

        string GetShipToState(string shipToId);

        string GetThePrimaryLocationForTheState(string state);

        GeoSpatial GetGeoLocation(string zipcode);

        List<GeoSpatial> GetGeoLocations(IEnumerable<string> zipcodes);
    }
}
