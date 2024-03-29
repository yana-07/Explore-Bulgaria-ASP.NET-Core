﻿namespace ExploreBulgaria.Web.ViewModels.Users
{
    using ExploreBulgaria.Data.Models;
    using ExploreBulgaria.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.User;

    public class EditUserProfileInputModel : IMapFrom<ApplicationUser>
    {
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string? FirstName { get; set; } = null!;

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string? LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        public string UserName { get; set; } = null!;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

        public string? AvatarUrlPreliminary { get; set; }

        public IFormFile? AvatarUrlUploaded { get; set; }
    }
}
