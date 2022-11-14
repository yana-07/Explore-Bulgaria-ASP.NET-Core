using ExploreBulgaria.Web.ViewModels.Comments;

namespace ExploreBulgaria.Services.Data
{
    public interface ICommentsService
    {
        Task<int> PostCommentAsync(CommentInputModel model, string userId);

        Task<int> LikeCommentAsync(int commentId, string userId);

        Task<int> DislikeCommentAsync(int commentId, string userId);

        Task<int> AddReplyAsync(ReplyInputModel model, string userId);

        Task<IEnumerable<ReplyCommentViewModel>> GetRepliesAsync(int commentId);
    }
}
