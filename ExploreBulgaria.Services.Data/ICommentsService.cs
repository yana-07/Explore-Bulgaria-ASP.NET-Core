using ExploreBulgaria.Web.ViewModels.Comments;

namespace ExploreBulgaria.Services.Data
{
    public interface ICommentsService
    {
        Task PostCommentAsync(CommentInputModel model, string userId);
    }
}
