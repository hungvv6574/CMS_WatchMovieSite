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
    public class AdminCategoryController : BaseAdminController
    {
        public AdminCategoryController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblCategories";
        }

        [Url("admin/category")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý chuyên mục"), Url = "#" });

            var result = new ControlGridFormResult<CategoryInfo>
            {
                Title = T("Quản lý chuyên mục"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetCategories,
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
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("Mã"))
                .AlignCenter()
                .HasWidth(60);
            result.AddColumn(x => x.ParentName, T("Chuyên mục cha")).HasWidth(150);
            result.AddColumn(x => x.ShortName, T("Chuyên mục")).HasWidth(150);
            result.AddColumn(x => x.Name, "Tên hỗ trợ SEO");
            result.AddColumn(x => x.OrderBy, "Vị trí")
                .AlignCenter()
                .HasWidth(60);
            result.AddColumn(x => x.IsActived)
                .HasHeaderText(T("Hiển thị"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasRow(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSites(false, string.Empty))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol3);

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
                .HasButtonSize(ButtonSize.ExtraSmall)
                .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return result;
        }

        private ControlGridAjaxData<CategoryInfo> GetCategories(ControlGridFormRequest options)
        {
            var service = WorkContext.Resolve<ICategoryService>();
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

            bool status = false;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                var statusId = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
                switch ((Status)statusId)
                {
                    case Status.Approved:
                        status = false;
                        break;
                    case Status.Deleted:
                        status = true;
                        break;
                }
            }

            int totals;
            service.LanguageCode = languageCode;
            service.SiteId = siteId;
            var list = service.GetPaged(status, options.PageIndex, options.PageSize, out totals);

            return new ControlGridAjaxData<CategoryInfo>(list, totals);
        }

        [Url("admin/category/edit")]
        public ActionResult Edit(int id)
        {
            var model = new CategoryModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<ICategoryService>();
                model = service.GetById(id); 
            }
            
            var result = new ControlFormResult<CategoryModel>(model)
            {
                Title = T("Thông tin chuyên mục"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = false,
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.ParentId, Url.Action("GetCategoriesBySite"));

            result.AddAction().HasText(T("Làm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasCssClass("btn btn-success");

            result.AddAction().HasText(T("Trở về"))
               .HasUrl(Url.Action("Index"))
               .HasCssClass("btn btn-danger");

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/category/update")]
        public ActionResult Update(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            CategoryInfo categoryInfo;
            var service = WorkContext.Resolve<ICategoryService>();
            service.SiteId = model.SiteId;
            service.LanguageCode = model.LanguageCode;

            var alias = Utilities.GetAlias(model.Name);
            if (!string.IsNullOrEmpty(alias))
            {
                if (service.CheckAlias(model.Id, alias))
                {
                    alias += DateTime.Now.ToString("ddMMyyyyhhmm");
                }
            }

            var url = model.Url;
            if (string.IsNullOrEmpty(url))
            {
                url = alias;
            }

            categoryInfo = model.Id == 0 ? new CategoryInfo() : service.GetById(model.Id);

            categoryInfo.SiteId = model.SiteId;
            categoryInfo.ParentId = model.ParentId;
            categoryInfo.ShortName = model.ShortName;
            categoryInfo.Name = model.Name;
            categoryInfo.Alias = alias;
            categoryInfo.LanguageCode = model.LanguageCode;
            categoryInfo.IsActived = model.IsActived;
            categoryInfo.OrderBy = model.OrderBy;
            categoryInfo.Description = model.Description;
            categoryInfo.SiteId = model.SiteId;
            categoryInfo.CreateDate = DateTime.Now.Date;
            categoryInfo.IsDeleted = model.IsDeleted;
            categoryInfo.Url = url;
            categoryInfo.IsHome = model.IsHome;
            categoryInfo.HasChilden = model.HasChilden;
            categoryInfo.Tags = model.Tags;

            service.Save(categoryInfo);

            try
            {
                #region Add Search
                var service2 = WorkContext.Resolve<ISearchService>();
                var searchObject = service2.GetBySearchId(categoryInfo.Id.ToString(), (int)SearchType.Category);
                long id = 0;
                if (searchObject != null)
                {
                    id = searchObject.Id;
                }

                var search = new SearchInfo
                {
                    Id = id,
                    Title = categoryInfo.ShortName,
                    Alias = categoryInfo.Alias,
                    CategoryIds = categoryInfo.Id.ToString(),
                    CreateDate = categoryInfo.CreateDate,
                    IsBlock = categoryInfo.IsDeleted,
                    LanguageCode = categoryInfo.LanguageCode,
                    SearchId = categoryInfo.Id.ToString(),
                    SiteId = categoryInfo.SiteId,
                    Sumary = categoryInfo.Name,
                    Tags = categoryInfo.Tags,
                    Type = (int)SearchType.Category,
                    TitleEnglish = string.Empty,
                    Images = string.Empty,
                    ViewCount = "0",
                    Processed = 0
                };

                service2.Save(search);

                var cacheSearch = new LuceneService();
                cacheSearch.UpdateLuceneIndex(search);
                #endregion
            }
            catch (Exception)
            {
                return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE")
                    .Alert(T("Không thể thêm dữ liệu tìm kiếm."));
            }

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<ICategoryService>();
            var obj = service.GetById(id);
            service.LanguageCode = obj.LanguageCode;
            service.SiteId = obj.SiteId;
            obj.IsDeleted = true;
            service.Update(obj);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE");
        }
    }
}
