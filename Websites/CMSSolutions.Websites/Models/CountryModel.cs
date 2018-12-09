using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class CountryModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 50, LabelText = "Mã nước", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0)]
        public string Code { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 50, LabelText = "Tên nước", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Name { get; set; }
        
        public static implicit operator CountryModel(CountryInfo entity)
        {
            return new CountryModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name
            };
        }
    }
}
