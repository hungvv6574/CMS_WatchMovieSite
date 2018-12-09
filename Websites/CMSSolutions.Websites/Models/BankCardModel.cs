using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class BankCardModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Mã ngân hàng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string BankCode { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Tên ngân hàng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string BankName { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public int Status { get; set; }
        
        public static implicit operator BankCardModel(BankCardInfo entity)
        {
            return new BankCardModel
            {
                Id = entity.Id,
                BankCode = entity.BankCode,
                BankName = entity.BankName,
                Status = entity.Status
            };
        }
    }
}
