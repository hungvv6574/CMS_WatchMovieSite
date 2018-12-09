using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class SliderModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Chọn ngôn ngữ", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.CategoryId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Chọn trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Chọn chuyên mục", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int CategoryId { get; set; }

        [ControlCascadingDropDown(EnableChosen = true, CssClass = Extensions.Constants.CssControlCustom, LabelText = "Chọn phim", ParentControl = "CategoryId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public long FilmId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trang hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int PageId { get; set; }

        [ControlNumeric(LabelText = "Thứ tự", Required = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int OrderBy { get; set; }

        [ControlFileUpload(EnableFineUploader = true, LabelText = "Ảnh nền", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 6, ShowThumbnail = true)]
        public string Images { get; set; }

        public static implicit operator SliderModel(SliderInfo entity)
        {
            return new SliderModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                CategoryId = entity.CategoryId,
                FilmId = entity.FilmId,
                PageId = entity.PageId,
                OrderBy = entity.OrderBy,
                Images = entity.Images
            };
        }
    }
}