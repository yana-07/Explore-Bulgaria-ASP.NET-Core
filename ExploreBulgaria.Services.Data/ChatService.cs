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
            var msgsToDelete = await repo
                .All()
                .Where(c => c.FromVisitorId == model.FromVisitorId &&
                    c.ToVisitorId == model.ToVisitorId)
                .ToListAsync();
            msgsToDelete.ForEach(m => repo.Delete(m));
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessageViewModel>> GetMessages(string fromId, string toId)
        {
            var fromVisitor = await visitorRepo
                .AllAsNoTracking()
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == fromId);
            guard.AgainstNull(fromVisitor, InvalidUserId);

            return await repo
                .AllAsNoTracking()
                .Where(c => c.FromVisitorId == fromId && c.ToVisitorId == toId)
                .Select(c => new ChatMessageViewModel
                {
                    Name = $"{fromVisitor!.User.FirstName} {fromVisitor.User.LastName}",
                    AvatarUrl = fromVisitor.User.AvatarUrl,
                    SentOn = c.SentOn,
                    Message = c.Message,
                })
                .ToListAsync();
        }

        public async Task SendChat(string fromVisitorId, string message, DateTime sentOn)
        {
            var adminVisitor = await visitorRepo
                .AllAsNoTracking()
                .Include(u => u.User)
                .FirstOrDefaultAsync(v => v.User.Email == "adminuser@abv.bg");
            guard.AgainstNull(adminVisitor, InvalidVisitorId);

            await repo.AddAsync(new Chat
            {
                FromVisitorId = fromVisitorId, 
                ToVisitorId = adminVisitor!.Id,
                Message = message,
                SentOn = sentOn 
            });

            await repo.SaveChangesAsync();
        }
    }
}
