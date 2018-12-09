using System;
using System.Collections.Generic;
using System.Linq;
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
    public class HomeViewController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeViewController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
        }

        [Url("xem-phim/{alias}/f{id}.html")]
        public ActionResult FilmView(string alias, long id, int tap = 0, int trang = 1)
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            SiteId = (int)Site.Home;
            BuildFilmModules(id, tap, trang);

            return View("FilmView");
        }

        [HttpPost, ValidateInput(false)]
        [Url("xem-tap/phan-trang.html")]
        public ActionResult EpisodesPager()
        {
            var pageIndex = Convert.ToInt32(Request.Form["pageIndex"]);
            var siteId = Convert.ToInt32(Request.Form["siteId"]);
            var filmId = long.Parse(Request.Form["filmId"]);
            var pageSize = Convert.ToInt32(Request.Form["pageSize"]);
            var episodeId = Convert.ToInt32(Request.Form["episodeId"]);
            int totalRow;
            var service = WorkContext.Resolve<IFilmService>();
            service.LanguageCode = WorkContext.CurrentCulture;
            service.SiteId = siteId;
            var model = new DataViewerModel();
            var listData = service.GetFilmEpisodes(filmId, pageIndex, pageSize, out  totalRow);
            if (listData != null && listData.Count > 0)
            {
                var html = new StringBuilder();
                for (int i = 0; i < listData.Count; i ++)
                {
                    html.Append(BuildItem(listData[i], episodeId, pageIndex));
                }

                model = new DataViewerModel
                {
                    Status = true,
                    PageSize = pageSize,
                    TotalRow = totalRow,
                    Data = html.ToString()
                };
            }

            return Json(model);
        }

        private void BuildFilmModules(long filmId, int episodeId, int pageIndex)
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

            var model = new DataViewerModel();
            model.CustomerCode = CustomerCode;
            #region HomeSliderBanner
            var service = WorkContext.Resolve<IFilmService>();
            service.LanguageCode = WorkContext.CurrentCulture;
            service.SiteId = SiteId;

            model.Skin = Extensions.Constants.JWplayerSkin;
            model.JwplayerKey = Extensions.Constants.JWplayerKey;
            model.FilmDetails = service.GetFilmsMovie(filmId, episodeId) ?? service.GetFilmDetails(filmId);
            model.DataType = (int)LinkType.Streaming;
            var trailer = service.GetTrailerMovie(filmId);
            if (trailer != null)
            {
                model.Url = trailer.EncodeSourceUrl;
            }

            ViewData[Extensions.Constants.HeaderTitle] = model.FilmDetails.FilmName;
            ViewData[Extensions.Constants.HeaderDescription] = model.FilmDetails.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = model.FilmDetails.Tags;
            service.UpdateViewCount(model.FilmDetails.Id);

            var advertisementService = WorkContext.Resolve<IAdvertisementGroupService>();
            model.Advertisement = advertisementService.GetAdByCategories(WorkContext.CurrentCulture, SiteId, model.FilmDetails.CategoryIds);
            model.AdvertisementsPath = Convert.ToBase64String(Encoding.UTF8.GetBytes("/"));
            if (model.Advertisement != null)
            {
                model.AdvertisementsPath = model.Advertisement.EncodeFullPath;
            }

            if (!string.IsNullOrEmpty(model.FilmDetails.UrlAlbum))
            {
                model.DataType = (int)LinkType.Picasa;
                var picasaService = new PicasaService();
                model.ListFilmsPicasa = picasaService.ParseRssFile(model.FilmDetails.UrlAlbum, model.FilmDetails.UrlSource);
            }

            if (model.FilmDetails.IsClip)
            {
                model.DataType = (int)LinkType.Youtube;
                model.Url = model.FilmDetails.EncodeSourceUrl;
            }

            var viewSliderBanner = viewRenderer.RenderPartialView(Extensions.Constants.MoviesFilePath, model);
            WorkContext.Layout.SliderBanner.Add(new MvcHtmlString(viewSliderBanner));
            #endregion

            #region CategoryContentLeftFirst
            model.SliderName = "ShowListEpisodes";
            if (model.FilmDetails.IsFilmLengthEpisodes)
            {
                model.IsShow = true;
            }

            if (model.FilmDetails.IsFilm && model.FilmDetails.IsFilmLengthEpisodes)
            {
                model.Title = "Tập phim:";
            }

            if (model.FilmDetails.IsShow && model.FilmDetails.IsFilmLengthEpisodes)
            {
                model.Title = "Tập show:";
            }

            model.EpisodeId = model.FilmDetails.EpisodeId;
            model.SiteId = SiteId;
            model.PageSize = 99999;
            model.PageIndex = 1;

            var totalRow = 0;
            var listData = service.GetFilmEpisodes(filmId, 1, 99999, out  totalRow);
            if (listData != null && listData.Count > 0)
            {
                var html = new StringBuilder();
                for (int i = 0; i < listData.Count; i++)
                {
                    html.Append(BuildItem2(listData[i], episodeId, pageIndex));
                }

                model.Status = true;
                model.PageSize = 99999;
                model.TotalRow = totalRow;
                model.Data = html.ToString();
            }

            var viewFirst = viewRenderer.RenderPartialView(Extensions.Constants.ListMoviesViewFilePath, model);
            WorkContext.Layout.CategoryContentLeftFirst.Add(new MvcHtmlString(viewFirst));
            #endregion

            var viewModel = new DataViewCategoryModel();
            viewModel.UrlNext = "http://" + Extensions.Constants.HomeDomainName + Url.Action("FilmView", "HomeView", new { @alias = model.FilmDetails.FilmAlias, @id = model.FilmDetails.Id, @tap = model.FilmDetails.EpisodeId });
            
            #region CategoryContentLeftSecond
            var viewCommentSecond = viewRenderer.RenderPartialView(Extensions.Constants.FacebookCommentsFilePath, viewModel);
            WorkContext.Layout.CategoryContentLeftSecond.Add(new MvcHtmlString(viewCommentSecond));
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
            WorkContext.Layout.CategoryContentLeftThird.Add(new MvcHtmlString(viewFilmFirst));
            #endregion

            #region BannerRightFirst
            viewModel.FilmDetails = model.FilmDetails;
            viewModel.Breadcrumb = BuildBreadcrumb(Utilities.ParseListInt(model.FilmDetails.CategoryIds)[0], model.FilmDetails);
            var bannerRightFirst = viewRenderer.RenderPartialView(Extensions.Constants.FilmRateFilePath, viewModel);
            WorkContext.Layout.AdBannerRightFirst.Add(new MvcHtmlString(bannerRightFirst));
            #endregion

            #region DisplayStatistic3
            var modelStatistic3 = new DataViewCategoryModel
            {
                Type = (int)HomeDisplayFilmType.StatisticalNextFilm,
                Title = "PHIM LIÊN QUAN",
                FilmDetails = model.FilmDetails
            };
            if (model.FilmDetails.IsClip)
            {
                modelStatistic3.Title = "CLIP LIÊN QUAN";
            }
            if (model.FilmDetails.IsShow)
            {
                modelStatistic3.Title = "SHOW LIÊN QUAN";
            }
            if (model.FilmDetails.IsTrailer)
            {
                modelStatistic3.Title = "TRAILER LIÊN QUAN";
            }
            var viewStatistic3 = viewRenderer.RenderPartialView(Extensions.Constants.Statistic6FilePath, modelStatistic3);
            WorkContext.Layout.DisplayStatistic1.Add(new MvcHtmlString(viewStatistic3));
            #endregion
        }

        private string BuildBreadcrumb(int cateId, FilmInfo film)
        {
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            serviceCategory.SiteId = SiteId;
            CategoryInfo category = serviceCategory.GetByIdCache(cateId);

            var html = new StringBuilder();
            html.Append("<div class=\"mh-breadcrumb-dv\">");
            html.Append("<div>");
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\"><span><img width=\"22\" src=\"/Images/themes/ico-homesm.png\"></span></a>", "Dành cho víp", "/");
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
            html.AppendFormat("<h1 class=\"title_h1_st3\">{0}</h1>", film.FilmName);
            html.AppendFormat("<p>{0}</p>", film.Summary);
            html.Append("</div>");

            return html.ToString();
        }

        private string BuildItem(FilmInfo item, int episodeId, int trang)
        {
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id, @tap = item.EpisodeId, @trang = trang });
            var html = new StringBuilder();
            if (episodeId == item.EpisodeId)
            {
                html.Append("<div class=\"show_tv-epi-img film-actived\">");
            }
            else
            {
                html.Append("<div class=\"show_tv-epi-img\">");
            }
            html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
            html.AppendFormat("<img alt=\"{0}\" width=\"106\" height=\"80\" src=\"{1}\">", item.FilmAlias, item.ImageIcon);
            html.AppendFormat("<span class=\"epi_span\">{0}</span>", item.EpisodeName);
            html.Append("</a>");
            html.Append(" </div>");

            return html.ToString();
        }

        private string BuildItem2(FilmInfo item, int episodeId, int trang)
        {
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id, @tap = item.EpisodeId, @trang = trang });
            var html = new StringBuilder();
            html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
            if (episodeId == item.EpisodeId)
            {
                html.Append("<div class=\"show_tv-epi-img film-actived\">");
            }
            else
            {
                html.Append("<div class=\"show_tv-epi-img\">");
            }
           
            html.AppendFormat("<span class=\"epi_span\">{0}</span>", item.EpisodeIndex);
            html.Append(" </div>");
            html.Append("</a>");

            return html.ToString();
        }
    }
}
