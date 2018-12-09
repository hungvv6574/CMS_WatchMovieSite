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
    public class AdminAdvertisementGroupController : BaseAdminController
    {
        public AdminAdvertisementGroupController(IWorkContextAccessor workContextAccessor)
            : base(workContextAccessor)
        {
            TableName = "tblAdvertisementGroup";
        }

        [Url("admin/advertisementgroup")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý nhóm quảng cáo"), Url = "#" });

            var result = new ControlGridFormResult<AdvertisementGroupInfo>
            {
                Title = T("Quản lý nhóm quảng cáo"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetAdvertisementGroup,
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
            result.AddCustomVar(Extensions.Constants.CategoryId, "$('#" + Extensions.Constants.CategoryId + "').val();", true);

            result.AddColumn(x => x.Code, T("Mã Nhóm"));
            result.AddColumn(x => x.GroupName, T("Tên nhóm"));
            result.AddColumn(x => x.CategoryName, T("Chuyên mục"));
            result.AddColumn(x => x.FullPath, T("Đường dẫn Xml"));
            result.AddColumn(x => x.TextCreateDate, "Ngày bắt đầu");
            result.AddColumn(x => x.TextFinishTime, "Ngày hết hạn");
            result.AddColumn(x => x.IsActived)
                .HasHeaderText(T("Sử dụng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage(false);

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasCssClass(Constants.RowLeft);

            result.AddAction().HasText(T("Tạo Xml"))
                .HasUrl(Url.Action("Index", "AdminVastXml"))
                .HasButtonStyle(ButtonStyle.Success)
                .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSites(true, Extensions.Constants.CategoryId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildCategories)).HasParentClass(Constants.ContainerCssClassCol3).HasRow(true);

            result.AddRowAction()
               .HasText(T("Sửa"))
               .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
               .HasButtonStyle(ButtonStyle.Default)
               .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddRowAction(true)
                .HasText(T("Xóa"))
                .HasName("Delete")
                .HasValue(x => x.Id.ToString(CultureInfo.InvariantCulture.ToString()))
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return result;
        }

        private ControlGridAjaxData<AdvertisementGroupInfo> GetAdvertisementGroup(ControlGridFormRequest options)
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

            var categoryId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.CategoryId]))
            {
                categoryId = Convert.ToInt32(Request.Form[Extensions.Constants.CategoryId]);
            }

            int totals;
            var items = WorkContext.Resolve<IAdvertisementGroupService>().SearchPaged(languageCode, siteId, categoryId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<AdvertisementGroupInfo>(items, totals);

            return result;
        }

        [Url("admin/advertisementgroup/edit")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý nhóm quảng cáo"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin nhóm quảng cáo"), Url = "#" });

            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var model = new AdvertisementGroupModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IAdvertisementGroupService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<AdvertisementGroupModel>(model)
            {
                Title = T("Thông tin nhóm quảng cáo"),
                UpdateActionName = "Update",
                FormMethod = FormMethod.Post,
                SubmitButtonText = T("Lưu lại"),
                ShowBoxHeader = false,
                ShowCancelButton = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.MakeReadOnlyProperty(x => x.Code);
            result.AddAction().HasText(T("Làm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasCssClass("btn btn-success");

            result.AddAction().HasText(T("Trở về"))
               .HasUrl(Url.Action("Index"))
               .HasCssClass("btn btn-danger");

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.CategoryId, Url.Action("GetCategoriesBySite"));
            result.RegisterCascadingDropDownDataSource(x => x.AdvertisementIds, Url.Action("GetAdBySite"));

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/advertisementgroup/update")]
        public ActionResult Update(AdvertisementGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IAdvertisementGroupService>();
            AdvertisementGroupInfo item = model.Id == 0 ? new AdvertisementGroupInfo() : service.GetById(model.Id);
            item.LanguageCode = model.LanguageCode;
            item.SiteId = model.SiteId;
            item.CategoryId = model.CategoryId;
            item.Code = model.Code;
            item.GroupName = model.GroupName;
            item.Description = model.Description;
            item.AdvertisementIds = Utilities.ParseString(model.AdvertisementIds);
            item.IsGenerate = model.IsGenerate;
            item.IsActived = model.IsActived;
            item.CreateDate = DateTime.Now;
            item.FinishDate = DateTime.ParseExact(model.FinishDate, Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            item.FinishTime = model.FinishTime;
            service.Save(item);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IAdvertisementGroupService>();
            var obj = service.GetById(id);
            obj.IsActived = false;
            service.Update(obj);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
