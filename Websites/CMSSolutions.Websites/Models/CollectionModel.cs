using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class CollectionModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int SiteId { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Tên bộ phim", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public string Name { get; set; }

        [ControlNumeric(LabelText = "Vị trí", Required = true, MaxLength = 3, ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 3)]
        public int OrderBy { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Bộ phim mới cập nhật", ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 3)]
        public bool IsHot { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true,LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 4)]
        public int Status { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, MaxLength = 2000, LabelText = "Giới thiệu", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 5)]
        public string Description { get; set; }
        
        public static implicit operator CollectionModel(CollectionInfo entity)
        {
            return new CollectionModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                Name = entity.Name,
                IsHot = entity.IsHot,
                Description = entity.Description,
                Status = entity.Status,
                OrderBy = entity.OrderBy
            };
        }
    }
}
