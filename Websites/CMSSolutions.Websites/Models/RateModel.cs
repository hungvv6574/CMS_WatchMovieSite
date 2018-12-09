using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class RateModel
    {
        [ControlHidden]
        public long Id { get; set; }

        [ControlText(Type = ControlText.TextBox, MaxLength = 250, LabelText = "Tên phim", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0)]
        public string FilmName { get; set; }

        [ControlNumeric(LabelText = "Đánh giá", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int Rate { get; set; }

        [ControlText(Type = ControlText.TextBox, MaxLength = 50, LabelText = "Mã khách hàng", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public string CustomerCode { get; set; }

        [ControlText(Type = ControlText.TextBox, MaxLength = 250, LabelText = "Họ và Tên", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public string CustomerName { get; set; }

        [ControlText(Type = ControlText.TextBox, MaxLength = 250, LabelText = "Thông báo giật lag", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 2)]
        public string AlertLag { get; set; }

        [ControlText(Type = ControlText.TextBox, MaxLength = 250, LabelText = "Thông báo lỗi", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string AlertError { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, MaxLength = 2000, LabelText = "Nội dung thông báo lỗi", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string Messages { get; set; }
        
        public static implicit operator RateModel(RateInfo entity)
        {
            return new RateModel
            {
                Id = entity.Id,
                FilmName = entity.FilmName,
                Rate = entity.Rate,
                CustomerCode = entity.CustomerCode,
                CustomerName = entity.CustomerName,
                AlertError = entity.AlertError,
                AlertLag = entity.AlertLag,
                Messages = entity.Messages
            };
        }

    }
}
