using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSSolutions.Configuration;
using CMSSolutions.ContentManagement.Widgets.Services;
using CMSSolutions.DisplayManagement;
using CMSSolutions.Security.Cryptography;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeCustomerController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeCustomerController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
        }

        private void BuildModules()
        {
            var widget = WorkContext.Resolve<IWidgetService>();
            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region Customer
            var customerService = WorkContext.Resolve<ICustomerService>();
            var customer = customerService.GetById(UserId);
            var viewCustomer = viewRenderer.RenderPartialView(Extensions.Constants.CustomerViewFilePath, customer);
            WorkContext.Layout.AdBannerRightFirst.Add(new MvcHtmlString(viewCustomer));
            #endregion

            #region SocialNetwork
            var socialNetwork = widget.GetWidget(HomeWidgets.DisplaySocialNetwork.ToString());
            if (socialNetwork != null)
            {
                var widgetShape = shapeFactory.Widget();
                widgetShape.Widget = socialNetwork;
                WorkContext.Layout.DisplaySocialNetwork(widgetShape);
            }
            #endregion
        }

        [Url("nguoi-dung/thong-bao")]
        public ActionResult UserMessages()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            var category = serviceCategory.GetById((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();

            return View("UserMessages");
        }

        [Url("nguoi-dung/phim-ua-thich")]
        public ActionResult UserHistories1()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            var category = serviceCategory.GetById((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();

            return View("UserHistories1");
        }

        [Url("nguoi-dung/phim-da-xem")]
        public ActionResult UserHistories2()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            var category = serviceCategory.GetById((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();

            return View("UserHistories2");
        }

        [Url("nguoi-dung/phim-dang-xem")]
        public ActionResult UserHistories3()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            var category = serviceCategory.GetById((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();

            return View("UserHistories3");
        }

        [Url("nguoi-dung/sua-thong-tin")]
        public ActionResult UserProfile()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            var category = serviceCategory.GetById((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();

            var customerService = WorkContext.Resolve<ICustomerService>();
            var cityService = WorkContext.Resolve<ICityService>();
            var countryService = WorkContext.Resolve<ICountryService>();
            var filmTypesService = WorkContext.Resolve<IFilmTypesService>();

            var customer = customerService.GetById(UserId);
            var model = new DataViewerModel
            {
                Customer = customer,
                ListCities = cityService.GetRecords(x => x.Status == (int) Status.Approved).ToList(),
                ListCountries = countryService.GetRecords().ToList()
            };
            model.ListFilmTypes = filmTypesService.GetByType(WorkContext.CurrentCulture,SiteId, 1).ToList();
            model.ListDay = Utilities.GetListDay();
            model.ListMonth = Utilities.GetListMonth();
            model.ListYear = Utilities.GetListYear();

            return View("CustomerProfile", model);
        }

        [HttpPost]
        [Url("nguoi-dung/cap-nhat")]
        public ActionResult UserUpdate()
        {
            var redirect = new DataViewerModel();

            var fullName = Request.Form["txtFullName"];
            var email = Request.Form["txtEmail"];
            var address = Request.Form["txtAddress"];
            var city = Request.Form["ddlCity"];
            var phoneNumber = Request.Form["txtPhoneNumber"];
            var day = Request.Form["ddlDay"];
            var month = Request.Form["ddlMonth"];
            var year = Request.Form["ddlYear"];
            var gender = Request.Form["radGender"];
            var password = Request.Form["txtPassword"];
            var captcha = Request.Form["txtCaptcha"];
            var selectFilmTypes = Request.Form["selectFilmTypes[]"];
            var selectCountries = Request.Form["selectCountries[]"];
            var imgImageIcon = Request.Form["imgImageIcon"];

            if (string.IsNullOrEmpty(email))
            {
                redirect.Data = "E-mail không để trống.";
                redirect.Status = false;
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(city) || city == "0")
            {
                redirect.Data = "Vui lòng chọn Tỉnh/Thành.";
                redirect.Status = false;
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(day) || day == "0")
            {
                redirect.Data = "Vui lòng chọn Ngày sinh của bạn.";
                redirect.Status = false;
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(month) || month == "0")
            {
                redirect.Data = "Vui lòng chọn Tháng sinh của bạn.";
                redirect.Status = false;
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(year) || year == "0")
            {
                redirect.Data = "Vui lòng chọn Năm sinh của bạn.";
                redirect.Status = false;
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(selectFilmTypes))
            {
                redirect.Data = "Bạn phải chọn thể loại phim.";
                redirect.Status = false;
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(selectCountries))
            {
                redirect.Data = "Bạn phải chọn quốc gia.";
                redirect.Status = false;
                return Json(redirect);
            }

            HttpPostedFileBase uploadfile = Request.Files[0];
            if (uploadfile == null && string.IsNullOrEmpty(imgImageIcon))
            {
                redirect.Status = false;
                redirect.Data = "Bạn chưa chọn ảnh đại diện.";
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(captcha) || captcha.ToUpper() != GlobalCapcha.ToUpper())
            {
                redirect.Status = false;
                redirect.Data = "Mã xác thực không đúng.";
                return Json(redirect);
            }

            var customerService = WorkContext.Resolve<ICustomerService>();
            var customer = customerService.GetById(UserId);
            if (customer.Password != EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, Utilities.RemoveInjection(password)))
            {
                redirect.Status = false;
                redirect.Data = "Mật khẩu không đúng bạn không thể cập nhật thông tin cá nhân cho tài khoản này.";
                return Json(redirect);
            }

            var imageUrl = customer.ImageIcon;
            if (string.IsNullOrEmpty(customer.ImageIcon) || uploadfile == null || string.IsNullOrEmpty(uploadfile.FileName))
            {
                imageUrl = "/Images/avatars/avatar-no-image.png";
            }
            else
            {
                imageUrl = "/Media/Default/Customers/" + DateTime.Now.ToString("dd-MM-yyyy") + uploadfile.FileName;
            }
            var ngaySinh = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            var fullUrl = Server.MapPath("~/Media/Default/Customers/" + DateTime.Now.ToString("dd-MM-yyyy") + uploadfile.FileName);
            uploadfile.SaveAs(fullUrl);

            customer.FullName = fullName;
            customer.Email = email;
            customer.Address = address;
            customer.Birthday = ngaySinh;
            customer.CityId = city;
            customer.PhoneNumber = phoneNumber;
            customer.Sex = int.Parse(gender);
            customer.FilmTypeIds = selectFilmTypes;
            customer.CountryIds = selectCountries;
            customer.ImageIcon = imageUrl;
            customerService.Update(customer);
            redirect.Data = "Đã cập nhật thông tin tài khoản thành công.";
            redirect.Status = true;
            redirect.Url = Url.Action("UserProfile", "HomeCustomer");

            return Json(redirect);
        }

        [HttpPost, ValidateInput(false)]
        [Url("nguoi-dung/quen-mat-khau")]
        public ActionResult UserForgotPassword()
        {
            var redirect = new DataViewerModel();
            return Json(redirect);
        }

        [Url("nguoi-dung/doi-mat-khau")]
        public ActionResult UserChangePassword()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            var category = serviceCategory.GetById((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();

            return View("UserChangePassword");
        }

        [HttpPost, ValidateInput(false)]
        [Url("nguoi-dung/doi-mat-khau/cap-nhat")]
        public ActionResult UpdateChangePassword()
        {
            var redirect = new DataViewerModel();
            if (!IsLogin)
            {
                redirect.Data = "Bạn chưa đăng nhập.";
                redirect.Status = false;
                return Json(redirect);
            }

            var passwordOld = Request.Form["txtPasswordOld"];
            var passwordNew = Request.Form["txtPasswordNew"];
            var passwordConfirm = Request.Form["txttPasswordConfilm"];
            var captcha = Request.Form["txtCaptcha"];
            if (string.IsNullOrEmpty(captcha) || captcha.ToUpper() != GlobalCapcha.ToUpper())
            {
                redirect.Status = false;
                redirect.Data = "Mã xác thực không đúng.";
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(passwordOld))
            {
                redirect.Status = false;
                redirect.Data = "Mật khẩu cũ không để trống.";
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(passwordNew))
            {
                redirect.Status = false;
                redirect.Data = "Mật khẩu mới không để trống.";
                return Json(redirect);
            }

            if (passwordNew.Length <= 5)
            {
                redirect.Status = false;
                redirect.Data = "Mật khẩu mới độ dài tối đa 6 ký tự.";
                return Json(redirect);
            }

            if (passwordNew != passwordConfirm)
            {
                redirect.Status = false;
                redirect.Data = "Mật khẩu xác nhận không đúng.";
                return Json(redirect);
            }

            var service = WorkContext.Resolve<ICustomerService>();
            var status = service.ChangePassword(Utilities.RemoveInjection(UserName),
                EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, Utilities.RemoveInjection(passwordOld)), 
                EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, Utilities.RemoveInjection(passwordNew)));
            if (status != 0)
            {
                redirect.Status = true;
                redirect.Data = "Mật khẩu của bạn đã được đổi thành công.";
                var customerService = WorkContext.Resolve<ICustomerService>();
                var customer = customerService.GetById(UserId);
                SetCustomerState(customer);
            }
            else
            {
                redirect.Status = false;
                redirect.Data = "Mật khẩu cũ của bạn không đúng.";
            }

            return Json(redirect);
        }

        [Url("nguoi-dung/upload-videos")]
        public ActionResult UploadVideos()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            var category = serviceCategory.GetById((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = category.Name;
            ViewData[Extensions.Constants.HeaderDescription] = category.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = category.Tags;

            BuildModules();
            return View("CustomerUploadVideos");
        }

        [HttpPost, ValidateInput(false)]
        [Url("nguoi-dung/luu-tru-videos")]
        public ActionResult UserSaveVideos()
        {
            var redirect = new DataViewerModel();
            redirect.Data = "Thực hiện thất bại. Chưa hỗ trợ upload videos cho khách hàng.";
            redirect.Status = false;

            return Json(redirect);
        }
    }
}
