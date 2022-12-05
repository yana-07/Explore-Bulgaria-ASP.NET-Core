using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class VisitorWantToVisitAttractionConfiguration : IEntityTypeConfiguration<VisitorWantToVisitAttraction>
    {
        public void Configure(EntityTypeBuilder<VisitorWantToVisitAttraction> builder)
            => builder.HasKey(x => new { x.VisitorId, x.AttractionId });
    }
}
