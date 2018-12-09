using CMSSolutions.Localization;
using CMSSolutions.Web.UI.Navigation;
using CMSSolutions.Websites.Permissions;

namespace CMSSolutions.Websites.Menus
{
    public class NavigationProvider : INavigationProvider
    {
        public Localizer T { get; set; }

        public NavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Home"), "0", BuildHomeMenu);
            builder.Add(T("Danh mục"), "1", BuildManagers);

            builder.Add(T("Quản lý chuyên mục"), "2", b => b.Action("Index", "AdminCategory", new { area = "" })
                .IconCssClass("fa-th")
                .Permission(AdminPermissions.ManagerCategories));

            builder.Add(T("Quản lý tin tức"), "3", b => b.Action("Index", "AdminArticles", new { area = "" })
                .IconCssClass("fa-rss-square")
                .Permission(AdminPermissions.ManagerArticles));

            builder.Add(T("Quản lý phim"), "4", BuildMenuFilms);
            builder.Add(T("Quản lý giao dịch"), "5", BuildMenuCustomers);
            builder.Add(T("Quản lý logs"), "6", BuildMenuLogs);
            builder.Add(T("Quản lý đối soát"), "7", BuildReports);
            builder.Add(T("Hỏi đáp"), "8", BuildQandA);
            builder.Add(T("Đào xu"), "9", b => b.Action("Index", "AdminDownloadGames", new { area = "" })
                .IconCssClass("fa-bullseye")
                .Permission(AdminPermissions.ManagerDownloadGames));
            builder.Add(T("Quảng cáo"), "10", BuildAds);
        }

        private void BuildHomeMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-home")
                .Action("Index", "Admin", new { area = "" })
                .Permission(AdminPermissions.ManagerAdmin);
        }

        private void BuildManagers(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-qrcode");

            builder.Add(T("Quản lý trang"), "0", b => b
                .Action("Index", "AdminSite", new {area = ""})
                .IconCssClass("fa-flag")
                .Permission(AdminPermissions.ManagerSites));

            builder.Add(T("Nước sản xuất"), "1", b => b
                .Action("Index", "AdminCountry", new { area = "" })
                .Permission(AdminPermissions.ManagerCountries));

            builder.Add(T("Đạo diễn"), "2", b => b
                .Action("Index", "AdminDirector", new { area = "" })
                .Permission(AdminPermissions.ManagerDirectors));

            builder.Add(T("Diễn viên"), "3", b => b
                .Action("Index", "AdminActor", new { area = "" })
                .Permission(AdminPermissions.ManagerActors));

            builder.Add(T("Thể loại phim"), "4", b => b
                .Action("Index", "AdminFilmTypes", new { area = "" })
                .Permission(AdminPermissions.ManagerFilmTypes));

            builder.Add(T("Bộ phim"), "5", b => b
                .Action("Index", "AdminCollection", new { area = "" })
                .Permission(AdminPermissions.ManagerCollections));

            builder.Add(T("Tập phim"), "6", b => b
                .Action("Index", "AdminEpisode", new { area = "" })
                .Permission(AdminPermissions.ManagerEpisodes));

            builder.Add(T("Tags"), "7", b => b
                .Action("Index", "AdminTag", new { area = "" })
                .Permission(AdminPermissions.ManagerTags));
        }

        private void BuildMenuFilms(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-film");

            builder.Add(T("Máy chủ"), "0", b => b
                .Action("Index", "AdminFilmServer", new { area = "" })
                .Permission(AdminPermissions.ManagerServerFilms));

            builder.Add(T("Videos"), "1", b => b
               .Action("Index", "FilmFiles", new { area = "" })
               .Permission(AdminPermissions.ManagerFilmFiles));

            builder.Add(T("Phim"), "2", b => b
               .Action("Index", "AdminFilm", new { area = "" })
               .Permission(AdminPermissions.ManagerFilms));

            builder.Add(T("Slider"), "3", b => b
               .Action("Index", "AdminSlider", new { area = "" })
               .Permission(AdminPermissions.ManagerSlider));
        }

        private void BuildMenuCustomers(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-shopping-cart");

            builder.Add(T("Loại thẻ cào"), "0", b => b
              .Action("Index", "AdminCardType", new { area = "" })
              .Permission(AdminPermissions.ManagerCardTypes));

            builder.Add(T("Thẻ ngân hàng"), "1", b => b
              .Action("Index", "AdminBankCard", new { area = "" })
              .Permission(AdminPermissions.ManagerBankCards));

            builder.Add(T("Thẻ VIP"), "2", b => b
              .Action("Index", "AdminVIPCard", new { area = "" })
              .Permission(AdminPermissions.ManagerVipCards));

            builder.Add(T("Khách hàng"), "3", b => b
              .Action("Index", "AdminCustomer", new { area = "" })
              .Permission(AdminPermissions.ManagerCustomers));

            builder.Add(T("Khuyến mãi"), "4", b => b
                .Action("Index", "AdminPromotion", new { area = "" })
                .Permission(AdminPermissions.ManagerPromotions));
        }

        private void BuildMenuLogs(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-bug");

            builder.Add(T("Logs hệ thống"), "0", b => b
                .Action("Index", "AdminLog", new { area = "" })
                .Permission(AdminPermissions.ManagerSystemLogs));
        }

        private void BuildReports(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa fa-bar-chart-o");

            builder.Add(T("Đối soát SMS"), "0", b => b
                .Action("Index", "AdminReportSms", new { area = "" })
                .Permission(AdminPermissions.ManagerAdminReportSms));

            builder.Add(T("Đối soát ATM"), "1", b => b
                .Action("Index", "AdminReportBank", new { area = "" })
                .Permission(AdminPermissions.ManagerAdminReportAtm));

            builder.Add(T("Đối soát thẻ cào"), "2", b => b
                .Action("Index", "AdminReportCard", new { area = "" })
                .Permission(AdminPermissions.ManagerAdminReportCard));
        }

        private void BuildAds(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-desktop");

            builder.Add(T("Danh mục"), "0", b => b
                .Action("Index", "AdminAdvertisement", new { area = "" })
                .Permission(AdminPermissions.ManagerAdvertisement));

            builder.Add(T("Nội dung"), "1", b => b
                .Action("Index", "AdminVast", new { area = "" })
                .Permission(AdminPermissions.ManagerVast));

            builder.Add(T("Phân nhóm"), "2", b => b
                .Action("Index", "AdminAdvertisementGroup", new { area = "" })
                .Permission(AdminPermissions.ManagerAdvertisementGroup));
        }

        private void BuildQandA(NavigationItemBuilder builder)
        {
            builder.IconCssClass("fa-comments-o");

            builder.Add(T("Hỏi đáp"), "0", b => b
                .Action("Index", "AdminSupport", new { area = "" })
               .Permission(AdminPermissions.ManagerSupports));

            builder.Add(T("Đánh giá, báo lỗi"), "1", b => b
                .Action("Index", "AdminRate", new { area = "" })
               .Permission(AdminPermissions.ManagerRates));
        }
    }
}