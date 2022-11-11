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
            await commentsService.PostCommentAsync(model, User.Id());

            return Ok(new CommentViewModel
            {
                AttractionId = model.AttractionId,
                Text = model.Text,
                CreatedOn = DateTime.UtcNow, 
                AddedByUser = new UserGenericViewModel
                {
                    FirstName = User.FirstName(),
                    LastName = User.LastName(),
                    AvatarUrl = User.AvatarUrl()
                }           
            });
        }
    }
}
