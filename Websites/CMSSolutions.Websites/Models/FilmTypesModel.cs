using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class FilmTypesModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", Required = true, ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int SiteId { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Tên thể loại phim", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public string Name { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Phim trang chủ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 3)]
        public bool IsFilm { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Show", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 3)]
        public bool IsShow { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Clip", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 3)]
        public bool IsClip { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Phim trang JJ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 3)]
        public bool IsJJFilm { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 3)]
        public int Status { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, MaxLength = 2000, LabelText = "Giới thiệu", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string Description { get; set; }
        
        public static implicit operator FilmTypesModel(FilmTypesInfo entity)
        {
            return new FilmTypesModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                IsFilm =  entity.IsFilm,
                IsShow = entity.IsShow,
                IsClip = entity.IsClip,
                IsJJFilm = entity.IsJJFilm
            };
        }
    }
}
