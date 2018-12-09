using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class DirectorModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Họ và Tên", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0)]
        public string FullName { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public int Status { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 3, MaxLength = 2000, LabelText = "Giới thiệu", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string Description { get; set; }

        public static implicit operator DirectorModel(DirectorInfo entity)
        {
            return new DirectorModel
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Description = entity.Description,
                Status = entity.Status
            };
        }
    }
}
