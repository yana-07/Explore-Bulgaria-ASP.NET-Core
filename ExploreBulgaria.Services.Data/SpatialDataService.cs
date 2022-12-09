using System.Device.Location;
using NetTopologySuite.Geometries;

namespace ExploreBulgaria.Services.Data
{
    public class SpatialDataService : ISpatialDataService
    {
        public NetTopologySuite.Geometries.Point[] GetGeometryPointsByStringCoordinates(string[] coordinates)
        {
            Point[] geographyPoints = new NetTopologySuite.Geometries.Point[coordinates.Length / 2];
            var idx = 0;
            var arrIdx = 0;
            while (idx < coordinates.Length - 1)
            {
                double lng = Convert.ToDouble(coordinates[idx]);
                double lat = Convert.ToDouble(coordinates[idx + 1]);
                idx += 2;
                geographyPoints[arrIdx] = new Point(lng, lat) { SRID = 4326 };
                arrIdx++;
            }

            return geographyPoints;
        }
    }
}
