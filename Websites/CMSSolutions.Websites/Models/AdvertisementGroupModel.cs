using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Models
{
    public class AdvertisementGroupModel
    {
        public AdvertisementGroupModel()
        {
            Code = Guid.NewGuid().ToString().Replace("-", string.Empty);
            IsActived = true;
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.CategoryId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Chuyên mục hiển thị", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int CategoryId { get; set; }

        [ControlCascadingDropDown(AllowMultiple = true, EnableChosen = true, LabelText = "Quảng cáo", ParentControl = "SiteId", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 1)]
        public int[] AdvertisementIds { get; set; }

        [ControlText(Type = ControlText.TextBox, ReadOnly = true, LabelText = "Mã quảng cáo", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public string Code { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên nhóm", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol8, ContainerRowIndex = 2)]
        public string GroupName { get; set; }

        [ControlText(LabelText = "Ghi chú", Type = ControlText.MultiText, Rows = 2, MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string Description { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Đã tạo Xml", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public bool IsGenerate { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Đang sử dụng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public bool IsActived { get; set; }

        [ControlDatePicker(LabelText = "Ngày hết hạn", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public string FinishDate { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Giờ hết hạn", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public string FinishTime { get; set; }

        public static implicit operator AdvertisementGroupModel(AdvertisementGroupInfo entity)
        {
            return new AdvertisementGroupModel
            {
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                CategoryId = entity.CategoryId,
                Id = entity.Id,
                Code = entity.Code,
                GroupName = entity.GroupName,
                Description = entity.Description,
                AdvertisementIds = Utilities.ParseListInt(entity.AdvertisementIds),
                IsGenerate = entity.IsGenerate,
                IsActived = entity.IsActived,
                FinishDate = entity.FinishDate.ToString(Extensions.Constants.DateTimeFomat),
                FinishTime = entity.FinishTime
            };
        }
    }
}
