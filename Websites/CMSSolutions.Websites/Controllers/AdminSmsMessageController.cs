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
    public class AdminSmsMessageController : BaseAdminController
    {
        public AdminSmsMessageController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
            TableName = "tblSmsMessages";
        }

        [Url("admin/sms-messages")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý tin nhắn"), Url = "#" });

            var result = new ControlGridFormResult<SmsMessageInfo>
            {
                Title = T("Quản lý tin nhắn"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetSmsMessages,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);

            result.AddColumn(x => x.Code, T("Mã"));
            result.AddColumn(x => x.Message, T("Thông báo"));
            result.AddColumn(x => (x.IsEvent ? "Sự kiện" :"Không sự kiện"), T("Loại tin"));

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(false, string.Empty))).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
                .HasText(T("Sửa"))
                .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                .HasButtonStyle(ButtonStyle.Default)
                .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            return result;
        }

        private ControlGridAjaxData<SmsMessageInfo> GetSmsMessages(ControlGridFormRequest options)
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var items = WorkContext.Resolve<ISmsMessageService>().GetRecords(x => x.LanguageCode == languageCode);
            var result = new ControlGridAjaxData<SmsMessageInfo>(items);
            return result;
        }

        [Url("admin/sms-messages/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var model = new SmsMessageModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<ISmsMessageService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<SmsMessageModel>(model)
            {
                Title = T("Thông tin tin nhắn"),
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
    }
}
