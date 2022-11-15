using ExploreBulgaria.Web.ViewModels.Comments;

namespace ExploreBulgaria.Services.Data
{
    public interface ICommentsService
    {
        Task<int> PostCommentAsync(CommentInputModel model, string visitorId);

        Task<int> LikeCommentAsync(int commentId, string visitorId);

        Task<int> DislikeCommentAsync(int commentId, string visitorId);

        Task<int> AddReplyAsync(ReplyInputModel model, string visitorId);

        Task<IEnumerable<ReplyCommentViewModel>> GetRepliesAsync(ShortReplyInputModel model);
    }
}
