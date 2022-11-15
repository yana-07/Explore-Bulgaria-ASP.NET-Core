using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Users;
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

        public async Task<int> PostCommentAsync(CommentInputModel model, string userId)
        {
            var comment = new Comment
            {
                AttractionId = model.AttractionId,
                AddedByUserId = userId,
                Text = model.Text
            };

            await commentsRepo.AddAsync(comment);

            await commentsRepo.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> LikeCommentAsync(int commentId, string userId)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.LikedByUsers)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new InvalidOperationException("Invalid comment Id.");
            }

            var userLikedComment = comment.LikedByUsers.FirstOrDefault(x => x.UserId == userId);

            if (userLikedComment != null)
            {
                comment.LikedByUsers.Remove(userLikedComment);

                await commentsRepo.SaveChangesAsync();

                return comment.LikedByUsers.Count;
            }

            var userDislikedComment = comment.DislikedByUsers.FirstOrDefault(x => x.UserId == userId);

            if (userDislikedComment != null)
            {
                return comment.LikedByUsers.Count;
            }

            userLikedComment = new UserLikedComment
            {
                UserId = userId,
                CommentId = commentId
            };

            comment.LikedByUsers.Add(userLikedComment);

            await commentsRepo.SaveChangesAsync();

            return comment.LikedByUsers.Count;
        }

        public async Task<int> DislikeCommentAsync(int commentId, string userId)
        {
            var comment = await commentsRepo.All()
                .Include(c => c.DislikedByUsers)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new InvalidOperationException("Invalid comment Id.");
            }

            var userDislikedComment = comment.DislikedByUsers.FirstOrDefault(x => x.UserId == userId);

            if (userDislikedComment != null)
            {
                comment.DislikedByUsers.Remove(userDislikedComment);

                await commentsRepo.SaveChangesAsync();

                return comment.DislikedByUsers.Count;
            }

            var userLikedComment = comment.LikedByUsers.FirstOrDefault(x => x.UserId == userId);

            if (userLikedComment != null)
            {
                return comment.DislikedByUsers.Count;
            }

            userDislikedComment = new UserDislikedComment
            {
                UserId = userId,
                CommentId = commentId
            };

            comment.DislikedByUsers.Add(userDislikedComment);

            await commentsRepo.SaveChangesAsync();

            return comment.DislikedByUsers.Count;
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
                Author = new UserGenericViewModel
                {
                    Id = r.AuthorId,
                    FirstName = r.Author.FirstName,
                    LastName = r.Author.LastName,
                    AvatarUrl = r.Author.AvatarUrl,
                },
                CreatedOn = r.CreatedOn
            }).ToList();
        }
    }
}
