using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Services.Data.Tests.Models
{
    public class AttractionTempMockModel : IMapFrom<AttractionTemporary>
    {
        public string Name { get; set; } = null!;
    }
}
