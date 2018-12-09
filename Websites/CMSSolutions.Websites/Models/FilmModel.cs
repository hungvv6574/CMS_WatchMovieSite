using System;
using CMSSolutions.Web.UI.ControlForms;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Models
{
    public class FilmModel
    {
        public FilmModel()
        {
            PublishedDate = DateTime.Now.ToString(Extensions.Constants.DateTimeFomat);
            IsPublished = true;
            VideoType = (int)VideoTypes.IsFilm;
            Status = (int)Extensions.Status.Approved;
        }

        [ControlHidden]
        public long Id { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Ngôn ngữ hiển thị", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0, OnSelectedIndexChanged = "$('#" + Extensions.Constants.CategoryIds + "').empty();")]
        public string LanguageCode { get; set; }

        [ControlCascadingDropDown(LabelText = "Trang web", ParentControl = "LanguageCode", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int SiteId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, Required = true, LabelText = "Máy chủ mặc định", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int ServerId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, EnableChosen = true, CssClass = Extensions.Constants.CssControlCustom, Required = true, LabelText = "Đạo diễn", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 0)]
        public int DirectorId { get; set; }

        [ControlCascadingDropDown(AllowMultiple = true, EnableChosen = true, LabelText = "Chuyên mục hiển thị", ParentControl = "SiteId", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int[] CategoryIds { get; set; }

        [ControlChoice(ControlChoice.DropDownList, AllowMultiple = true, CssClass = Extensions.Constants.CssControlCustom, EnableChosen = true, Required = true, LabelText = "Thể loại phim", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int[] FilmTypeIds { get; set; }

        [ControlChoice(ControlChoice.DropDownList, AllowMultiple = true, EnableChosen = true, CssClass = Extensions.Constants.CssControlCustom, Required = true, LabelText = "Diễn viên", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 1)]
        public int[] ActorIds { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Phim chiếu rạp", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public bool IsTheater { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Nhóm phim", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public int FilmGroup { get; set; }

        [ControlCascadingDropDown(LabelText = "Tên bộ phim", EnableChosen = true, CssClass = Extensions.Constants.CssControlCustom, ParentControl = "FilmGroup", AbsoluteParentControl = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public int CollectionId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, EnableChosen = true, CssClass = Extensions.Constants.CssControlCustom, Required = true, LabelText = "Nước sản xuất", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 2)]
        public int CountryId { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Loại video", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 3)]
        public int VideoType { get; set; }

        [ControlText(Type = ControlText.TextBox, Required = true, MaxLength = 250, LabelText = "Tên phim tiếng việt", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 3)]
        public string FilmName { get; set; }

        [ControlText(Type = ControlText.TextBox, MaxLength = 250, LabelText = "Tên phim tiếng anh", ContainerCssClass = Constants.ContainerCssClassCol4, ContainerRowIndex = 3)]
        public string FilmNameEnglish { get; set; }

        [ControlText(Type = ControlText.TextBox, LabelText = "Tên không dấu", PlaceHolder = "Mặc định để trống tự sinh theo tên phim tiếng việt", MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 3)]
        public string FilmAlias { get; set; } 

        [ControlFileUpload(EnableFineUploader = true, LabelText = "Ảnh đại diện", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 4, Required = true, ShowThumbnail = true)]
        public string ImageIcon { get; set; }

        [ControlFileUpload(EnableFineUploader = true, LabelText = "Ảnh đại diện lớn", ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 4, Required = true, ShowThumbnail = true)]
        public string ImageThumb { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Hiện trên trang chủ", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 5)]
        public bool IsHome { get; set; }

        [ControlNumeric(LabelText = "Năm sản xuất", MinimumValue = "1900", MaximumValue = "2050", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 5)]
        public int ReleaseYear { get; set; }

        [ControlText(Required = true, MaxLength = 50, LabelText = "Thời gian(Phút)", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 5)]
        public string Time { get; set; }

        [ControlText(Required = true, MaxLength = 50, LabelText = "Tổng dung lượng(MB)", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 5)]
        public string Capacity { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Có bản quyền", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 6)]
        public bool HasCopyright { get; set; }

        [ControlNumeric(LabelText = "Giá nhập", Required = true, ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 6)]
        public float Prices { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Xuất bản", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 6)]
        public bool IsPublished { get; set; }

        [ControlDatePicker(LabelText = "Ngày xuất bản", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 6)]
        public string PublishedDate { get; set; }

        [ControlChoice(ControlChoice.CheckBox, LabelText = "", PrependText = "Phim hot", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 7)]
        public bool IsHot { get; set; }

        [ControlDatePicker(LabelText = "Từ ngày", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 7)]
        public string StartDate { get; set; }

        [ControlDatePicker(LabelText = "Đến ngày", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 7)]
        public string EndDate { get; set; }

        [ControlChoice(ControlChoice.DropDownList, LabelText = "Trạng thái", ContainerCssClass = Constants.ContainerCssClassCol3, ContainerRowIndex = 7)]
        public int Status { get; set; }

        [ControlText(LabelText = "Tóm tắt phim", PlaceHolder = "Tối đa 400 ký tự.", Type = ControlText.MultiText, Rows = 3, Required = true, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 8)]
        public string Summary { get; set; }

        [ControlText(LabelText = "Nội dung phim", Required = true, Type = ControlText.RichText, ContainerCssClass = Constants.ContainerCssClassCol12, ContainerRowIndex = 9)]
        public string Contents { get; set; }

        [ControlText(LabelText = "Từ khóa SEO", Required = true, PlaceHolder = "Tối đa 400 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 400, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 10)]
        public string Description { get; set; }

        [ControlText(LabelText = "Tags SEO", Required = true, PlaceHolder = "Nhập từ khóa SEO cách nhau bởi dấu ','và tối đa 250 ký tự.", Type = ControlText.MultiText, Rows = 2, MaxLength = 250, ContainerCssClass = Constants.ContainerCssClassCol6, ContainerRowIndex = 10)]
        public string Tags { get; set; }
        
        public static implicit operator FilmModel(FilmInfo entity)
        {
            var item = new FilmModel
            {
                Id = entity.Id,
                FilmNameEnglish = entity.FilmNameEnglish,
                FilmName = entity.FilmName,
                FilmAlias = entity.FilmAlias,
                LanguageCode = entity.LanguageCode,
                SiteId = entity.SiteId,
                CategoryIds = Utilities.ParseListInt(entity.CategoryIds),
                FilmTypeIds = Utilities.ParseListInt(entity.FilmTypeIds),
                CountryId = entity.CountryId,
                DirectorId = entity.DirectorId,
                ActorIds = Utilities.ParseListInt(entity.ActorIds),
                CollectionId = entity.CollectionId,
                Time = entity.Time,
                Capacity = entity.Capacity,
                ReleaseYear = entity.ReleaseYear,
                Contents = entity.Contents,
                Summary = entity.Summary,
                Description = entity.Description,
                Tags = entity.Tags,
                IsPublished = entity.IsPublished,
                IsHot = entity.IsHot,
                IsHome = entity.IsHome,
                Prices = (float)entity.Prices,
                HasCopyright = entity.HasCopyright,
                ImageIcon = entity.ImageIcon,
                ImageThumb = entity.ImageThumb,
                ServerId = entity.ServerId,
                Status = entity.Status,
                IsTheater = entity.IsTheater
            };
            if (entity.IsFilm)
            {
                item.VideoType = (int)VideoTypes.IsFilm;
            }
            if (entity.IsShow)
            {
                item.VideoType = (int)VideoTypes.IsShow;
            }
            if (entity.IsClip)
            {
                item.VideoType = (int)VideoTypes.IsClip;
            }
            if (entity.IsTrailer)
            {
                item.VideoType = (int)VideoTypes.IsTrailer;
            }
            if (entity.PublishedDate != null 
                && entity.PublishedDate.Value.ToString(Extensions.Constants.DateTimeFomat) != Extensions.Constants.DateTimeMin)
            {
                item.PublishedDate = entity.PublishedDate.Value.ToString(Extensions.Constants.DateTimeFomat);
            }

            if (entity.StartDate != null 
                && entity.StartDate.Value.ToString(Extensions.Constants.DateTimeFomat) != Extensions.Constants.DateTimeMin)
            {
                item.StartDate = entity.StartDate.Value.ToString(Extensions.Constants.DateTimeFomat);
            }

            if (entity.EndDate != null 
                && entity.EndDate.Value.ToString(Extensions.Constants.DateTimeFomat) != Extensions.Constants.DateTimeMin)
            {
                item.EndDate = entity.EndDate.Value.ToString(Extensions.Constants.DateTimeFomat);
            }

            return item;
        }
    }
}
