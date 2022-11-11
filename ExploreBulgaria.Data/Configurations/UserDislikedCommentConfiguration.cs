using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class UserDislikedCommentConfiguration : IEntityTypeConfiguration<UserDislikedComment>
    {
        public void Configure(EntityTypeBuilder<UserDislikedComment> builder)
            => builder.HasKey(x => new { x.UserId, x.CommentId});    
    }
}
