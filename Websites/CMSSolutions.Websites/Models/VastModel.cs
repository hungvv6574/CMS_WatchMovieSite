using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class VastModel
    {
        public VastModel()
        {
            KeyCode = Guid.NewGuid().ToString().Replace("-", string.Empty);
            AdSystemVersion = "3.0";
            Skipoffset = "00:00:05";
            Duration = "00:00:30.03";
            MediaFileBitrate = 660;
            MediaFileDelivery = "progressive";
            MediaFileHeight = 480;
            MediaFileWidth = 854;
            MediaFileMaintainAspectRatio = true;
            MediaFileScalable = true;
            MediaFileType = "video/mp4";
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.AdId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Quảng cáo", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public int AdId { get; set; }

        [ControlText(Type = ControlText.TextBox, ReadOnly = true, LabelText = "Mã Vast", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string KeyCode { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Version", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string AdSystemVersion { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Thời gian bỏ qua", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string Skipoffset { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Thời gian chạy", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string Duration { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tiêu đề", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string AdSystemValue { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Mô tả", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string AdTitle { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn lỗi", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string LinkError { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn impression", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 4)]
        public string LinkImpression { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn đơn vị quảng cáo", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 5)]
        public string LinkClickThrough { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn tracking 1", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 6)]
        public string TrackingValue1 { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn tracking 2", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 7)]
        public string TrackingValue2 { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn tracking 3", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 8)]
        public string TrackingValue3 { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn tracking 4", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 9)]
        public string TrackingValue4 { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn tracking 5", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 10)]
        public string TrackingValue5 { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn tracking 6", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 11)]
        public string TrackingValue6 { get; set; }

        [ControlNumeric(LabelText = "Bitrate", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 12)]
        public int MediaFileBitrate { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Delivery", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 12)]
        public string MediaFileDelivery { get; set; }

        [ControlNumeric(LabelText = "Chiều cao", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 12)]
        public int MediaFileHeight { get; set; }

        [ControlNumeric(LabelText = "Chiều rộng", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 12)]
        public int MediaFileWidth { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Duy trì tỷ lệ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 13)]
        public bool MediaFileMaintainAspectRatio { get; set; }
        
        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Scalable", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 13)]
        public bool MediaFileScalable { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn video quảng cáo", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol10, ContainerRowIndex = 14)]
        public string MediaFileValue { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Loại file", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 14)]
        public string MediaFileType { get; set; }

        public static implicit operator VastModel(VastInfo entity)
        {
            return new VastModel
            {
                Id = entity.Id,
                LanguageCode =  entity.LanguageCode,
                SiteId = entity.SiteId,
                KeyCode = entity.KeyCode,
                AdId = entity.AdId,
                AdSystemVersion = entity.AdSystemVersion,
                AdSystemValue = entity.AdSystemValue,
                AdTitle = entity.AdTitle,
                LinkError = entity.LinkError,
                LinkImpression = entity.LinkImpression,
                Skipoffset = entity.Skipoffset,
                Duration = entity.Duration,
                LinkClickThrough = entity.LinkClickThrough,
                TrackingValue1 = entity.TrackingValue1,
                TrackingValue2 = entity.TrackingValue2,
                TrackingValue3 = entity.TrackingValue3,
                TrackingValue4 = entity.TrackingValue4,
                TrackingValue5 = entity.TrackingValue5,
                TrackingValue6 = entity.TrackingValue6,
                MediaFileBitrate = entity.MediaFileBitrate,
                MediaFileDelivery = entity.MediaFileDelivery,
                MediaFileHeight = entity.MediaFileHeight,
                MediaFileWidth = entity.MediaFileWidth,
                MediaFileMaintainAspectRatio = entity.MediaFileMaintainAspectRatio,
                MediaFileScalable = entity.MediaFileScalable,
                MediaFileType = entity.MediaFileType,
                MediaFileValue = entity.MediaFileValue
            };
        }

    }
}
