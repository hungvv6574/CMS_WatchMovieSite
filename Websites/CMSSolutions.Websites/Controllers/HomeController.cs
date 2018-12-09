using System;
using System.Web.Mvc;
using CMSSolutions.ContentManagement.Widgets.Services;
using CMSSolutions.DisplayManagement;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;
using Microsoft.Web.WebPages.OAuth;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
        }

        [Url("", Priority = 10)]
        public ActionResult Index()
        {
            UrlLogin = Url.Action("Index", "Home");
            if (IsLogin)
            {
                var customerService = WorkContext.Resolve<ICustomerService>();
                CustomerInfo customer = customerService.GetCustomerByCacheId(UserId);
                SetCustomerState(customer);
            }

            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            if (WorkContext.DomainName == Extensions.Constants.JJDomainName)
            {
                SiteId = (int)Site.JJHome;
                serviceCategory.SiteId = (int)Site.JJHome;
                var jjHome = serviceCategory.GetHomePage();
                ViewData[Extensions.Constants.HeaderTitle] = jjHome.Name;
                ViewData[Extensions.Constants.HeaderDescription] = jjHome.Description;
                ViewData[Extensions.Constants.HeaderKeywords] = jjHome.Tags;
                ViewBag.ReturnUrl = "";

                return View("JJIndex", OAuthWebSecurity.RegisteredClientData);
            }

            SiteId = (int)Site.Home;
            serviceCategory.SiteId = (int)Site.Home;
            var home = serviceCategory.GetHomePage();
            ViewData[Extensions.Constants.HeaderTitle] = home.Name;
            ViewData[Extensions.Constants.HeaderDescription] = home.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = home.Tags;

            BuildModules();

            return View("Index", OAuthWebSecurity.RegisteredClientData);
        }

        public override void BuildModules()
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

                #region BannerContentLeftFirst
                var bannerContentLeftFirst = widget.GetWidget(HomeWidgets.BannerContentLeftFirst.ToString());
                if (bannerContentLeftFirst != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerContentLeftFirst;
                    WorkContext.Layout.AdBannerContentLeftFirst(widgetShape);
                }
                #endregion

                #region BannerContentLeftSecond
                var bannerContentLeftSecond = widget.GetWidget(HomeWidgets.BannerContentLeftSecond.ToString());
                if (bannerContentLeftSecond != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerContentLeftSecond;
                    WorkContext.Layout.AdBannerContentLeftSecond(widgetShape);
                }
                #endregion

                #region BannerContentLeftThird
                var bannerContentLeftThird = widget.GetWidget(HomeWidgets.BannerContentLeftThird.ToString());
                if (bannerContentLeftThird != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerContentLeftThird;
                    WorkContext.Layout.AdBannerContentLeftThird(widgetShape);
                }
                #endregion

                #region BannerContentLeftFourth
                var bannerContentLeftFourth = widget.GetWidget(HomeWidgets.BannerContentLeftFourth.ToString());
                if (bannerContentLeftFourth != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerContentLeftFourth;
                    WorkContext.Layout.AdBannerContentLeftFourth(widgetShape);
                }
                #endregion

                #region BannerContentLeftFifth
                var bannerContentLeftFifth = widget.GetWidget(HomeWidgets.BannerContentLeftFifth.ToString());
                if (bannerContentLeftFifth != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerContentLeftFifth;
                    WorkContext.Layout.AdBannerContentLeftFifth(widgetShape);
                }
                #endregion

                #region BannerContentLeftSixth
                var bannerContentLeftSixth = widget.GetWidget(HomeWidgets.BannerContentLeftSixth.ToString());
                if (bannerContentLeftSixth != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerContentLeftSixth;
                    WorkContext.Layout.AdBannerContentLeftSixth(widgetShape);
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

                #region BannerRightSixth
                var bannerRightSixth = widget.GetWidget(HomeWidgets.BannerRightSixth.ToString());
                if (bannerRightSixth != null)
                {
                    var widgetShape = shapeFactory.Widget();
                    widgetShape.Widget = bannerRightSixth;
                    WorkContext.Layout.AdBannerRightSixth(widgetShape);
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

            var modelSlider = new DataViewerModel {Data = BuildSlider(SiteId, (int) SliderPages.Home)};
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
            var modelFirst = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.FilmHot,
                Title = "Sự kiện: Phim Đang HOT",
                TextNext = "Còn nữa ››",
                UrlNext = Url.Action("FilmHot", "HomeCategory"),
                SliderName = "FilmFirst"
            };
            modelFirst.HtmlFilmHot = GetFilmHtml(modelFirst.Type);
            var viewFilmFirst = viewRenderer.RenderPartialView(Extensions.Constants.CategoryContentFirstViewFilePath, modelFirst);
            WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFilmFirst));
            #endregion

            #region CategoryContentSecond
            var modelSecond = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.FilmRetail,
                Title = "Phim lẻ mới up",
                TextNext = "Tất Tần Tật ››",
                UrlNext = Url.Action("Index", "HomeCategory", new { @alias = "phim-le", @id = 2 }),
                SliderName = "FilmSecond"
            };
            modelSecond.HtmlFilmRetail = GetFilmHtml(modelSecond.Type);
            var viewFilmSecond = viewRenderer.RenderPartialView(Extensions.Constants.CategoryContentSecondViewFilePath, modelSecond);
            WorkContext.Layout.CategoryContentLeftSecond.Add(new MvcHtmlString(viewFilmSecond));
            #endregion

            #region CategoryContentThird
            var modelThird = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.FilmLengthEpisodes,
                Title = "Phim bộ mới up",
                TextNext = "Tất Tần Tật ››",
                UrlNext = Url.Action("Index", "HomeCategory", new { @alias = "phim-bo", @id = 3 }),
                SliderName = "FilmThird"
            };
            modelThird.HtmlFilmLengthEpisodes = GetFilmHtml(modelThird.Type);
            var viewFilmThird = viewRenderer.RenderPartialView(Extensions.Constants.CategoryContentThirdViewFilePath, modelThird);
            WorkContext.Layout.CategoryContentLeftThird.Add(new MvcHtmlString(viewFilmThird));
            #endregion

            #region CategoryContentFourth
            var modelFourth = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.FilmJJChannelIntroduce,
                Title = "JJ Channel giới thiệu",
                TextNext = "Tất Tần Tật ››",
                UrlNext = Url.Action("Index", "HomeJJChannel", new { @alias = "jj-channel-viphd", @id = 9 }),
                SliderName = "FilmFourth"
            };
            modelFourth.HtmlFilmJJChannelIntroduce = GetFilmHtml(modelFourth.Type);
            var viewFilmFourth = viewRenderer.RenderPartialView(Extensions.Constants.CategoryContentFourthViewFilePath, modelFourth);
            WorkContext.Layout.CategoryContentLeftFourth.Add(new MvcHtmlString(viewFilmFourth));
            #endregion

            #region CategoryContentFifth
            var modelFifth = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.FilmTheater,
                Title = "Phim chiếu rạp",
                TextNext = "Tất Tần Tật ››",
                UrlNext = Url.Action("Index", "HomeTheater"),
                SliderName = "FilmFifth"
            };
            modelFifth.HtmlFilmTheater = GetFilmHtml(modelFifth.Type);
            var viewFilmFifth = viewRenderer.RenderPartialView(Extensions.Constants.CategoryContentFifthViewFilePath, modelFifth);
            WorkContext.Layout.CategoryContentLeftFifth.Add(new MvcHtmlString(viewFilmFifth));
            #endregion

            #region CategoryContentSixth
            var modelSixth = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.TVShow,
                Title = "TV Show",
                TextNext = "Tất Tần Tật ››",
                UrlNext = Url.Action("Index", "HomeShow", new { @alias = "shows", @id = 4 }),
                SliderName = "TVShows"
            };
            modelSixth.HtmlTVShow = GetFilmHtml(modelSixth.Type);
            var viewFilmSixth = viewRenderer.RenderPartialView(Extensions.Constants.CategoryContentSixthViewFilePath, modelSixth);
            WorkContext.Layout.CategoryContentLeftSixth.Add(new MvcHtmlString(viewFilmSixth));
            #endregion

            #region CategoryContentSeventh
            var modelSeventh = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.Clip,
                Title = "Clip",
                TextNext = "Tất Tần Tật ››",
                UrlNext = Url.Action("Index", "HomeClip", new { @alias = "clip-hay", @id = 5 }),
                SliderName = "Clip"
            };
            modelSeventh.HtmlClip = GetFilmHtml(modelSeventh.Type);
            var viewFilmSeventh = viewRenderer.RenderPartialView(Extensions.Constants.CategoryContentSeventhViewFilePath, modelSeventh);
            WorkContext.Layout.CategoryContentLeftSeventh.Add(new MvcHtmlString(viewFilmSeventh));
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
    }
}
