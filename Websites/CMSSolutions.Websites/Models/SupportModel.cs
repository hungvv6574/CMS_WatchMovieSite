using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class SupportModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.ParentId+ "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Chọn nhóm", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int ParentId { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Tạo nhóm", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public bool IsGroup { get; set; }

        [ControlNumeric(LabelText = "Sắp xếp theo nhóm", Required = true, MaxLength = 15, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int OrderBy { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int Status { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Câu hỏi", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string Title { get; set; }

        [ControlText(LabelText = "Trả lời câu hỏi", Type = ControlText.RichText, MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string Messages { get; set; }

        public static implicit operator SupportModel(SupportInfo entity)
        {
            return new SupportModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                Title = entity.Title,
                OrderBy = entity.OrderBy,
                Messages = entity.Messages,
                Status = entity.Status,
                ParentId = entity.ParentId
            };
        }
    }
}