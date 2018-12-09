using System;
using System.Text;
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
    public class AdminVastController : BaseAdminController
    {
        public AdminVastController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {
            TableName = "tblVast";
        }

        [Url("admin/vast")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý vast"), Url = "#" });

            var result = new ControlGridFormResult<VastInfo>
            {
                Title = T("Quản lý vast"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetVast,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                EnablePaginate = true,
                DefaultPageSize = WorkContext.DefaultPageSize,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SiteId, "$('#" + Extensions.Constants.SiteId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.AdId, "$('#" + Extensions.Constants.AdId + "').val();", true);

            result.AddColumn(x => x.KeyCode, T("Mã Vast"));
            result.AddColumn(x => x.AdTitle, T("Tiêu đề"));
            result.AddColumn(x => x.AdName, T("Tên quảng cáo"));
            result.AddColumn(x => x.MediaFileValue, T("Đường dẫn file"));
            result.AddColumn(x => x.MediaFileType, T("Loại file"));

            result.AddAction().HasText(T("Thêm mới"))
              .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
              .HasButtonStyle(ButtonStyle.Primary)
              .HasBoxButton(false)
              .HasCssClass(Constants.RowLeft)
              .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSiteAds(true, Extensions.Constants.AdId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildAds)).HasParentClass(Constants.ContainerCssClassCol3).HasRow(true);
            
            result.AddRowAction()
               .HasText(T("Sửa"))
               .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
               .HasButtonStyle(ButtonStyle.Default)
               .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddRowAction(true)
                .HasText(T("Xóa"))
                .HasName("Delete")
                .HasValue(x => x.Id)
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall)
                .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return result;
        }

        private string BuildAds()
        {
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

            var service = WorkContext.Resolve<IAdvertisementService>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Chuyên mục") + " <select id=\"" + Extensions.Constants.CategoryId + "\" name=\"" + Extensions.Constants.CategoryId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            var list = service.GetRecords(x => x.LanguageCode == languageCode && x.SiteId == siteId);
            foreach (var cate in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", cate.Title, cate.Id);
            }

            sb.Append("</select>");

            return sb.ToString();
        }

        private ControlGridAjaxData<VastInfo> GetVast(ControlGridFormRequest options)
        {
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

            var adId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.AdId]))
            {
                adId = Convert.ToInt32(Request.Form[Extensions.Constants.AdId]);
            }

            int totals;
            var items = WorkContext.Resolve<IVastService>().SearchPaged(languageCode, siteId, adId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<VastInfo>(items, totals);

            return result;
        }

        [Url("admin/vast/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý vast"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin vast"), Url = "#" });

            var model = new VastModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IVastService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<VastModel>(model)
            {
                Title = T("Thông tin vast"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = true,
                ShowBoxHeader = false,
                CancelButtonText = T("Đóng"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.MakeReadOnlyProperty(x => x.KeyCode);
            result.MakeReadOnlyProperty(x => x.AdSystemVersion);
            result.MakeReadOnlyProperty(x => x.MediaFileDelivery);
            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.AdId, Url.Action("GetAdBySite"));

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/vast/update")]
        public ActionResult Update(VastModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IVastService>();
            VastInfo item = model.Id == 0 ? new VastInfo() : service.GetById(model.Id);

            item.LanguageCode = model.LanguageCode;
            item.SiteId = model.SiteId;
            item.KeyCode = model.KeyCode;
            item.AdId = model.AdId;
            item.AdSystemVersion = model.AdSystemVersion;
            item.AdSystemValue = model.AdSystemValue;
            item.AdTitle = model.AdTitle;
            item.LinkError = model.LinkError;
            item.LinkImpression = model.LinkImpression;
            item.Skipoffset = model.Skipoffset;
            item.Duration = model.Duration;
            item.LinkClickThrough = model.LinkClickThrough;
            item.TrackingEvent1 = "start";
            item.TrackingValue1 = model.TrackingValue1;
            item.TrackingEvent2 = "firstQuartile";
            item.TrackingValue2 = model.TrackingValue2;
            item.TrackingEvent3 = "midpoint";
            item.TrackingValue3 = model.TrackingValue3;
            item.TrackingEvent4 = "thirdQuartile";
            item.TrackingValue4 = model.TrackingValue4;
            item.TrackingEvent5 = "complete";
            item.TrackingValue5 = model.TrackingValue5;
            item.TrackingEvent6 = "creativeView";
            item.TrackingValue6 = model.TrackingValue6;
            item.MediaFileBitrate = model.MediaFileBitrate;
            item.MediaFileDelivery = model.MediaFileDelivery;
            item.MediaFileHeight = model.MediaFileHeight;
            item.MediaFileWidth = model.MediaFileWidth;
            item.MediaFileMaintainAspectRatio = model.MediaFileMaintainAspectRatio;
            item.MediaFileScalable = model.MediaFileScalable;
            item.MediaFileType = model.MediaFileType;
            item.MediaFileValue = model.MediaFileValue;
            service.Save(item);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"))
                .CloseModalDialog();
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IVastService>();
            var item = service.GetById(id);
            service.Delete(item);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu đã xóa khỏi hệ thống!"));
        }
    }
}
