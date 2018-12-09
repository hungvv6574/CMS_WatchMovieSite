using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class SmsMessageModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string LanguageCode { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Mã", PlaceHolder = "Tối đa 50 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Code { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Thông báo", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Messages { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Loại tin", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public bool IsEvent { get; set; }

        public static implicit operator SmsMessageModel(SmsMessageInfo entity)
        {
            return new SmsMessageModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                Code = entity.Code,
                Messages = entity.Message,
                IsEvent = entity.IsEvent
            };
        }
    }
}