using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using CMSSolutions.Extensions;
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
    public class AdminLogController : BaseAdminController
    {
        public AdminLogController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblLogs";
        }

        [Url("admin/logs")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý logs"), Url = "#" });

            var result = new ControlGridFormResult<LogInfo>
            {
                Title = T("Quản lý logs"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetLogs,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                EnablePaginate = true,
                DefaultPageSize = WorkContext.DefaultPageSize,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.FromDate, "$('#" + Extensions.Constants.FromDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ToDate, "$('#" + Extensions.Constants.ToDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);
            result.AddCustomVar(Extensions.Constants.TypeSearch, "$('#" + Extensions.Constants.TypeSearch + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(60);
            result.AddColumn(x => x.Keyword, T("Thực hiện bởi"));
            result.AddColumn(x => x.TextCreateDate, T("Ngày tạo"));
            result.AddColumn(x => x.Messages, T("Thông báo"));

            result.AddAction(new ControlFormHtmlAction(() => BuildFromDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildToDate())).HasParentClass(Constants.ContainerCssClassCol3).HasRow(true);
            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildLogTypes)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildLogStatus)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
                .HasText(T("Chi tiết"))
                .HasUrl(x => Url.Action("ViewDetails", RouteData.Values.Merge(new { id = x.Id })))
                .HasButtonStyle(ButtonStyle.Info)
                .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");

            return result;
        }

        [Url("admin/logs/view/{id}")]
        public ActionResult ViewDetails(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý logs"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin log"), Url = "#" });

            var service = WorkContext.Resolve<ILogService>();
            var model = new LogModel();
            if (id > 0)
            {
                model = service.GetById(id);
            }

            var result = new ControlFormResult<LogModel>(model)
            {
                Title = T("Thông tin log"),
                ShowSubmitButton = false,
                ShowCancelButton = true,
                ShowBoxHeader = false,
                CancelButtonText = T("Trở về"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.ReadOnly = true;

            result.RegisterExternalDataSource(x => x.Type, y => BindLogTypes());
            result.RegisterExternalDataSource(x => x.Status, y => BindLogStatus());

            return result;
        }

        private string BuildLogTypes()
        {
            var list = EnumExtensions.GetListItems<LogType>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Loại") + " <select id=\"" + Extensions.Constants.TypeSearch + "\" name=\"" + Extensions.Constants.TypeSearch + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        private IEnumerable<SelectListItem> BindLogTypes()
        {
            var items = EnumExtensions.GetListItems<LogType>();
            items[0].Selected = true;
            return items;
        }

        private string BuildLogStatus()
        {
            var list = EnumExtensions.GetListItems<LogStatus>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Trạng thái") + " <select id=\"" + Extensions.Constants.StatusId + "\" name=\"" + Extensions.Constants.StatusId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        private IEnumerable<SelectListItem> BindLogStatus()
        {
            var items = EnumExtensions.GetListItems<LogStatus>();
            items[0].Selected = true;
            return items;
        }

        private ControlGridAjaxData<LogInfo> GetLogs(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            var fromDate = DateTime.Now.Date;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FromDate]))
            {
                fromDate = DateTime.ParseExact(Request.Form[Extensions.Constants.FromDate], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var toDate = DateTime.Now.Date;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ToDate]))
            {
                toDate = DateTime.ParseExact(Request.Form[Extensions.Constants.ToDate], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var type = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.TypeSearch]))
            {
                type = Convert.ToInt32(Request.Form[Extensions.Constants.TypeSearch]);
            }

            var status = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                status = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }

            int totals;
            var items = WorkContext.Resolve<ILogService>().GetPaged(searchText, fromDate,toDate, type,status, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<LogInfo>(items, totals);
            return result;
        }
    }
}
