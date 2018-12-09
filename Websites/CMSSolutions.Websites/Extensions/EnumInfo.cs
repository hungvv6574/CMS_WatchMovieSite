using System.ComponentModel.DataAnnotations;

namespace CMSSolutions.Websites.Extensions
{
    public enum AlertErrorVideo
    {
        [Display(Name = "..Chọn thông báo lỗi..")]
        None = 0,

        [Display(Name = "Lỗi tiêu đề")]
        Aler1 = 1,

        [Display(Name = "Lỗi hình ảnh")]
        Aler2 = 2,

        [Display(Name = "Lỗi âm thanh")]
        Aler3 = 3,

        [Display(Name = "Không xem được")]
        Aler4 = 4,

        [Display(Name = "Lỗi đứng hình, tiếng vẫn phát")]
        Aler5 = 5,

        [Display(Name = "Lỗi khác")]
        Aler6 = 6,
    }

    public enum LinkType
    {
        Streaming = 1,

        Picasa = 2,

        Youtube = 3
    }

    public enum SliderPages
    {
        [Display(Name = "Trang chủ")]
        Home = 1,

        [Display(Name = "Chuyên mục phim lẻ")]
        CatetegoryPhimLe = 2,

        [Display(Name = "Phim mới nhất")]
        FilmHot = 3,

        [Display(Name = "Chiếu rạp")]
        FilmTheater = 4,

        [Display(Name = "Show")]
        Show = 5,

        [Display(Name = "Clip")]
        Clip = 6,

        [Display(Name = "Trailer")]
        Trailer = 7,

        [Display(Name = "JJ Channel")]
        JJChannel = 8,

        [Display(Name = "Chuyên mục phim bộ")]
        CatetegoryPhimBo = 9,
    }

    public enum SearchSortBy
    {
        [Display(Name = "Tăng dần")]
        ASC = 1,

        [Display(Name = "Giảm dần")]
        DESC = 2,
    }

    public enum SearchOrderBy
    {
        [Display(Name = "Xếp theo")]
        Id = 0,

        [Display(Name = "Mới Up")]
        CreateDate = 1,

        [Display(Name = "Năm sản xuất")]
        ReleaseYear = 2,

        [Display(Name = "Tên phim")]
        FilmName = 3,

        [Display(Name = "Lượt xem")]
        ViewCount = 4,

        [Display(Name = "Lượt like")]
        CommentCount = 5
    }

    public enum FixCategories
    {
        [Display(Name = "Nạp VIP")]
        NapVIP = 63,

        [Display(Name = "Phim hót")]
        FilmHot = 101,

        [Display(Name = "Phim lẻ")]
        FilmLe = 2,

        [Display(Name = "Phim bộ")]
        FilmBo = 3,

        [Display(Name = "Tất cả")]
        AllNews = 59,

        [Display(Name = "Sự kiện")]
        Events = 60,

        [Display(Name = "Tin tức")]
        News = 61,
        
        [Display(Name = "Tuyển  dụng")]
        Recruitment = 62,

        [Display(Name = "Phim chiếu rạp")]
        FilmTheater = 102,

        [Display(Name = "Hỗ trợ")]
        Support = 64
    }

    public enum PromotionStatus
    {
        [Display(Name = "Đang thực hiện")]
        Running = 1,

        [Display(Name = "Đã kết thúc")]
        Finish = 2
    }

    public enum ExchangeMoney
    {
        [Display(Name = "10.000")]
        VipXu10 = 100,

        [Display(Name = "20.000")]
        VipXu20 = 200,

        [Display(Name = "30.000")]
        VipXu30 = 300,

        [Display(Name = "50.000")]
        VipXu50 = 500,

        [Display(Name = "100.000")]
        VipXu100 = 1000,

        [Display(Name = "200.000")]
        VipXu200 = 2000,

        [Display(Name = "300.000")]
        VipXu300 = 3000,

        [Display(Name = "500.000")]
        VipXu500 = 5000
    }

    public enum  ExchangeDay
    {
        [Display(Name = "15 Ngày")]
        Package15 = 15,

        [Display(Name = "50 Ngày")]
        Package50 = 50,

        [Display(Name = "100 Ngày")]
        Package100 = 100,

