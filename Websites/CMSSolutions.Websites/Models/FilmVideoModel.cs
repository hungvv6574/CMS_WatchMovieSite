using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class FilmVideoModel
    {
        public FilmVideoModel()
        {
            IsActived = true;
        }

        [ControlHidden]
        public long Id { get; set; }
        
        [ControlHidden]
        public string ReturnUrl { get; set; }

        [ControlHidden]
        public long FilmId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.ServerId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Máy chủ", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int ServerId { get; set; }

        [ControlCascadingDropDown(LabelText = "Thư mục gốc", ParentControl = "ServerId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public string RootFolders { get; set; }

        [ControlCascadingDropDown(LabelText = "Thư mục ngày", ParentControl = "RootFolders", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public string FolderDay { get; set; }

        [ControlCascadingDropDown(LabelText = "Thư mục theo ngày", ParentControl = "FolderDay", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public string ChildrenFolders { get; set; }

        [ControlCascadingDropDown( AllowMultiple = true, LabelText = "Đường dẫn files", ParentControl = "ChildrenFolders", CssClass = Extensions.Constants.CssControlCustom, EnableChosen = true, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public long[] FileIds { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, LabelText = "Đường dẫn nguồn album", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 3)]
        public string UrlAlbum { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, LabelText = "Đường dẫn nguồn video", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 3)]
        public string UrlSource { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, Required = true, LabelText = "Đường dẫn files subtitle", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string Subtitle { get; set; }

        [ControlChoice(ControlChoice.DropDownList, AllowMultiple = true, CssClass = Extensions.Constants.CssControlCustom, EnableChosen = true, Required = true, LabelText = "Chọn tập phim", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 4)]
        public int[] EpisodeIds { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Trailer phim", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public bool IsTraller { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Sử dụng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public bool IsActived { get; set; }

        //[ControlFileUpload(EnableFineUploader = true, LabelText = "Ảnh đại diện", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4, ShowThumbnail = true)]
        //public string ImageIcon { get; set; }

        public static implicit operator FilmVideoModel(FilmVideoInfo entity)
        {
            return new FilmVideoModel
            {
                Id = entity.Id,
                FilmId = entity.FilmId,
                FileIds = new[] { entity.FileId },
                IsTraller = entity.IsTraller,
                IsActived = entity.IsActived,
                Subtitle = entity.Subtitle,
                UrlSource = entity.UrlSource,
                EpisodeIds = new[]{entity.EpisodeId},
                UrlAlbum = entity.UrlAlbum
            };
        }
    }
}
