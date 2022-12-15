using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class ClearAdminNotificationInputModel
    {
        [Required]
        public string Group { get; set; } = null!;
    }
}
