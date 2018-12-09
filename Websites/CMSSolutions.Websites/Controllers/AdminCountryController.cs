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
    public class AdminCountryController : BaseAdminController
    {
        public AdminCountryController(IWorkContextAccessor workContextAccessor) : 
                base(workContextAccessor)
        {
            TableName = "tblCountries";
        }

        [Url("admin/country")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý nước sản xuất phim"), Url = "#" });

            var result = new ControlGridFormResult<CountryInfo>
            {
                Title = T("Quản lý nước sản xuất phim"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetCountries,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(60);
            result.AddColumn(x => x.Code, T("Mã nước"));
            result.AddColumn(x => x.Name, T("Tên nước"));

            result.AddAction().HasText(T("Thêm mới"))
              .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
              .HasButtonStyle(ButtonStyle.Primary)
              .HasBoxButton(false)
              .HasCssClass(Constants.RowLeft)
              .HasRow(true)
              .ShowModalDialog();

            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
                 .HasText(T("Sửa"))
                 .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                 .HasButtonStyle(ButtonStyle.Default)
                 .HasButtonSize(ButtonSize.ExtraSmall)
                 .ShowModalDialog();

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

        private ControlGridAjaxData<CountryInfo> GetCountries(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            int totals;
            var items = WorkContext.Resolve<ICountryService>().SearchPaged(searchText, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<CountryInfo>(items, totals);

            return result;
        }

        [Themed(false)]
        [Url("admin/country/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý nước sản xuất phim"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin nước sản xuất phim"), Url = "#" });

            var service = WorkContext.Resolve<ICountryService>();
            var model = new CountryModel();
            if (id > 0)
            {
                model = service.GetById(id);
            }

            var result = new ControlFormResult<CountryModel>(model)
            {
                Title = T("Thông tin nước sản xuất phim"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = true,
                ShowBoxHeader = false,
                CancelButtonText = T("Đóng"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/country/update")]
        public ActionResult Update(CountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<ICountryService>();
            CountryInfo item = model.Id == 0 ? new CountryInfo() : service.GetById(model.Id);
            if (service.CheckExist(model.Id, model.Code))
            {
                return new AjaxResult()
                    .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                    .Alert(T("Mã nước sản xuất đã tồn tại!"));
            }

            if (service.CheckExist(model.Id, model.Name))
            {
                return new AjaxResult()
                    .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                    .Alert(T("Tên nước sản xuất đã tồn tại!"));
            }

            item.Code = model.Code;
            item.Name = model.Name;
            service.Save(item);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"))
                .CloseModalDialog();
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<ICountryService>();
            var item = service.GetById(id);
            service.Delete(item);

			return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE");
        }
    }
}
