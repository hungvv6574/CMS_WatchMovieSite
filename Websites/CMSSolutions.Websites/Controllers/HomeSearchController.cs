using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMSSolutions.ContentManagement.Widgets.Services;
using CMSSolutions.DisplayManagement;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeSearchController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeSearchController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
            PageIndex = 1;
        }

        [HttpGet]
        [Url("tim-kiem")]
        public ActionResult Index()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            string keyword = Request.QueryString["keyword"];
            BuildModulesSearch(keyword);

            return View();
        }

        private void BuildModulesSearch(string keyword)
        {
            var widget = WorkContext.Resolve<IWidgetService>();
            var viewRenderer = new ViewRenderer { Context = ControllerContext };
            if (!IsVip)
            {
                
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

            #region BannerRightFirst
            var view = viewRenderer.RenderPartialView(Extensions.Constants.DivDisplayFilePath, null);
            WorkContext.Layout.AdBannerRightFirst.Add(new MvcHtmlString(view));
            #endregion

            #region HomeFacebookFanpage
            var homeFacebookFanpage = widget.GetWidget(HomeWidgets.HomeFacebookFanpage.ToString());
            if (homeFacebookFanpage != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = homeFacebookFanpage;
                WorkContext.Layout.AdFacebookFanpage(widgetShape);
            }
            #endregion

            #region News
            var viewNews = viewRenderer.RenderPartialView(Extensions.Constants.NewsViewFilePath, null);
            WorkContext.Layout.DisplayNews.Add(new MvcHtmlString(viewNews));
            #endregion

            #region Tags
            var serviceTags = WorkContext.Resolve<ITagService>();
            var modelTags = new TagViewModel { ListTags = serviceTags.GetDisplayTags() };
            var viewTags = viewRenderer.RenderPartialView(Extensions.Constants.TagViewFilePath, modelTags);
            WorkContext.Layout.DisplayTags.Add(new MvcHtmlString(viewTags));
            #endregion

            #region CategoryContentFirst
            if (Request.QueryString["trang"] != null)
            {
                PageIndex = int.Parse(Request.QueryString["trang"]);
            }
            PageSize = 100;
            SiteId = (int)Site.Home;
            ViewData[Extensions.Constants.HeaderTitle] = "Từ khóa " + keyword + " trang " + PageIndex;
            ViewData[Extensions.Constants.HeaderDescription] = "xem phim, phim hay, phim online, phim hd, phim miễn phí, xem phim hay, xem phim online, xem phim hd, xem phim miễn phí";
            ViewData[Extensions.Constants.HeaderKeywords] = "Từ khóa " + keyword + " trang " + PageIndex;

            var condition = new List<SearchCondition>
            {
                new SearchCondition(new[]
                {
                    SearchField.Title.ToString(), 
                    SearchField.Keyword.ToString(), 
                    SearchField.KeywordEN.ToString()
                }, keyword)
            };

            var service = WorkContext.Resolve<ISearchService>();
            service.SiteId = SiteId;
            var totalRow = 0;
            var data = service.Search(condition, PageIndex, PageSize, ref totalRow);

            var modelFirst = new DataViewCategoryModel();
            if (data != null && data.Count > 0)
            {
                modelFirst.ListSearchFilms = data.Where(x => x.IsFilm).ToList();
                modelFirst.ListSearchClip = data.Where(x => x.IsClip).ToList();
                modelFirst.ListSearchShow = data.Where(x => x.IsShow).ToList();
                modelFirst.ListSearchTrailer = data.Where(x => x.IsTrailer).ToList();
            }

            modelFirst.PageIndex = PageIndex;
            modelFirst.PageSize = PageSize;
            modelFirst.TotalRow = totalRow;
            modelFirst.Keyword = keyword;

            var viewFilmFirst = viewRenderer.RenderPartialView(Extensions.Constants.SearchResultsViewFilePath, modelFirst);
            WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFilmFirst));
            #endregion
        }
    }
}
