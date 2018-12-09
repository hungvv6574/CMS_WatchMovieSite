using System;
using System.Web.Mvc;
using CMSSolutions.Configuration;
using CMSSolutions.Extensions;
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
    public class AdminFilmServerController : BaseAdminController
    {
        public AdminFilmServerController(IWorkContextAccessor workContextAccessor) : 
                base(workContextAccessor)
        {
            TableName = "tblFilmServers";
        }

        [Url("admin/server-films")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý máy chủ phim"), Url = "#" });

            var result = new ControlGridFormResult<FilmServerInfo>
            {
                Title = T("Quản lý máy chủ phim"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetFilmServer,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SiteId, "$('#" + Extensions.Constants.SiteId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(60);
            result.AddColumn(x => x.ServerName, T("Tên máy chủ"));
            result.AddColumn(x => x.ServerIP, T("Địa chỉ IP"));
            result.AddColumn(x => x.Locations,T("Địa điểm"));
            result.AddColumn(x => x.IsVip)
                .HasHeaderText(T("Máy chủ VIP"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage(false);
            result.AddColumn(x => EnumExtensions.GetDisplayName((Status)x.Status), T("Trạng thái"));

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
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
                .HasValue(x => x.Id)
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall)
                .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }

        private ControlGridAjaxData<FilmServerInfo> GetFilmServer(ControlGridFormRequest options)
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

            var statusId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                statusId = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }

            int totals;
            var items = WorkContext.Resolve<IFilmServersService>().GetPaged(languageCode, siteId, statusId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<FilmServerInfo>(items, totals);

            return result;
        }
       
        [Url("admin/server-films/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý máy chủ phim"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin máy chủ phim"), Url = "#" });

            var service = WorkContext.Resolve<IFilmServersService>();
            var model = new FilmServerModel();
            if (id > 0)
            {
                model = service.GetById(id);
                string password = EncryptionExtensions.Decrypt(KeyConfiguration.PublishKey, model.Password);
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException(T(SecurityConstants.ErrorConfigKey).Text);
                }
                model.Password = password;
                string userName = EncryptionExtensions.Decrypt(KeyConfiguration.PublishKey, model.UserName);
                if (string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentException(T(SecurityConstants.ErrorConfigKey).Text);
                }
                model.UserName = userName;
            }

            var result = new ControlFormResult<FilmServerModel>(model)
            {
                Title = T("Thông tin máy chủ phim"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                CancelButtonText = T("Trở về"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterExternalDataSource(x => x.Status, y => BindStatus());

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/server-films/update")]
        public ActionResult Update(FilmServerModel model)
        {
            if (!ModelState.IsValid)
            {
				return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            string password = EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, model.Password);
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(T(SecurityConstants.ErrorConfigKey).Text);
            }

            string userName = EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, model.UserName);
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(T(SecurityConstants.ErrorConfigKey).Text);
            }

            var service = WorkContext.Resolve<IFilmServersService>();
            FilmServerInfo item = model.Id == 0 ? new FilmServerInfo() : service.GetById(model.Id);

            item.LanguageCode = model.LanguageCode;
            item.SiteId = model.SiteId;
            item.ServerName = model.ServerName;
            item.ServerIP = model.ServerIP;
            item.Password = password;
            item.UserName = userName;
            item.FolderRoot = model.FolderRoot;
            item.IsDefault = model.IsDefault;
            item.Locations = model.Locations;
            item.IsVip = model.IsVip;
            item.Description = model.Description;
            item.Status = model.Status;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IFilmServersService>();
            var item = service.GetById(id);
            item.Status = (int)Status.Deleted;
            service.Update(item);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
