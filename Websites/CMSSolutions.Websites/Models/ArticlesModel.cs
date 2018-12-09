using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class ArticlesModel
    {
        public ArticlesModel()
        {
            CategoryId = 59;
            IsPublished = true;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.CategoryId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Chuyên mục hiển thị", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int CategoryId { get; set; }

        [ControlText(LabelText = "Bài viết liên quan", Type = ControlText.TextBox, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int ParentId { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Đăng tin", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsPublished { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Hiện trên trang chủ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsHome { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Không sử dụng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsDeleted { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Tin có videos", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public bool IsVideo { get; set; }

        [ControlText(Type = ControlText.TextBox, PlaceHolder = "Vui lòng nhập tiêu đề bài viết. Tối đa 200 ký tự.", LabelText = "Tiêu đề", Required = true, MaxLength = 200, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Title { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tiêu đề hỗ trợ SEO", PlaceHolder = "Mặc định để trống, tự sinh.", MaxLength = 200, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string Alias { get; set; } 

        [ControlFileUpload(EnableFineUploader = true, LabelText = "Ảnh đại diện", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 3, Required = true, ShowThumbnail = true)]
        public string Icon { get; set; }

        [ControlText(Type = ControlText.MultiText, PlaceHolder = "Nhập nhiều đường dẫn videos ở đây chú ý tách nhau bởi dấu ';'", LabelText = "Đường dẫn vieos", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string VideosUrl { get; set; }

        [ControlText(LabelText = "Tóm tắt nội dung", PlaceHolder = "Vui lòng tóm tắt nội dung bài viết. Tối đa 400 ký tự.", Type = ControlText.MultiText, Rows = 2, Required = true, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 5)]
        public string Summary { get; set; }

        [ControlText(LabelText = "Nội dung bài viết", Required = true, Type = ControlText.RichText, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 6)]
        public string Contents { get; set; }

        [ControlText(LabelText = "Từ khóa SEO", Required = true, PlaceHolder = "Vui lòng nhập từ khóa SEO cho bài viết. Tối đa 400 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 7)]
        public string Description { get; set; }

        [ControlText(LabelText = "Tags SEO", Required = true, PlaceHolder = "Nhập từ khóa SEO cách nhau bởi dấu, Tối đa 500 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 8)]
        public string Tags { get; set; }

        public static implicit operator ArticlesModel(ArticlesInfo other)
        {
            if (other == null)
            {
                return null;
            }

            return new ArticlesModel
            {
                Id = other.Id,
                LanguageCode = other.LanguageCode,
                Alias = other.Alias,
                ParentId = other.ParentId,
                CategoryId = other.CategoryId,
                Title = other.Title,
                Icon =  other.Icon,
                IsPublished = other.IsPublished,
                IsHome = other.IsHome,
                IsVideo = other.IsVideo,
                Summary = other.Summary,
                Contents = other.Contents,
                Description = other.Description,
                IsDeleted = other.IsDeleted,
                VideosUrl = other.VideosUrl,
                Tags = other.Tags,
                SiteId = other.SiteId
            };
        }
    }
}