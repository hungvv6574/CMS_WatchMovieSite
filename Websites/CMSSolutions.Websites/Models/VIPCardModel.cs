using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class VIPCardModel
    {
        public VIPCardModel()
        {
            VIPValue = 0;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.ServerId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Máy chủ", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public int ServerId { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Mã thẻ VIP", PlaceHolder = "Tối đa 50 ký tự", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 3)]
        public string VIPCode { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên thẻ VIP", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 4)]
        public string VIPName { get; set; }

        [ControlNumeric(LabelText = "Định mức giá trị thẻ VIP", Required = true, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 5)]
        public decimal VIPValue { get; set; }

        public static implicit operator VIPCardModel(VIPCardInfo entity)
        {
            return new VIPCardModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                ServerId = entity.ServerId,
                VIPCode =  entity.VIPCode,
                VIPName = entity.VIPName,
                VIPValue = entity.VIPValue
            };
        }
    }
}
