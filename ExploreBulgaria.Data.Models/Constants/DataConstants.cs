namespace ExploreBulgaria.Data.Models.Constants
{
    public class DataConstants
    {
        public const int NameMaxLength = 50;

        public class User
        {
            public const int UserNameMaxLength = 40;
            public const int EmailMaxLength = 60;
        }

        public class Attraction
        {
            public const int DescriptionMaxLength = 200;
        }

        public class Comment
        {
            public const int TextMaxLength = 300;
        }

        public class Image
        {
            public const int ExtensionMaxLength = 7; 
        }
    }
}
