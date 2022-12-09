using System.Device.Location;
using NetTopologySuite.Geometries;

namespace ExploreBulgaria.Services.Data
{
    public interface ISpatialDataService
    {
        public Point[] GetGeometryPointsByStringCoordinates(string[] coordinates);
    }
}
