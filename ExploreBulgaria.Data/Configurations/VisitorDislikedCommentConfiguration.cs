using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExploreBulgaria.Data.Configurations
{
    public class VisitorDislikedCommentConfiguration : IEntityTypeConfiguration<VisitorDislikedComment>
    {
        public void Configure(EntityTypeBuilder<VisitorDislikedComment> builder)
            => builder.HasKey(x => new { x.VisitorId, x.CommentId});    
    }
}
