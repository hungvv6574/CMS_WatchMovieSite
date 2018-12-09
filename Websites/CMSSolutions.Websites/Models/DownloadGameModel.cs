using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class DownloadGameModel
    {
        public DownloadGameModel()
        {
            Code = Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Mã sự kiện", Required = true, MaxLength = 100, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 0)]
        public string Code { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Tên sự kiện", ContainerCssClass = Constants.ContainerCssClassCol8, ContainerRowIndex = 0)]
        public string Title { get; set; }

        [ControlFileUpload(EnableFineUploader = true, LabelText = "Logo", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1, Required = true, ShowThumbnail = true)]
        public string Logo { get; set; }

        [ControlFileUpload(EnableFineUploader = true, LabelText = "Banner", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1, Required = true, ShowThumbnail = true)]
        public string UrlBanner { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 500, LabelText = "Đường dẫn tới game", ContainerCssClass = Constants.ContainerCssClassCol8, ContainerRowIndex = 2)]
        public string GooglePlayUrl { get; set; }

        [ControlNumeric(LabelText = "VIPXU", Required = true, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 2)]
        public int VipXu { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 500, LabelText = "Đường dẫn tới website", ContainerCssClass = Constants.ContainerCssClassCol8, ContainerRowIndex = 3)]
        public string WebsiteUrl { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Hiện trên trang chủ", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 3)]
        public bool IsActived { get; set; }

        public static implicit operator DownloadGameModel(DownloadGameInfo entity)
        {
            var item = new DownloadGameModel
            {
                Id = entity.Id,
                IsActived = entity.IsActived,
                Code = entity.Code,
                UrlBanner = entity.UrlBanner,
                Logo = entity.Logo,
                Title = entity.Title,
                GooglePlayUrl = entity.GooglePlayUrl,
                WebsiteUrl = entity.WebsiteUrl,
                VipXu = entity.VipXu
            };
            
            return item;
        }
    }
}