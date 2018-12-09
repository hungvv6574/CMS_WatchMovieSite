using System;
using System.Globalization;
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
    public class AdminActorController : BaseAdminController
    {
        public AdminActorController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblActors";
        }
        
        [Url("admin/actors")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý diễn viên"), Url = "#" });

            var result = new ControlGridFormResult<ActorInfo>
            {
                Title = T("Quản lý diễn viên"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetActors,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(60);
            result.AddColumn(x => x.FullName, T("Họ và Tên"));
            result.AddColumn(x => EnumExtensions.GetDisplayName((Status)x.Status), T("Trạng thái"));

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasRow(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true)
                .ShowModalDialog();

            result.AddAction(new ControlFormHtmlAction(BuildSearchText)).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildStatus)).HasParentClass(Constants.ContainerCssClassCol3);
            
            result.AddRowAction()
                .HasText(T("Sửa"))
                .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                .HasButtonStyle(ButtonStyle.Default)
                .HasButtonSize(ButtonSize.ExtraSmall)
                .ShowModalDialog();

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

        private ControlGridAjaxData<ActorInfo> GetActors(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            var statusId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                statusId = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }

            int totals;
            var items = WorkContext.Resolve<IActorService>().SearchPaged(searchText, statusId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<ActorInfo>(items, totals);

            return result;
        }
        
        [Themed(false)]
        [Url("admin/actors/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý diễn viên"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin diễn viên"), Url = "#" });

            var model = new ActorModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IActorService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<ActorModel>(model)
            {
                Title = T("Thông tin diễn viên"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                CancelButtonText = T("Đóng"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterExternalDataSource(x => x.Status, y => BindStatus());

            return result;
        }
        
        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/actors/update")]
        public ActionResult Update(ActorModel model)
        {
            if (!ModelState.IsValid)
            {
				return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IActorService>();
            ActorInfo item = model.Id == 0 ? new ActorInfo() : service.GetById(model.Id);

            if (service.CheckExist(model.Id, model.FullName))
            {
                return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE")
                    .Alert(T("Họ và Tên đã tồn tại."));
            }

            item.FullName = model.FullName;
            item.Description = model.Description;
            item.Status = model.Status;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"))
                .CloseModalDialog();
        }
        
        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IActorService>();
            var item = service.GetById(id);
            item.Status = (int)Status.Deleted;
            service.Update(item);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