        [Display(Name = "300 Ngày")]
        Package300 = 300
    }

    public enum CardStatus
    {
        [Display(Name = "Nạp thẻ thất bại")]
        Error = 1,

        [Display(Name = "Nạp thẻ thành công")]
        Success = 2
    }

    public enum PaymentStatus
    {
        [Display(Name = "Đang thực hiện")]
        Open = 1,

        [Display(Name = "Đã hoàn thành")]
        Close = 2
    }

    public enum TransferType
    {
        [Display(Name = "Gửi đi")]
        Send = 1,

        [Display(Name = "Nhận về")]
        Rereceive = 2
    } 

    public enum VideoTypes
    {
        [Display(Name = "Phim")]
        IsFilm = 1,

        [Display(Name = "Show")]
        IsShow = 2,

        [Display(Name = "Clip")]
        IsClip = 3,

        [Display(Name = "Trailer")]
        IsTrailer = 4
    }

    public enum Site
    {
        [Display(Name = "viphd.vn")]
        Home = 1,

        [Display(Name = "jj.viphd.vn")]
        JJHome = 2
    }

    public enum HomeDisplayFilmType
    {
        [Display(Name = "Sự kiện: Phim Đang HOT")]
        FilmHot = 1,

        [Display(Name = "Phim lẻ mới up")]
        FilmRetail = 2,

        [Display(Name = "Phim bộ mới up")]
        FilmLengthEpisodes = 3,

        [Display(Name = "JJ Channel giới thiệu")]
        FilmJJChannelIntroduce = 4,

        [Display(Name = "Phim chiếu rạp")]
        FilmTheater = 5,

        [Display(Name = "TV Show")]
        TVShow = 6,

        [Display(Name = "Clip")]
        Clip = 7,

        [Display(Name = "Bình luận Facebook")]
        FacebookComments = 8,

        [Display(Name = "Phim lẻ xem nhiều nhất")]
        StatisticalFilmRetail = 9,

        [Display(Name = "Phim bộ xem nhiều nhất")]
        StatisticalLengthEpisodes = 10,

        [Display(Name = "Show xem nhiều nhất")]
        StatisticalShows = 11,

        [Display(Name = "Clip xem nhiều nhất")]
        StatisticalClips = 12,

        [Display(Name = "Phim sắp chiếu")]
        StatisticalTrailer = 13,

        [Display(Name = "Phim liên quan")]
        StatisticalNextFilm = 14
    }

    public enum LogStatus
    {
        [Display(Name = "Thông báo lỗi")]
        Error = 1,

        [Display(Name = "Thông báo thành công")]
        Success = 2,

        [Display(Name = "Cập nhập dữ liệu")]
        Information = 3
    }

    public enum LogType
    {
        [Display(Name = "Tin nhắn tổng đài")]
        SMS = 1,

        [Display(Name = "Thẻ ngân hàng")]
        BankCard = 2,

        [Display(Name = "Thẻ cào")]
        ScratchCard = 3,

        [Display(Name = "Khác")]
        Other = 4,
    }

    public enum CustomerLogType
    {
        [Display(Name = "Hệ thống log")]
        SystemLogs = 1,

        [Display(Name = "Nạp tiền")]
        NapVipLogs = 2,

        [Display(Name = "Đổi VipXu")]
        VipXuLogs = 3,

        [Display(Name = "Đào Xu")]
        DaoXuLogs = 4
    }

    public enum AmountType
    {
        [Display(Name = "Không tính tiền khách hàng")]
        NotAmount = 0,

        [Display(Name = "Tính tiền khách hàng")]
        Amount = 1,

        [Display(Name = "Trả lại tiền khách hàng")]
        ResultAmount = 2
    }

    public enum SMSType
    {
        [Display(Name = "Nhận dữ liệu từ nhà mạng")]
        MO = 1,

        [Display(Name = "Gửi thông báo cho khách hàng")]
        MT = 2
    }

    public enum RequestStatus
    {
        [Display(Name = "Thực hiện giao dịch thành công")]
        Messages200 = 200,

        [Display(Name = "Thực hiện giao dịch thất bại, yêu cầu gửi lại")]
        Messages01 = 1,

