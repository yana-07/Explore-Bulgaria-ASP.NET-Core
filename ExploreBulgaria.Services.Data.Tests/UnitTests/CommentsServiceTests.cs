using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Data.Repositories;
using ExploreBulgaria.Services.Data.Tests.Mocks;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.ViewModels.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection.Metadata.Ecma335;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class CommentsServiceTests : UnitTestsBase
    {
        private IDeletableEnityRepository<Comment> commentsRepo;
        private ICommentsService commentsService;
        public override void SetUp()
        {
            base.SetUp();

            commentsRepo = new EfDeletableEntityRepository<Comment>(context);
            commentsService = new CommentsService(commentsRepo, visitorsRepo, guard);
        }

        [Test]
        public async Task PostCommentAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var model = new CommentInputModel
            {
                AttractionId = (await attrRepo
                   .AllAsNoTracking()
                   .FirstOrDefaultAsync())!.Id,
                Text = "TestCommentText"
            };

            var visitorId = (await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync())!.Id;

            await commentsService.PostCommentAsync(model, visitorId);
            var comments = await commentsRepo.AllAsNoTracking().ToListAsync();

            Assert.That(comments.Count, Is.EqualTo(1));
            Assert.That(comments[0].Text, Is.EqualTo("TestCommentText"));
            Assert.That(comments[0].AddedByVisitorId, Is.EqualTo(visitorId));
        }

        [Test]
        public async Task PostCommentAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var model = new CommentInputModel
            {
                AttractionId = (await attrRepo
                   .AllAsNoTracking()
                   .FirstOrDefaultAsync())!.Id,
                Text = "TestCommentText"
            };

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await commentsService.PostCommentAsync(model, ""));
        }

        //[Test]
        //public async Task PostCommentAsync_ShouldThrowExceptionIfSavingToDbFails()
        //{
        //    context = new DatabaseMock().CreateContext(new FailCommandInterceptorMock());
        //    context.Database.EnsureCreated();
        //    commentsRepo = new EfDeletableEntityRepository<Comment>(context);
        //    visitorsRepo = new EfDeletableEntityRepository<Visitor>(context);
        //    commentsService = new CommentsService(commentsRepo, visitorsRepo, guard);

        //    await SeedAttractionsAsync();

        //    var model = new CommentInputModel
        //    {
        //        AttractionId = (await attrRepo
        //           .AllAsNoTracking()
        //           .FirstOrDefaultAsync())!.Id,
        //        Text = "TestCommentText"
        //    };

        //    var visitorId = (await visitorsRepo
        //        .AllAsNoTracking()
        //        .FirstOrDefaultAsync())!.Id;

        //    Assert.ThrowsAsync<ExploreBulgariaDbException>(
        //        async () => await commentsService.PostCommentAsync(new CommentInputModel(), ""));
        //}

        [Test]
        public async Task LikeCommentAsync_VisitorShouldLikeCommentIfNotAlreadyLiked()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var likedByVisitorsCount = await commentsService
                .LikeCommentAsync(comment!.Id, visitor!.Id);

            Assert.That(likedByVisitorsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task LikeCommentAsync_VisitorShouldDislikeCommentIfAlreadyLiked()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .All()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            comment!.LikedByVisitors.Add(new VisitorLikedComment
            {
                VisitorId = visitor!.Id
            });
            await commentsRepo.SaveChangesAsync();

            var likedByVisitorsCount = await commentsService
                .LikeCommentAsync(comment.Id, visitor.Id);

            Assert.That(likedByVisitorsCount, Is.EqualTo(0));
        }

        [Test]
        public async Task LikeCommentAsync_VisitorShouldNotBeAbleToLikeCommentIfAlreadyDisliked()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .All()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            comment!.DislikedByVisitors.Add(new VisitorDislikedComment
            {
                VisitorId = visitor!.Id
            });
            await commentsRepo.SaveChangesAsync();

            var likedByVisitorsCount = await commentsService
                .LikeCommentAsync(comment.Id, visitor.Id);

            Assert.That(likedByVisitorsCount, Is.EqualTo(0));
        }

        [Test]
        public async Task LikeCommentAsync_ShouldThrowExceptionIfCommentIdNotValid()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var visitor = await visitorsRepo
               .AllAsNoTracking()
               .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await commentsService.LikeCommentAsync(7, visitor!.Id),
                InvalidVisitorId);
        }

        //[Test]
        //public async Task LikeCommentAsync_ShouldThrowExceptionIfSavingToDbFails()
        //{
        //    await SeedAttractionsAsync();
        //    await SeedCommentAsync();

        //    var comment = await commentsRepo
        //        .All()
        //        .FirstOrDefaultAsync();
        //    var visitor = await visitorsRepo
        //       .AllAsNoTracking()
        //       .FirstOrDefaultAsync();

        //    context = new DatabaseMock().CreateContext(new FailCommandInterceptorMock());
        //    context.Database.EnsureCreated();
        //    commentsRepo = new EfDeletableEntityRepository<Comment>(context);
        //    visitorsRepo = new EfDeletableEntityRepository<Visitor>(context);
        //    commentsService = new CommentsService(commentsRepo, visitorsRepo, guard);

        //    Assert.ThrowsAsync<ExploreBulgariaDbException>(
        //        async () => await commentsService.LikeCommentAsync(comment!.Id, visitor!.Id),
        //        SavingToDatabase);
        //}

        [Test]
        public async Task DislikeCommentAsync_VisitorShouldDislikeCommentIfNotAlreadyDisliked()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var dislikedByVisitorsCount = await commentsService
                .DislikeCommentAsync(comment!.Id, visitor!.Id);

            Assert.That(dislikedByVisitorsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task DislikeCommentAsync_VisitorShouldLikeCommentIfAlreadyDisliked()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .All()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            comment!.DislikedByVisitors.Add(new VisitorDislikedComment
            {
                VisitorId = visitor!.Id
            });
            await commentsRepo.SaveChangesAsync();

            var dislikedByVisitorsCount = await commentsService
                .DislikeCommentAsync(comment.Id, visitor.Id);

            Assert.That(dislikedByVisitorsCount, Is.EqualTo(0));
        }

        [Test]
        public async Task DislikeCommentAsync_VisitorShouldNotBeAbleToDislikeCommentIfAlreadyLiked()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .All()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            comment!.LikedByVisitors.Add(new VisitorLikedComment
            {
                VisitorId = visitor!.Id
            });
            await commentsRepo.SaveChangesAsync();

            var dislikedByVisitorsCount = await commentsService
                .DislikeCommentAsync(comment.Id, visitor.Id);

            Assert.That(dislikedByVisitorsCount, Is.EqualTo(0));
        }

        [Test]
        public async Task DislikeCommentAsync_ShouldThrowExceptionIfCommentIdNotValid()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var visitor = await visitorsRepo
               .AllAsNoTracking()
               .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await commentsService.DislikeCommentAsync(7, visitor!.Id),
                InvalidVisitorId);
        }

        [Test]
        public async Task AddReplyAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var model = new ReplyInputModel
            {
                CommentId = comment!.Id,
                ReplyText = "TestReplyText"
            };

            var repliesCount = await commentsService
                .AddReplyAsync(model, visitor!.Id);

            Assert.That(repliesCount, Is.EqualTo(1));
        }

        [Test]
        public async Task AddReplyAsync_ShouldThrowExceptionIfCommentIdNotValid()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var visitor = await visitorsRepo
               .AllAsNoTracking()
               .FirstOrDefaultAsync();
            var model = new ReplyInputModel
            {
                CommentId = 7,
                ReplyText = "TestReplyText"
            };

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await commentsService.AddReplyAsync(model, visitor!.Id));
        }

        [Test]
        public async Task AddReplyAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var model = new ReplyInputModel
            {
                CommentId = comment!.Id,
                ReplyText = "TestReplyText"
            };

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await commentsService.AddReplyAsync(model, ""));
        }

        [Test]
        public async Task GetRepliesAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();
            await SeedCommentAsync();

            var comment = await commentsRepo
                .All()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
               .AllAsNoTracking()
               .FirstOrDefaultAsync();
            comment!.Replies.Add(new Reply
            {
                AuthorId = visitor!.Id,
                Text = "TestReplyText"
            });
            await commentsRepo.SaveChangesAsync();
            var model = new ShortReplyInputModel
            {
                CommentId = comment!.Id
            };

            var replies = await commentsService.GetRepliesAsync(model);

            Assert.That(replies.Count(), Is.EqualTo(1));
        }

        private async Task SeedCommentAsync()
        {
            var visitor = await visitorsRepo.AllAsNoTracking().FirstOrDefaultAsync();
            var attraction = await attrRepo.AllAsNoTracking().FirstOrDefaultAsync();
            await commentsRepo.AddAsync(new Comment
            {
                AddedByVisitorId = visitor!.Id,
                AttractionId = attraction!.Id,
                Text = "TestCommentText"
            });
            await commentsRepo.SaveChangesAsync();
        }
    }
}
