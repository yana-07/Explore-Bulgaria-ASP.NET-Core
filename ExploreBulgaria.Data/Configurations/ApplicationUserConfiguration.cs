using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static ExploreBulgaria.Data.Models.Constants.DataConstants.User;

namespace ExploreBulgaria.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> appUser)
        {
            appUser
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(UserNameMaxLength);

            appUser
               .Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(EmailMaxLength);

            appUser
                .HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            appUser
                .HasMany(u => u.Logins)
                .WithOne()
                .HasForeignKey(l => l.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            appUser
                .HasMany(u => u.Roles)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