        [Display(Name = "Trùng MTID")]
        Messages02 = 2,

        [Display(Name = "IP của đối tác không hợp lệ hoặc chưa được cấu hình trong hệ thống")]
        Messages03 = 3,

        [Display(Name = "Không tìm thấy thông tin Command Code trên hệ thống")]
        Messages04 = 4,

        [Display(Name = "Không tìm thấy service number trên hệ thống")]
        Messages05 = 5,

        [Display(Name = "Hệ thống tạm thời gặp lỗi")]
        Messages06 = 6,

        [Display(Name = "Sai check sum")]
        Messages17 = 17,

        [Display(Name = "Sai định dạng MTID")]
        Messages18 = 18,

        [Display(Name = "Method không được hỗ trợ")]
        Messages19 = 19,

        [Display(Name = "Không tim thấy MO của MT này(trong trường hợp xử dụng method= mtReceiver)")]
        Messages20 = 20
    }

    public enum ShortCode
    {
        [Display(Name = "8076")]
        DauSo8076 = 8076,

        [Display(Name = "8176")]
        DauSo8176 = 8176,

        [Display(Name = "8276")]
        DauSo8276 = 8276,

        [Display(Name = "8376")]
        DauSo8376 = 8376,

        [Display(Name = "8476")]
        DauSo8476 = 8476,

        [Display(Name = "8576")]
        DauSo8576 = 8576,

        [Display(Name = "8676")]
        DauSo8676 = 8676,

        [Display(Name = "8776")]
        DauSo8776 = 8776
    }

    public enum Gender
    {
        [Display(Name = "Không xác định")]
        None = 0,

        [Display(Name = "Nam")]
        Male = 1,

        [Display(Name = "Nữ")]
        Female = 2,
    }

    public enum LoginTypes
    {
        [Display(Name = "Mặc định")]
        None = 0,

        [Display(Name = "Facebook")]
        Facebook = 1,

        [Display(Name = "Google")]
        Google = 2, 
    }

    public enum HomeWidgets
    {
        [Display(Name = "Banner ở bên trái slide chính")]
        BannerPageLeft = 1,

        [Display(Name = "Banner ở dưới slide chính")]
        BannerPageCenter = 2,

        [Display(Name = "Banner ở bên phải slide chính")]
        BannerPageRight = 3,

        [Display(Name = "Banner vị trí thứ nhất nội dung bên trái")]
        BannerContentLeftFirst = 4,

        [Display(Name = "Banner vị trí thứ hai nội dung bên trái")]
        BannerContentLeftSecond = 5,

        [Display(Name = "Banner vị trí thứ ba nội dung bên trái")]
        BannerContentLeftThird = 6,

        [Display(Name = "Banner vị trí thứ tư nội dung bên trái")]
        BannerContentLeftFourth = 7,

        [Display(Name = "Banner vị trí thứ năm nội dung bên trái")]
        BannerContentLeftFifth = 8,

        [Display(Name = "Banner vị trí thứ sáu nội dung bên trái")]
        BannerContentLeftSixth = 9,

        [Display(Name = "Banner vị trí thứ bảy nội dung bên trái")]
        BannerContentLeftSeventh = 10,

        [Display(Name = "Banner vị trí thứ nhất bên phải")]
        BannerRightFirst = 11,

        [Display(Name = "Banner vị trí thứ hai bên phải")]
        BannerRightSecond = 12,

        [Display(Name = "Banner vị trí thứ ba nội dung bên trái")]
        BannerRightThird = 13,

        [Display(Name = "Banner vị trí thứ tư nội dung bên trái")]
        BannerRightFourth = 14,

        [Display(Name = "Slider chính")]
        HomeSliderBanner = 15,

        [Display(Name = "Footer thông tin bản quyền")]
        HomeFooterLogoInformation = 16,

        [Display(Name = "Facebook Fanpage")]
        HomeFacebookFanpage = 17,

        [Display(Name = "Facebook Activity")]
        HomeFacebookActivity = 18,

        [Display(Name = "Facebook Follow")]
        HomeFacebookFollow = 19,

        [Display(Name = "Google Badge")]
        HomeGoogleBadge = 20,

