using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class SiteModel
    {
        public SiteModel()
        {
            IsActived = true;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string LanguageCode { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Sử dụng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public bool IsActived { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên trang web", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Name { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn", PlaceHolder = "Tối đa 250 ký tự", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string Url { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên miền", PlaceHolder = "Tối đa 50 ký tự", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string Domain { get; set; }

        [ControlText(LabelText = "Mô tả", PlaceHolder = "Tối đa 2000 ký tự", Type = ControlText.MultiText, Rows = 2, MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string Description { get; set; }

        public static implicit operator SiteModel(SiteInfo entity)
        {
            return new SiteModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                Name = entity.Name,
                Url = entity.Url,
                Domain = entity.Domain,
                IsActived = entity.IsActived,
                Description = entity.Description
            };
        }
    }
}
