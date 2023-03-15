using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Migrations;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Web.ViewModels.Chat;
using Microsoft.EntityFrameworkCore;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data
{
    public class ChatService : IChatService
    {
        private readonly IDeletableEnityRepository<Chat> repo;
        private readonly IDeletableEnityRepository<Visitor> visitorRepo;
        private readonly IGuard guard;

        public ChatService(
            IDeletableEnityRepository<Chat> repo,
            IDeletableEnityRepository<Visitor> visitorRepo,
            IGuard guard)
        {
            this.repo = repo;
            this.visitorRepo = visitorRepo;
            this.guard = guard;
        }

        public async Task ClearMessages(ClearChatMessageViewModel model)
        {
            var fromVisitorId = await visitorRepo
                .AllAsNoTracking()
                .Where(v => v.UserId == model.FromUserId)
                .Select(v => v.Id)
                .FirstOrDefaultAsync();
            guard.AgainstNull(fromVisitorId, InvalidUserId);

            var toVisitorId = await visitorRepo
                .AllAsNoTracking()
                .Where(v => v.UserId == model.ToUserId)
                .Select(v => v.Id)
                .FirstOrDefaultAsync();
            guard.AgainstNull(toVisitorId, InvalidUserId);

            var msgsToDelete = await repo
                .All()
                .Where(c => c.FromVisitorId == fromVisitorId && c.ToVisitorId == toVisitorId)
                .ToListAsync();
            msgsToDelete.ForEach(m => repo.Delete(m));
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessageViewModel>> GetMessages(string fromId, string toId)
        {
            var fromVisitor = await visitorRepo
                .AllAsNoTracking()
                .Include(v => v.User)
                .Where(v => v.UserId == fromId)
                .FirstOrDefaultAsync();
            guard.AgainstNull(fromVisitor, InvalidUserId);

            string? toVisitorId = await visitorRepo
                .AllAsNoTracking()
                .Where(v => v.UserId == toId)
                .Select(v => v.Id)
                .FirstOrDefaultAsync();
            guard.AgainstNull(toVisitorId, InvalidUserId);

            return await repo
                .AllAsNoTracking()
                .Where(c => c.FromVisitorId == fromVisitor!.Id && c.ToVisitorId == toVisitorId)
                .Select(c => new ChatMessageViewModel
                {
                    Name = $"{fromVisitor!.User.FirstName} {fromVisitor.User.LastName}",
                    AvatarUrl = fromVisitor.User.AvatarUrl,
                    SentOn = c.SentOn,
                    Message = c.Message,
                })
                .ToListAsync();
        }

        public async Task SendChat(string fromId, string message, DateTime sentOn)
        {
            var adminVisitor = await visitorRepo
                .AllAsNoTracking()
                .Include(u => u.User)
                .FirstOrDefaultAsync(v => v.User.Email == "adminuser@abv.bg");
            guard.AgainstNull(adminVisitor, InvalidVisitorId);

            var sendingChatVisitorId = await visitorRepo
                .AllAsNoTracking()
                .Where(v => v.UserId == fromId)
                .Select(v => v.Id)
                .FirstOrDefaultAsync();
            guard.AgainstNull(sendingChatVisitorId, InvalidVisitorId);

            await repo.AddAsync(new Chat
            {
                FromVisitorId = sendingChatVisitorId!, 
                ToVisitorId = adminVisitor!.Id,
                Message = message,
                SentOn = sentOn 
            });

            await repo.SaveChangesAsync();
        }
    }
}
