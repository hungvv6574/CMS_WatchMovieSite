using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
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
    public class AdminFilmVideosController : BaseAdminController
    {
        public AdminFilmVideosController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {

        }

        [Url("admin/films/select-videos")]
        public ActionResult Index()
        {
            var id = int.Parse(Request.QueryString["id"]);
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý phim"), Url = Url.Action("Index", "AdminFilm") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin phim"), Url = Url.Action("Edit", "AdminFilm", new { id }) });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Chọn video hoặc trailer cho phim"), Url = "#" });
            
            var returnUrl = Request.QueryString[Extensions.Constants.ReturnUrl];
            var model = new FilmVideoModel { FilmId = id, ReturnUrl = returnUrl };
            var result = new ControlFormResult<FilmVideoModel>(model)
            {
                Title = T("Chọn video hoặc trailer cho phim"),
                UpdateActionName = "Update",
                FormMethod = FormMethod.Post,
                SubmitButtonText = T("Lưu lại"),
                ShowBoxHeader = false,
                CancelButtonText = T("Trở về"),
                CancelButtonUrl = "javascript: backUrl();",
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml,
                SetJavascript = GetUrl()
            };

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.ServerId, Url.Action("GetServersBySite"));
            result.RegisterCascadingDropDownDataSource(x => x.RootFolders, Url.Action("GetRootFolders"));
            result.RegisterCascadingDropDownDataSource(x => x.FolderDay, Url.Action("GetDayFolders"));
            result.RegisterCascadingDropDownDataSource(x => x.ChildrenFolders, Url.Action("GetChildrenFolders"));
            result.RegisterCascadingDropDownDataSource(x => x.FileIds, Url.Action("GetFilesByFolder"));
            result.RegisterExternalDataSource(x => x.EpisodeIds, y => BindEpisodes());

            var result2 = new ControlGridFormResult<FilmVideoInfo>
            {
                Title = T("Danh sách phim và trailer"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetFilmVideos,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result2.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(60);
            result2.AddColumn(x => x.FileName, T("Tên video"));
            result2.AddColumn(x => x.EpisodeName, T("Tập phim"));
            result2.AddColumn(x => x.FullPath, T("Đường dẫn"));
            result2.AddColumn(x => x.IsTraller)
               .HasHeaderText(T("Traller"))
               .AlignCenter()
               .HasWidth(100)
               .RenderAsStatusImage();
            result2.AddColumn(x => x.IsActived)
               .HasHeaderText(T("Sử dụng"))
               .AlignCenter()
               .HasWidth(100)
               .RenderAsStatusImage();

            result2.AddRowAction(true)
                .HasText(T("Xóa"))
                .HasName("Delete")
                .HasValue(x => x.Id)
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall)
                .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result2.AddCustomVar(Extensions.Constants.Id, id);
            result2.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return new ControlFormsResult(result, result2);
        }

        private string GetUrl()
        {
            var pageIndex = 1;
            if (Session[Extensions.Constants.PageIndex] != null)
            {
                pageIndex = int.Parse(Session[Extensions.Constants.PageIndex].ToString());
            }

            Session[Extensions.Constants.BackPageIndex] = "BACKPAGE";
            var returnUrl = Request.QueryString[Extensions.Constants.ReturnUrl];
            var url = Url.Action("Index", "AdminFilm");
            var sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("function backUrl() {");
            sb.AppendFormat("window.location = '{0}?{1}={2}&{3}={4}';", url, Extensions.Constants.ReturnUrl, returnUrl, Extensions.Constants.PageIndex, pageIndex);
            sb.Append("}");
            sb.Append("</script>");
            return sb.ToString();
        }

        private IEnumerable<SelectListItem> BindEpisodes()
        {
            var service = WorkContext.Resolve<IEpisodesService>();
            var items = service.GetAll(WorkContext.CurrentCulture, 0, (int)Status.Approved);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.EpisodeName,
                Value = item.Id.ToString()
            }));

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/select-videos/update")]
        public ActionResult Update(FilmVideoModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IFilmVideoService>();
            var service2 = WorkContext.Resolve<IFilmFilesService>();
            var service3 = WorkContext.Resolve<IFilmService>();
            var film = service3.GetById(model.FilmId);
            if (model.Subtitle == null)
            {
                return new AjaxResult().Alert(T("Bạn chưa nhập Subtitle."));
            }
            var listSub = model.Subtitle.Split(',');
            if (listSub.Length != model.EpisodeIds.Length)
            {
                return new AjaxResult().Alert(T("Kiểm tra lại số lượng Subtitle so với số tập phim."));
            }

            var index = 0;
            if (model.FileIds != null && model.FileIds.Length > 0)
            {
                if (listSub.Length != model.FileIds.Length)
                {
                    return new AjaxResult().Alert(T("Kiểm tra lại số lượng Subtitle so với số phim."));
                }

                var server = WorkContext.Resolve<IFilmServersService>().GetById(model.ServerId);
                foreach (var fileId in model.FileIds)
                {
                    var item = service.GetByFilmFile(model.FilmId, fileId) ?? new FilmVideoInfo();
                    var file = service2.GetById(fileId);
                    if (file != null)
                    {
                        item.FileId = file.Id;
                        item.FullPath = file.FullPath;
                        item.BaseUrl = file.FullPath;
                        item.StreamingUrl = string.Format(Extensions.Constants.StreamingUrl, server.ServerIP, file.FolderPath + "/" + file.FileName);
                    }
                    item.FilmId = model.FilmId;
                    item.EpisodeId = model.EpisodeIds[index];
                    item.IsTraller = model.IsTraller;
                    item.IsActived = model.IsActived;
                    item.ImageIcon = film.ImageIcon;
                    item.Subtitle = listSub[index];
                    item.UrlAlbum = string.Empty;
                    
                    service.Save(item);

                    #region Update name
                    if (file != null)
                    {
                        file.Name = film.FilmName;
                        file.HasUse = true;
                        service2.Update(file);
                    }
                    #endregion

                    index++;
                }
            }
            else
            {
                var listFilms = model.UrlSource.Split(',');
                if (listSub.Length != listFilms.Length)
                {
                    return new AjaxResult().Alert(T("Kiểm tra lại số lượng Subtitle so với số url phim."));
                }

                foreach (var url in listFilms)
                {
                    var item = new FilmVideoInfo
                    {
                        FilmId = model.FilmId,
                        EpisodeId = model.EpisodeIds[index],
                        IsTraller = model.IsTraller,
                        IsActived = model.IsActived,
                        ImageIcon = film.ImageIcon,
                        UrlSource = url,
                        Subtitle = listSub[index],
                        FullPath = url,
                        BaseUrl = url,
                        StreamingUrl = url,
                        UrlAlbum = model.UrlAlbum
                    };

                    service.Save(item);
                    index++;
                }
            }

            var redirectUrl = string.Format("{0}?id={1}", Url.Action("Index", "AdminFilmVideos"), film.Id); 
            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                redirectUrl = string.Format("{0}?id={1}&{2}={3}", Url.Action("Index", "AdminFilmVideos"), film.Id, Extensions.Constants.ReturnUrl, model.ReturnUrl);  
            }

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"))
                .Redirect(redirectUrl);
        }

        [Url("admin/film-files/get-files-by-folders")]
        public ActionResult GetFilesByFolder()
        {
            var folderPath = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ChildrenFolders]))
            {
                folderPath = Request.Form[Extensions.Constants.ChildrenFolders];
            }

            var service = WorkContext.Resolve<IFilmVideoService>();
            var items = new List<SelectListItem>();
            var listFiles = service.GetFilesByFile(folderPath);
            if (listFiles != null && listFiles.Count > 0)
            {
                items.AddRange(listFiles.Select(item => new SelectListItem
                {
                    Text = item.FullPath,
                    Value = item.Id.ToString()
                }));
            }

            return Json(items);
        }

        [Url("admin/film-files/get-day-folders")]
        public ActionResult GetDayFolders()
        {
            var folderName = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.RootFolders]))
            {
                folderName = Request.Form[Extensions.Constants.RootFolders];
            }

            var service = WorkContext.Resolve<IFilmFilesService>();
            var items = new List<SelectListItem>();
            var listFolders = service.GetDayFolderByRoot(folderName);
            if (listFolders != null && listFolders.Count > 0)
            {
                items.AddRange(listFolders.Select(item => new SelectListItem
                {
                    Text = item.FolderDay,
                    Value = item.FolderDay
                }));
            }

            return Json(items);
        }

        [Url("admin/film-files/get-children-folders")]
        public ActionResult GetChildrenFolders()
        {
            var folderName = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FolderDay]))
            {
                folderName = Request.Form[Extensions.Constants.FolderDay];
            }

            var service = WorkContext.Resolve<IFilmFilesService>();
            var items = new List<SelectListItem>();
            var listFolders = service.GetChildrenByRootFolders(folderName);
            if (listFolders != null && listFolders.Count > 0)
            {
                items.AddRange(listFolders.Select(item => new SelectListItem
                {
                    Text = item.FolderPath,
                    Value = item.FolderPath
                }));
            }

            return Json(items);
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IFilmVideoService>();
            var item = service.GetById(id);
            if (item.FileId > 0)
            {
                var fileService = WorkContext.Resolve<IFilmFilesService>();
                var file = fileService.GetById(item.FileId);
                if (file != null)
                {
                    file.HasUse = false;
                    file.Name = string.Empty;
                    fileService.Update(file); 
                }
            }
            service.Delete(item);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE");
        }

        private ControlGridAjaxData<FilmVideoInfo> GetFilmVideos(ControlGridFormRequest options)
        {
            var filmId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.Id]))
            {
                filmId = Convert.ToInt32(Request.Form[Extensions.Constants.Id]);
            }

            var totals = 0;
            var items = WorkContext.Resolve<IFilmVideoService>().GetByFilm(filmId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<FilmVideoInfo>(items, totals);

            return result;
        }
    }
}
