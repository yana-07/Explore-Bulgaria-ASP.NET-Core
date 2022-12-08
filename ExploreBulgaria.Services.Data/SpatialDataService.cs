using System.Device.Location;

namespace ExploreBulgaria.Services.Data
{
    public class SpatialDataService : ISpatialDataService
    {
        public GeoCoordinate[] GetGeometryPointsByStringCoordinates(string[] coordinates)
        {
            GeoCoordinate[] geographyPoints = new GeoCoordinate[coordinates.Length / 2];
            var idx = 0;
            var arrIdx = 0;
            while (idx < coordinates.Length - 1)
            {
                double lng = Convert.ToDouble(coordinates[idx]);
                double lat = Convert.ToDouble(coordinates[idx + 1]);
                idx += 2;
                geographyPoints[arrIdx] = new GeoCoordinate(lat, lng);
                arrIdx++;
            }

            return geographyPoints;
        }
    }
}
