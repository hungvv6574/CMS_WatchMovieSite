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
    public class AdminVipCardController : BaseAdminController
    {
        public AdminVipCardController(IWorkContextAccessor workContextAccessor) : 
                base(workContextAccessor)
        {
            TableName = "tblVipcards";
        }
        
        [Url("admin/vipcards")]
        public ActionResult Index()
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý thẻ VIP"), Url = "#" });

            var result = new ControlGridFormResult<VIPCardInfo>
            {
                Title = T("Quản lý thẻ VIP"),
                CssClass = "table table-bordered table-striped",
                IsAjaxSupported = true,
                FetchAjaxSource = GetVipCards,
                UpdateActionName = "Update",
                ActionsColumnWidth = 100,
                ClientId = TableName,
                GridWrapperStartHtml = Constants.Grid.GridWrapperStartHtml,
                GridWrapperEndHtml = Constants.Grid.GridWrapperEndHtml
            };

            result.AddCustomVar(Extensions.Constants.LanguageCode, "$('#" + Extensions.Constants.LanguageCode + "').val();", true);
            result.AddCustomVar(Extensions.Constants.SiteId, "$('#" + Extensions.Constants.SiteId + "').val();", true);
            result.AddCustomVar(Extensions.Constants.StatusId, "$('#" + Extensions.Constants.ServerId + "').val();", true);

            result.AddColumn(x => x.VIPCode, T("Mã thẻ")).AlignCenter().HasWidth(100);
            result.AddColumn(x => x.VIPName, T("Tên thẻ"));
            result.AddColumn(x => x.ServerName, T("Máy chủ"));
            result.AddColumn(x => x.VIPValue, T("Định mức"));

            result.AddAction().HasText(T("Thêm mới"))
                .HasUrl(Url.Action("Edit", RouteData.Values.Merge(new { id = 0 })))
                .HasButtonStyle(ButtonStyle.Primary)
                .HasBoxButton(false)
                .HasCssClass(Constants.RowLeft)
                .HasRow(true);

            result.AddAction(new ControlFormHtmlAction(() => BuildLanguages(true, Extensions.Constants.SiteId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(() => BuildSiteServers(true, Extensions.Constants.ServerId))).HasParentClass(Constants.ContainerCssClassCol3);
            result.AddAction(new ControlFormHtmlAction(BuildServers)).HasParentClass(Constants.ContainerCssClassCol3);

            result.AddRowAction()
                 .HasText(T("Sửa"))
                 .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
                 .HasButtonStyle(ButtonStyle.Default)
                 .HasButtonSize(ButtonSize.ExtraSmall);

            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");
            return result;
        }

        private ControlGridAjaxData<VIPCardInfo> GetVipCards(ControlGridFormRequest options)
        {
            var languageCode = WorkContext.CurrentCulture;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.LanguageCode]))
            {
                languageCode = Request.Form[Extensions.Constants.LanguageCode];
            }

            var siteId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.SiteId]))
            {
                siteId = Convert.ToInt32(Request.Form[Extensions.Constants.SiteId]);
            }

            var serverId = 0;
            if (Utilities.IsNotNull(Request.Form[Extensions.Constants.ServerId]))
            {
                serverId = Convert.ToInt32(Request.Form[Extensions.Constants.ServerId]);
            }

            int totals;
            var items = WorkContext.Resolve<IVIPCardService>().SearchPaged(string.Empty, languageCode, siteId, serverId, options.PageIndex, options.PageSize, out totals);
            var result = new ControlGridAjaxData<VIPCardInfo>(items, totals);

            return result;
        }

        [Url("admin/vipcards/edit/{id}")]
        public ActionResult Edit(int id)
        {
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Quản lý thẻ VIP"), Url = Url.Action("Index") });
            WorkContext.Breadcrumbs.Add(new Breadcrumb { Text = T("Thông tin thẻ VIP"), Url = "#" });

            var model = new VIPCardModel();
            if (id > 0)
            {
                var service = WorkContext.Resolve<IVIPCardService>();
                model = service.GetById(id);
            }

            var result = new ControlFormResult<VIPCardModel>(model)
            {
                Title = T("Thông tin thẻ VIP"),
                FormMethod = FormMethod.Post,
                UpdateActionName = "Update",
                SubmitButtonText = T("Lưu lại"),
                ShowCancelButton = true,
                ShowBoxHeader = false,
                CancelButtonText = T("Trở về"),
                FormWrapperStartHtml = Constants.Form.FormWrapperStartHtml,
                FormWrapperEndHtml = Constants.Form.FormWrapperEndHtml
            };

            result.RegisterExternalDataSource(x => x.LanguageCode, y => BindLanguages());
            result.RegisterCascadingDropDownDataSource(x => x.SiteId, Url.Action("GetSitesByLanguage"));
            result.RegisterCascadingDropDownDataSource(x => x.ServerId, Url.Action("GetServersBySite"));
          
            return result;
        }

        [HttpPost, ValidateInput(false), FormButton("Save")]
        [Url("admin/vipcards/update")]
        public ActionResult Update(VIPCardModel model)
        {
            if (!ModelState.IsValid)
            {
				return new AjaxResult().Alert(T(Constants.Messages.InvalidModel));
            }

            var service = WorkContext.Resolve<IVIPCardService>();
            VIPCardInfo item = model.Id == 0 ? new VIPCardInfo() : service.GetById(model.Id);

            if (service.CheckVipCode(item.Id, model.VIPCode))
            {
                return new AjaxResult()
                    .NotifyMessage("UPDATE_ENTITY_COMPLETE")
                    .Alert(T("Đã tồn tại mã thẻ VIP này!"));
            }

            item.VIPCode = model.VIPCode.ToUpper();
            item.VIPName = model.VIPName;
            item.VIPValue = model.VIPValue;
            item.LanguageCode = model.LanguageCode;
            item.SiteId = model.SiteId;
            item.ServerId = model.ServerId;

            service.Save(item);

            return new AjaxResult().NotifyMessage("UPDATE_ENTITY_COMPLETE")
                .Alert(T("Cập nhật thành công!"))
                .CloseModalDialog();
        }
    }
}
