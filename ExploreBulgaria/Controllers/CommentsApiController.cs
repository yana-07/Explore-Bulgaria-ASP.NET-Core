using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Common;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Users;
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
            var userId = User.Id();

            var commentId = await commentsService.PostCommentAsync(model, userId);

            return Ok(new CommentViewModel
            {
                Id = commentId,
                AttractionId = model.AttractionId,
                Text = model.Text,
                CreatedOn = DateTime.UtcNow, 
                AddedByUser = new UserGenericViewModel
                {
                    Id = userId,
                    FirstName = User.FirstName(),
                    LastName = User.LastName(),
                    AvatarUrl = User.AvatarUrl()
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
    }
}
