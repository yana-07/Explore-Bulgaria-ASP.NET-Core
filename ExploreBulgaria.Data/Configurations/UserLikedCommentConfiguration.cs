using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class UserLikedCommentConfiguration : IEntityTypeConfiguration<UserLikedComment>
    {
        public void Configure(EntityTypeBuilder<UserLikedComment> builder)
            => builder.HasKey(x => new { x.UserId, x.CommentId });       
    }
}
