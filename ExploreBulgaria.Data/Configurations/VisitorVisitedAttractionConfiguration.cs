using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class VisitorVisitedAttractionConfiguration : IEntityTypeConfiguration<VisitorVisitedAttraction>
    {
        public void Configure(EntityTypeBuilder<VisitorVisitedAttraction> builder)
            => builder.HasKey(x => new { x.VisitorId, x.AttractionId });
    }
}
