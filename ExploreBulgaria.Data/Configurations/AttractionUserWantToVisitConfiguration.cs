using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class AttractionUserWantToVisitConfiguration : IEntityTypeConfiguration<AttractionUserWantToVisit>
    {
        public void Configure(EntityTypeBuilder<AttractionUserWantToVisit> builder)
            => builder.HasKey(x => new { x.AttractionId,x.UserId });
    }
}
