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
    public class AdminSiteController : BaseAdminController
    {
        public AdminSiteController(IWorkContextAccessor workContextAccessor) : base(workContextAccessor)
        {
            TableName = "tblSites";
        }

        [Url("admin/sites")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý trang web"), Url = "#" });

            var result = new ControlGridFormResult<SiteInfo>
            {
                Title = T("Quản lý trang web"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetSites,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                DefaultPageSize = WorkContext.DefaultPageSize,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(60);
            result.AddColumn(x => x.Name, T("Tên trang web"));
            result.AddColumn(x => x.Domain, T("Tên miền"));
            result.AddColumn(x => x.Url, T("Đường dẫn"));
            result.AddColumn(x => x.IsActived)
                .HasHeaderText(T("Sử dụng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();


            result.AddAction().HasText(T("Thêm mới"))
               .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
               .HasButtonStyle(ButtonStyle.Primary)
               .HasBoxButton(false)
               .HasCssClass(Constants.RowLeft)
               .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(false, string.Empty))).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
                .HasText(T("Sửa"))
                .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                .HasButtonStyle(ButtonStyle.Default)
                .HasButtonSize(ButtonSize.ExtraSmall);

            //result.AddRowAction(true)
            //    .HasText(T("Xóa"))
            //    .HasName("Delete")
            //    .HasValue(x => x.Id)
            //    .HasButtonStyle(ButtonStyle.Danger)
            //    .HasButtonSize(ButtonSize.ExtraSmall)
            //    .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            //result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }

        private ControlGridAjaxData<SiteInfo> GetSites(ControlGridFormRequest options)
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var items = WorkContext.Resolve<ISiteService>().GetRecords(x => x.LanguageCode == languageCode);
            var result = new ControlGridAjaxData<SiteInfo>(items);
            return result;
        }
        
        [Url("admin/sites/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var model = new SiteModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<ISiteService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<SiteModel>(model)
            {
                Title = T("Thông tin trang web"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = false,
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.AddAction().HasText(T("Trở về"))
               .HasUrl(Url.Action("Index"))
               .HasCssClass("btn btn-danger");

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/sites/update")]
        public ActionResult Update(SiteModel model)
        {
            SiteInfo item;
            var service = WorkContext.Resolve<ISiteService>();
            if (model.Id == 0)
            {
                item = new SiteInfo();
            }
            else
            {
                item = service.GetById(model.Id);
            }

            item.Name = model.Name;
            item.LanguageCode = model.LanguageCode;
            item.Url = model.Url;
            item.Domain = model.Domain;
            item.IsActived = model.IsActived;
            item.Description = model.Description;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Cập nhật thành công!"));
        }

        //[FormButton("Delete")]
        //[HttpPost, ActionName("Update")]
        //public ActionResult Delete(int id)
        //{
        //    var service = WorkContext.Resolve<ISiteService>();
        //    var model = service.GetById(id);
        //    service.Delete(model);

        //    return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE");
        //}
    }
}
