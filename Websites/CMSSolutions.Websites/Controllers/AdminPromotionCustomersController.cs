using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
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
    public class AdminPromotionCustomersController : BaseAdminController
    {
        public AdminPromotionCustomersController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblPromotionCustomers";
        }

        [Url("admin/promotion-customer/apply/{promotionId}")]
        public ActionResult Index(int promotionId)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý đợt khuyến mãi"), Url = Url.Action("Index","AdminPromotion") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Áp dụng chương trình khuyến mãi"), Url = "#" });
           
            var promotionCustomers = WorkContext.Resolve<IPromotionCustomersService>().GetCode(promotionId, 0);
            var model = new PromotionCustomersModel();
            var promotion = WorkContext.Resolve<IPromotionService>().GetById(promotionId);
            if (promotion != null)
            {
                model.PromotionId = promotion.Id;
                model.PromotionName = promotion.Title;
                if (promotionCustomers != null)
                {
                    model.Code = promotionCustomers.Code;
                    model.Value = Convert.ToInt32(promotionCustomers.Value);
                }
            }

            var result = new ControlFormResult<PromotionCustomersModel>(model)
            {
                Title = T("Áp dụng chương trình khuyến mãi"),
                UpdateActionName = "Update",
                FormMethod = FormMethod.Post,
                SubmitButtonText = T("Lưu lại"),
                ShowBoxHeader = false,
                CancelButtonText = T("Trở về"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.MakeReadOnlyProperty(x => x.Code);
            result.MakeReadOnlyProperty(x => x.PromotionName);
            if (promotionCustomers != null)
            {
                result.MakeReadOnlyProperty(x => x.Value);
            }

            result.RegisterExternalDataSource(x => x.CustomerIds, y => BindCustomers(promotionId));
            result.AddAction().HasText(T("Gửi thông báo")).HasButtonStyle(ButtonStyle.Info).HasUrl("javascript: void(0);")
                .OnClientClick("$.ajax({ url: '" + Url.Action("Send", "AdminPromotionCustomers") + "', data: {\"promotionId\": " + promotionId + "}, type: 'POST', dataType: 'json', success: function (result) { alert(result.Data);}});");

            var result2 = new ControlGridFormResult<PromotionCustomerInfo>
            {
                Title = T("Khách hàng được khuyến mãi"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetPromotionCustomers,
                DefaultPageSize = WorkContext.DefaultPageSize,
                EnablePaginate = true,
                UpdateActionName = "Update",
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml,
                ClientId = TableName,
                ActionsColumnWidth = 100
            };

            result2.AddColumn(x => x.CustomerCode, T("Mã KH"));
            result2.AddColumn(x => x.FullName, T("Họ và Tên"));
            result2.AddColumn(x => x.Code, T("Mã KM"));
            result2.AddColumn(x => x.Value, T("Giá trị(VIPXU)"));
            result2.AddColumn(x => x.Description, T("Thông báo"));
            result2.AddColumn(x => x.IsUsed)
                .HasHeaderText(T("Sử dụng"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();
            result2.AddColumn(x => x.IsSendMessages)
                .HasHeaderText(T("Gửi TB"))
                .AlignCenter()
                .HasWidth(100)
                .RenderAsStatusImage();

            result2.AddRowAction(true)
                .HasText(T("Xóa"))
                .HasName("Delete")
                .HasValue(x => x.Id)
                .HasButtonStyle(ButtonStyle.Danger)
                .HasButtonSize(ButtonSize.ExtraSmall)
                .HasConfirmMessage(T(Constants.Messages.ConfirmDeleteRecord).Text);

            result2.AddCustomVar(Extensions.Constants.Id, promotionId);
            result2.AddReloadEvent("DELETE_ENTITY_COMPLETE");

            return new ControlFormsResult(result, result2);
        }

        [HttpPost, ValidateInput(false)]
        [Url("admin/promotion-customer/send")]
        public ActionResult Send()
        {
            var redirect = new DataViewerModel();
            var promotionId = Request.Form["promotionId"];
            if (string.IsNullOrEmpty(promotionId))
            {
                redirect.Status = true;
                redirect.Data = "Không tìm thấy ID đợt khuyến mãi.";
                return Json(redirect);
            }
            var id = int.Parse(promotionId);
            var promotion = WorkContext.Resolve<IPromotionService>().GetById(id);
            var customerHistoriesService = WorkContext.Resolve<ICustomerHistoriesService>();
            var service = WorkContext.Resolve<IPromotionCustomersService>();
            var listPromotionCustomers = service.GetByPromotion(id);
            foreach (var item in listPromotionCustomers)
            {
                if (item.IsSendMessages)
                {
                    continue;
                }

                var historyCustomer = new CustomerHistoriesInfo
                {
                    Type = (int)CustomerLogType.NapVipLogs,
                    CustomerId = item.CustomerId,
                    CreateDate = DateTime.Now,
                    Action = item.Title,
                    Description = string.Format("Bạn nhận được mã khuyến mãi {0} từ website VIPHD.VN. Vui lòng đăng nhập để kích hoạt khuyến mãi. Thời gian từ ngày {1} đến hết ngày {2}. Xin cảm ơn.",
                            item.Code,
                            promotion.FromDate.ToString(Extensions.Constants.DateTimeFomat),
                            promotion.ToDate.ToString(Extensions.Constants.DateTimeFomat)),
                    Status = (int)Status.Approved,
                    TransactionCode = item.Code
                };
                customerHistoriesService.Insert(historyCustomer);
                item.IsSendMessages = true;
                item.SendDate = DateTime.Now;
                service.Update(item);
            }
            redirect.Status = true;
            redirect.Data = "Đã gửi thông báo tới tất cả khách hàng có trong đợt khuyến mãi này.";

            return Json(redirect);
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/promotion-customer/update")]
        public ActionResult Update(PromotionCustomersModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IPromotionCustomersService>();
            var customerService = WorkContext.Resolve<ICustomerService>();
            var promotion = WorkContext.Resolve<IPromotionService>().GetById(model.PromotionId);

            var listCustomerIds = model.CustomerIds;
            if (listCustomerIds.Contains(0))
            {
                var items = customerService.GetPromotions(model.PromotionId);
                listCustomerIds = items.Select(x => x.Id).ToArray();
            }

            for (int i = 0; i < listCustomerIds.Length; i++)
            {
                var customerId = listCustomerIds[i];
                var customer = customerService.GetById(customerId);
                var promotionCustomers = service.GetCode(promotion.Id, customer.Id);
                if (promotionCustomers != null && customerId != 0)
                {
                    continue;
                }

                var entity = new PromotionCustomerInfo
                {
                    PromotionId = model.PromotionId,
                    CustomerId = customer.Id,
                    CustomerCode = customer.CustomerCode,
                    CreateDate = DateTime.Now,
                    Code = model.Code,
                    Value = model.Value.ToString(),
                    Description = string.Format("Bạn nhận được mã khuyến mãi {0} từ website VIPHD.VN. Vui lòng đăng nhập để kích hoạt khuyến mãi. Thời gian từ ngày {1} đến hết ngày {2}. Xin cảm ơn.",
                        model.Code, promotion.FromDate.ToString(Extensions.Constants.DateTimeFomat), promotion.ToDate.ToString(Extensions.Constants.DateTimeFomat)),
                    IsUsed = false,
                    UsedDate = Utilities.DateNull(),
                    IsSendMessages = false,
                    SendDate = Utilities.DateNull()
                };

                service.Insert(entity);
            }

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"))
                .Redirect(Url.Action("Index", new { @promotionId = model.PromotionId }));
        }

        [FormButton("Delete")]
        [HttpPost, ActionName("Update")]
        public ActionResult Delete(int id)
        {
            var service = WorkContext.Resolve<IPromotionCustomersService>();
            var item = service.GetById(id);
            if (item.IsUsed)
            {
                return new AjaxResult().Alert("Dữ liệu này đã sử dụng bạn không thể xóa.");
            }
            service.Delete(item);
            return new AjaxResult()
                .NotifyMessage("DELETE_ENTITY_COMPLETE")
                .Redirect(Url.Action("Index", new { @promotionId = item.PromotionId }));
        }

        private ControlGridAjaxData<PromotionCustomerInfo> GetPromotionCustomers(ControlGridFormRequest options)
        {
            var promotionId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.Id]))
            {
                promotionId = Convert.ToInt32(Request.Form[Extensions.Constants.Id]);
            }

            var items = WorkContext.Resolve<IPromotionCustomersService>().GetByPromotion(promotionId);
            var result = new ControlGridAjaxData<PromotionCustomerInfo>(items);

            return result;
        }

        private IEnumerable<SelectListItem> BindCustomers(int promotionId)
        {
            var service = WorkContext.Resolve<ICustomerService>();
            var items = service.GetPromotions(promotionId);
            var result = new List<SelectListItem>();
            result.AddRange(items.Select(item => new SelectListItem
            {
                Text = item.FullName,
                Value = item.Id.ToString()
            }));
            result.Insert(0, new SelectListItem {Text = "--- Áp dụng cho tất cả khách hàng ---", Value = "0"});

            return result;
        }
    }
}
