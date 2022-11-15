using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Visitors;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEnityRepository<Comment> commentsRepo;
        private readonly IDeletableEnityRepository<ApplicationUser> usersRepo;

        public CommentsService(
            IDeletableEnityRepository<Comment> commentsRepo,
            IDeletableEnityRepository<ApplicationUser> usersRepo)
        {
            this.commentsRepo = commentsRepo;
            this.usersRepo = usersRepo;
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

            if (comment == null)
            {
                throw new InvalidOperationException("Invalid comment Id.");
            }

            var userLikedComment = comment.LikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (userLikedComment != null)
            {
                comment.LikedByVisitors.Remove(userLikedComment);

                await commentsRepo.SaveChangesAsync();

                return comment.LikedByVisitors.Count;
            }

            var userDislikedComment = comment.DislikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (userDislikedComment != null)
            {
                return comment.LikedByVisitors.Count;
            }

            userLikedComment = new VisitorLikedComment
            {
                VisitorId = visitorId,
                CommentId = commentId
            };

            comment.LikedByVisitors.Add(userLikedComment);

            await commentsRepo.SaveChangesAsync();

            return comment.LikedByVisitors.Count;
        }

        public async Task<int> DislikeCommentAsync(int commentId, string visitorId)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.DislikedByVisitors)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new InvalidOperationException("Invalid comment Id.");
            }

            var userDislikedComment = comment.DislikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (userDislikedComment != null)
            {
                comment.DislikedByVisitors.Remove(userDislikedComment);

                await commentsRepo.SaveChangesAsync();

                return comment.DislikedByVisitors.Count;
            }

            var userLikedComment = comment.LikedByVisitors.FirstOrDefault(x => x.VisitorId == visitorId);

            if (userLikedComment != null)
            {
                return comment.DislikedByVisitors.Count;
            }

            userDislikedComment = new VisitorDislikedComment
            {
                VisitorId = visitorId,
                CommentId = commentId
            };

            comment.DislikedByVisitors.Add(userDislikedComment);

            await commentsRepo.SaveChangesAsync();

            return comment.DislikedByVisitors.Count;
        }

        public async Task<int> AddReplyAsync(ReplyInputModel model, string userId)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == model.CommentId);

            if (comment == null)
            {
                throw new InvalidOperationException("Invalid comment Id.");
            }

            var user = await usersRepo.AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id.");
            }

            comment.Replies.Add(new Reply
            {
                AuthorId = userId,
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
                .FirstOrDefaultAsync(c => c.Id == model.CommentId);

            if (comment == null)
            {
                throw new InvalidOperationException("Invalid comment Id.");
            }

            return comment.Replies.Select(r => new ReplyCommentViewModel
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
