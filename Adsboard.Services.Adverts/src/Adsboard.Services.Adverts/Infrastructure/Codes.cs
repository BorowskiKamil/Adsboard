namespace Adsboard.Services.Adverts.Infrastructure
{
    public static class Codes
    {
        public static string AdvertNotFound => "advert_not_found";
        public static string SavingAdvertError => "saving_advert_error";
        public static string AdvertAlreadyArchived => "advert_already_archived";
        public static string WrongAdvertTitle => "wrong_advert_title";
        public static string WrongAdvertDescription => "wrong_advert_description";
        public static string CategoryNotFound => "category_not_found";
        public static string UserNotFound => "user_not_found";
        public static string CategoryAlreadyExists => "category_already_exists";
        public static string UserAlreadyExists => "user_already_exists";
        public static string SavingCategoryError => "saving_category_error";
        public static string SavingUserError => "saving_user_error"; 
        public static string RemovingCategoryError => "removing_category_error";
        public static string RemovingUserError => "removing_user_error";
    }
}