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
    public class HomeTheaterController : BaseHomeController
    {
         private readonly dynamic shapeFactory;
         public HomeTheaterController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
             : base(workContextAccessor)
         {
             PageIndex = 1;
             this.shapeFactory = shapeFactory;
         }

         [HttpGet]
         [Url("phim-chieu-rap.html")]
         public ActionResult Index()
         {
             UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
             SiteId = (int)Site.Home;
             var serviceCategory = WorkContext.Resolve<ICategoryService>();
             serviceCategory.LanguageCode = WorkContext.CurrentCulture;
             serviceCategory.SiteId = (int)Site.Home;
             var category = serviceCategory.GetByIdCache((int)FixCategories.FilmTheater);
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
                 #region BannerPageLeft
                 var bannerPageLeft = widget.GetWidget(HomeWidgets.BannerPageLeft.ToString());
                 if (bannerPageLeft != null)
                 {
                     var widgetShape = shapeFactory.Widget();
                     widgetShape.Widget = bannerPageLeft;
                     WorkContext.Layout.AdBannerPageLeft(widgetShape);
                 }
                 #endregion

                 #region BannerPageCenter
                 var bannerPageCenter = widget.GetWidget(HomeWidgets.BannerPageCenter.ToString());
                 if (bannerPageCenter != null)
                 {
                     var widgetShape = shapeFactory.Widget();
                     widgetShape.Widget = bannerPageCenter;
                     WorkContext.Layout.AdBannerPageCenter(widgetShape);
                 }
                 #endregion

                 #region BannerPageRight
                 var bannerPageRight = widget.GetWidget(HomeWidgets.BannerPageRight.ToString());
                 if (bannerPageRight != null)
                 {
                     var widgetShape = shapeFactory.Widget();
                     widgetShape.Widget = bannerPageRight;
                     WorkContext.Layout.AdBannerPageRight(widgetShape);
                 }
                 #endregion

                 #region BannerContentLeftSeventh
                 var bannerContentLeftSeventh = widget.GetWidget(HomeWidgets.BannerContentLeftSeventh.ToString());
                 if (bannerContentLeftSeventh != null)
                 {
                     var widgetShape = shapeFactory.Widget();
                     widgetShape.Widget = bannerContentLeftSeventh;
                     WorkContext.Layout.AdBannerContentLeftSeventh(widgetShape);
                 }
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

             #region HomeSliderBanner
             var modelSlider = new DataViewerModel { Data = BuildSlider(SiteId, (int)SliderPages.FilmTheater) };
             var viewSlider = viewRenderer.RenderPartialView(Extensions.Constants.SliderBannerFilePath, modelSlider);
             WorkContext.Layout.SliderBanner.Add(new MvcHtmlString(viewSlider));

             var viewRegisterResource = viewRenderer.RenderPartialView(Extensions.Constants.RegisterResourceFilePath, null);
             WorkContext.Layout.RegisterResource.Add(new MvcHtmlString(viewRegisterResource));
             #endregion

             #region HomeFooterLogoInformation
             var homeFooterLogoInformation = widget.GetWidget(HomeWidgets.HomeFooterLogoInformation.ToString());
             if (homeFooterLogoInformation != null)
             {
                 var widgetShape = shapeFactory.Widget();
                 widgetShape.Widget = homeFooterLogoInformation;
                 WorkContext.Layout.FooterLogoInformation(widgetShape);
             }
             #endregion
             #region DisplayStatistic1
             var modelStatistic1 = new DataViewCategoryModel
             {
                 Type = (int)HomeDisplayFilmType.StatisticalFilmRetail,
                 Title = "PHIM LẺ XEM NHIỀU NHẤT"
             };
             var viewStatistic1 = viewRenderer.RenderPartialView(Extensions.Constants.Statistic1FilePath, modelStatistic1);
             WorkContext.Layout.DisplayStatistic1.Add(new MvcHtmlString(viewStatistic1));
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
             WorkContext.Layout.DisplayTags.Add(new MvcHtmlString(viewTags));
             #endregion

             #region CategoryContentFirst
             var modelFirst = new DataViewCategoryModel();
             var countryService = WorkContext.Resolve<ICountryService>();
             var filmTypesService = WorkContext.Resolve<IFilmTypesService>();
             modelFirst.Breadcrumb = BuildBreadcrumb(category);
             modelFirst.ListCountries = countryService.GetRecords().ToList();
             modelFirst.ListFilmTypes = filmTypesService.GetByType(WorkContext.CurrentCulture, SiteId, 1).ToList();
             modelFirst.SearchOrderBy = EnumExtensions.GetListItems<SearchOrderBy>();

             modelFirst.SelectedFilmTypes = 0;
             if (Request.QueryString["theloaiphim"] != null)
             {
                 modelFirst.SelectedFilmTypes = int.Parse(Request.QueryString["theloaiphim"]);
             }

             modelFirst.SelectedCountry = 0;
             if (Request.QueryString["quocgia"] != null)
             {
                 modelFirst.SelectedCountry = int.Parse(Request.QueryString["quocgia"]);
             }

             modelFirst.SelectedOrderBy = "0";
             if (Request.QueryString["sortOrder"] != null)
             {
                 modelFirst.SelectedOrderBy = Request.QueryString["sortOrder"];
             }

             modelFirst.SelectedSortBy = (int)SearchSortBy.DESC;
             if (Request.QueryString["sortBy"] != null)
             {
                 modelFirst.SelectedSortBy = int.Parse(Request.QueryString["sortBy"]);
             }
             modelFirst.SliderName = "DataListView";
             modelFirst.CurrentCategory = category;
             modelFirst.PageIndex = PageIndex;
             modelFirst.PageSize = PageSize;

             var service = WorkContext.Resolve<IFilmService>();
             service.SiteId = SiteId;
             service.LanguageCode = WorkContext.CurrentCulture;
             var totalRow = 0;
             var listFilmCategory = service.GetByFilmTheater(
                 SiteId, WorkContext.CurrentCulture,
                 modelFirst.SelectedFilmTypes,
                 modelFirst.SelectedCountry,
                 int.Parse(modelFirst.SelectedOrderBy), modelFirst.SelectedSortBy,
                 modelFirst.PageIndex, modelFirst.PageSize, out totalRow);
             modelFirst.TotalRow = totalRow;
             modelFirst.ListFilms = listFilmCategory;

             var viewFilmFirst = viewRenderer.RenderPartialView(Extensions.Constants.FilmTheaterViewFilePath, modelFirst);
             WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFilmFirst));
             #endregion
         }
    }
}
