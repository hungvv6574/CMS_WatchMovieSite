using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeDigCoinsController : BaseHomeController
    {
        public HomeDigCoinsController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            PageIndex = 1;
        }

        [HttpGet]
        [Url("{alias}/d{id}.html")]
        public ActionResult Index(string alias, int id)
        {
            if (!IsLogin)
            {
                return Redirect(Url.Action("Index", "Home"));
            }

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
            PageSize = 10;

            BuildFilmModules();

            return View();
        }

        [Url("{alias}/thongtin/{id}.html")]
        public ActionResult ShowInformations(string alias, int id)
        {
            if (!IsLogin)
            {
                return Redirect(Url.Action("Index", "Home"));
            }

            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            SiteId = (int)Site.Home;
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            serviceCategory.SiteId = SiteId;
            var category = serviceCategory.GetByIdCache(id);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            var viewRenderer = new ViewRenderer { Context = ControllerContext };
            var customerService = WorkContext.Resolve<ICustomerService>();
            var model = new DataViewerModel
            {
                Customer = customerService.GetCustomerByCacheId(UserId),
                DataType = 0
            };

            var view = viewRenderer.RenderPartialView(Extensions.Constants.DaoXuInformationsFilePath, model);
            WorkContext.Layout.FullContent.Add(new MvcHtmlString(view));

            return View("Index");
        }

        [Url("{alias}/xem-logs/{id}.html")]
        public ActionResult ShowLogs(string alias, int id)
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

            var viewRenderer = new ViewRenderer { Context = ControllerContext };
            var customerService = WorkContext.Resolve<ICustomerService>();
            var service = WorkContext.Resolve<IDownloadCustomerService>();
            var model = new DataViewerModel
            {
                Customer = customerService.GetCustomerByCacheId(UserId),
                DataType = 2,
                ListHistoryDownload = service.GetHistory(UserId)
            };

            var view = viewRenderer.RenderPartialView(Extensions.Constants.DaoXuLogsFilePath, model);
            WorkContext.Layout.FullContent.Add(new MvcHtmlString(view));

            return View("Index");
        }

        [Url("{alias}/huong-dan/{id}.html")]
        public ActionResult ShowHelps(string alias, int id)
        {
            if (!IsLogin)
            {
                return Redirect(Url.Action("Index", "Home"));
            }

            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            SiteId = (int)Site.Home;
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;
            serviceCategory.SiteId = SiteId;
            var category = serviceCategory.GetByIdCache(id);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            var viewRenderer = new ViewRenderer { Context = ControllerContext };
            var customerService = WorkContext.Resolve<ICustomerService>();
            var model = new DataViewerModel
            {
                Customer = customerService.GetCustomerByCacheId(UserId),
                DataType = 3
            };

            var view = viewRenderer.RenderPartialView(Extensions.Constants.DaoXuHelpsFilePath, model);
            WorkContext.Layout.FullContent.Add(new MvcHtmlString(view));

            return View("Index");
        }

        private void BuildFilmModules()
        {
            var viewRenderer = new ViewRenderer {Context = ControllerContext};
            if (!IsVip)
            {

            }

            var customerService = WorkContext.Resolve<ICustomerService>();
            var service = WorkContext.Resolve<IDownloadGameService>();
            var downloadService = WorkContext.Resolve<IDownloadCustomerService>();
            var model = new DataViewerModel
            {
                Customer = customerService.GetCustomerByCacheId(UserId),
                PageIndex = PageIndex,
                PageSize = PageSize,
                DataType = 1,
                ListDownloadCustomer = downloadService.GetByCustomer(UserId)
            };

            var totalRow = 0;
            model.ListGames = service.GetPaged(model.PageIndex, model.PageSize, out totalRow);
            model.TotalRow = totalRow;
            var view = viewRenderer.RenderPartialView(Extensions.Constants.DigCoinsFilePath, model);
            WorkContext.Layout.FullContent.Add(new MvcHtmlString(view));
        }
    }
}
