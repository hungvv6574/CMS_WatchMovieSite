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
    public class AdminPromotionController : BaseAdminController
    {
        public AdminPromotionController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblPromotions";
        }

        [Url("admin/promotions")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý đợt khuyến mãi"), Url = "#" });

            var result = new ControlGridFormResult<PromotionInfo>
            {
                Title = T("Quản lý đợt khuyến mãi"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetPromotions,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 250
            };

            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.StatusId + "').val();", true);

            result.AddColumn(x => x.Id, T("ID")).HasWidth(60).AlignCenter();
            result.AddColumn(x => x.Title, T("Thông tin khuyến mãi"));
            result.AddColumn(x => x.TextFromDate, T("Ngày bắt đầu"));
            result.AddColumn(x => x.TextToDate, T("Ngày kết thúc"));
            result.AddColumn(x => EnumExtensions.GetDisplayName((PromotionStatus)x.Status), T("Trạng thái"));

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(BuildPromotionStatus)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction().HasText(T("Áp dụng KM"))
               .HasUrl(x => Url.Action("Index","AdminPromotionCustomers", RouteData.Values.Merge(new { @promotionId = x.Id })))
               .HasButtonStyle(ButtonStyle.Info)
               .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddRowAction()
                .HasText(T("Sửa"))
                .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                .HasButtonStyle(ButtonStyle.Default)
                .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddRowAction(true)
                .HasText(T("Đã kết thúc"))
                .HasName("Delete")
                .HasValue(x => x.Id.ToString(CultureInfo.InvariantCulture.ToString()))
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }

        private string BuildPromotionStatus()
        {
            var list = EnumExtensions.GetListItems<PromotionStatus>();
            var sb = new StringBuilder();
            sb.AppendFormat(T("Trạng thái") + " <select id=\"" + Extensions.Constants.StatusId + "\" name=\"" + Extensions.Constants.StatusId + "\" autocomplete=\"off\" class=\"uniform form-control col-md-3\" onchange=\"$('#" + TableName + "').jqGrid().trigger('reloadGrid');\">");
            foreach (var status in list)
            {
                sb.AppendFormat("<option value=\"{1}\">{0}</option>", status.Text, status.Value);
            }

            sb.Append("</select>");
            return sb.ToString();
        }

        private ControlGridAjaxData<PromotionInfo> GetPromotions(ControlGridFormRequest options)
        {
            var statusId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.StatusId]))
            {
                statusId = Convert.ToInt32(Request.Form[Extensions.Constants.StatusId]);
            }
            int totals;
            var items = WorkContext.Resolve<IPromotionService>().SearchPaged(statusId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<PromotionInfo>(items, totals);

            return result;
        }

        [Url("admin/promotions/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý đợt khuyến mãi"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin đợt khuyến mãi"), Url = "#" });

            var model = new PromotionModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IPromotionService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<PromotionModel>(model)
            {
                Title = T("Thông tin đợt khuyến mãi"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                CancelButtonText = T("Trở về"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterExternalDataSource(x => x.Status, y => EnumExtensions.GetListItems<PromotionStatus>());
            if (id > 0)
            {
                result.AddAction().HasText(T("Áp dụng KM"))
                .HasUrl(Url.Action("Index","AdminPromotionCustomers", new { @promotionId = id }))
                .HasButtonStyle(ButtonStyle.Info);
            }

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/promotions/update")]
        public ActionResult Update(PromotionModel model)
        {
            PromotionInfo item;
            var service = WorkContext.Resolve<IPromotionService>();
            if (model.Id == 0)
            {
                item = new PromotionInfo();
            }
            else
            {
                item = service.GetById(model.Id);
            }

            item.Title = model.Title;
            item.Contents = model.Contents;
            item.FromDate = model.FromDate;
            item.ToDate = model.ToDate;
            item.Status = model.Status;
            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE").Alert(T("Cập nhật thành công!"));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IPromotionService>();
            var model = service.GetById(id);
            model.Status = (int)PromotionStatus.Finish;
            service.Update(model);

            return new AjaxResult().NotifyMessage("DELETE_ENTITY_COMPLETE");
        }
    }
}
