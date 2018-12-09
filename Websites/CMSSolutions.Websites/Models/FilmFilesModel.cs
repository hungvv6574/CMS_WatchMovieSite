using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class FilmFilesModel
    {
        public FilmFilesModel()
        {
            FileCode = Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        [ControlHidden]
        public long Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.ServerId + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlCascadingDropDown(LabelText = "Máy chủ", ParentControl = "SiteId", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int ServerId { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Mã file", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string FileCode { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên videos", PlaceHolder = "Tối đa 250 ký tự", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol9, ContainerRowIndex = 1)]
        public string Name { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Thư mục gốc", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string FolderName { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn thư mục", MaxLength = 300, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string FolderPath { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên file", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string FileName { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đôi mở rộng", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public string Extentions { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Đường dẫn đầy đủ", MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string FullPath { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Kích thước", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public string Size { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "Đang sử dụng", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public bool HasUse { get; set; }

        public static implicit operator FilmFilesModel(FilmFilesInfo entity)
        {
            return new FilmFilesModel
            {
                Id = entity.Id,
                LanguageCode = entity.LanguageCode,
                Name = entity.Name,
                SiteId = entity.SiteId,
                FolderName = entity.FolderName,
                FolderPath = entity.FolderPath,
                FileName = entity.FileName,
                Extentions = entity.Extentions,
                FullPath = entity.FullPath,
                Size = entity.Size,
                HasUse = entity.HasUse
            };
        }
    }
}