        [Display(Name = "Banner vị trí thứ năm bên phải")]
        BannerRightFifth = 21,
        
        [Display(Name = "Banner vị trí thứ sáu bên phải")]
        BannerRightSixth = 22,

        [Display(Name = "Banner khu vực VIP")]
        BannerPageCenterVip = 23,

        [Display(Name = "Banner khu vực VIP trái")]
        BannerPageLeftVip = 24,

        [Display(Name = "Banner khu vực VIP phải")]
        BannerPageRightVip = 25,

        [Display(Name = "Chia sẻ mạng xã hội")]
        DisplaySocialNetwork = 26
    }

    public enum  BankErrorMessages
    {
        [Display(Name = "Thành công.")]
        Success = 0,

        [Display(Name = "Thất bại.")]
        Fail = 1,

        [Display(Name = "Chưa confirm được.")]
        NotConfirmedYet = 2,

        [Display(Name = "Đã confirm trước đó.")]
        ConfirmedBefore = 3,

        [Display(Name = "Giao dịch Pending.")]
        TransactionPending = 4,

        [Display(Name = "Sai MAC.")]
        MacFail = 5,

        [Display(Name = "Không xác định mã lỗi.")]
        Exception = 6,

        [Display(Name = "Giao dịch không tồn tại.")]
        NotExistTransaction = 7,

        [Display(Name = "Thông tin không đầy đủ.")]
        FieldsNotFull = 8,

        [Display(Name = "Đại lý không tồn tại.")]
        NotExistMerchant = 9,

        [Display(Name = "Sai định dạng.")]
        FalseFormat = 10,

        [Display(Name = "Sai thông tin.")]
        WrongInformation = 11,

        [Display(Name = "Ngân hàng tạm khóa hoặc không tồn tại.")]
        BankNotActive = 12,

        [Display(Name = "Có lỗi.")]
        Error = 13,

        [Display(Name = "Code không hợp lệ.")]
        NotExactlyCode = 14,

        [Display(Name = "Ngân hàng từ chối giao dịch.")]
        BankDeclined = 801,

        [Display(Name = "Mã đơn vị không tồn tại.")]
        MerchantNotExist = 803,

        [Display(Name = "Mã đơn vị không tồn tại.")]
        InvalidAccessCode = 804,

        [Display(Name = "Số tiền không hợp lệ.")]
        InvalidAmount = 805,

        [Display(Name = "Mã tiền tệ không tồn tại.")]
        InvalidCurrencyCode = 806,

        [Display(Name = "Lỗi không xác định.")]
        UnspecifiedFailure = 807,

        [Display(Name = "Số thẻ không đúng.")]
        InvalidCardNumber = 808,

        [Display(Name = "Tên chủ thẻ không đúng.")]
        InvalidCardName = 809,

        [Display(Name = "Thẻ hết hạn/thẻ bị khóa.")]
        ExpiredCard = 810,

        [Display(Name = "Thẻ chưa đăng ký dịch vụ Internet banking.")]
        CardNotRegisterService = 811,

        [Display(Name = "Ngày  phát  hành/hết  hạn  không đúng.")]
        InvalidCardDate = 812,

        [Display(Name = "Vượt quá hạn mức thanh toán.")]
        ExistAmount = 813,

        [Display(Name = "Số tiền không đủ để thanh toán.")]
        InsufficientFund = 821,

        [Display(Name = "Người sử dụng cancel.")]
        UserCancel = 899,

        [Display(Name = "Merchant_code không hợp lệ.")]
        InvalidMerchantCode = 901,

        [Display(Name = "Chuỗi mã hóa không hợp lệ.")]
        InvalidEncryption = 902,

        [Display(Name = "Merchant_tran_id không hợp lệ.")]
        InvalidMerchantTranId = 903,

        [Display(Name = "Không tìm thấy giao dịch trong hệ thống.")]
        NotFoundTransaction = 904,

        [Display(Name = "Đã xác nhận trước đó.")]
        AlreadyConfirmed = 906,

        [Display(Name = "Lỗi timeout xảy ra do không nhận thông điệp trả về.")]
        ErrorTimeout = 908,

        [Display(Name = "Số tiền không hợp lệ.")]
        NotInvalidAmount = 911,

