using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AdminSliderController : BaseAdminController
    {
        public AdminSliderController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblSlider";
        }

        [Url("admin/slider")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý slider"), Url = "#" });

            var result = new ControlGridFormResult<SliderInfo>
            {
                Title = T("Quản lý slider"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetSlider,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                DefaultPageSize = WorkContext.DefaultPageSize,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SiteId, "$('#" + Extensions.Constants.SiteId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.PageId, "$('#" + Extensions.Constants.PageId + "').val();", true);

            result.AddColumn(x => x.OrderBy, T("Vị trí")).AlignCenter().HasWidth(60);
            result.AddColumn(x => EnumExtensions.GetDisplayName((SliderPages)x.PageId), T("Trang hiển thị"));
            result.AddColumn(x => x.CategoryName, T("Chuyên mục"));
            result.AddColumn(x => x.FilmName, T("Tên phim"));
          
            result.AddAction().HasText(T("Thêm mới"))
               .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
               .HasButtonStyle(ButtonStyle.Primary)
               .HasBoxButton(false)
               .HasCssClass(Constants.RowLeft)
               .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSites(false, null))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildPages)).HasParentClass(Constants.ContainerCssClassCol3);

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

        protected ControlGridAjaxData<SliderInfo> GetSlider(ControlGridFormRequest options)
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

            var pageId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.PageId]))
            {
                pageId = Convert.ToInt32(Request.Form[Extensions.Constants.PageId]);
            }

            int totals;
            var service = WorkContext.Resolve<ISliderService>();
            service.LanguageCode = languageCode;
            service.SiteId = siteId;
            var records = service.GetPaged(
                pageId,
                options.PageIndex,
                options.PageSize,
                out totals);

            return new ControlGridAjaxData<SliderInfo>(records, totals);
        }

        private string BuildPages()
        {
            var list = EnumExtensions.GetListItems<SliderPages>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Trang hiển thị") + " <select id=\"" + Extensions.Constants.PageId + "\" name=\"" + Extensions.Constants.PageId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var item in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", item.Text, item.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        [Url("admin/slider/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý slider"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin slider"), Url = "#" });

            var model = new SliderModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<ISliderService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<SliderModel>(model)
            {
                Title = T("Thông tin slider"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                CancelButtonText = T("Đóng"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterFileUploadOptions("Images.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.CategoryId, Url.Action("GetCategoriesBySite"));
            result.RegisterCascadingDropDownDataSource(x => x.FilmId, Url.Action("GetFilmsByCategory"));
            result.RegisterExternalDataSource(x => x.PageId, y => BindPages());

            return result;
        }

        [Url("admin/get-films-by-category")]
        public ActionResult GetFilmsByCategory()
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = int.Parse(Request.Form[Extensions.Constants.SiteId]);
            }

            var categoryId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.CategoryId]))
            {
                categoryId = int.Parse(Request.Form[Extensions.Constants.CategoryId]);
            }

            var service = WorkContext.Resolve<IFilmService>();
            service.SiteId = siteId;
            service.LanguageCode = languageCode;
            service.CategoryId = categoryId;
            var items = service.GetAllByCategoryId();
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.FilmName,
                Value = item.Id.ToString()
            }));

            result.Insert(0, new SelectListItem { Text = "--- Chọn phim ---", Value = "0" });

            return Json(result);
        }


        private IEnumerable<SelectListItem> BindPages()
        {
            var items = EnumExtensions.GetListItems<SliderPages>();
            return items;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/slider/update")]
        public ActionResult Update(SliderModel model)
        {
            var service = WorkContext.Resolve<ISliderService>();
            var service2 = WorkContext.Resolve<IFilmService>();
            var film = service2.GetById(model.FilmId);
            SliderInfo item = model.Id == 0 ? new SliderInfo() : service.GetById(model.Id);
            item.LanguageCode = model.LanguageCode;
            item.SiteId = model.SiteId;
            item.CategoryId = model.CategoryId;
            item.FilmId = model.FilmId;
            item.Images = model.Images;
            if (film != null && string.IsNullOrEmpty(model.Images))
            {
                item.Images = film.ImageThumb; 
            }
            item.PageId = model.PageId;
            item.OrderBy = model.OrderBy;

            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<ISliderService>();
            var model = service.GetById(id);
            service.Delete(model);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE");
        }
    }
}
