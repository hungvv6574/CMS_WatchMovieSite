using System.Collections.Generic;
using CMSSolutions.Web.Security.Permissions;

namespace CMSSolutions.Websites.Permissions
{
    public class AdminPermissions : IPermissionProvider
    {
        public static readonly Permission ManagerSlider = new Permission
        {
            Name = "ManagerSlider",
            Category = "Management",
            Description = "Quản lý slider"
        };

        public static readonly Permission ManagerFilms = new Permission
        {
            Name = "ManagerFilms",
            Category = "Management",
            Description = "Quản lý phim"
        };

        public static readonly Permission ManagerFilmTypes = new Permission
        {
            Name = "ManagerFilmTypes",
            Category = "Management",
            Description = "Quản lý thể loại phim"
        };

        public static readonly Permission ManagerServerFilms = new Permission
        {
            Name = "ManagerServerFilms",
            Category = "Management",
            Description = "Quản lý máy chủ phim"
        };

        public static readonly Permission ManagerEpisodes = new Permission
        {
            Name = "ManagerEpisodes",
            Category = "Management",
            Description = "Quản lý các tập phim"
        };

        public static readonly Permission ManagerDirectors = new Permission
        {
            Name = "ManagerDirectors",
            Category = "Management",
            Description = "Quản lý đạo diễn phim"
        };

        public static readonly Permission ManagerCountries = new Permission
        {
            Name = "ManagerCountries",
            Category = "Management",
            Description = "Quản lý nước sản xuất phim"
        };

        public static readonly Permission ManagerCollections = new Permission
        {
            Name = "ManagerCollections",
            Category = "Management",
            Description = "Quản lý các bộ phim"
        };

        public static readonly Permission ManagerActors = new Permission
        {
            Name = "ManagerActors",
            Category = "Management",
            Description = "Quản lý diễn viên"
        };

        public static readonly Permission ManagerTags = new Permission
        {
            Name = "ManagerTags",
            Category = "Management",
            Description = "Quản lý tags"
        };

        public static readonly Permission ManagerArticles = new Permission
        {
            Name = "ManagerArticles",
            Category = "Management",
            Description = "Quản lý tin tức"
        };
        public static readonly Permission ManagerDownloadGames = new Permission
        {
            Name = "ManagerDownloadGames",
            Category = "Management",
            Description = "Quản lý sự kiện đào xu"
        };
        public static readonly Permission ManagerSites = new Permission
        {
            Name = "ManagerSites",
            Category = "Management",
            Description = "Quản lý các site"
        };

        public static readonly Permission ManagerCategories = new Permission
        {
            Name = "ManagerCategories",
            Category = "Management",
            Description = "Quản lý chuyên mục"
        };

        public static readonly Permission ManagerAdvertisementGroup = new Permission
        {
            Name = "ManagerAdvertisementGroup",
            Category = "Management",
            Description = "Quản lý nhóm quảng cáo"
        };

        public static readonly Permission ManagerAdvertisement = new Permission
        {
            Name = "ManagerAdvertisement",
            Category = "Management",
            Description = "Quản lý quảng cáo"
        };
        
        public static readonly Permission ManagerVast = new Permission
        {
            Name = "ManagerVast",
            Category = "Management",
            Description = "Quản lý Vast"
        };

        public static readonly Permission ManagerSupports = new Permission
        {
            Name = "ManagerSupports",
            Category = "Management",
            Description = "Hỏi đáp"
        };

        public static readonly Permission ManagerRates = new Permission
        {
            Name = "ManagerRates",
            Category = "Management",
            Description = "Đánh giá và báo lỗi"
        };
        
        public static readonly Permission ManagerAdmin = new Permission
        {
            Name = "ManagerAdmin",
            Category = "Management",
            Description = "Bảng điều khiển"
        };

        public static readonly Permission ManagerBankCards = new Permission
        {
            Name = "ManagerBankCards",
            Category = "Management",
            Description = "Quản lý dịch vụ thẻ ngân hàng"
        };

        public static readonly Permission ManagerCardTypes = new Permission
        {
            Name = "ManagerCardTypes",
            Category = "Management",
            Description = "Quản lý loại thẻ cào"
        };

        public static readonly  Permission ManagerVipCards = new Permission
        {
            Name = "ManagerVipCards",
            Category = "Management",
            Description = "Quản lý thẻ VIP"
        };

        public static readonly Permission ManagerPromotions = new Permission
        {
            Name = "ManagerPromotions",
            Category = "Management",
            Description = "Quản lý khuyến mãi"
        };

        public static readonly Permission ManagerCustomers = new Permission
        {
            Name = "ManagerCustomers",
            Category = "Management",
            Description = "Quản lý khách hàng"
        };

        public static readonly Permission ManagerFilmFiles  = new Permission
        {
            Name = "ManagerFilmFiles",
            Category = "Management",
            Description = "Quản lý video files"
        };

        public static readonly Permission ManagerSystemLogs = new Permission
        {
            Name = "ManagerSystemLogs",
            Category = "Management",
            Description = "Quản lý logs hệ thống"
        };

        public static readonly Permission ManagerAdminReportSms = new Permission
        {
            Name = "ManagerAdminReportSms",
            Category = "Management",
            Description = "Đối soát SMS"
        };

        public static readonly Permission ManagerAdminReportAtm = new Permission
        {
            Name = "ManagerAdminReportAtm",
            Category = "Management",
            Description = "Đối soát ATM"
        };

        public static readonly Permission ManagerAdminReportCard = new Permission
        {
            Name = "ManagerAdminReportCard",
            Category = "Management",
            Description = "Đối soát thẻ cào"
        };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ManagerAdmin;
            yield return ManagerCategories;
            yield return ManagerSites;
            yield return ManagerArticles;
            yield return ManagerDirectors;
            yield return ManagerCountries;
            yield return ManagerCollections;
            yield return ManagerActors;
            yield return ManagerEpisodes;
            yield return ManagerServerFilms;
            yield return ManagerFilmTypes;
            yield return ManagerFilms;
            yield return ManagerVipCards;
            yield return ManagerCustomers;
            yield return ManagerFilmFiles;
            yield return ManagerSystemLogs;
            yield return ManagerPromotions;
            yield return ManagerAdminReportSms;
            yield return ManagerAdminReportAtm;
            yield return ManagerAdminReportCard;
            yield return ManagerSupports;
            yield return ManagerAdvertisementGroup;
            yield return ManagerAdvertisement;
            yield return ManagerVast;
            yield return ManagerRates;
            yield return ManagerDownloadGames;
            yield return ManagerSlider;
            yield return ManagerTags;
        }   
    }
}