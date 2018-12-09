using System;
using System.Configuration;

namespace CMSSolutions.Websites.Extensions
{
    public class Constants
    {
        public const string SearchText = "SearchText";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string LanguageCode = "LanguageCode";
        public const string SiteId = "SiteId";
        public const string CategoryId = "CategoryId";
        public const string CategoryIds = "CategoryIds";
        public const string ParentId = "ParentId";
        public const string ServerId = "ServerId";
        public const string IsNull = "null";
        public const string IsUndefined = "undefined";
        public const string UserId = "UserId";
        public const string DirectorId = "DirectorId";
        public const string ActorId = "ActorId";
        public const string FilmTypesId = "FilmTypesId";
        public const string FilmGroup = "FilmGroup";
        public const string StatusId = "StatusId";
        public const string TypeId = "TypeId";
        public const string BankCode = "BankCode";
        public const string CardCode = "CardCode";
        public const string Locked = "Locked";
        public const string TypeSearch = "TypeSearch";
        public const string ChildrenFolders = "ChildrenFolders";
        public const string RootFolders = "RootFolders";
        public const string FolderDay = "FolderDay";
        public const string ValueAll = "All";
        public const string FilmId = "FilmId";
        public const string Id = "id";
        public const string PageId = "PageId";
        public const string AdId = "AdId";
        public const string CountryId = "CountryId";
        public const string GlobalCapcha = "GLObAL_CAPCHA";
        public const string GlobalCustomerCode = "GLObAL_LOGIN_USER_CODE";
        public const string GlobalUserName = "GLObAL_LOGIN_USERNAME";
        public const string GlobalFullName = "GLObAL_LOGIN_USER_FULLNAME";
        public const string GlobalUserId = "GLObAL_LOGIN_USERID";
        public const string GlobalVipXu = "GLObAL_LOGIN_USER_VIPXU";
        public const string GlobalSiteId = "GLObAL_SITE_ID";
        public const string TransactionCode = "TRANSACTION_CODE";
        public const string CurrentDate = "CURRENT_DATETIME";
        public const string UrlLoginHistory = "URL_LOGIN_HISTORY";
        public const string PageIndex = "pageIndex";
        public const string ReturnUrl = "returnUrl";
        public const string BackPageIndex = "BackPageIndex";

        public const string GlobalTotalMoney = "GLObAL_TOTAL_MONEY";
        public const string GlobalTotalDay = "GLObAL_TOTAL_DAY";
        public const string GlobalStartDate = "GLObAL_START_DATE";
        public const string GlobalEndDate = "GLObAL_END_DATE";

        public const string DefaultPassword = "Viphd@123456";
        
        public const string CssControlCustom = "form-control-custom";
        public const string RecieveSmsParameters = "RecieveSms?partnerid={partnerid}&moid={moid}&userid={userid}&shortcode={shortcode}&keyword={keyword}&content={content}&transdate={transdate}&checksum={checksum}&amount={amount}";
        public const string SMSSyntax = "Tin nhan sai cu phap vui long kiem tra lai.";
        public const string SMSCustomerNotFound = "Rat tiec ID khong ton tai. Vui long dang nhap viphd.vn vao muc GOI TAI TRO va nhap ma giao dich sau {0}";
        public const string SMSKeywordNotFound = "Tin nhan sai cu phap. Soan VIPXU HD GUI 8376 de nhan huong dan dich vu cua 8x76 (Cuoc phi tin nhan 3.000VND)";
        public const string SMSUserHelp = "Soan tin VIPXU MaKH GUI 8576(5.000VND/SMS) hoac VIPXU MaKH GUI 8676(10.000VND/SMS) hoac VIPXU MaKH GUI 8776(15.000VND/SMS)";
        public const string SMSSystemError = "He thong thanh toan dang qua tai ban vui long thu lai sau.";
        public const string VipXuKeyword = "VIPXU";

