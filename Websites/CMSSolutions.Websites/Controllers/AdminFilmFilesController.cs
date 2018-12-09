using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMSSolutions.Configuration;
using CMSSolutions.Security.Cryptography;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Routing;
using CMSSolutions.Web.Security;
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
    public class FilmFilesController : BaseAdminController
    {
        public FilmFilesController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {
            TableName = "tblVideoFiles";
        }

        [Url("admin/video-files")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý video files"), Url = "#" });

            var result = new ControlGridFormResult<FilmFilesInfo>
            {
                Title = T("Quản lý video files"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetFilmFiles,
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
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.ServerId + "').val();", true);

            result.AddColumn(x => x.Name, T("Tên file"));
            result.AddColumn(x => x.FullPath, T("Đường dẫn đầy đủ"));
            result.AddColumn(x => x.ServerName, T("Máy chủ"));
            result.AddColumn(x => x.DisplayDate, T("Ngày tạo")).HasWidth(100);
            result.AddColumn(x => x.Size, T("Kích thước")).HasWidth(100).AlignCenter();
            result.AddColumn(x => x.HasUse, T("Sử dụng"))
                .HasHeaderText(T("Sử dụng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddAction().HasText(T("Đồng bộ videos"))
                .HasUrl(Url.Action("RefreshVideos"))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasRow(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true)
                .ShowModalDialog();

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSiteServers(true, Extensions.Constants.ServerId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildServers)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
                 .HasText(T("Xem"))
                 .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                 .HasButtonStyle(ButtonStyle.Default)
                 .HasButtonSize(ButtonSize.ExtraSmall);

            //result.AddRowAction(true)
            //   .HasText(T("Xóa"))
            //   .HasName("Delete")
            //   .HasValue(x => x.Id)
            //   .HasButtonStyle(ButtonStyle.Danger)
            //   .HasButtonSize(ButtonSize.ExtraSmall)
            //   .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }

        private ControlGridAjaxData<FilmFilesInfo> GetFilmFiles(ControlGridFormRequest options)
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

            var serverId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ServerId]))
            {
                serverId = Convert.ToInt32(Request.Form[Extensions.Constants.ServerId]);
            }

            int totals;
            var items = WorkContext.Resolve<IFilmFilesService>().SearchPaged(string.Empty, languageCode, siteId, serverId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<FilmFilesInfo>(items, totals);

            return result;
        }

        [Themed(false)]
        [Url("admin/video-files/refresh-videos")]
        public ActionResult RefreshVideos()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý video files"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Đồng bộ videos từ máy chủ"), Url = "#" });
            var model = new VideoRefreshModel();
            var result = new ControlFormResult<VideoRefreshModel>(model)
            {
                Title = T("Đồng bộ videos từ máy chủ"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Refresh",
                SubmitButtonText = T("Đồng bộ"),
                CancelButtonText = T("Đóng"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterExternalDataSource(x => x.ServerId, y => BindServers());
            result.RegisterCascadingDropDownDataSource(x => x.RootFolders, Url.Action("GetRootFolders"));
            result.RegisterCascadingDropDownDataSource(x => x.ChildrenFolders, Url.Action("GetChildrenFolders"));

            return result;
        }

        [Url("admin/video-files/get-children-folders")]
        public ActionResult GetChildrenFolders()
        {
            var serverId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ServerId]))
            {
                serverId = Convert.ToInt32(Request.Form[Extensions.Constants.ServerId]);
            }

            var rootFolderName = Extensions.Constants.ValueAll;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.RootFolders]))
            {
                rootFolderName = Request.Form[Extensions.Constants.RootFolders];
            }

            var items = new List<SelectListItem>();
            string rootFolder;
            string languageCode;
            int siteId;
            var client = GetConnectionFtp(serverId, out rootFolder, out languageCode, out siteId);
            if (client != null)
            {
                var list = client.DirectoryList(rootFolderName);
                foreach (var folderPath in list)
                {
                    if (string.IsNullOrEmpty(folderPath))
                    {
                        continue;
                    }

                    if (rootFolderName == Extensions.Constants.ValueAll)
                    {
                        items.Add(new SelectListItem {Text = folderPath, Value = folderPath});
                        continue;
                    }

                    var index = folderPath.LastIndexOf('/');
                    if (index != -1)
                    {
                        var folderName = folderPath.Substring(index + 1, folderPath.Length - (index + 1));
                        items.Add(new SelectListItem { Text = string.Format("{0}/{1}", rootFolder, folderName), Value = folderName });
                    }
                }
            }

            return Json(items);
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/video-files/refresh")]
        public ActionResult Refresh(VideoRefreshModel model)
        {
            var childrenFolders = Extensions.Constants.ValueAll;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ChildrenFolders]))
            {
                childrenFolders = Request.Form[Extensions.Constants.ChildrenFolders];
            }

            if (!string.IsNullOrEmpty(childrenFolders))
            {
                var service = WorkContext.Resolve<IFilmFilesService>();
                string rootFolder;
                string languageCode;
                int siteId;
                var client = GetConnectionFtp(model.ServerId, out rootFolder, out languageCode, out siteId);
                if (client != null)
                {
                    var folder = string.Empty;
                    var f = rootFolder.LastIndexOf('/');
                    if (f != -1)
                    {
                        folder = rootFolder.Substring(f + 1, rootFolder.Length - (f + 1));
                    }
                    var list = childrenFolders.Split(',');
                    foreach (var path in list)
                    {
                        if (string.IsNullOrEmpty(path))
                        {
                            continue;
                        }

                        var listFolderFiles = client.DirectoryList(string.Format("{0}/{1}", rootFolder, path));
                        if (listFolderFiles != null && listFolderFiles.Any())
                        {
                            foreach (var folderFilePath in listFolderFiles)
                            {
                                if (string.IsNullOrEmpty(folderFilePath))
                                {
                                    continue;
                                }

                                var listFiles = client.DirectoryList(string.Format("{0}/{1}", rootFolder, folderFilePath));
                                if (listFiles != null && listFiles.Any())
                                {
                                    var folderName = string.Empty;
                                    var n = folderFilePath.LastIndexOf('/');
                                    if (n != -1)
                                    {
                                        folderName = folderFilePath.Substring(n + 1, folderFilePath.Length - (n + 1));
                                    }
                                    foreach (var filePath in listFiles)
                                    {
                                        if (string.IsNullOrEmpty(filePath))
                                        {
                                            continue;
                                        }

                                        var index = filePath.LastIndexOf('/');
                                        if (index != -1)
                                        {
                                            var fileName = filePath.Substring(index + 1, filePath.Length - (index + 1));
                                            var fullPath = string.Format("{0}/{1}/{2}", rootFolder, folderFilePath, fileName);
                                            var info = service.GetByFullPathFile(fullPath);
                                            if (info == null)
                                            {
                                                var y = fileName.LastIndexOf('.');
                                                if (y != -1)
                                                {
                                                    var extentions = fileName.Substring(y + 1, fileName.Length - (y + 1));
                                                    var file = new FilmFilesInfo
                                                    {
                                                        Id = 0,
                                                        LanguageCode = languageCode,
                                                        SiteId = siteId,
                                                        ServerId = model.ServerId,
                                                        Name = string.Empty,
                                                        FolderRoot = rootFolder,
                                                        FileName = fileName,
                                                        FolderName = folderName,
                                                        FolderDay = path,
                                                        FolderPath = string.Format("{0}/{1}", folder, folderFilePath),
                                                        Extentions = extentions,
                                                        FullPath = fullPath,
                                                        Size = "",//client.GetFileSize(fullPath),
                                                        CreateDate = DateTime.Now.Date,
                                                        HasUse = false,
                                                        FileCode = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                                    };

                                                    service.Insert(file);
                                                }
                                            }
                                        }  
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [Url("admin/video-files/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý video files"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin video files"), Url = "#" });

            var model = new FilmFilesModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IFilmFilesService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<FilmFilesModel>(model)
            {
                Title = T("Thông tin video files"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                ShowCancelButton = true,
                ShowSubmitButton = false,
                ShowBoxHeader = false,
                CancelButtonText = T("Trở về"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.ServerId, Url.Action("GetServersBySite"));

            result.MakeReadOnlyProperty(x => x.FolderName);
            result.MakeReadOnlyProperty(x => x.FullPath);
            result.MakeReadOnlyProperty(x => x.FileName);
            result.MakeReadOnlyProperty(x => x.FolderPath);
            result.MakeReadOnlyProperty(x => x.Extentions);
            result.MakeReadOnlyProperty(x => x.FileCode);
            result.MakeReadOnlyProperty(x => x.Size);
            result.MakeReadOnlyProperty(x => x.Name);
            result.MakeReadOnlyProperty(x => x.HasUse);

            return result;
        }
        
        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/video-files/update")]
        public ActionResult Update(FilmFilesModel model)
        {
            if (!ModelState.IsValid)
            {
				return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IFilmFilesService>();
            FilmFilesInfo item = model.Id == 0 ? new FilmFilesInfo() : service.GetById(model.Id);

            item.Name = model.Name;
            item.FolderName = model.FolderName;
            item.HasUse = model.HasUse;

            service.Save(item);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        //[FormButton("Delete")]
        //[HttpPost, ActionName("Update")]
        //public ActionResult Delete(int id)
        //{
        //    return new AjaxResult()
        //        .NotifyMessage("DELETE_ENTITY_COMPLETE")
        //        .Alert(T("Đã xóa videos thành công !"));
        //}
    }
}
