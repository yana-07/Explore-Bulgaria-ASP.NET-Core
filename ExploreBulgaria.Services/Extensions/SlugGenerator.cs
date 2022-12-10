using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ExploreBulgaria.Services.Extensions
{
    public static class SlugGenerator
    {
        public static string GenerateSlug(this IAttractionModel model)
        {
            StringBuilder sb = new StringBuilder(); 
            sb.Append($"{model.Name} ");
            sb.Append($"{model.CategoryName} ");
            sb.Append($"{model.SubcategoryName} ");
            sb.Append($"{model.RegionName} ");
            sb.Append($"{model.VillageName} ");

            var str = sb.ToString().Trim();
            str = ConvertCyrilicToLatinLetters(str);

            str = str.Replace(" ", "-").Replace("--", "-").Replace("--", "-");

            str = Regex.Replace(str, @"[^a-zA-z0-9_-]+", string.Empty, RegexOptions.Compiled);

            return str.Substring(0, Math.Min(100, str.Length)).Trim('-');
        }

        private static string ConvertCyrilicToLatinLetters(string input)
        {
            var bulgarianLetters = new[]
            {
                "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п",
                "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ь", "ю", "я",
            };

            var latinRepresentationsOfBulgarianLetters = new[]
            {
                "a", "b", "v", "g", "d", "e", "j", "z", "i", "y", "k",
                "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "h",
                "c", "ch", "sh", "sht", "u", "i", "yu", "ya",
            };

            for (int i = 0; i < bulgarianLetters.Length; i++)
            {
                input = input.Replace(bulgarianLetters[i], latinRepresentationsOfBulgarianLetters[i]);
                input = input.Replace(bulgarianLetters[i].ToUpper(), CapitalizeFirstLetter(latinRepresentationsOfBulgarianLetters[i]));
            }

            return input;
        }

        private static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + input.Substring(1);
        }
    }
}
