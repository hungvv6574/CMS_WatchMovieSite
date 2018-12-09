using System.Web.Mvc;
using CMSSolutions.ContentManagement.Widgets.Services;
using CMSSolutions.DisplayManagement;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeSupportController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeSupportController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
        }

        [Url("ho-tro.html")]
        public ActionResult Index()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            SiteId = (int)Site.Home;
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            serviceCategory.SiteId = SiteId;
            var category = serviceCategory.GetByIdCache((int)FixCategories.Support);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();

            return View();
        }

        public override void BuildModules()
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

            #region DisplayStatistic3
            var modelStatistic3 = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.StatisticalTrailer,
                Title = "PHIM SẮP CHIẾU"
            };
            var viewStatistic3 = viewRenderer.RenderPartialView(Extensions.Constants.Statistic5FilePath, modelStatistic3);
            WorkContext.Layout.AdBannerRightSecond.Add(new MvcHtmlString(viewStatistic3));
            #endregion

            #region Tags
            var serviceTags = WorkContext.Resolve<ITagService>();
            var modelTags = new TagViewModel { ListTags = serviceTags.GetDisplayTags() };
            var viewTags = viewRenderer.RenderPartialView(Extensions.Constants.TagViewFilePath, modelTags);
            WorkContext.Layout.DisplayTags.Add(new MvcHtmlString(viewTags));
            #endregion

            #region HomeFacebookFollow
            var facebookFollow = widget.GetWidget(HomeWidgets.HomeFacebookFollow.ToString());
            if (facebookFollow != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = facebookFollow;
                WorkContext.Layout.AdBannerRightThird(widgetShape);
            }
            #endregion

            #region HomeFacebookActivity
            var facebookActivity = widget.GetWidget(HomeWidgets.HomeFacebookActivity.ToString());
            if (facebookActivity != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = facebookActivity;
                WorkContext.Layout.DisplayTags(widgetShape);
            }
            #endregion

            #region HomeGoogleBadge
            var googleBadge = widget.GetWidget(HomeWidgets.HomeGoogleBadge.ToString());
            if (googleBadge != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = googleBadge;
                WorkContext.Layout.AdBannerRightFourth(widgetShape);
            }
            #endregion

            #region CategoryContentFirst
            var modelFirst = new DataViewCategoryModel();
            var service = WorkContext.Resolve<ISupportService>();
            modelFirst.Title = "FAQs / Những câu hỏi thường gặp";
            service.SiteId = SiteId;
            service.LanguageCode = WorkContext.CurrentCulture;
            modelFirst.ListSupportParents = service.GetAllParents((int) Status.Approved);
            modelFirst.ListSupportChildren = service.GetAllChildren((int) Status.Approved);
            var viewFilmFirst = viewRenderer.RenderPartialView(Extensions.Constants.SupportsViewFilePath, modelFirst);
            WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFilmFirst));
            #endregion
        }
    }
}
