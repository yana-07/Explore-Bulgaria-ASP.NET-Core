namespace ExploreBulgaria.Services.Common.Constants
{
    public static class EntityAndVMConstants
    {
        public const int NameMaxLength = 50;
        public const int NameMinLength = 5;
        public const int TextMaxLength = 1000;

        public class User
        {
            public const int UserNameMaxLength = 40;
            public const int UserNameMinLength = 5;
            public const int EmailMaxLength = 60;
            public const int EmailMinLength = 10;
            public const int PasswordMinLength = 6;
        }

        public class Attraction
        {
            public const int AttractionNameMaxLength = 120;
            public const int AttractionNameMinLength = 5;
            public const int DescriptionMaxLength = 10000;
            public const int DescriptionMinLength = 100;
            public const int GuidMaxLength = 36;
        }

        public class Region
        {
            public const int RegionNameMaxLength = 50;
            public const int RegionNameMinLength = 3;
        }

        public class Image
        {
            public const int ExtensionMaxLength = 7;
            public const int BlobStorageUrlMaxLength = 42;
        }
    }
}
