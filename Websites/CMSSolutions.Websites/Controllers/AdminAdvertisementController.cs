using System;
using System.Globalization;
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
    public class AdminAdvertisementController : BaseAdminController
    {
        public AdminAdvertisementController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblAdvertisements";
        }

        [Url("admin/advertisements")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý quảng cáo"), Url = "#" });

            var result = new ControlGridFormResult<AdvertisementInfo>
            {
                Title = T("Quản lý quảng cáo"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetAdvertisements,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SiteId, "$('#" + Extensions.Constants.SiteId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FromDate, "$('#" + Extensions.Constants.FromDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ToDate, "$('#" + Extensions.Constants.ToDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);

            result.AddColumn(x => x.KeyCode, T("Mã QC"));
            result.AddColumn(x => x.Title, T("Tiêu đề"));
            result.AddColumn(x => x.Link, T("Đường dẫn vast"));
            result.AddColumn(x => x.IsBlock)
                .HasHeaderText(T("Tạm khóa"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage(false);

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasRow(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSites(true, Extensions.Constants.CategoryId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildFromDate(false))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildToDate(false))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
               .HasText(T("Sửa"))
               .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
               .HasButtonStyle(ButtonStyle.Default)
               .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddRowAction(true)
                .HasText(T("Khóa"))
                .HasName("Delete")
                .HasValue(x => x.Id.ToString(CultureInfo.InvariantCulture.ToString()))
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return result;
        }

        private ControlGridAjaxData<AdvertisementInfo> GetAdvertisements(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var fromDate = Utilities.DateNull();
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FromDate]))
            {
                fromDate = DateTime.ParseExact(Request.Form[Extensions.Constants.FromDate], Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            }

            var toDate = Utilities.DateNull();
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ToDate]))
            {
                toDate = DateTime.ParseExact(Request.Form[Extensions.Constants.ToDate], Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            }

            int totals;
            var items = WorkContext.Resolve<IAdvertisementService>().SearchPaged(searchText, languageCode, siteId, fromDate, toDate, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<AdvertisementInfo>(items, totals);

            return result;
        }

        [Url("admin/advertisements/edit")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý quảng cáo"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin quảng cáo"), Url = "#" });

            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var model = new AdvertisementModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IAdvertisementService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<AdvertisementModel>(model)
            {
                Title = T("Thông tin quảng cáo"),
                UpdateActionName = "Update",
                FormMethod = FormMethod.Post,
                SubmitButtonText = T("Lưu lại"),
                ShowBoxHeader = false,
                ShowCancelButton = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.MakeReadOnlyProperty(x => x.KeyCode);
            result.AddAction().HasText(T("Làm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasCssClass("btn btn-success");

            result.AddAction().HasText(T("Trở về"))
               .HasUrl(Url.Action("Index"))
               .HasCssClass("btn btn-danger");

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/advertisements/update")]
        public ActionResult Update(AdvertisementModel model)
        {
            var service = WorkContext.Resolve<IAdvertisementService>();
            AdvertisementInfo adsInfo = model.Id == 0 ? new AdvertisementInfo() : service.GetById(model.Id);
            adsInfo.Id = model.Id;
            adsInfo.LanguageCode = model.LanguageCode;
            adsInfo.SiteId = model.SiteId;
            adsInfo.Title = model.Title;
            adsInfo.KeyCode = model.KeyCode;
            adsInfo.Price = Convert.ToDecimal(model.Price);
            adsInfo.Code = model.Code;
            adsInfo.Link = model.Link;
            adsInfo.Type = model.Type;
            adsInfo.Duration = model.Duration;
            adsInfo.Position = model.Position;
            adsInfo.Skip = model.Skip;
            adsInfo.IsBlock = model.IsBlock;
            adsInfo.Click = string.Empty;
            service.Save(adsInfo);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IAdvertisementService>();
            var obj = service.GetById(id);
            obj.IsBlock = true;
            service.Update(obj);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
