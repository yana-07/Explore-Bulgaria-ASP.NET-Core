using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Comments;

namespace ExploreBulgaria.Services.Data
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEnityRepository<Comment> repo;

        public CommentsService(IDeletableEnityRepository<Comment> repo)
        {
            this.repo = repo;
        }

        public async Task PostCommentAsync(CommentInputModel model, string userId)
        {
            await repo.AddAsync(new Comment
            {
                AttractionId = model.AttractionId,
                AddedByUserId = userId,
                Text = model.Text
            });

            await repo.SaveChangesAsync();
        }
    }
}
