using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class LogModel
    {
        [ControlHidden]
        public long Id { get; set; }

        [ControlText(LabelText = "Ngày tạo", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public string TextCreateDate { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Loại", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int Type { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int Status { get; set; }

        [ControlText(LabelText = "Thực hiện bởi", Type = ControlText.TextBox, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Keyword { get; set; }

        [ControlText(LabelText = "Thông báo", Type = ControlText.MultiText, Rows = 3, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string Messages { get; set; }

        [ControlText(LabelText = "Nội dung thực hiện", Type = ControlText.MultiText, Rows = 2, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string Contents { get; set; }

        public static implicit operator LogModel(LogInfo entity)
        {
            return new LogModel
            {
                Id = entity.Id,
                TextCreateDate = entity.TextCreateDate,
                Type = entity.Type,
                Messages = entity.Messages,
                Contents = entity.Contents,
                Keyword = entity.Keyword,
                Status = entity.Status
            };
        }
    }
}