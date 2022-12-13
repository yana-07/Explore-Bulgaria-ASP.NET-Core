using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Visitors;
using Microsoft.EntityFrameworkCore;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEnityRepository<Comment> commentsRepo;
        private readonly IDeletableEnityRepository<Visitor> visitorsRepo;
        private readonly IGuard guard;

        public CommentsService(
            IDeletableEnityRepository<Comment> commentsRepo,
            IDeletableEnityRepository<Visitor> visitorsRepo,
            IGuard guard)
        {
            this.commentsRepo = commentsRepo;
            this.visitorsRepo = visitorsRepo;
            this.guard = guard;
        }      

        public async Task<int> PostCommentAsync(CommentInputModel model, string visitorId)
        {
            var comment = new Comment
            {
                AttractionId = model.AttractionId,
                AddedByVisitorId = visitorId,
                Text = model.Text
            };

            await commentsRepo.AddAsync(comment);

            await commentsRepo.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> LikeCommentAsync(int commentId, string visitorId)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.LikedByVisitors)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            guard.AgainstNull(comment, InvalidCommentId);

            var visitorLikedComment = comment!.LikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (visitorLikedComment != null)
            {
                comment.LikedByVisitors.Remove(visitorLikedComment);

                await commentsRepo.SaveChangesAsync();

                return comment.LikedByVisitors.Count;
            }

            var userDislikedComment = comment.DislikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (userDislikedComment != null)
            {
                return comment.LikedByVisitors.Count;
            }

            visitorLikedComment = new VisitorLikedComment
            {
                VisitorId = visitorId,
                CommentId = commentId
            };

            comment.LikedByVisitors.Add(visitorLikedComment);

            await commentsRepo.SaveChangesAsync();

            return comment.LikedByVisitors.Count;
        }

        public async Task<int> DislikeCommentAsync(int commentId, string visitorId)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.DislikedByVisitors)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            guard.AgainstNull(comment, InvalidCommentId);

            var visitorDislikedComment = comment!.DislikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (visitorDislikedComment != null)
            {
                comment.DislikedByVisitors.Remove(visitorDislikedComment);

                await commentsRepo.SaveChangesAsync();

                return comment.DislikedByVisitors.Count;
            }

            var userLikedComment = comment.LikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (userLikedComment != null)
            {
                return comment.DislikedByVisitors.Count;
            }

            visitorDislikedComment = new VisitorDislikedComment
            {
                VisitorId = visitorId,
                CommentId = commentId
            };

            comment.DislikedByVisitors.Add(visitorDislikedComment);

            await commentsRepo.SaveChangesAsync();

            return comment.DislikedByVisitors.Count;
        }

        public async Task<int> AddReplyAsync(ReplyInputModel model, string visitorId)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == model.CommentId);

            guard.AgainstNull(comment, InvalidCommentId);

            var user = await visitorsRepo.AllAsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == visitorId);

            guard.AgainstNull(user, InvalidUserId);

            comment!.Replies.Add(new Reply
            {
                AuthorId = visitorId,
                CommentId = model.CommentId,
                Text = model.ReplyText
            });

            await commentsRepo.SaveChangesAsync();

            return comment.Replies.Count;
        }

        public async Task<IEnumerable<ReplyCommentViewModel>> GetRepliesAsync(ShortReplyInputModel model)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.Replies)
                .ThenInclude(r => r.Author)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(c => c.Id == model.CommentId);

            guard.AgainstNull(comment, InvalidCommentId);

            return comment!.Replies.Select(r => new ReplyCommentViewModel
            {
                Text = r.Text,
                Author = new VisitorGenericViewModel
                {
                    Id = r.AuthorId,
                    UserFirstName = r.Author.User.FirstName,
                    UserLastName = r.Author.User.LastName,
                    UserAvatarUrl = r.Author.User.AvatarUrl,
                },
                CreatedOn = r.CreatedOn
            }).ToList();
        }
    }
}
