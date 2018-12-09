using System;
using System.Text;
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
    public class HomeNewsController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeNewsController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory)
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
            PageIndex = 1;
        }

        [Url("tintuc.html")]
        public ActionResult Index()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            BuildNewsModules((int)FixCategories.AllNews, true);

            return View();
        }

        [Url("{alias}/nc{id}.html")]
        public ActionResult ViewNewsCategory(string alias, int id)
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            BuildNewsModules(id, true);

            return View();
        }

        [Url("tintuc/{alias}/n{id}.html")]
        public ActionResult ViewDetails(string alias, int id)
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            BuildNewsModules(id, false);

            return View();
        }

        private void BuildNewsModules(int id, bool isCategory)
        {
            var widget = WorkContext.Resolve<IWidgetService>();
            var viewRenderer = new ViewRenderer { Context = ControllerContext };
            if (!IsVip)
            {
                #region BannerRightFifth
                var bannerRightFifth = widget.GetWidget(HomeWidgets.BannerRightFifth.ToString());
                if (bannerRightFifth != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerRightFifth;
                    WorkContext.Layout.AdBannerRightFifth(widgetShape);
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

            #region SocialNetwork
            var socialNetwork = widget.GetWidget(HomeWidgets.DisplaySocialNetwork.ToString());
            if (socialNetwork != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = socialNetwork;
                WorkContext.Layout.DisplaySocialNetwork(widgetShape);
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
            SiteId = (int)Site.Home;
            PageSize = 15;
            if (Request.QueryString["trang"] != null)
            {
                PageIndex = int.Parse(Request.QueryString["trang"]);
            }
            var serviceNews = WorkContext.Resolve<IArticlesService>();
            serviceNews.LanguageCode = WorkContext.CurrentCulture;
            serviceNews.SiteId = SiteId;
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.SiteId = SiteId;
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            var modelFirst = new DataViewCategoryModel();
            if (!isCategory)
            {
                var news = serviceNews.GetById(id);
                ViewData[Extensions.Constants.HeaderTitle] = news.Title;
                ViewData[Extensions.Constants.HeaderDescription] = news.Description;
                ViewData[Extensions.Constants.HeaderKeywords] = news.Tags;

                modelFirst.News = news;
                modelFirst.CurrentCategory = serviceCategory.GetByIdCache(news.CategoryId);

                var viewFilmFirst = viewRenderer.RenderPartialView(Extensions.Constants.NewsDetailsViewFilePath, modelFirst);
                WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFilmFirst));

                #region CategoryContentLeftSecond
                var viewModel = new DataViewCategoryModel();
                viewModel.UrlNext = "http://" + Extensions.Constants.HomeDomainName + Url.Action("ViewDetails", "HomeNews", new { @alias = news.Alias, @id = news.Id });

                var viewCommentFirst = viewRenderer.RenderPartialView(Extensions.Constants.FacebookCommentsFilePath, viewModel);
                WorkContext.Layout.CategoryContentLeftSecond.Add(new MvcHtmlString(viewCommentFirst));
                #endregion
            }
            else
            {
                var category = serviceCategory.GetByIdCache(id);
                ViewData[Extensions.Constants.HeaderTitle] = category.Name;
                ViewData[Extensions.Constants.HeaderDescription] = category.Description;
                ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

                modelFirst.Breadcrumb = BuildBreadcrumb(category);
                modelFirst.CurrentCategory = category;

                var cateId = category.Id;
                if (cateId == (int)FixCategories.AllNews)
                {
                    cateId = 0;
                }
                serviceNews.CategoryId = cateId;
                var totalRow = 0;
                modelFirst.ListArticles = serviceNews.GetByCategoryPaged(PageIndex, PageSize, out  totalRow);
                modelFirst.TotalRow = totalRow;
                modelFirst.PageIndex = PageIndex;
                modelFirst.PageSize = PageSize;

                var viewFilmFirst = viewRenderer.RenderPartialView(Extensions.Constants.NewsCategoryViewFilePath, modelFirst);
                WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFilmFirst));
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

            #region DisplayStatistic2
            var modelStatistic2 = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.StatisticalLengthEpisodes,
                Title = "PHIM BỘ XEM NHIỀU NHẤT"
            };
            var viewStatistic2 = viewRenderer.RenderPartialView(Extensions.Constants.Statistic2FilePath, modelStatistic2);
            WorkContext.Layout.DisplayStatistic2.Add(new MvcHtmlString(viewStatistic2));
            #endregion
        }

        private string BuildBreadcrumb(CategoryInfo category)
        {
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            serviceCategory.SiteId = SiteId;

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
    }
}
