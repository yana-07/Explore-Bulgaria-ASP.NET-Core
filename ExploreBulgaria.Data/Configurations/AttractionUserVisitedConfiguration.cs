using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class AttractionUserVisitedConfiguration : IEntityTypeConfiguration<AttractionUserVisited>
    {
        public void Configure(EntityTypeBuilder<AttractionUserVisited> builder)
            => builder.HasKey(x => new { x.AttractionId, x.UserId });
    }
}
