using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class CardTypeModel
    {
        [ControlHidden()]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Mã loại thẻ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string Code { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Tên loại thẻ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string Name { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Có serial", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public bool HasSerial { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 3)]
        public int Status { get; set; }
        
        public static implicit operator CardTypeModel(CardTypeInfo entity)
        {
            return new CardTypeModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                HasSerial = entity.HasSerial,
                Status = entity.Status
            };
        }
    }
}