        [Display(Name = "Phí không hợp lệ.")]
        InvalidFee = 912,

        [Display(Name = "Tax không hợp lệ.")]
        InvalidTax = 913
    }

    public enum ErrorMessages
    {
        [Display(Name = "Giao dịch thành công")]
        Success = 1,

        [Display(Name = "Thẻ đã sử dụng")]
        CardIsUsed = -1,

        [Display(Name = "Thẻ đã khóa")]
        CardIsLocked = -2,

        [Display(Name = "Thẻ đã hết hạn sử dụng")]
        CardIsExpired = -3,

        [Display(Name = "Mã thẻ chưa được kích hoạt")]
        CardIsNotActivate = -4,

        [Display(Name = "Mã thẻ sai định dạng")]
        CardFormatIsIncorrect = -10,

        [Display(Name = "Thẻ không tồn tại")]
        CardIsNotExist = -12,

        [Display(Name = "Lỗi thẻ VMS không đúng định dạng.")]
        VMSCardWrongFomat = -99,

        [Display(Name = "Partner nhập sai mã thẻ quá 5 lần.")]
        WrongCardInputExceed = 5,

        [Display(Name = "Sai thông tin partner.")]
        PartnerInformationError = 6,

        [Display(Name = "Chưa nhận được kết quả trả về từ nhà cung cấp mã thẻ.")]
        TransactionPending = 99,

        [Display(Name = "Dữ liệu carddata không đúng.")]
        BadCardData = -24,

        [Display(Name = "Nhà cung cấp không tồn tại.")]
        NoProviderFound = -11,

        [Display(Name = "Sai IP.")]
        InvalidIPRequest = 8,

        [Display(Name = "Sai Session.")]
        InvalidSessionID = 3,

        [Display(Name = "Session hết hạn.")]
        SessionTimeOut = 7,

        [Display(Name = "Thẻ không sử dụng được.")]
        CardIsIncorrect = 4,

        [Display(Name = "Hệ thống tạm thời bận.")]
        SystemBusy = 13,

        [Display(Name = "Giao dịch thất bại.")]
        Fail = 0,

        [Display(Name = "Tạm thời khóa kênh nạp VMS do quá tải.")]
        VMSChargingChanelOverload  = 9,

        [Display(Name = "Hệ thống nhà cung cấp gặp lỗi.")]
        ProviderError = 10,

        [Display(Name = "Nhà cung cấp tạm thời khóa partner do lỗi hệ thống.")]
        PartnerLocked = 10,

        [Display(Name = "Trùng TransactionId.")]
        TransactionDuplicate = 12,

        [Display(Name = "Thẻ đã sử dụng hoặc không tồn tại.")]
        CardIsUsedOrCardDoNotExist = 50,

        [Display(Name = "Seri thẻ không đúng.")]
        CardSerialIsInvalid = 51,

        [Display(Name = "Mã thẻ và serial không khớp.")]
        CardSerialAndPinIsNotMatch = 52,

        [Display(Name = "Serial hoặc mã thẻ không đúng.")]
        CardSerialOrPinIsIncorrect = 53,

        [Display(Name = "Serial hoặc mã thẻ không đúng.")]
        CardIsWaitingForActivate = 54,

        [Display(Name = "Các tạm thời bị block 24h.")]
        CardIsBlockFor24Hours = 55
    }

    public enum Sex
    {
        [Display(Name = "Không xác định")]
        Other = 0,

        [Display(Name = "Nam")]
        Male = 1,

        [Display(Name = "Nữ")]
        Female = 2
    }

    public enum Status
    {
        [Display(Name = "Đang sử dụng")]
        Approved = 1,

        [Display(Name = "Xóa tạm thời")]
        Deleted = 2
    }

    public enum FilmGroup
    {
        [Display(Name = "Phim một tập")]
        FilmRetail = 1,

        [Display(Name = "Phim dài tập")]
        FilmLengthEpisodes = 2
    }

    public enum  SearchType
    {
        [Display(Name = "Phim")]
        Film = 1,

        [Display(Name = "Tin tức")]
        Articles = 2,

        [Display(Name = "Chuyên mục")]
        Category = 3
    }
}