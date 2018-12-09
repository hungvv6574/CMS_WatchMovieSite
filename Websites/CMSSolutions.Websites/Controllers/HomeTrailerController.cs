using System.Linq;
using System.Text;
using System.Web.Mvc;
using CMSSolutions.ContentManagement.Widgets.Services;
using CMSSolutions.DisplayManagement;
using CMSSolutions.Extensions;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeTrailerController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeTrailerController(
            IWorkContextAccessor workContextAccessor, 
            IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
            PageIndex = 1;
        }

        [HttpGet]
        [Url("{alias}/t{id}.html")]
        public ActionResult Index(string alias, int id)
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            SiteId = (int)Site.Home;
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            serviceCategory.SiteId = SiteId;
            var category = serviceCategory.GetByIdCache(id);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            if (Request.QueryString["trang"] != null)
            {
                PageIndex = int.Parse(Request.QueryString["trang"]);
            }
            PageSize = 40;

            BuildModulesCategory(category);

            return View();
        }

        private string BuildBreadcrumb(CategoryInfo category)
        {
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            serviceCategory.SiteId = (int)Site.Home;

            var html = new StringBuilder();
            html.Append("<div class=\"mh-breadcrumb-dv\">");
            html.Append("<div>");
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\"><span><img width=\"22\" src=\"/Images/themes/ico-home_trans.png\"></span></a>", "Dành cho víp", "/");
            html.Append("</div>/ ");
            if (category.ParentId > 0)
            {
                var parent = serviceCategory.GetByIdCache(category.ParentId);
                html.Append("<div>");
                html.AppendFormat("<a title=\"{0}\" href=\"{1}\"><span>{0}</span></a>", parent.ShortName, Url.Action("Index", "HomeCategory", new { @alias = parent.Alias, @id = parent.Id }));
                html.Append("</div>/ ");
            }
            html.Append("<div>");
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\"><span>{0}</span></a>", category.ShortName, "#");
            html.Append("</div>");

            html.Append("</div>");
            return html.ToString();
        }

        private void BuildModulesCategory(CategoryInfo category)
        {
            var widget = WorkContext.Resolve<IWidgetService>();
            var viewRenderer = new ViewRenderer { Context = ControllerContext };
            if (!IsVip)
            {
                #region BannerContentLeftSeventh
                var bannerContentLeftSeventh = widget.GetWidget(HomeWidgets.BannerContentLeftSeventh.ToString());
                if (bannerContentLeftSeventh != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerContentLeftSeventh;
                    WorkContext.Layout.AdBannerContentLeftSeventh(widgetShape);
                }
                #endregion

                #region BannerRightFree
                WorkContext.Layout.AdBannerRightFree.Add(new MvcHtmlString("<div style=\"margin-top: 125px;\"></div>"));
                #endregion

                #region BannerRightFirst
                var bannerRightFirst = widget.GetWidget(HomeWidgets.BannerRightFirst.ToString());
                if (bannerRightFirst != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerRightFirst;
                    WorkContext.Layout.AdBannerRightFirst(widgetShape);
                }
                #endregion

                #region BannerRightSecond
                var bannerRightSecond = widget.GetWidget(HomeWidgets.BannerRightSecond.ToString());
                if (bannerRightSecond != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerRightSecond;
                    WorkContext.Layout.AdBannerRightSecond(widgetShape);
                }
                #endregion

                #region BannerRightThird
                var bannerRightThird = widget.GetWidget(HomeWidgets.BannerRightThird.ToString());
                if (bannerRightThird != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerRightThird;
                    WorkContext.Layout.AdBannerRightThird(widgetShape);
                }
                #endregion

                #region BannerRightFourth
                var bannerRightFourth = widget.GetWidget(HomeWidgets.BannerRightFourth.ToString());
                if (bannerRightFourth != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerRightFourth;
                    WorkContext.Layout.AdBannerRightFourth(widgetShape);
                }
                #endregion
            }
            else
            {
                #region BannerPageLeftVip
                var bannerPageLeftVip = widget.GetWidget(HomeWidgets.BannerPageLeftVip.ToString());
                if (bannerPageLeftVip != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerPageLeftVip;
                    WorkContext.Layout.AdBannerPageLeftVip(widgetShape);
                }
                #endregion

                #region BannerPageCenterVip
                var bannerPageCenterVip = widget.GetWidget(HomeWidgets.BannerPageCenterVip.ToString());
                if (bannerPageCenterVip != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerPageCenterVip;
                    WorkContext.Layout.AdBannerPageCenterVip(widgetShape);
                }
                #endregion

                #region BannerPageRightVip
                var bannerPageRightVip = widget.GetWidget(HomeWidgets.BannerPageRightVip.ToString());
                if (bannerPageRightVip != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerPageRightVip;
                    WorkContext.Layout.AdBannerPageRightVip(widgetShape);
                }
                #endregion
            }

            #region HomeFooterLogoInformation
            var homeFooterLogoInformation = widget.GetWidget(HomeWidgets.HomeFooterLogoInformation.ToString());
            if (homeFooterLogoInformation != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = homeFooterLogoInformation;
                WorkContext.Layout.FooterLogoInformation(widgetShape);
            }
            #endregion

            #region SocialNetwork
            var socialNetwork = widget.GetWidget(HomeWidgets.DisplaySocialNetwork.ToString());
            if (socialNetwork != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = socialNetwork;
                WorkContext.Layout.DisplaySocialNetwork(widgetShape);
            }
            #endregion

            #region Tags
            var serviceTags = WorkContext.Resolve<ITagService>();
            var modelTags = new TagViewModel { ListTags = serviceTags.GetDisplayTags() };
            var viewTags = viewRenderer.RenderPartialView(Extensions.Constants.TagViewFilePath, modelTags);
            viewTags = "<div style=\"margin-top: 110px;\"></div>" + viewTags;
            WorkContext.Layout.DisplayTags.Add(new MvcHtmlString(viewTags));
            #endregion

            #region CategoryContentFirst
            var modelFirst = new DataViewCategoryModel();
            var filmTypesService = WorkContext.Resolve<IFilmTypesService>();
            modelFirst.Breadcrumb = BuildBreadcrumb(category);
            modelFirst.ListFilmTypes = filmTypesService.GetByType(WorkContext.CurrentCulture, SiteId, 2).ToList();
            modelFirst.SearchOrderBy = EnumExtensions.GetListItems<SearchOrderBy>();

            modelFirst.SliderName = "DataListView";
            modelFirst.CurrentCategory = category;
            modelFirst.PageIndex = PageIndex;
            modelFirst.PageSize = PageSize;

            var service = WorkContext.Resolve<IFilmService>();
            service.SiteId = SiteId;
            service.LanguageCode = WorkContext.CurrentCulture;
            var totalRow = 0;
            var listFilmCategory = service.GetByTrailer(
                SiteId, WorkContext.CurrentCulture,
                modelFirst.PageIndex, modelFirst.PageSize, out totalRow);
            modelFirst.TotalRow = totalRow;
            modelFirst.ListFilms = listFilmCategory;
            var viewFilmFirst = viewRenderer.RenderPartialView(Extensions.Constants.TrailerListViewFilePath, modelFirst);
            viewFilmFirst = "<div style=\"margin-top: 125px;\"></div>" + viewFilmFirst;
            WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFilmFirst));
            #endregion
        }
    }
}
