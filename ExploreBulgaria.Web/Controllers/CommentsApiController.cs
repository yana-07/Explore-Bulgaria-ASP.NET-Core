using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.Extensions;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Visitors;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentsService commentsService;
        private readonly ILogger<CommentsApiController> logger;

        public CommentsApiController(
            ICommentsService commentsService,
            ILogger<CommentsApiController> logger)
        {
            this.commentsService = commentsService;
            this.logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> PostComment(CommentInputModel model)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            model.Text = sanitizer.Sanitize(model.Text);

            var visitorId = User.VisitorId();
            int commentId;
            try
            {
                commentId = await commentsService.PostCommentAsync(model, visitorId);
            }
            catch(ExploreBulgariaException ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
            }
            
            return Created("/Attractions/Details", new CommentViewModel
            {
                Id = commentId,
                AttractionId = model.AttractionId,
                Text = model.Text,
                CreatedOn = DateTime.UtcNow, 
                AddedByVisitor = new VisitorGenericViewModel
                {
                    Id = visitorId,
                    UserFirstName = User.FirstName(),
                    UserLastName = User.LastName(),
                    UserAvatarUrl = User.AvatarUrl()
                }           
            });
        }

        [HttpPost("like")]
        public async Task<IActionResult> Like(CommentLikeDislikeInputModel model)
        {
            int likesCount;
            try
            {
                likesCount = await commentsService
                    .LikeCommentAsync(model.CommentId, User.VisitorId());
            }
            catch (ExploreBulgariaException ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
            }

            return Ok(likesCount);
        }

        [HttpPost("dislike")]
        public async Task<IActionResult> Dislike(CommentLikeDislikeInputModel model)
        {
            int dislikesCount;
            try
            {
                dislikesCount = await commentsService
                    .DislikeCommentAsync(model.CommentId, User.VisitorId());
            }
            catch (ExploreBulgariaException ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
            }

            return Ok(dislikesCount);
        }

        [HttpPost("addReply")]
        public async Task<IActionResult> AddReply(ReplyInputModel model)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            model.ReplyText = sanitizer.Sanitize(model.ReplyText);

            int repliesCount;
            try
            {
                repliesCount = await commentsService.AddReplyAsync(model, User.VisitorId());
            }
            catch (ExploreBulgariaException ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
            }

            return Created("/Attractions/Details", new ReplyViewModel
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
        public async Task<IActionResult> GetReplies(ShortReplyInputModel model)
        {
            return Ok(await commentsService.GetRepliesAsync(model));
        }
    }
}
