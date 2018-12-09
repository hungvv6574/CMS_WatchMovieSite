using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class AdminFilmController : BaseAdminController
    {
        public AdminFilmController(IWorkContextAccessor workContextAccessor) : 
                base(workContextAccessor)
        {
            TableName = "tblFilms";
        }

        private int pageSize = 50;

        [Url("admin/films")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý phim"), Url = "#" });
            var result = new ControlGridFormResult<FilmInfo>
            {
                Title = T("Quản lý phim"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetFilms,
                UpdateActionName = "Update",
                ActionsColumnWidth = 150,
                ClientId = TableName,
                EnablePaginate = true,
                DefaultPageSize = pageSize,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SiteId, "$('#" + Extensions.Constants.SiteId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.UserId, "$('#" + Extensions.Constants.UserId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.CategoryId, "$('#" + Extensions.Constants.CategoryId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.DirectorId, "$('#" + Extensions.Constants.DirectorId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ActorId, "$('#" + Extensions.Constants.ActorId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FilmGroup, "$('#" + Extensions.Constants.FilmGroup + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FilmTypesId, "$('#" + Extensions.Constants.FilmTypesId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.FromDate, "$('#" + Extensions.Constants.FromDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.ToDate, "$('#" + Extensions.Constants.ToDate + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(40);
            result.AddColumn(x => x.ImageIcon, T("Ảnh đại diện"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsImage(y => y.ImageIcon, Extensions.Constants.CssThumbsSize);

            result.AddColumn(x => x.FilmName, T("Tên phim"));
            result.AddColumn(x => x.FilmNameEnglish, T("Tên tiếng anh"));
            result.AddColumn(x => x.CollectionName, T("Tên bộ phim"));
            result.AddColumn(x => x.FilmTypeNames, T("Thể loại phim"));
            result.AddColumn(x => x.DisplayDate, T("Ngày tạo"));
            result.AddColumn(x => x.FullName, T("Người đăng"));
            result.AddColumn(x => x.IsPublished)
                .HasHeaderText(T("Đã đăng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();
            result.AddColumn(x => x.IsHot)
                .HasHeaderText(T("Phim mới"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();
            result.AddColumn(x => x.HasCopyright)
                .HasHeaderText(T("Có BQ"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl("javascript:  redirectUrl(0, '" + Url.Action("Edit", "AdminFilm") + "');")
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasRow(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSites(true, Extensions.Constants.CategoryId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildCategories)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildUsers).HasParentClass(Constants.ContainerCssClassCol3));
            result.AddAction(new ControlFormHtmlAction(BuildDirectors)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildActors)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildFilmTypes)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildFilmGroups)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildFromDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildToDate())).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildHtmlScript)).HasBoxButton(false);

            result.AddRowAction()
                .HasText(T("Sửa"))
                .HasUrl(x => "javascript:  redirectUrl("+x.Id+", '" + Url.Action("Edit", "AdminFilm") + "');")
                .HasButtonStyle(ButtonStyle.Default)
                .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddRowAction()
                .HasText(T("Videos"))
                .HasUrl(x => "javascript:  redirectUrl(" + x.Id + ", '" + Url.Action("Index", "AdminFilmVideos") + "');")
                .HasButtonStyle(ButtonStyle.Info)
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

        private string BuildHtmlScript()
        {
            var sb = new StringBuilder();

            sb.Append("<script type=\"text/javascript\">");
            sb.Append("function redirectUrl(id, url) {");
            sb.Append("var languageCode = $(\"#" + Extensions.Constants.LanguageCode + "\").val();");
            sb.Append("var siteId = $(\"#" + Extensions.Constants.SiteId + "\").val();");
            sb.Append("var categoryId = $(\"#" + Extensions.Constants.CategoryId + "\").val();");
            sb.Append("var userId = $(\"#" + Extensions.Constants.UserId + "\").val();");
            sb.Append("var fromDate = $(\"#" + Extensions.Constants.FromDate + "\").val();");
            sb.Append("var toDate = $(\"#" + Extensions.Constants.ToDate + "\").val();");
            sb.Append("var searchText =  $(\"#" + Extensions.Constants.SearchText + "\").val();");
            sb.Append("var statusId = $(\"#" + Extensions.Constants.StatusId + "\").val();");
            sb.Append("var directorId = $(\"#" + Extensions.Constants.DirectorId + "\").val();");
            sb.Append("var actorId = $(\"#" + Extensions.Constants.ActorId + "\").val();");
            sb.Append("var filmGroup = $(\"#" + Extensions.Constants.FilmGroup + "\").val();");
            sb.Append("var filmTypesId = $(\"#" + Extensions.Constants.FilmTypesId + "\").val();");
            sb.AppendFormat("var returnUrl = languageCode + '{0}' + siteId + '{0}' + categoryId + '{0}' + userId + '{0}'", ",");
            sb.AppendFormat("+ directorId + '{0}' + actorId + '{0}' + filmTypesId + '{0}' + filmGroup + '{0}' + statusId + '{0}'", ",");
            sb.AppendFormat("+ fromDate + '{0}' + toDate + '{0}' + encode_utf8(searchText);", ",");
            sb.Append("var encodedString = btoa(returnUrl);");
            sb.AppendFormat("window.location = url + '{0}' + id + '{1}' + encodedString;", "?id=", "&" + Extensions.Constants.ReturnUrl + "=");
            sb.Append("}");
            sb.Append("</script>");

            return sb.ToString();
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

        private ControlGridAjaxData<FilmInfo> GetFilms(ControlGridFormRequest options)
        {
            if (Request.QueryString[Extensions.Constants.PageIndex] != null && Session[Extensions.Constants.BackPageIndex] != null)
            {
                options.PageIndex = int.Parse(Request.QueryString[Extensions.Constants.PageIndex]);
            }

            var languageCode = WorkContext.CurrentCulture;
            if (!string.IsNullOrEmpty(GetQueryString(0)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                languageCode = GetQueryString(0);
            }

            var siteId = 0;
            if (!string.IsNullOrEmpty(GetQueryString(1)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                siteId = int.Parse(GetQueryString(1));
            }

            var categoryId = 0;
            if (!string.IsNullOrEmpty(GetQueryString(2)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                categoryId = Convert.ToInt32(GetQueryString(2));
            }

            long userId = 0;
            if (!string.IsNullOrEmpty(GetQueryString(3)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                userId = long.Parse(GetQueryString(3));
            }

            var directorId = 0;
            if (!string.IsNullOrEmpty(GetQueryString(4)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                directorId = int.Parse(GetQueryString(4));
            }

            var actorId = 0;
            if (!string.IsNullOrEmpty(GetQueryString(5)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                actorId = int.Parse(GetQueryString(5));
            }

            var filmTypesId = 0;
            if (!string.IsNullOrEmpty(GetQueryString(6)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                filmTypesId = int.Parse(GetQueryString(6));
            }

            var filmGroup = 0;
            if (!string.IsNullOrEmpty(GetQueryString(7)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                filmGroup = int.Parse(GetQueryString(7));
            }

            var statusId = 0;
            if (!string.IsNullOrEmpty(GetQueryString(8)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                statusId = int.Parse(GetQueryString(8));
            }

            var fromDate = DateTime.Now.Date;
            if (!string.IsNullOrEmpty(GetQueryString(9)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                fromDate = DateTime.ParseExact(GetQueryString(9), Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            }

            var toDate = DateTime.Now.Date;
            if (!string.IsNullOrEmpty(GetQueryString(10)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                toDate = DateTime.ParseExact(GetQueryString(10), Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            }

            var searchText = string.Empty;
            if (!string.IsNullOrEmpty(GetQueryString(11)) && Session[Extensions.Constants.BackPageIndex] != null)
            {
                searchText = GetQueryString(11);
            }

            int countryId = 0;
            int collectionId = 0;
            int articlesId = 0;
            int serverId = 0;
            int releaseYear = 0;
            DateTime createDate = Utilities.DateNull();
            DateTime publishedDate = Utilities.DateNull();
            int isPublished = -1;
            int isHot = -1;
            int isHome = -1;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }
           
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.CategoryId]))
            {
                categoryId = Convert.ToInt32(Request.Form[Extensions.Constants.CategoryId]);
            }
           
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FromDate]))
            {
                fromDate = DateTime.ParseExact(Request.Form[Extensions.Constants.FromDate], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ToDate]))
            {
                toDate = DateTime.ParseExact(Request.Form[Extensions.Constants.ToDate], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
           
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }
            
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.UserId]))
            {
                userId = long.Parse(Request.Form[Extensions.Constants.UserId]);
            }
            
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                statusId = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }
            
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.DirectorId]))
            {
                directorId = Convert.ToInt32(Request.Form[Extensions.Constants.DirectorId]);
            }
            
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ActorId]))
            {
                actorId = Convert.ToInt32(Request.Form[Extensions.Constants.ActorId]);
            }
            
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FilmGroup]))
            {
                filmGroup = Convert.ToInt32(Request.Form[Extensions.Constants.FilmGroup]);
            }

            int isFilmRetail = -1;
            int isFilmLengthEpisodes = -1;
            if (filmGroup > 0)
            {
                switch ((FilmGroup)filmGroup)
                {
                    case FilmGroup.FilmRetail:
                        isFilmRetail = 1;
                        isFilmLengthEpisodes = 0;
                        break;
                    case FilmGroup.FilmLengthEpisodes:
                        isFilmRetail = 0;
                        isFilmLengthEpisodes = 1;
                        break;
                }
            }

            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FilmTypesId]))
            {
                filmTypesId = Convert.ToInt32(Request.Form[Extensions.Constants.FilmTypesId]);
            }

            int totals;
            var service = WorkContext.Resolve<IFilmService>();
            var records = service.SearchPaged(
                searchText, languageCode, siteId, categoryId,
                filmTypesId, countryId, directorId,
                actorId, collectionId, articlesId,
                userId, serverId, releaseYear, fromDate,
                toDate, createDate, publishedDate, isPublished,
                isHot, isHome, isFilmRetail, isFilmLengthEpisodes, 
                statusId, options.PageIndex, options.PageSize,
                out totals);

            Session[Extensions.Constants.PageIndex] = options.PageIndex;
            Session[Extensions.Constants.BackPageIndex] = null;

            return new ControlGridAjaxData<FilmInfo>(records, totals);
        }

        [Url("admin/films/edit")]
        public ActionResult Edit()
        {
            var id = int.Parse(Request.QueryString["id"]);            
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý phim"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin phim"), Url = "#" });

            var model = new FilmModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IFilmService>();
                var obj = service.GetById(id);
                model = obj;
                if (obj.IsFilmRetail)
                {
                    model.FilmGroup = 1;
                }

                if (obj.IsFilmLengthEpisodes)
                {
                    model.FilmGroup = 2;
                }
            }

            var result = new ControlFormResult<FilmModel>(model)
            {
                Title = T("Thông tin phim"),
                UpdateActionName = "Update",
                FormMethod = FormMethod.Post,
                SubmitButtonText = T("Lưu lại"),
                ShowBoxHeader = false,
                ShowCancelButton = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml,
                SetJavascript = GetUrl()
            };

            result.RegisterFileUploadOptions("ImageIcon.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            result.RegisterFileUploadOptions("ImageThumb.FileName", new ControlFileUploadOptions
            {
                AllowedExtensions = "jpg,jpeg,png,gif"
            });

            if (id > 0)
            {
                result.AddAction().HasText(T("Chọn videos"))
                .HasUrl(Url.Action("Index", "AdminFilmVideos", new { @id = id }))
                .HasButtonStyle(ButtonStyle.Info);
            }

            result.AddAction().HasText(T("Làm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Success);

            result.AddAction().HasText(T("Trở về"))
               .HasUrl("javascript: backUrl();")
               .HasButtonStyle(ButtonStyle.Danger);

            result.MakeReadOnlyProperty(x => x.StartDate);
            result.MakeReadOnlyProperty(x => x.EndDate);
            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterExternalDataSource(x => x.FilmTypeIds, y => BindFilmTypes());
            result.RegisterExternalDataSource(x => x.CountryId, y => BindCountries());
            result.RegisterExternalDataSource(x => x.DirectorId, y => BindDirectors());
            result.RegisterExternalDataSource(x => x.ActorIds, y => BindActors());
            result.RegisterExternalDataSource(x => x.ServerId, y => BindServers());
            result.RegisterExternalDataSource(x => x.FilmGroup, y => BindFilmGroups());
            result.RegisterCascadingDropDownDataSource(x => x.CollectionId, Url.Action("GetCollectionsByType"));
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.CategoryIds, Url.Action("GetCategoriesBySite"));
            result.RegisterExternalDataSource(x => x.Status, y => BindStatus());
            result.RegisterExternalDataSource(x => x.VideoType, y => BindVideoTypes());

            return result;
        }

        private IEnumerable<SelectListItem> BindVideoTypes()
        {
            return EnumExtensions.GetListItems<VideoTypes>();
        }

        [Url("admin/get-collections-by-type")]
        public ActionResult GetCollectionsByType()
        {
            var filmGroup = 1;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.FilmGroup]))
            {
                filmGroup = Convert.ToInt32(Request.Form[Extensions.Constants.FilmGroup]);
            }

            var result = new List<SelectListItem>();
            if ((FilmGroup)filmGroup == FilmGroup.FilmLengthEpisodes)
            {
                var service = WorkContext.Resolve<ICollectionService>();
                var items = service.GetRecords(x => x.Status == (int)Status.Approved).OrderByDescending(x => x.Id);

                result.AddRange(items.Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }));
            }

            result.Insert(0, new SelectListItem { Text = "--- Chọn tên bộ phim ---", Value = "0" });
            return Json(result);
        }

        private IEnumerable<SelectListItem> BindDirectors()
        {
            var service = WorkContext.Resolve<IDirectorService>();
            var items = service.GetRecords(x => x.Status == (int)Status.Approved).OrderByDescending(x => x.Id);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = "--- Đạo diễn phim ---", Value = "0" });

            return result;
        }

        private IEnumerable<SelectListItem> BindActors()
        {
            var service = WorkContext.Resolve<IActorService>();
            var items = service.GetRecords(x => x.Status == (int)Status.Approved).OrderByDescending(x => x.Id);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            }));

            return result;
        }

        private IEnumerable<SelectListItem> BindFilmGroups()
        {
            var items = EnumExtensions.GetListItems<FilmGroup>();
            items.Insert(0, new SelectListItem { Text = "--- Chọn nhóm film ---", Value = "0"});

            return items;
        }

        private IEnumerable<SelectListItem> BindServers()
        {
            var service = WorkContext.Resolve<IFilmServersService>();
            var items = service.GetRecords(x => x.Status == (int)Status.Approved);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.ServerName,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem { Text = "--- Máy chủ phim ---", Value = "0" });

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/films/update")]
        public ActionResult Update(FilmModel model)
        {
            if (!ModelState.IsValid)
            {
				return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IFilmService>();
            FilmInfo item;
            if (model.Id == 0)
            {
                item = new FilmInfo();
                item.ViewCount = 20000;
            }
            else
            {
                item = service.GetById(model.Id);
            }

            var alias = model.FilmAlias;
            if (string.IsNullOrEmpty(alias))
            {
                alias = Utilities.GetAlias(model.FilmName);
                if (service.CheckAlias(0, alias))
                {
                    alias += "-" + DateTime.Now.ToString("ddMMyyyyhhmm");
                }
            }

            if (model.Id == 0 && service.CheckName(model.FilmName))
            {
                return new AjaxResult().Alert(T("Đã tồn tại phim này!"));
            }

            if (model.Id == 0)
            {
                item.FilmCode = service.GetLatestFilmCode();
            }

            item.FilmNameEnglish = model.FilmNameEnglish;
            item.FilmName = model.FilmName;
            item.FilmAlias = alias;
            item.LanguageCode = model.LanguageCode;
            item.SiteId = model.SiteId;
            item.CategoryIds = Utilities.ParseString(model.CategoryIds);
            item.FilmTypeIds = Utilities.ParseString(model.FilmTypeIds);
            item.CountryId = model.CountryId;
            item.DirectorId = model.DirectorId;
            item.ActorIds = Utilities.ParseString(model.ActorIds);
            item.CollectionId = model.CollectionId;
            item.ArticlesId = 0;
            item.Time = model.Time;
            item.Capacity = model.Capacity;
            item.ReleaseYear = model.ReleaseYear;
            item.CreateByUserId = WorkContext.CurrentUser.Id;
            item.CreateDate = DateTime.Now;
            item.Contents = model.Contents;
            item.Summary = model.Summary;
            item.Description = model.Description;
            item.Tags = model.Tags;
            item.IsPublished = model.IsPublished;
            if (string.IsNullOrEmpty(model.PublishedDate))
            {
                model.PublishedDate = Extensions.Constants.DateTimeMin;
            }
            item.PublishedDate = DateTime.ParseExact(model.PublishedDate, Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);
            item.IsHot = model.IsHot;
            item.IsHome = model.IsHome;
            item.Prices = (decimal)model.Prices;
            item.HasCopyright = model.HasCopyright;

            item.StartDate = DateTime.Now;

            //if (string.IsNullOrEmpty(model.StartDate))
            //{
            //    model.StartDate = Extensions.Constants.DateTimeMin;
            //}
            //item.StartDate = DateTime.ParseExact(model.StartDate, Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);

            item.EndDate = DateTime.Now;
            //if (string.IsNullOrEmpty(model.EndDate))
            //{
            //    model.EndDate = Extensions.Constants.DateTimeMin;
            //}
            //item.EndDate = DateTime.ParseExact(model.EndDate, Extensions.Constants.DateTimeFomat, CultureInfo.InvariantCulture);

            item.ImageIcon = model.ImageIcon;
            item.ImageThumb = model.ImageThumb;
            item.ServerId = model.ServerId;
            var server = WorkContext.Resolve<IFilmServersService>().GetById(model.ServerId);
            if (server != null)
            {
                item.ServerIp = server.ServerIP;
            }
            item.Status = model.Status;
            bool isFilmRetail = false;
            bool isFilmLengthEpisodes = false;
            switch ((FilmGroup)model.FilmGroup)
            {
                case FilmGroup.FilmRetail:
                    isFilmRetail = true;
                    break;
                case FilmGroup.FilmLengthEpisodes:
                    isFilmLengthEpisodes = true;
                    break;
            }
            item.IsFilmRetail = isFilmRetail;
            item.IsFilmLengthEpisodes = isFilmLengthEpisodes;
            item.IsTheater = model.IsTheater;
            switch ((VideoTypes)model.VideoType)
            {
                case VideoTypes.IsFilm:
                    item.IsFilm = true;
                    item.IsClip = false;
                    item.IsShow = false;
                    item.IsTrailer = false;
                    break;
                case VideoTypes.IsShow:
                    item.IsFilm = false;
                    item.IsClip = false;
                    item.IsShow = true;
                    item.IsTrailer = false;
                    break;
                case VideoTypes.IsClip:
                    item.IsFilm = false;
                    item.IsClip = true;
                    item.IsShow = false;
                    item.IsTrailer = false;
                    break;
                case VideoTypes.IsTrailer:
                    item.IsFilm = false;
                    item.IsClip = false;
                    item.IsShow = false;
                    item.IsTrailer = true;
                    break;
            }

            service.Save(item);

            try
            {
                #region Add Search
                var service2 = WorkContext.Resolve<ISearchService>();
                var searchObject = service2.GetBySearchId(item.Id.ToString(), (int)SearchType.Film);
                long id = 0;
                if (searchObject != null)
                {
                    id = searchObject.Id;
                }

                var search = new SearchInfo
                {
                    Id = id,
                    Title = item.FilmName,
                    Alias = item.FilmAlias,
                    CategoryIds = item.CategoryIds,
                    CreateDate = item.CreateDate,
                    IsBlock = !item.IsPublished,
                    LanguageCode = item.LanguageCode,
                    SearchId = item.Id.ToString(),
                    SiteId = item.SiteId,
                    Sumary = item.Summary,
                    Tags = item.Tags,
                    Type = (int)SearchType.Film,
                    TitleEnglish = item.FilmNameEnglish,
                    Images = item.ImageIcon,
                    ViewCount = item.ViewCount.ToString(),
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
            var service = WorkContext.Resolve<IFilmService>();
            var item = service.GetById(id);
            item.Status = (int)Status.Deleted;
            service.Update(item);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
