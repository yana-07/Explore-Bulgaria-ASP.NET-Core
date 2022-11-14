using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Comments;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEnityRepository<Comment> repo;

        public CommentsService(IDeletableEnityRepository<Comment> repo)
        {
            this.repo = repo;
        }      

        public async Task<int> PostCommentAsync(CommentInputModel model, string userId)
        {
            var comment = new Comment
            {
                AttractionId = model.AttractionId,
                AddedByUserId = userId,
                Text = model.Text
            };

            await repo.AddAsync(comment);

            await repo.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> LikeCommentAsync(int commentId, string userId)
        {
            var comment = await repo.All()
                .Include(c => c.LikedByUsers)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new InvalidOperationException("No such comment exists.");
            }

            var userLikedComment = comment.LikedByUsers.FirstOrDefault(x => x.UserId == userId);

            if (userLikedComment != null)
            {
                comment.LikedByUsers.Remove(userLikedComment);

                await repo.SaveChangesAsync();

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

            await repo.SaveChangesAsync();

            return comment.LikedByUsers.Count;
        }

        public async Task<int> DislikeCommentAsync(int commentId, string userId)
        {
            var comment = await repo.All()
                .Include(c => c.DislikedByUsers)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new InvalidOperationException("No such comment exists.");
            }

            var userDislikedComment = comment.DislikedByUsers.FirstOrDefault(x => x.UserId == userId);

            if (userDislikedComment != null)
            {
                comment.DislikedByUsers.Remove(userDislikedComment);

                await repo.SaveChangesAsync();

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

            await repo.SaveChangesAsync();

            return comment.DislikedByUsers.Count;
        }
    }
}
