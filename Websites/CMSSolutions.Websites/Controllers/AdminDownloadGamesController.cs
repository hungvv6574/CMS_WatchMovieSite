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
    public class AdminDownloadGamesController : BaseAdminController
    {
        public AdminDownloadGamesController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblDownloadGames";
        }

        [Url("admin/download-games")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Sự kiện đào xu"), Url = "#" });

            var result = new ControlGridFormResult<DownloadGameInfo>
            {
                Title = T("Sự kiện đào xu"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetDownloadGames,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);
            result.AddColumn(x => x.Id, T("Mã")).HasWidth(60);
            result.AddColumn(x => x.Logo, T("Logo"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsImage(y => y.Logo, Extensions.Constants.CssThumbsSize);

            result.AddColumn(x => x.Title, T("Tiêu đề"));
            result.AddColumn(x => x.VipXu, T("VIPXU"));
            result.AddColumn(x => x.IsActived)
                .HasHeaderText(T("Đã sử dụng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage(true);

            result.AddAction().HasText(T("Thêm mới"))
               .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
               .HasButtonStyle(ButtonStyle.Primary)
               .HasBoxButton(false)
               .HasCssClass(Constants.RowLeft)
               .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);

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

        private ControlGridAjaxData<DownloadGameInfo> GetDownloadGames(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            int totals;
            var items = WorkContext.Resolve<IDownloadGameService>().SearchPaged(searchText, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<DownloadGameInfo>(items, totals);
            return result;
        }

        [Url("admin/download-games/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Sự kiện đào xu"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin sự kiện"), Url = "#" });

            var model = new DownloadGameModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IDownloadGameService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<DownloadGameModel>(model)
            {
                Title = T("Thông tin sự kiện"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = true,
                ShowBoxHeader = false,
                CancelButtonText = T("Trở về"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.MakeReadOnlyProperty(x => x.Code);

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/download-games/update")]
        public ActionResult Update(DownloadGameModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IDownloadGameService>();
            DownloadGameInfo item = model.Id == 0 ? new DownloadGameInfo() : service.GetById(model.Id);

            item.Title = model.Title;
            item.Code = model.Code;
            item.UrlBanner = model.UrlBanner;
            item.Logo = model.Logo;
            item.GooglePlayUrl = model.GooglePlayUrl;
            item.WebsiteUrl = model.WebsiteUrl;
            item.VipXu = model.VipXu;
            item.IsActived = model.IsActived;

            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IDownloadGameService>();
            var item = service.GetById(id);
            service.Delete(item);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Đã xóa dữ liệu này thành công!"));
        }
    }
}
