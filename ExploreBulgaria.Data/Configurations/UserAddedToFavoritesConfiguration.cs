using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class UserAddedToFavoritesConfiguration : IEntityTypeConfiguration<AttractionUserAddedToFavorites>
    {
        public void Configure(EntityTypeBuilder<AttractionUserAddedToFavorites> builder)
            => builder.HasKey(x => new { x.AttractionId, x.UserId });
    }
}
