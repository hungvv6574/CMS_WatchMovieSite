using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Mvc;
using CMSSolutions.Extensions;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Web.UI.Navigation;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Services;
using GemBox.Spreadsheet;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = true), Authorize]
    public class AdminReportSmsController : BaseAdminController
    {
        public AdminReportSmsController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblReportSms";
        }

        [Url("admin/report/sms")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Báo cáo SMS"), Url = "#" });

            var result = new ControlGridFormResult<TransactionSmsInfo>
            {
                Title = T("Báo cáo SMS"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetReportSMS,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 150
            };

            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.TypeId, "$('#" + Extensions.Constants.TypeId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.Locked, "$('#" + Extensions.Constants.Locked + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FromDate, "$('#" + Extensions.Constants.FromDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ToDate, "$('#" + Extensions.Constants.ToDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);

            result.AddColumn(x => x.TransactionCode, T("Mã GD")).HasWidth(60).AlignCenter();
            result.AddColumn(x => x.CustomerCode, T("Mã KH"));
            result.AddColumn(x => x.FullName, T("Tên KH"));
            result.AddColumn(x => x.Amount, T("Số tiền"));
            result.AddColumn(x => EnumExtensions.GetDisplayName((RequestStatus)x.Status), T("Trạng thái"));
            result.AddColumn(x => x.TextStartDate, T("Từ ngày"));
            result.AddColumn(x => x.TextEndDate, T("Đến ngày"));
            result.AddColumn(x => x.IsLock)
                .HasHeaderText(T("Đang khóa"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddAction().HasText(T("Chốt đối soát"))
                .HasUrl(Url.Action("Index", "AdminClosedSms"))
                .HasButtonStyle(ButtonStyle.Danger)
                .HasCssClass(Constants.RowLeft);

            result.AddAction().HasText(T("Xuất dữ liệu MO"))
                .HasUrl(Url.Action("ExportExcelMo"))
                .HasButtonStyle(ButtonStyle.Info)
                .HasCssClass(Constants.RowLeft);

            result.AddAction().HasText(T("Xuất dữ liệu MT"))
               .HasUrl(Url.Action("ExportExcelMt"))
               .HasButtonStyle(ButtonStyle.Info)
               .HasCssClass(Constants.RowLeft)
               .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol6);
            result.AddAction(new ControlFormHtmlAction(BuildTypes)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildLocked)).HasParentClass(Constants.ContainerCssClassCol3).HasRow(true);
            result.AddAction(new ControlFormHtmlAction(() => BuildFromDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildToDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);

            return result;
        }

        public override string BuildStatus()
        {
            var list = EnumExtensions.GetListItems<RequestStatus>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Trạng thái lỗi") + " <select id=\"" + Extensions.Constants.StatusId + "\" name=\"" + Extensions.Constants.StatusId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                if ((int)RequestStatus.Messages200 == int.Parse(status.Value))
                {
                    sb.AppendFormat("<option value=\"{1}\" selected>{0}</option>", status.Text, status.Value);
                    continue;
                }
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        private string BuildTypes()
        {
            var list = EnumExtensions.GetListItems<SMSType>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Loại tin nhắn") + " <select id=\"" + Extensions.Constants.TypeId + "\" name=\"" + Extensions.Constants.TypeId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        private string BuildLocked()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem{Value = "0", Text = "Chưa chốt sổ đối soát"},
                new SelectListItem{Value = "1", Text = "Đã chốt sổ đối soát"}
            };
            var sb = new StringBuilder();
            sb.AppendFormat(T("Đối soát") + " <select id=\"" + Extensions.Constants.Locked + "\" name=\"" + Extensions.Constants.Locked + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        private ControlGridAjaxData<TransactionSmsInfo> GetReportSMS(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            var type = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.TypeId]))
            {
                type = Convert.ToInt32(Request.Form[Extensions.Constants.TypeId]);
            }

            var status = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                status = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }

            var locked = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.Locked]))
            {
                locked = Convert.ToInt32(Request.Form[Extensions.Constants.Locked]);
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

            int totals;
            var items = WorkContext.Resolve<ITransactionSmsService>().GetPaged(searchText, type, status, locked, fromDate, toDate, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<TransactionSmsInfo>(items, totals);

            return result;
        }

        [Url("admin/report/sms/export-excel-mo")]
        public ActionResult ExportExcelMo()
        {
            var ef = new ExcelFile();
            string fileName = Server.MapPath("/Media/Default/ExcelTemplate/DuLieuSMS.xls");
            ef.LoadXls(fileName, XlsOptions.PreserveAll);
            ExcelWorksheet ws = ef.Worksheets[0];
            var data = WorkContext.Resolve<ITransactionSmsService>().ExportExcelSms(Utilities.DateNull(), Utilities.DateNull(), (int)SMSType.MO);
            if (data != null && data.Count > 0)
            {
                int index = 1;
                foreach (var item in data)
                {
                    ws.Cells[index, 0].Value = index;
                    ws.Cells[index, 1].Value = item.CustomerCode;
                    ws.Cells[index, 2].Value = item.FullName;
                    ws.Cells[index, 3].Value = item.Amount;
                    ws.Cells[index, 4].Value = item.ShortCode;
                    ws.Cells[index, 5].Value = item.UserId;
                    ws.Cells[index, 6].Value = item.CreateDate.ToString(Extensions.Constants.DateTimeFomatFull);
                    ws.Cells[index, 7].Value = EnumExtensions.GetDisplayName((RequestStatus)item.Status);
                    index++;
                    ws.Rows[index].InsertCopy(1, ws.Rows[1]);
                }

                ws.Rows[index].Delete();
            }

            byte[] fileContents;
            using (var stream = new MemoryStream())
            {
                ef.SaveXls(stream);

                fileContents = stream.ToArray();
            }

            return File(fileContents, "application/vnd.ms-excel");
        }

        [Url("admin/report/sms/export-excel-mt")]
        public ActionResult ExportExcelMt()
        {
            var ef = new ExcelFile();
            string fileName = Server.MapPath("/Media/Default/ExcelTemplate/DuLieuSMS.xls");
            ef.LoadXls(fileName, XlsOptions.PreserveAll);
            ExcelWorksheet ws = ef.Worksheets[0];
            var data = WorkContext.Resolve<ITransactionSmsService>().ExportExcelSms(Utilities.DateNull(), Utilities.DateNull(), (int)SMSType.MT);
            if (data != null && data.Count > 0)
            {
                int index = 1;
                foreach (var item in data)
                {
                    ws.Cells[index, 0].Value = index;
                    ws.Cells[index, 1].Value = item.CustomerCode;
                    ws.Cells[index, 2].Value = item.FullName;
                    ws.Cells[index, 3].Value = item.Amount;
                    ws.Cells[index, 4].Value = item.ShortCode;
                    ws.Cells[index, 5].Value = item.UserId;
                    ws.Cells[index, 6].Value = item.CreateDate.ToString(Extensions.Constants.DateTimeFomatFull);
                    ws.Cells[index, 7].Value = EnumExtensions.GetDisplayName((RequestStatus)item.Status);
                    index++;
                    ws.Rows[index].InsertCopy(1, ws.Rows[1]);
                }

                ws.Rows[index].Delete();
            }

            byte[] fileContents;
            using (var stream = new MemoryStream())
            {
                ef.SaveXls(stream);

                fileContents = stream.ToArray();
            }

            return File(fileContents, "application/vnd.ms-excel");
        }
    }
}
