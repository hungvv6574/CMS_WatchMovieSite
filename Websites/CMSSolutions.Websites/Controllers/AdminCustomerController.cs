using System;
using System.Globalization;
using System.IO;
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
using GemBox.Spreadsheet;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = true), Authorize]
    public class AdminCustomerController : BaseAdminController
    {
        public AdminCustomerController(IWorkContextAccessor workContextAccessor) : 
                base(workContextAccessor)
        {
            TableName = "tblCustomers";
        }
        
        [Url("admin/customers")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý khách hàng"), Url = "#" });

            var result = new ControlGridFormResult<CustomerInfo>
            {
                Title = T("Quản lý khách hàng"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetCustomers,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 200
            };

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FilmTypesId, "$('#" + Extensions.Constants.FilmTypesId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.CountryId, "$('#" + Extensions.Constants.CountryId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FromDate, "$('#" + Extensions.Constants.FromDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ToDate, "$('#" + Extensions.Constants.ToDate + "').val();", true);

            result.AddColumn(x => x.CustomerCode, T("Mã KH")).HasWidth(100).AlignCenter();
            result.AddColumn(x => x.FullName, T("Họ và Tên"));
            result.AddColumn(x => x.Email, T("Email"));
            result.AddColumn(x => x.VipXu, T("VIP Xu"));
            result.AddColumn(x => x.TotalDay, T("Tổng ngày"));
            result.AddColumn(x => x.TextStartDate, T("Từ ngày"));
            result.AddColumn(x => x.TextEndDate, T("Đến ngày"));

            result.AddAction().HasText(T("Thêm mới"))
              .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
              .HasButtonStyle(ButtonStyle.Primary)
              .HasCssClass(Constants.RowLeft);

            result.AddAction().HasText(T("Xuất danh sách KH"))
                .HasUrl(Url.Action("ExportExcel"))
                .HasButtonStyle(ButtonStyle.Info)
                .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildFilmTypes)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildCountries)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildFromDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildToDate())).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction(true)
                .HasText(T("Kích hoạt"))
                .HasName("SetActived")
                .HasValue(x => x.Id)
                .HasButtonSize(ButtonSize.ExtraSmall)
                .HasButtonStyle(ButtonStyle.Success)
                .EnableWhen(x => x.IsBlock);

            result.AddRowAction()
                 .HasText(T("Sửa"))
                 .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                 .HasButtonStyle(ButtonStyle.Default)
                 .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddRowAction()
                 .HasText(T("Lịch sử"))
                 .HasUrl(x => Url.Action("History", RouteData.Values.Merge(new { id = x.Id })))
                 .HasButtonStyle(ButtonStyle.Warning)
                 .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return result;
        }

        private ControlGridAjaxData<CustomerInfo> GetCustomers(ControlGridFormRequest options)
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

            var filmTypesId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FilmTypesId]))
            {
                filmTypesId = Convert.ToInt32(Request.Form[Extensions.Constants.FilmTypesId]);
            }

            var countryId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.CountryId]))
            {
                countryId = Convert.ToInt32(Request.Form[Extensions.Constants.CountryId]);
            }

            var status = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                status = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }

            int totals;
            var service = WorkContext.Resolve<ICustomerService>();
            var records = service.SearchPaged(
                searchText, 
                filmTypesId, countryId, fromDate,
                toDate, -1, status, options.PageIndex, options.PageSize,
                out totals);

            return new ControlGridAjaxData<CustomerInfo>(records, totals);
        }

        [ActionName("Update")]
        [HttpPost, FormButton("SetActived")]
        public ActionResult SetDefault(int id)
        {
            var service = WorkContext.Resolve<ICustomerService>();
            var item = service.GetById(id);
            item.IsBlock = false;
            service.Update(item);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Đã kích hoạt tài khoản " + item.UserName));
        }
        
        [Url("admin/customers/history")]
        public ActionResult History(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý khách hàng"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Lịch sử khách hàng"), Url = "#" });
            TableName = "tblHistories";

            var result = new ControlGridFormResult<CustomerHistoriesInfo>
            {
                Title = T("Lịch sử khách hàng"),
                CssClass = "table table-bordered table-striped",
                FetchAjaxSource = (x => GetHistories(id, x)),
                ActionsColumnWidth = 100,
                EnablePaginate = true,
                DefaultPageSize = 50,
                ClientId = TableName,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.FromDate, "$('#" + Extensions.Constants.FromDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ToDate, "$('#" + Extensions.Constants.ToDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.TypeSearch, "$('#" + Extensions.Constants.TypeSearch + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.TransactionCode, T("Mã giao dịch"));
            result.AddColumn(x => x.Action, T("Hành động"));
            result.AddColumn(x => x.TextCreateDate, T("Ngày thực hiện"));
            result.AddColumn(x => x.Description, T("Nội dung thực hiện"));
            result.AddColumn(x => EnumExtensions.GetDisplayName((CustomerLogType)x.Type), T("Loại log"));

            result.AddAction().HasText(T("Trở về"))
               .HasUrl(Url.Action("Index"))
               .HasButtonStyle(ButtonStyle.Danger)
               .HasBoxButton(false)
               .HasCssClass(Constants.RowLeft)
               .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildFromDate(false))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildToDate(false))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildTypes)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol3);

            return result;
        }

        private string BuildTypes()
        {
            var list = EnumExtensions.GetListItems<CustomerLogType>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Loại log") + " <select id=\"" + Extensions.Constants.TypeSearch + "\" name=\"" + Extensions.Constants.TypeSearch + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        private ControlGridAjaxData<CustomerHistoriesInfo> GetHistories(int id, ControlGridFormRequest options)
        {
            var fromDate = Utilities.DateNull();
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FromDate]))
            {
                fromDate = DateTime.ParseExact(Request.Form[Extensions.Constants.FromDate], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var toDate = Utilities.DateNull();
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
            var service = WorkContext.Resolve<ICustomerHistoriesService>();
            var items = service.GetPaged(id, fromDate ,toDate, type, status, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<CustomerHistoriesInfo>(items, totals);

            return result;
        }

        [Url("admin/customers/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý khách hàng"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin khách hàng"), Url = "#" });

            var service = WorkContext.Resolve<ICustomerService>();
            var model = new CustomerModel();
            if (id > 0)
            {
                model = service.GetById(id);
            }
            else
            {
                model.CutomerCode = service.GetLatestCustomerCode();
            }

            var result = new ControlFormResult<CustomerModel>(model)
            {
                Title = T("Thông tin khách hàng"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = false,
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.RegisterFileUploadOptions("ImageIcon.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            result.MakeReadOnlyProperty(x => x.CutomerCode);
            result.MakeReadOnlyProperty(x => x.StartDate);
            result.MakeReadOnlyProperty(x => x.EndDate);
            result.MakeReadOnlyProperty(x => x.TotalDay);
            result.MakeReadOnlyProperty(x => x.TotalMoney);
            if (id > 0)
            {
                result.MakeReadOnlyProperty(x => x.UserName);
                result.MakeReadOnlyProperty(x => x.Password); 
            }

            result.RegisterExternalDataSource(x => x.Sex, y => BindSex());
            result.RegisterExternalDataSource(x => x.CityId, y => BindCities());
            result.RegisterExternalDataSource(x => x.FilmTypeIds, y => BindFilmTypes());
            result.RegisterExternalDataSource(x => x.CountryIds, y => BindCountries());
            result.RegisterExternalDataSource(x => x.Status, y => BindStatus());

            result.AddAction().HasText(T("Lịch sử"))
               .HasUrl(Url.Action("History", new {@id = id}))
               .HasButtonStyle(ButtonStyle.Warning);

            result.AddAction().HasText(T("Trở về"))
               .HasUrl(Url.Action("Index"))
               .HasButtonStyle(ButtonStyle.Danger);

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/customers/update")]
        public ActionResult Update(CustomerModel model)
        {
            var service = WorkContext.Resolve<ICustomerService>();
            CustomerInfo item = model.Id == 0 ? new CustomerInfo() : service.GetById(model.Id);

            item.CustomerCode = model.CutomerCode;
            if (model.Id == 0)
            {
                item.CustomerCode = service.GetLatestCustomerCode();
            }
            
            item.FullName = model.FullName;
            item.Email = model.Email;
            item.Address = model.Address;
            item.PhoneNumber = model.PhoneNumber;
            item.CityId = model.CityId;
            item.Birthday = model.Birthday;
            item.Sex = model.Sex;
            item.ImageIcon = model.ImageIcon;
            item.FilmTypeIds = Utilities.ParseString(model.FilmTypeIds);
            item.CountryIds = Utilities.ParseString(model.CountryIds);
            item.MemberDate = DateTime.Now.Date;
            item.Skype = model.Skype;
            item.ZingMe = model.ZingMe;
            item.Facebook = model.Facebook;
            item.Google = model.Google;
            item.Yahoo = model.Yahoo;
            item.VipXu = model.VipXu;
            item.TotalMoney = model.TotalMoney;
            item.Status = model.Status;
            item.IsBlock = model.IsBlock;
            item.Description = model.Description;
            service.Save(item);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [Url("admin/customers/export-excel")]
        public ActionResult ExportExcel()
        {
            var ef = new ExcelFile();
            string fileName = Server.MapPath("/Media/Default/ExcelTemplate/DuLieuKhachHang.xls");
            ef.LoadXls(fileName, XlsOptions.PreserveAll);
            byte[] fileContents;
            ExcelWorksheet ws = ef.Worksheets[0];
            var data = WorkContext.Resolve<ICustomerService>().ExportExcel();
            if (data != null && data.Count > 0)
            {
                int index = 1;
                foreach (var item in data)
                {
                    ws.Cells[index, 0].Value = index;
                    ws.Cells[index, 1].Value = item.CustomerCode;
                    ws.Cells[index, 2].Value = item.FullName;
                    ws.Cells[index, 3].Value = item.MemberDate.ToString(Extensions.Constants.DateTimeFomatFull);
                    ws.Cells[index, 4].Value = item.Email;
                    ws.Cells[index, 5].Value = item.PhoneNumber;
                    ws.Cells[index, 6].Value = (int)item.VipXu;
                    ws.Cells[index, 7].Value = item.TotalDay;
                    ws.Cells[index, 8].Value = item.TextStartDate;
                    ws.Cells[index, 9].Value = item.TextEndDate;
                    index++;
                    ws.Rows[index].InsertCopy(1, ws.Rows[1]);
                }

                ws.Rows[index].Delete();
            }

            using (var stream = new MemoryStream())
            {
                ef.SaveXls(stream);

                fileContents = stream.ToArray();
            }

            return File(fileContents, "application/vnd.ms-excel");
        }
    }
}
