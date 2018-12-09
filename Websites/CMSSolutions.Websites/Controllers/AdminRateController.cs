using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Routing;
using CMSSolutions.Web.Themes;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Web.UI.Navigation;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = true), Authorize]
    public class AdminRateController : BaseAdminController
    {
            public AdminRateController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblRates";
        }
        
        [Url("admin/rates")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Đánh giá và Báo lỗi phim"), Url = "#" });

            var result = new ControlGridFormResult<RateInfo>
            {
                Title = T("Đánh giá và Báo lỗi phim"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetRates,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);

            result.AddColumn(x => x.FilmId, T("Mã phim"));
            result.AddColumn(x => x.FilmName, T("Tên phim"));
            result.AddColumn(x => x.CustomerCode, T("Mã KH"));
            result.AddColumn(x => x.CustomerName, T("Họ và Tên"));

            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
               .HasText(T("Xem"))
               .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
               .HasButtonStyle(ButtonStyle.Default)
               .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return result;
        }

        private ControlGridAjaxData<RateInfo> GetRates(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            int totals;
            var items = WorkContext.Resolve<IRateService>().SearchPaged(searchText, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<RateInfo>(items, totals);

            return result;
        }
        
        [Url("admin/rates/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Đánh giá và Báo lỗi phim"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin đánh giá và Báo lỗi phim"), Url = "#" });

            var model = new RateModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IRateService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<RateModel>(model)
            {
                Title = T("Thông tin đánh giá và Báo lỗi phim"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                CancelButtonText = T("Trở về"),
                ShowBoxHeader = false,
                ShowSubmitButton = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.ReadOnly = true;

            return result;
        }
    }
}
