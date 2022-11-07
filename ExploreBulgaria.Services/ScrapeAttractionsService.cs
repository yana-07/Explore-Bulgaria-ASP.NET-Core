using AngleSharp;
using AngleSharp.Html.Dom;
using ExploreBulgaria.Services.Models;
using System.Net;

namespace ExploreBulgaria.Services
{
    public class ScrapeAttractionsService : IScrapeAttractionsService
    {
        private static IConfiguration config = Configuration.Default.WithDefaultLoader();
        private static IBrowsingContext context = BrowsingContext.New(config);
        private static ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
        public static string BaseUrl { get; } = "https://opoznai.bg/all/sort:newest/page:{0}";

        public IEnumerable<AttractionDto> ScrapeAttractions()
        {
            var attractions = new List<AttractionDto>();

            for (int i = 1; i <= 214; i++)
            {
                try
                {
                    attractions.AddRange(GetAttractionsOnPage(i));
                }
                catch (Exception)
                {
                }
            }

            return attractions;
        }

        private static IEnumerable<AttractionDto> GetAttractionsOnPage(int page)
        {
            var url = string.Format(BaseUrl, page);

            var document = context.OpenAsync(url).GetAwaiter().GetResult();

            var attractions = new List<AttractionDto>();

            if (document.StatusCode == HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException();
            }

            var articleElements = document.QuerySelectorAll(".browse_list .article_browse");

            Parallel.ForEach(articleElements, options, element =>
            {
                var anchorElement = element.QuerySelector(".article_padding h3 a") as IHtmlAnchorElement;
                var anchorUrl = anchorElement?.Href;

                attractions.Add(GetAttraction(anchorUrl!));
            });

            return attractions;
        }

        private static AttractionDto GetAttraction(string anchorUrl)
        {
            var document = context.OpenAsync(anchorUrl).GetAwaiter().GetResult();

            var data = document.QuerySelectorAll(".breadcrumbs-list-wrap li").Select(x => x.TextContent.Trim()).ToArray();

            string areaName = "", categoryName = "", subCategoryName = "", attractionName = "";

            if (data.Length == 5)
            {
                areaName = data[1];
                categoryName = data[2];
                subCategoryName = data[3];
                attractionName = data[4];
            }
            else if (data.Length == 6)
            {
                areaName = data[1];
                categoryName = data[3];
                subCategoryName = data[4];
                attractionName = data[5];
            }

            var location = document.QuerySelector(
                "span.location a")
                ?.TextContent;

            var description = document.QuerySelector(
                ".main_article_text")
                !.TextContent;

            var altitude = document.QuerySelector(
                "span.attitude")
                ?.TextContent;

            var urls = document.QuerySelectorAll(
                ".gallery-img-holder a")
                .Select(aEl => aEl.Attributes
                   .FirstOrDefault(attr => attr.Name == "style")
                   ?.Value
                   .Replace("background-image: url('", string.Empty)
                   .Replace("background-image:url('", string.Empty)
                   .Replace("');", string.Empty))
                .ToList();

            var coordinates = document.QuerySelector(
                ".info_list li:nth-child(2)")
                ?.TextContent.Replace("GPS Координати:", string.Empty)
                .Split(",").Select(x => x.Trim())
                .ToList();

            var latitude = coordinates?[0];
            var longitude = coordinates?[1];

            return new AttractionDto
            {
                CategoryName = categoryName.Trim(),
                SubCategoryName = subCategoryName.Trim(),
                AreaName = areaName.Trim(),
                AttractionName = attractionName.Trim(),
                Location = location.Trim(),
                Description = description.Trim(),
                Altitutude = altitude?.Trim(),
                Latitude = latitude,
                Longitude = longitude,
                ImagesUrls = urls
            };
        }
    }
}
