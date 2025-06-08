using System;
using System.Device.Location;

namespace Acme.P21.Data.Models
{
    public class GeoSpatial
    {
        public GeoSpatial()
        {

        }

        public GeoSpatial(string record)
        {
            try
            {
                var lines = record.Split(',');
                Country = lines[0];
                ZipCode = lines[1];
                City = lines[2];
                State = lines[3];
                StateCode = lines[4];
                Community = lines[5];
                SubDivision = string.IsNullOrEmpty(lines[6]) ? 0 : Convert.ToInt32(lines[6]);
                Community1 = lines[7];
                Community2 = lines[8];
                Latitude = string.IsNullOrEmpty(lines[9]) ? 0 : Convert.ToDouble(lines[9]);
                Longitude = string.IsNullOrEmpty(lines[10]) ? 0 : Convert.ToDouble(lines[10]);
                Accuracy = string.IsNullOrEmpty(lines[11]) ? 0 : Convert.ToInt32(lines[11]);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public GeoCoordinate Coordinates => new GeoCoordinate(Latitude, Longitude);

        public double GetDistanceTo(double latitude, double longitude)
        {
            var location = new GeoCoordinate(latitude, longitude);
            return Coordinates.GetDistanceTo(location);
        }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string StateCode { get; set; }

        public string Community { get; set; }

        public int SubDivision { get; set; }

        public string Community1 { get; set; }
        public string Community2 { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Accuracy { get; set; }

    }
}