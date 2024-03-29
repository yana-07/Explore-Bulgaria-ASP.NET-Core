﻿namespace ExploreBulgaria.Services.Constants
{
    public static class ExceptionConstants
    {
        public const string InvalidAttractionId = "Invalid Attraction ID.";

        public const string InvalidCategoryId = "Invalid Category ID.";

        public const string InvalidCommentId = "Invalid Comment ID.";

        public const string InvalidVisitorId = "Invalid Visitor ID.";

        public const string InvalidUserId = "Invalid User ID.";

        public const string InvalidRegionId = "Invalid Region ID.";

        public const string InvalidAttractionTemporaryId = "Invalid AttractionTemporary ID.";

        public const string InvalidImageExtension = "Невалидно разширение на снимката. - {0}";

        public const string SavingToDatabase = "Грешка в базата данни. Моля, опитайте отново.";

        public const string CategoryAlreadyExists = "Категорията, която се опитвате да добавите, вече съществува.";

        public const string SubcategoryAlreadyExists = "Подкатегорията, която се опитвате да добавите, вече съществува.";

        public const string RegionAlreadyExists = "Регионът / градът, който се опитвате да добавите, вече съществува.";

        public const string VillageAlreadyExists = "Селото, което се опитвате да добавите, вече съществува.";

        public const string EmailSenderException = "Error while sendin an email.";
    }
}
