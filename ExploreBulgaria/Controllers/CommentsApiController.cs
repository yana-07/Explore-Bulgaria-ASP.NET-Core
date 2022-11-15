using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Common;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Visitors;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentsService commentsService;

        public CommentsApiController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpPost("comments")]
        [Authorize]
        public async Task<ActionResult<CommentViewModel>> PostComment(CommentInputModel model)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            model.Text = sanitizer.Sanitize(model.Text);

            var userId = User.Id();

            var commentId = await commentsService.PostCommentAsync(model, userId);

            return Ok(new CommentViewModel
            {
                Id = commentId,
                AttractionId = model.AttractionId,
                Text = model.Text,
                CreatedOn = DateTime.UtcNow, 
                AddedByVisitor = new VisitorGenericViewModel
                {
                    Id = userId,
                    UserFirstName = User.FirstName(),
                    UserLastName = User.LastName(),
                    UserAvatarUrl = User.AvatarUrl()
                }           
            });
        }

        [HttpPost("like")]
        [Authorize]
        public async Task<ActionResult<int>> Like(CommentLikeDislikeInputModel model)
        {
            var likesCount = await commentsService.LikeCommentAsync(model.CommentId, User.Id());

            return Ok(likesCount);
        }

        [HttpPost("dislike")]
        [Authorize]
        public async Task<ActionResult<int>> Dislike(CommentLikeDislikeInputModel model)
        {
            var dislikesCount = await commentsService.DislikeCommentAsync(model.CommentId, User.Id());

            return Ok(dislikesCount);
        }

        [HttpPost("addReply")]
        [Authorize]
        public async Task<ActionResult<ReplyViewModel>> AddReply(ReplyInputModel model)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            model.ReplyText = sanitizer.Sanitize(model.ReplyText);

            var repliesCount = await commentsService.AddReplyAsync(model, User.Id());

            return Ok(new ReplyViewModel
            {
                Text = model.ReplyText,
                RepliesCount = repliesCount,
                AddedByVisitor = new VisitorGenericViewModel
                {
                    UserFirstName = User.FirstName(),
                    UserLastName = User.LastName(),
                    UserAvatarUrl = User.AvatarUrl()
                },
                CreatedOn = DateTime.UtcNow,
            });
        }

        [HttpPost("getReplies")]
        public async Task<ActionResult<IEnumerable<ReplyCommentViewModel>>> GetReplies(ShortReplyInputModel model)
        {
            return Ok(await commentsService.GetRepliesAsync(model));
        }
    }
}
