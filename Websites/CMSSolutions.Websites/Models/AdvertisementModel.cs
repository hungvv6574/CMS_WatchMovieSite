using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class AdvertisementModel
    {
        public AdvertisementModel()
        {
            KeyCode = Guid.NewGuid().ToString().Replace("-", string.Empty);
            Code = "pre-blueseed";
            Type = "tags";
            Position = 0;
            Price = 0;
            Duration = 15;
            Skip = 5;
            IsBlock = false;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.CategoryId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlText(Type = ControlText.TextBox, ReadOnly = true, LabelText = "Mã quảng cáo", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public string KeyCode { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tiêu đề", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public string Title { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn VAST", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string Link { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Key", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string Code { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Loại", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string Type { get; set; }

        [ControlNumeric(LabelText = "Thời gian", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public int Duration { get; set; }

        [ControlNumeric(LabelText = "Vị trí bắt đầu", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public int Position { get; set; }

        [ControlNumeric(LabelText = "Thời gian skip", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public int Skip { get; set; }

        [ControlNumeric(LabelText = "Chi phí", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public float Price { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Tạm khóa", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 5)]
        public bool IsBlock { get; set; }

        public static implicit operator AdvertisementModel(AdvertisementInfo entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new AdvertisementModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                Title = entity.Title,
                KeyCode = entity.KeyCode,
                Price = (float)entity.Price,
                Code = entity.Code,
                Link = entity.Link,
                Type = entity.Type,
                Duration = entity.Duration,
                Position = entity.Position,
                Skip = entity.Skip,
                IsBlock = entity.IsBlock
            };
        }
    }
}