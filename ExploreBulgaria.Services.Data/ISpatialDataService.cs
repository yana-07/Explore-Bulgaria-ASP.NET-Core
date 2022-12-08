using System.Device.Location;

namespace ExploreBulgaria.Services.Data
{
    public interface ISpatialDataService
    {
        public GeoCoordinate[] GetGeometryPointsByStringCoordinates(string[] coordinates);
    }
}
