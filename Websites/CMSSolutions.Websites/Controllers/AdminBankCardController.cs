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
    public class AdminBankCardController : BaseAdminController
    {
        public AdminBankCardController(IWorkContextAccessor workContextAccessor) 
             : base(workContextAccessor)
        {
            TableName = "tblBankCards";
        }

        [Url("admin/bank-cards")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý dịch vụ thẻ ngân hàng"), Url = "#" });

            var result = new ControlGridFormResult<BankCardInfo>
            {
                Title = T("Quản lý dịch vụ thẻ ngân hàng"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetBankCards,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).HasWidth(60).AlignCenter();
            result.AddColumn(x => x.BankCode, T("Mã ngân hàng")).AlignCenter().HasWidth(150);
            result.AddColumn(x => x.BankName, T("Tên ngân hàng"));
            result.AddColumn(x => EnumExtensions.GetDisplayName((Status)x.Status), T("Trạng thái"));

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasRow(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true)
                .ShowModalDialog();

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

        private ControlGridAjaxData<BankCardInfo> GetBankCards(ControlGridFormRequest options)
        {
            var status = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                status = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }

            int totals;
            var items = WorkContext.Resolve<IBankCardService>().GetPaged(status, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<BankCardInfo>(items, totals);

            return result;
        }

        [Themed(false)]
        [Url("admin/bank-cards/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý dịch vụ thẻ ngân hàng"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin dịch vụ thẻ ngân hàng"), Url = "#" });

            var model = new BankCardModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IBankCardService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<BankCardModel>(model)
            {
                Title = T("Thông tin dịch vụ thẻ ngân hàng"),
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
        [Url("admin/bank-cards/update")]
        public ActionResult Update(BankCardModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IBankCardService>();
            BankCardInfo item = model.Id == 0 ? new BankCardInfo() : service.GetById(model.Id);

            item.BankCode = model.BankCode;
            item.BankName = model.BankName;
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
            var service = WorkContext.Resolve<IBankCardService>();
            var item = service.GetById(id);
            item.Status = (int)Status.Deleted;
            service.Update(item);

            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Alert(T("Dữ liệu chuyển trạng thái xóa tạm thời!"));
        }
    }
}
