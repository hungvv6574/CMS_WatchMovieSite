using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class EpisodeModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int SiteId { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 50, LabelText = "Tên tập phim", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public string EpisodeName { get; set; }

        [ControlNumeric(LabelText = "Thư tự", Required = true, MaxLength = 15, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 3)]
        public int OrderBy { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 4)]
        public int Status { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, MaxLength = 2000, LabelText = "Giới thiệu", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 5)]
        public string Description { get; set; }

        public static implicit operator EpisodeModel(EpisodeInfo entity)
        {
            return new EpisodeModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                EpisodeName = entity.EpisodeName,
                OrderBy = entity.OrderBy,
                Description = entity.Description,
                Status = entity.Status
            };
        }

    }
}
