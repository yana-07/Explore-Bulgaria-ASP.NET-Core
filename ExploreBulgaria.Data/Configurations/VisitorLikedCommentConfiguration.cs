using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class VisitorLikedCommentConfiguration : IEntityTypeConfiguration<VisitorLikedComment>
    {
        public void Configure(EntityTypeBuilder<VisitorLikedComment> builder)
            => builder.HasKey(x => new { x.VisitorId, x.CommentId });       
    }
}
