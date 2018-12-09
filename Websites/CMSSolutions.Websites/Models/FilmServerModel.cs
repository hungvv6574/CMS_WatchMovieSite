using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Models
{
    public class FilmServerModel
    {
        [ControlHidden]
        public int Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 0)]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Tên máy chủ", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 0)]
        public string ServerName { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 50, LabelText = "Địa chỉ IP", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1)]
        public string ServerIP { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 50, LabelText = "Thư mục gốc", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 1)]
        public string FolderRoot { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, LabelText = "Tài khoản FTP", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string UserName { get; set; }

        [ControlText(Type = ControlText.Password, LabelText = "Mật khẩu", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 2)]
        public string Password { get; set; }

        [ControlText(Type = ControlText.TextBox, MaxLength = 250, LabelText = "Địa điểm đặt", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string Locations { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Máy chủ VIP", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public bool IsVip { get; set; }
        
        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Máy chủ mặc định", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 4)]
        public bool IsDefault { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 4)]
        public int Status { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 2, MaxLength = 2000, LabelText = "Ghi chú", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 6)]
        public string Description { get; set; }
        
        public static implicit operator FilmServerModel(FilmServerInfo entity)
        {
            return new FilmServerModel
            {
                Id = entity.Id,
                LanguageCode =  entity.LanguageCode,
                SiteId = entity.SiteId,
                ServerName = entity.ServerName,
                ServerIP = entity.ServerIP,
                UserName = entity.UserName,
                Password = entity.Password,
                Locations = entity.Locations,
                IsVip = entity.IsVip,
                IsDefault = entity.IsDefault,
                Description = entity.Description,
                Status = entity.Status,
                FolderRoot = entity.FolderRoot
            };
        }
    }
}
