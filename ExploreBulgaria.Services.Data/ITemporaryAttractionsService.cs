using ExploreBulgaria.Web.ViewModels.Attractions;

namespace ExploreBulgaria.Services.Data
{
    public interface ITemporaryAttractionsService
    {
        Task SaveTemporaryAsync(AddAttractionViewModel model, string visitorId);
    }
}
