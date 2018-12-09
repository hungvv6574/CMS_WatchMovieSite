using System;
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
    public class AdminTagController : BaseAdminController
    {
        public AdminTagController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblTags";
        }

        [Url("admin/tags")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý tags"), Url = "#" });

            var result = new ControlGridFormResult<TagInfo>
            {
                Title = T("Quản lý tags"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetTags,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                EnablePaginate = true,
                DefaultPageSize = WorkContext.DefaultPageSize,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.SearchText, "$('#" + Extensions.Constants.SearchText + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).AlignCenter().HasWidth(60);
            result.AddColumn(x => x.Name, T("Tiêu đề"));
            result.AddColumn(x => x.Alias, T("Tên không dấu"));
            result.AddColumn(x => x.IsDisplay)
                .HasHeaderText(T("Hiển thị"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result.AddAction().HasText(T("Thêm mới"))
               .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
               .HasButtonStyle(ButtonStyle.Primary)
               .HasBoxButton(false)
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
                .HasValue(x => x.Id)
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall)
                .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return result;
        }

        private ControlGridAjaxData<TagInfo> GetTags(ControlGridFormRequest options)
        {
            var searchText = string.Empty;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SearchText]))
            {
                searchText = Request.Form[Extensions.Constants.SearchText];
            }

            var status = -1;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                var display = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
                if (display == (int)Status.Approved)
                {
                    status = 1;
                }
                else
                {
                    status = 0;
                }
            }

            int totals;
            var items = WorkContext.Resolve<ITagService>().GetPaged(searchText,status, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<TagInfo>(items, totals);
            return result;
        }

        [Themed(false)]
        [Url("admin/tags/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý tags"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin tags"), Url = "#" });

            var service = WorkContext.Resolve<ITagService>();
            var model = new TagModel();
            if (id > 0)
            {
                model = service.GetById(id);
            }

            var result = new ControlFormResult<TagModel>(model)
            {
                Title = T("Thông tin tags"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = true,
                ShowBoxHeader = false,
                CancelButtonText = T("Đóng"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.MakeReadOnlyProperty(x => x.Alias);

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/tags/update")]
        public ActionResult Update(TagModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }
           
            var service = WorkContext.Resolve<ITagService>();
            TagInfo item = model.Id == 0 ? new TagInfo() : service.GetById(model.Id);
            if (service.CheckExist(model.Id, model.Name))
            {
                return new AjaxResult()
                    .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                    .Alert(T("Tiêu đề đã tồn tại!"));
            }

            var alias = Utilities.GetCharUnsigned(model.Name);
            if (service.CheckExist(model.Id, alias))
            {
                return new AjaxResult()
                    .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                    .Alert(T("Tên không dấu đã tồn tại!"));
            }

            item.Alias = Utilities.GetAlias(model.Name);
            item.Name = model.Name;
            item.IsDisplay = model.IsDisplay;

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
            var service = WorkContext.Resolve<ITagService>();
            var item = service.GetById(id);
            item.IsDisplay = false;
            service.Update(item);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
