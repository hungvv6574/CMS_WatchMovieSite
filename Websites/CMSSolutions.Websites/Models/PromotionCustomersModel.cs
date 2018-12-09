using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Models
{
    public class PromotionCustomersModel
    {
        public PromotionCustomersModel()
        {
            Code = Utilities.GenerateUniqueNumber();
            Value = 50;
        }

        [ControlHidden]
        public int PromotionId { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 50, LabelText = "Chương trình khuyến mãi", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0)]
        public string PromotionName { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 50, LabelText = "Mã khuyến mãi", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1)]
        public string Code { get; set; }

        [ControlNumeric(LabelText = "Giá trị khuyến mãi (VIPXU)", MinimumValue = "50", Required = true, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1)]
        public int Value { get; set; }

        [ControlChoice(ControlChoice.DropDownList, AllowMultiple = true, CssClass = Extensions.Constants.CssControlCustom, EnableChosen = true, Required = true, LabelText = "Chọn khách hàng", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public int[] CustomerIds { get; set; }
    }
}