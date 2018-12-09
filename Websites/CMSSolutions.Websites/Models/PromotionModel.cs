using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class PromotionModel
    {
        public PromotionModel()
        {
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.AddMonths(1).Date;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tiêu đề", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0)]
        public string Title { get; set; }

        [ControlText(LabelText = "Nội dung thông báo", Required = true, Type = ControlText.RichText, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Contents { get; set; }

        [ControlDatePicker(LabelText = "Ngày bắt đầu", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public DateTime FromDate { get; set; }

        [ControlDatePicker(LabelText = "Ngày kết thúc", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public DateTime ToDate { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public int Status { get; set; }

        public static implicit operator PromotionModel(PromotionInfo entity)
        {
            return new PromotionModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Contents = entity.Contents,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                Status = entity.Status
            };
        }
    }
}