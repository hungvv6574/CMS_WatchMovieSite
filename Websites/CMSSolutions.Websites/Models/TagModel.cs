using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class TagModel
    {
        public TagModel()
        {
            IsDisplay = true;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tiêu đề", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public string Name { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, LabelText = "Tên không dấu", PlaceHolder = "Hệ thống tự động sinh.", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public string Alias { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Được hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 3)]
        public bool IsDisplay { get; set; }

        public static implicit operator TagModel(TagInfo other)
        {
            if (other == null)
            {
                return null;
            }

            return new TagModel
            {
                Id = other.Id,
                Name = other.Name,
                Alias = other.Alias,
                IsDisplay = other.IsDisplay
            };
        }
    }
}