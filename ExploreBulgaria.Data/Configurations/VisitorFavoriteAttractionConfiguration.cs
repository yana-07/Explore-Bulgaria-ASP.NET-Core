using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class VisitorFavoriteAttractionConfiguration : IEntityTypeConfiguration<VisitorFavoriteAttraction>
    {
        public void Configure(EntityTypeBuilder<VisitorFavoriteAttraction> builder)
            => builder.HasKey(x => new { x.VisitorId, x.AttractionId });
    }
}
