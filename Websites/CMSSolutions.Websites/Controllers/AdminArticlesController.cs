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
    public class AdminArticlesController : BaseAdminController
    {
        public AdminArticlesController(
            IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblArticles";
        }

        [Url("admin/articles")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý bài viết"), Url = "#" });

            var result = new ControlGridFormResult<ArticlesInfo>
            {
                Title = T("Quản lý bài viết"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetArticles,
                DefaultPageSize = WorkContext.DefaultPageSize,
                UpdateActionName = "Update",
                EnablePaginate = true,
                ClientId =TableName,
                ActionsColumnWidth = 150,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SiteId, "$('#" + Extensions.Constants.SiteId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.UserId, "$('#" + Extensions.Constants.UserId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.CategoryId, "$('#" + Extensions.Constants.CategoryId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FromDate, "$('#" + Extensions.Constants.FromDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ToDate, "$('#" + Extensions.Constants.ToDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(100);
            result.AddColumn(x => x.Icon, T("Ảnh đại diện"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsImage(y => y.Icon, Extensions.Constants.CssThumbsSize);
            result.AddColumn(x => x.Title, T("Tiêu đề"));
            result.AddColumn(x => x.Alias, T("Tiêu đề không dấu"));
            result.AddColumn(x => x.CategoryName, "Chuyên mục");
            result.AddColumn(x => x.IsPublished)
                .HasHeaderText(T("Đã đăng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage(false);

            result.AddColumn(x => x.IsHome)
                .HasHeaderText(T("Trang chủ"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage(false);

            result.AddColumn(x => x.IsVideo)
                .HasHeaderText(T("Có Videos"))
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
            result.AddAction(new ControlFormHtmlAction(BuildCategories)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildUsers).HasParentClass(Constants.ContainerCssClassCol3));
            result.AddAction(new ControlFormHtmlAction(() => BuildFromDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildToDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
                .HasText(T("Sửa"))
                .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new {id = x.Id})))
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

        protected ControlGridAjaxData<ArticlesInfo> GetArticles(ControlGridFormRequest options)
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
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId] ))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var categoryId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.CategoryId]))
            {
                categoryId = Convert.ToInt32(Request.Form[Extensions.Constants.CategoryId]);
            }

            var fromDate = DateTime.Now.Date;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FromDate]))
            {
                fromDate = DateTime.ParseExact(Request.Form[Extensions.Constants.FromDate], Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            }

            var toDate = DateTime.Now.Date;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ToDate]))
            {
                toDate = DateTime.ParseExact(Request.Form[Extensions.Constants.ToDate], Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            }

            var status = -1;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                var statusId = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
                switch ((Status)statusId)
                {
                        case Status.Approved:
                        status = 0;
                        break;
                        case Status.Deleted:
                        status = 1;
                        break;
                }
            }

            var userId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.UserId]))
            {
                userId = int.Parse(Request.Form[Extensions.Constants.UserId]);
            }

            int totals;
            var service = WorkContext.Resolve<IArticlesService>();
            service.LanguageCode = languageCode;
            service.CategoryId = categoryId;
            var records = service.SearchPaged(
                searchText,
                siteId,
                userId,
                fromDate,
                toDate,
                status,
                options.PageIndex,
                options.PageSize,
                out totals);

            return new ControlGridAjaxData<ArticlesInfo>(records, totals);
        }

        [Url("admin/articles/edit")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý bài viết"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin bài viết"), Url = "#" });

            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var model = new ArticlesModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IArticlesService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<ArticlesModel>(model)
            {
                Title = T("Thông tin bài viết"),
                UpdateActionName = "Update",
                FormMethod = FormMethod.Post,
                SubmitButtonText = T("Lưu lại"),
                ShowBoxHeader = false,
                ShowCancelButton = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterFileUploadOptions("Icon.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            result.AddAction().HasText(T("Làm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasCssClass("btn btn-success");

            result.AddAction().HasText(T("Trở về"))
               .HasUrl(Url.Action("Index"))
               .HasCssClass("btn btn-danger");

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.CategoryId, Url.Action("GetCategoriesBySite"));

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/articles/update")]
        public ActionResult Update(ArticlesModel model)
        {
            var service = WorkContext.Resolve<IArticlesService>();
            service.CategoryId = model.CategoryId;
            service.LanguageCode = model.LanguageCode;

            var alias = model.Alias;
            if (string.IsNullOrEmpty(alias))
            {
                alias = Utilities.GetAlias(model.Title);
                if (service.CheckAlias(alias))
                {
                    alias += "-1";
                }
            }

            ArticlesInfo articlesInfo = model.Id == 0 ? new ArticlesInfo() : service.GetById(model.Id);

            articlesInfo.ParentId = model.ParentId;
            articlesInfo.SiteId = model.SiteId;
            articlesInfo.CategoryId = model.CategoryId;
            articlesInfo.Title = model.Title;
            articlesInfo.LanguageCode = model.LanguageCode;
            articlesInfo.Icon = model.Icon;
            articlesInfo.Alias = alias;
            articlesInfo.Summary = model.Summary;
            articlesInfo.Contents = model.Contents;
            articlesInfo.CreateDate = DateTime.Now.Date;
            articlesInfo.CreateByUser = WorkContext.CurrentUser.Id;
            articlesInfo.IsPublished = model.IsPublished;
            articlesInfo.PublishedDate = DateTime.Now.Date;
            articlesInfo.IsHome = model.IsHome;
            articlesInfo.IsVideo = model.IsVideo;
            articlesInfo.Description = model.Description;
            articlesInfo.IsDeleted = model.IsDeleted;
            articlesInfo.Tags = model.Tags;
            articlesInfo.VideosUrl = model.VideosUrl;

            service.Save(articlesInfo);

            try
            {
                #region Add Search
                var service2 = WorkContext.Resolve<ISearchService>();
                var searchObject = service2.GetBySearchId(articlesInfo.Id.ToString(), (int)SearchType.Articles);
                long id = 0;
                if (searchObject != null)
                {
                    id = searchObject.Id;
                }

                var search = new SearchInfo
                {
                    Id = id,
                    Title = articlesInfo.Title,
                    Alias = articlesInfo.Alias,
                    CategoryIds = articlesInfo.CategoryId.ToString(),
                    CreateDate = articlesInfo.CreateDate,
                    IsBlock = articlesInfo.IsDeleted,
                    LanguageCode = articlesInfo.LanguageCode,
                    SearchId = articlesInfo.Id.ToString(),
                    SiteId = articlesInfo.SiteId,
                    Sumary = articlesInfo.Summary,
                    Tags = articlesInfo.Tags,
                    Type = (int)SearchType.Articles,
                    TitleEnglish = string.Empty,
                    Images = articlesInfo.Icon,
                    ViewCount = articlesInfo.ViewCount.ToString(),
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

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IArticlesService>();
            var obj = service.GetById(id);
            service.CategoryId = obj.CategoryId;
            obj.IsDeleted = true;
            service.Update(obj);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
