using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            IsActived = true;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.ParentId+ "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Chuyên mục gốc", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int ParentId { get; set; }

        [ControlNumeric(LabelText = "Vị trí", Required = true, MaxLength = 3, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int OrderBy { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Làm trang chủ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsHome { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Được hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsActived { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Có chuyên mục con", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool HasChilden { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Không sử dụng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsDeleted { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên rút gọn", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public string ShortName { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên đầy đủ", PlaceHolder = "Tối đa 400 ký tự", Required = true, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol8, ContainerRowIndex = 2)]
        public string Name { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn", PlaceHolder = "Tối đa 250 ký tự", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string Url { get; set; }

        [ControlText(LabelText = "Mô tả SEO", PlaceHolder = "Tối đa 2000 ký tự", Type = ControlText.MultiText, Required = true, Rows = 2, MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 5)]
        public string Description { get; set; }

        [ControlText(LabelText = "Tags SEO", PlaceHolder = "Tối đa 2000 ký tự", Type = ControlText.MultiText, Required = true, Rows = 2, MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 6)]
        public string Tags { get; set; }

        public static implicit operator CategoryModel(CategoryInfo other)
        {
            if (other == null)
            {
                return null;
            }

            return new CategoryModel
            {
                Id = other.Id,
                SiteId = other.SiteId,
                ParentId = other.ParentId,
                ShortName = other.ShortName,
                Name = other.Name,
                LanguageCode = other.LanguageCode,
                IsActived = other.IsActived,
                OrderBy = other.OrderBy,
                Description = other.Description,
                Tags = other.Tags,
                Url = other.Url,
                IsHome = other.IsHome,
                HasChilden = other.HasChilden,
                IsDeleted = other.IsDeleted
            };
        }
    }
}