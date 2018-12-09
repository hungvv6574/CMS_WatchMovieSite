using System;
using System.Web.Mvc;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Web.UI.Navigation;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = true), Authorize]
    public class AdminClosedBankController : BaseAdminController
    {
        public AdminClosedBankController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {

        }

        [Url("admin/bank/close")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Báo cáo ATM"), Url = Url.Action("Index","AdminReportBank") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Chốt sổ đối soát ATM"), Url = "#" });

            var model = new BankCloseModel();
            var result = new ControlFormResult<BankCloseModel>(model)
            {
                Title = T("Chốt sổ đối soát ATM"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Chốt"),
                CancelButtonText = T("Trở về"),
                CancelButtonUrl = Url.Action("Index", "AdminReportBank"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.MakeReadOnlyProperty(x => x.Messages);

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/bank/close/update")]
        public ActionResult Update(BankCloseModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            if (model.EndDate > DateTime.Now)
            {
                return new AjaxResult().Alert(T("Thời gian bạn chọn chưa phát sinh giao dịch mới!"));
            }

            WorkContext.Resolve<ITransactionBankService>().ATMClosed(model.EndDate);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(string.Format(T("Đã chốt sổ đối soát đến hết ngày {0} thành công!"),
                model.EndDate.ToString(Extensions.Constants.DateTimeFomat)));
        }
    }
}
