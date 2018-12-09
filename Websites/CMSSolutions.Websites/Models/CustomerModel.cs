using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Birthday = DateTime.Now.AddYears(-10);
            ImageIcon = "/Images/avatars/avatar-no-image.png";
        }

        [ControlHidden]
        public int Id { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Mã khách hàng", PlaceHolder = "Tự động sinh mã", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string CutomerCode { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tài khoản đăng nhập", PlaceHolder = "Nhập ký tự không dấu, email, sđt.", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string UserName { get; set; }

        [ControlText(Type = ControlText.Password, LabelText = "Mật khẩu đăng nhập", PlaceHolder = "Mật khẩu chứa ký tự Hoa, Thường, Số, 1 ký tự đặc biệt.", Required = true, MaxLength = 500, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string Password { get; set; }

        [ControlText(Type = ControlText.Email, LabelText = "Email", Required = true, MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public string Email { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Họ và Tên", Required = true, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string FullName { get; set; }

        [ControlDatePicker(LabelText = "Ngày sinh", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public DateTime Birthday { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Giới tính", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public int Sex { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Số điện thoại", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 1)]
        public string PhoneNumber { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Khóa tài khoản", ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 2)]
        public bool IsBlock { get; set; }

        [ControlNumeric(LabelText = "VIP Xu", Required = true, ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 2)]
        public decimal VipXu { get; set; }

        [ControlNumeric(LabelText = "Tổng tiền", Required = true, ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 2)]
        public decimal TotalMoney { get; set; }

        [ControlNumeric(LabelText = "Tổng số ngày", Required = true, ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 2)]
        public int TotalDay { get; set; }

        [ControlText(Type = ControlText.TextBox, ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 2)]
        public string StartDate { get; set; }

        [ControlText(Type = ControlText.TextBox, ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 2)]
        public string EndDate { get; set; }

        [ControlFileUpload(EnableFineUploader = true, Required = true, LabelText = "Ảnh đại diện", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 3, ShowThumbnail = true)]
        public string ImageIcon { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Quận huyện/Thành phố", ContainerCssClass = Constants.ContainerCssClassCol2, ContainerRowIndex = 3)]
        public string CityId { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Địa chỉ", MaxLength = 2000, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 3)]
        public string Address { get; set; }

        [ControlChoice(ControlChoice.DropDownList, AllowMultiple = true, CssClass = Extensions.Constants.CssControlCustom, EnableChosen = true, Required = true, LabelText = "Thể loại phim", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 4)]
        public int[] FilmTypeIds { get; set; }

        [ControlChoice(ControlChoice.DropDownList, AllowMultiple = true, CssClass = Extensions.Constants.CssControlCustom, EnableChosen = true, Required = true, LabelText = "Nước sản xuất", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 4)]
        public int[] CountryIds { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Facebook", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 6)]
        public string Facebook { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Skype", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 6)]
        public string Skype { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Zing Me", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 6)]
        public string ZingMe { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Google", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 7)]
        public string Google { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Yahoo", MaxLength = 50, ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 7)]
        public string Yahoo { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 7)]
        public int Status { get; set; }

        [ControlText(Type = ControlText.MultiText, Rows = 3, MaxLength = 2000, LabelText = "Giới thiệu", ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 8)]
        public string Description { get; set; }

        public static implicit operator CustomerModel(CustomerInfo entity)
        {
            return new CustomerModel
            {
                Id = entity.Id,
                CutomerCode = entity.CustomerCode,
                UserName = entity.UserName,
                Password = entity.Password,
                FullName = entity.FullName,
                Email = entity.Email,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,
                CityId = entity.CityId,
                Birthday = entity.Birthday,
                Sex = entity.Sex,
                ImageIcon = entity.ImageIcon,
                FilmTypeIds = Utilities.ParseListInt(entity.FilmTypeIds),
                CountryIds = Utilities.ParseListInt(entity.CountryIds),
                Skype = entity.Skype,
                ZingMe = entity.ZingMe,
                Facebook = entity.Facebook,
                Google = entity.Google,
                Yahoo = entity.Yahoo,
                VipXu = entity.VipXu,
                TotalMoney = entity.TotalMoney,
                IsBlock = entity.IsBlock,
                Status = entity.Status,
                Description = entity.Description,
                TotalDay = entity.TotalDay,
                StartDate = entity.TextStartDate,
                EndDate = entity.TextEndDate
            };
        }
    }
}
