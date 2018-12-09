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
    public class AdminClosedCardController : BaseAdminController
    {
        public AdminClosedCardController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {

        }

        [Url("admin/card/close")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Báo cáo thẻ cào"), Url = Url.Action("Index", "AdminReportCard") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Chốt sổ đối soát thẻ cào"), Url = "#" });

            var model = new CardCloseModel();
            var result = new ControlFormResult<CardCloseModel>(model)
            {
                Title = T("Chốt sổ đối soát thẻ cào"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Chốt"),
                CancelButtonText = T("Trở về"),
                CancelButtonUrl = Url.Action("Index", "AdminReportCard"),
                ShowBoxHeader = false,
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };
            result.MakeReadOnlyProperty(x => x.Messages);

            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/card/close/update")]
        public ActionResult Update(CardCloseModel model)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            if (model.EndDate > DateTime.Now)
            {
                return new AjaxResult().Alert(T("Thời gian bạn chọn chưa phát sinh giao dịch mới!"));
            }

            WorkContext.Resolve<ITransactionCardService>().CardClosed(model.EndDate);

            return new AjaxResult()
                .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(string.Format(T("Đã chốt sổ đối soát đến hết ngày {0} thành công!"),
                model.EndDate.ToString(Extensions.Constants.DateTimeFomat)));
        }
    }
}