        public const string CategoryContentFirstViewFilePath = "~/Views/Shared/CategoryContentFirst.cshtml";
        public const string HomeFilmHotViewFilePath = "~/Views/Shared/HomeFilmHot.cshtml";
        public const string SliderOneViewFilePath = "~/Views/Shared/SliderViewOne.cshtml";
        public const string CategoryListViewViewFilePath = "~/Views/Shared/CategoryListView.cshtml";
        public const string CategoryJJListViewFilePath = "~/Views/Shared/CategoryJJListView.cshtml";
        public const string NewsDetailsViewFilePath = "~/Views/Shared/NewsDetails.cshtml";
        public const string NewsCategoryViewFilePath = "~/Views/Shared/NewsCategory.cshtml";
        public const string FilmTheaterViewFilePath = "~/Views/Shared/FilmTheater.cshtml";
        public const string ShowListViewFilePath = "~/Views/Shared/ShowListView.cshtml";
        public const string ClipListViewFilePath = "~/Views/Shared/ClipListView.cshtml";
        public const string TrailerListViewFilePath = "~/Views/Shared/TrailerListView.cshtml";
        public const string SupportsViewFilePath = "~/Views/Shared/HomeSupports.cshtml";
        public const string ListMoviesViewFilePath = "~/Views/Shared/ListMovies.cshtml";
        public const string SearchResultsViewFilePath = "~/Views/Shared/SearchResults.cshtml";

        public const string CategoryContentSecondViewFilePath = "~/Views/Shared/CategoryContentSecond.cshtml";
        public const string CategoryContentThirdViewFilePath = "~/Views/Shared/CategoryContentThird.cshtml";
        public const string CategoryContentFourthViewFilePath = "~/Views/Shared/CategoryContentFourth.cshtml";
        public const string CategoryContentFifthViewFilePath = "~/Views/Shared/CategoryContentFifth.cshtml";
        public const string CategoryContentSixthViewFilePath = "~/Views/Shared/CategoryContentSixth.cshtml";
        public const string CategoryContentSeventhViewFilePath = "~/Views/Shared/CategoryContentSeventh.cshtml";
        public const string SocialNetworkViewFilePath = "~/Views/Shared/SocialNetwork.cshtml";
        public const string CustomerViewFilePath = "~/Views/Shared/CustomerInfomations.cshtml";
        public const string NewsViewFilePath = "~/Views/Shared/NewsViewer.cshtml";
        public const string TagViewFilePath = "~/Views/Shared/Tags.cshtml";
        public const string DivDisplayFilePath = "~/Views/Shared/DivDisplay.cshtml";

        public const string Statistic1FilePath = "~/Views/Shared/Statistic1.cshtml";
        public const string Statistic2FilePath = "~/Views/Shared/Statistic2.cshtml";
        public const string Statistic3FilePath = "~/Views/Shared/Statistic3.cshtml";
        public const string Statistic4FilePath = "~/Views/Shared/Statistic4.cshtml";
        public const string Statistic5FilePath = "~/Views/Shared/Statistic5.cshtml";
        public const string Statistic6FilePath = "~/Views/Shared/Statistic6.cshtml";

        public const string RegisterResourceFilePath = "~/Views/Shared/HomeRegisterResource.cshtml";
        public const string MoviesFilePath = "~/Views/Shared/Movies.cshtml";
        public const string FacebookCommentsFilePath = "~/Views/Shared/FacebookComments.cshtml";
        public const string FilmRateFilePath = "~/Views/Shared/FilmRate.cshtml";
        public const string SliderBannerFilePath = "~/Views/Shared/HomeSliderBanner.cshtml";
        public const string DigCoinsFilePath = "~/Views/Shared/DigCoins.cshtml";
        public const string DaoXuInformationsFilePath = "~/Views/Shared/DaoXuInformations.cshtml";
        public const string DaoXuHelpsFilePath = "~/Views/Shared/DaoXuHelps.cshtml";
        public const string DaoXuLogsFilePath = "~/Views/Shared/DaoXuLogs.cshtml";

        public const string ImageMale = "/Images/avatars/avatar-no-image.png";
        public const string ImageFemale = "/Images/avatars/female-user.png";
        public const string DateTimeFomat = "dd/MM/yyyy";
        public const string DateTimeFomat2 = "dd.MM.yyyy";
        public const string DateTimeFomatFull = "dd/MM/yyyy H:mm:ss";
        public const string DateTimeFomatFull2 = "dd-MM-yyyy H:mm:ss";
        public const string DateTimeMin = "01/01/1900";
        public const string CssThumbsSize = "thumbs-size";
        public const string HeaderTitle = "HeaderTitle";
        public const string HeaderDescription = "HeaderDescription";
        public const string HeaderKeywords = "HeaderKeywords";
        //public static string SiteName = ConfigurationManager.AppSettings["SiteName"];
        //public static string ContactEmail = ConfigurationManager.AppSettings["ContactEmail"];
        public static string HomeDomainName = ConfigurationManager.AppSettings["HomeDomainName"];
        public static string JJDomainName = ConfigurationManager.AppSettings["JJDomainName"];
        public static string SmsLogFiles = ConfigurationManager.AppSettings["SmsLogFiles"];
        public static string SearchEngineDBPath = ConfigurationManager.AppSettings["SearchEngineDBPath"];
        public static string JWplayerSkin = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JWplayerSkin"]));
        public static string JWplayerKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JWplayerKey"]));
        public static string StreamingUrl = ConfigurationManager.AppSettings["StreamingUrl"];

        public class CacheKeys
        {
            public const string CATEGORY_ALL_TABLE = "CATEGORY_ALL_TABLE_{0}_{1}";
            public const string ARTICLES_BY_CATEGORY_ID = "ARTICLES_BY_CATEGORY_ID";
            public const string CATALOG_ALL_TABLE = "CATALOG_ALL_TABLE";
            public const string SETTING_ALL_TABLE = "SETTING_ALL_TABLE_{0}";
            public const string PRODUCT_ALL_TABLE = "PRODUCT_ALL_TABLE_{0}";
            public const string HOME_FILM_HOT = "HOME_FILM_HOT";
            public const string HOME_FILM_RETAIL = "HOME_FILM_RETAIL";
            public const string HOME_FILM_MANY_EPISODES = "HOME_FILM_MANY_EPISODES";
            public const string HOME_FILM_JJ_CHANNEL_INTRODUCE = "HOME_FILM_JJ_CHANNEL_INTRODUCE";
            public const string HOME_FILM_THEATER = "HOME_FILM_THEATER"; 
            public const string HOME_FILM_TV_SHOWS = "HOME_FILM_TV_SHOWS";
            public const string HOME_FILM_TV_CLIPS = "HOME_FILM_TV_CLIPS";
            public const string HOME_CATEGORY_ID = "HOME_CATEGORY_{0}";
            public const string SEARCH_TEXT = "HOME_SEARCH_TEXT_{0}";
            public const string HOME_STATISTICAL_1_TYPE = "HOME_STATISTICAL_1_TYPE_{0}";
            public const string HOME_STATISTICAL_2_TYPE = "HOME_STATISTICAL_2_TYPE_{0}";
            public const string HOME_STATISTICAL_3_TYPE = "HOME_STATISTICAL_3_TYPE_{0}";
            public const string HOME_STATISTICAL_4_TYPE = "HOME_STATISTICAL_4_TYPE_{0}";
            public const string HOME_STATISTICAL_5_TYPE = "HOME_STATISTICAL_5_TYPE_{0}";
            public const string HOME_SLIDER_PAGE = "HOME_SLIDER_PAGE_{0}";
            public const string CUSTOMERS_DATA_CARD = "CUSTOMERS_DATA_CARD";
        }
    }
}