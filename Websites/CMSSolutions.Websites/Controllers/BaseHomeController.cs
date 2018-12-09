using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CMSSolutions.Configuration;
using CMSSolutions.Extensions;
using CMSSolutions.Net.Mail;
using CMSSolutions.Security.Cryptography;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Services;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace CMSSolutions.Websites.Controllers
{
    public class BaseHomeController : BaseController
    {
        #region Paged

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        #endregion

        #region User Login

        public string GlobalCapcha
        {
            get
            {
                if (Session[Extensions.Constants.GlobalCapcha] != null)
                {
                    return Session[Extensions.Constants.GlobalCapcha].ToString();
                }

                return string.Empty;
            }
        }

        public bool IsVip
        {
            get
            {
                if (CurrentDate != DateTime.Now.Date)
                {
                    var customer = WorkContext.Resolve<ICustomerService>().GetById(UserId);
                    SetCustomerState(customer);
                }

                if (EndDate >= DateTime.Now)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsLogin
        {
            get
            {
                return UserId > 0 && !string.IsNullOrEmpty(CustomerCode);
            }
        }

        public string CustomerCode
        {
            get
            {
                return GetCookies(Extensions.Constants.GlobalCustomerCode);
            }
        }

        public string UserName
        {
            get
            {
                return GetCookies(Extensions.Constants.GlobalUserName);
            }
        }

        public string FullName
        {
            get
            {
                return GetCookies(Extensions.Constants.GlobalFullName);
            }
        }

        public int UserId
        {
            get
            {
                var id = GetCookies(Extensions.Constants.GlobalUserId);
                if (!string.IsNullOrEmpty(id))
                {
                    return int.Parse(id);
                }

                return 0;
            }
        }

        public decimal VipXu
        {
            get
            {
                var xu = GetCookies(Extensions.Constants.GlobalVipXu);
                if (!string.IsNullOrEmpty(xu))
                {
                    return int.Parse(xu);
                }

                return 0;
            }
        }

        public int TotalDay
        {
            get
            {
                var day = GetCookies(Extensions.Constants.GlobalTotalDay);
                if (!string.IsNullOrEmpty(day))
                {
                    return int.Parse(day);
                }

                return 0;
            }
        }

        public DateTime StartDate
        {
            get
            {
                var date = GetCookies(Extensions.Constants.GlobalStartDate);
                if (!string.IsNullOrEmpty(date))
                {
                    return DateTime.Parse(date);
                }

                return Utilities.DateNull();
            }
        }

        public DateTime EndDate
        {
            get
            {
                var date = GetCookies(Extensions.Constants.GlobalEndDate);
                if (!string.IsNullOrEmpty(date))
                {
                    return DateTime.Parse(date);
                }

                return Utilities.DateNull();
            }
        }

        public int SiteId 
        { 
            get
            {
                if (Session[Extensions.Constants.GlobalSiteId] != null)
                {
                    return (int)Session[Extensions.Constants.GlobalSiteId];
                }

                return (int)Site.Home;
            } 
            set { Session[Extensions.Constants.GlobalSiteId] = value; }
        }

        public string TransactionCode
        {
            get
            {
                if (Session[Extensions.Constants.TransactionCode] != null)
                {
                    return Session[Extensions.Constants.TransactionCode].ToString();
                }

                return string.Empty;
            }
            set
            {
                Session[Extensions.Constants.TransactionCode] = value;
            }
        }

        public DateTime CurrentDate
        {
            get
            {
                if (Session[Extensions.Constants.CurrentDate] != null)
                {
                    return DateTime.Parse(Session[Extensions.Constants.CurrentDate].ToString());
                }

                return Utilities.DateNull();
            }
            set
            {
                Session[Extensions.Constants.CurrentDate] = value;
            }
        }

        public string UrlLogin
        {
            get
            {
                if (Session[Extensions.Constants.UrlLoginHistory] != null)
                {
                    return Session[Extensions.Constants.UrlLoginHistory].ToString();
                }

                return Url.Action("Index","Home");
            }
            set
            {
                Session[Extensions.Constants.UrlLoginHistory] = value;
            }
        }

        #endregion

        public BaseHomeController(IWorkContextAccessor workContextAccessor) 
            : base(workContextAccessor)
        {
           
        }

        public virtual ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect(UrlLogin);
        }

        #region Cookies
        public void AddCookies(string key, string value, bool isRemember)
        {
            var expiration = DateTime.Now;
            if (isRemember)
            {
                expiration = DateTime.Now.AddMonths(1);
            }
            else
            {
                expiration = DateTime.Now.AddDays(1);
            }
            var cookie = new HttpCookie(key, EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, value))
            {
                Expires = expiration,
                HttpOnly = true
            };

            Response.Cookies.Add(cookie);
        }

        public string GetCookies(string key)
        {
            HttpCookie authCookie = Request.Cookies[key];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                return EncryptionExtensions.Decrypt(KeyConfiguration.PublishKey, authCookie.Value);
            }

            return string.Empty;
        }

        public void RemoveCookies(string key)
        {
            if (Request.Cookies[key] != null)
            {
                var myCookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
                Response.Cookies.Add(myCookie);
            }
        }
        #endregion

        #region Customers
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Url("mang-xa-hoi")]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        [Url("dang-nhap")]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            GoogleClient.RewriteRequest();
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToLocal(returnUrl);
            }

            var service = WorkContext.Resolve<ICustomerService>();
            var cutomerCode = service.GetLatestCustomerCode();
            //var loginType = EnumExtensions.Parse<LoginTypes>(OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName);
            //string userName = cutomerCode;
            //if (loginType == LoginTypes.Google)
            //{
            //    userName = result.ExtraData["username"];
            //}
            var fullName = result.ExtraData["name"];
            var gender = Gender.None;
            try
            {
                if (result.ExtraData["gender"] == null)
                {
                    gender = EnumExtensions.Parse<Gender>(result.ExtraData["gender"]);
                }
            }
            catch (Exception)
            {

            }

            var loginId = result.ExtraData["id"];
            var email = result.UserName;
            if (!Utilities.ValidateUserName(email))
            {
                email = loginId + "@nodomain.vn";
                returnUrl = Url.Action("Index", "Home");
                return RedirectToLocal(returnUrl);
            }

            var customer = new CustomerInfo
            {
                UserName = email,
                Password = EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, Extensions.Constants.DefaultPassword),
                Email = email,
                FullName = fullName,
                Sex = (int)gender,
                MemberDate = DateTime.Now.Date,
                Birthday = Utilities.DateNull(),
                VipXu = 0,
                Status = (int)Status.Approved,
                IsBlock = false
            };

            customer.ImageIcon = gender == Gender.Male ? Extensions.Constants.ImageMale : Extensions.Constants.ImageFemale;
            var item = service.CheckRegister(customer.UserName);
            if (item == null)
            {
                customer.CustomerCode = cutomerCode;
                service.Insert(customer);
                var status = SendEmailRegister("Thông tin đăng nhập thành viên VIPHD.VN", customer.CustomerCode, customer.UserName, Extensions.Constants.DefaultPassword, customer.Email);
                if (!status)
                {
                    var user1 = service.Login(customer.UserName, customer.Password);
                    if (user1 != null)
                    {
                        SetCustomerState(user1);
                    }

                    return View("Messages", new MessageErrorModel
                    {
                        TitleForm = "Thông tin đăng nhập thành viên VIPHD.VN",
                        GoBackText = "Quay lại",
                        Messages = "Chúc mừng bạn, bạn đã đăng ký thành viên viphd.vn thành công. Chúng tôi không thể xác thực được email " + customer.Email + " của bạn hãy truy cập: http://viphd.vn/nguoi-dung/sua-thong-tin Tài khoản: " + customer.UserName + " Mật khẩu: " + Extensions.Constants.DefaultPassword
                    });
                }
            }
            else
            {
                customer.UserName = item.UserName;
                customer.Password = item.Password;
            }

            var user = service.Login(customer.UserName, customer.Password);
            if (user != null)
            {
                SetCustomerState(user);
                return RedirectToLocal(returnUrl);
            }

            return View("Messages", new MessageErrorModel
            {
                TitleForm = "Thông báo",
                GoBackText = "Quay lại",
                Messages = "Đăng nhập thất bại"
            });
        }

        [HttpPost, ValidateInput(false)]
        [Url("dang-ky-tai-khoan-vip")]
        public virtual ActionResult RegisterUser()
        {
            var email = Request.Form["txtEmail"];
            var password = Request.Form["txtPassword"];
            var comfim = Request.Form["txtComfim"];
            var fullName = Request.Form["txtFullName"];
            var captcha = Request.Form["txtCaptcha"];
            var result = new DataViewerModel();
            if (password != comfim)
            {
                result.Status = false;
                result.Data = "Mật khẩu xác nhận mật khẩu không đúng.";

                return Json(result);
            }

            if (string.IsNullOrEmpty(captcha) || captcha.ToUpper() != GlobalCapcha.ToUpper())
            {
                result.Status = false;
                result.Data = "Bạn nhập mã xác thực không đúng vui lòng nhập lại.";
                return Json(result);
            }

            var customer = new CustomerInfo
            {
                UserName = Utilities.RemoveInjection(email),
                Password = EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, Utilities.RemoveInjection(password)),
                Email = Utilities.RemoveInjection(email),
                FullName = Utilities.RemoveInjection(fullName),
                Sex = 1,
                MemberDate = DateTime.Now.Date,
                Birthday = Utilities.DateNull(),
                VipXu = 0,
                Status = (int)Status.Approved,
                IsBlock = false,
                ImageIcon = Extensions.Constants.ImageMale
            };

            if (!Utilities.ValidateUserName(customer.UserName))
            {
                result.Status = false;
                result.Data = "Tài khoản là email hoặc tên không dấu và không có ký tự đặc biệt.";

                return Json(result);
            }

            var service = WorkContext.Resolve<ICustomerService>();
            var item = service.CheckRegister(customer.UserName);
            if (item != null)
            {
                result.Status = false;
                result.Data = "Tài khoản đã tồn tại trên hệ thống vui lòng nhập tài khoản khác.";

                return Json(result);
            }

            var cutomerCode = service.GetLatestCustomerCode();
            customer.CustomerCode = cutomerCode;
            service.Insert(customer);
            var user = service.Login(customer.UserName, customer.Password);
            result.Status = true;
            result.Data = "Không tìm thấy tài khoản của bạn vui lòng liên hệ quản trị.";
            if (user != null)
            {
                SetCustomerState(user);
                var status = SendEmailRegister("Thông tin đăng nhập thành viên VIPHD.VN", customer.CustomerCode, customer.UserName, Utilities.RemoveInjection(password), customer.Email);
                if (status)
                {
                    result.Status = true;
                    result.Data = "Chúc mừng bạn, bạn đã đăng ký thành viên viphd.vn thành công.<br/> Chúng tôi đã gửi thông tin tài khoản vào email<br/> " + customer.Email + ".";
                }
                else
                {
                    result.Status = true;
                    result.Data = "Chúc mừng bạn, bạn đã đăng ký thành viên viphd.vn thành công.<br/> Chúng tôi không thể xác thực được email<br/> " + customer.Email + " của bạn hãy truy cập<br/> http://viphd.vn/nguoi-dung/sua-thong-tin <br/>mật khẩu: " + Utilities.RemoveInjection(password);
                }
            }

            return Json(result);
        }

        [HttpPost, ValidateInput(false)]
        [Url("quen-mat-khau-thanh-vien")]
        public virtual ActionResult ForgetPasswordMember()
        {
            var email = Request.Form["txtEmail"];
            var captcha = Request.Form["txtCaptcha"];
            var result = new DataViewerModel();

            if (string.IsNullOrEmpty(captcha) || captcha.ToUpper() != GlobalCapcha.ToUpper())
            {
                result.Status = false;
                result.Data = "Bạn nhập mã xác thực không đúng vui lòng nhập lại.";
                return Json(result);
            }

            if (!Utilities.IsEmailValid(email))
            {
                result.Status = false;
                result.Data = "Email không đúng định dạng hoặc có ký tự đặc biệt.<br/> Định dạng chuẩn VD: abc@viphd.vn";

                return Json(result);
            }

            var newpass = EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, Utilities.GenerateUniqueNumber());
            var service = WorkContext.Resolve<ICustomerService>();
            var item = service.GetMemberForgetPassword(newpass, email.ToLower());
            if (item == null)
            {
                result.Status = false;
                result.Data = "Không tồn tại tài khoản có email này trên hệ thống.<br/> Vui lòng kiểm tra lại email.";

                return Json(result);
            }

            result.Status = true;
            result.Data = "Hệ thống đã gửi lại email cho bạn thông tin tài khoản.<br/> Bạn vui lòng đăng nhập vào email để nhận lại mật khẩu<br/> sau đó quay lại để sử dụng tài khoản của bạn.";
            try
            {
                SendEmailForgetPassword("Quên mật khẩu đăng nhập VIPHD.VN", item.CustomerCode, item.UserName, EncryptionExtensions.Decrypt(KeyConfiguration.PublishKey, item.Password), item.Email);
            }
            catch (Exception)
            {
                result.Status = false;
                result.Data = "Xin lỗi bạn hệ thống không thể gửi email cho bạn.<br/> Vui lòng liên hệ quản trị để được hỗ trợ.";
            }

            return Json(result);
        }

        public void SetCustomerState(CustomerInfo customer, bool isRemember = true)
        {
            if (customer!= null)
            {
                AddCookies(Extensions.Constants.GlobalCustomerCode, customer.CustomerCode, isRemember);
                AddCookies(Extensions.Constants.GlobalUserName, customer.UserName, isRemember);
                AddCookies(Extensions.Constants.GlobalFullName, customer.FullName, isRemember);
                AddCookies(Extensions.Constants.GlobalUserId, customer.Id.ToString(), isRemember);
                AddCookies(Extensions.Constants.GlobalVipXu, customer.VipXu.ToString(), isRemember);
                AddCookies(Extensions.Constants.GlobalTotalDay, customer.TotalDay.ToString(), isRemember);
                AddCookies(Extensions.Constants.GlobalStartDate, customer.StartDate != null ? customer.StartDate.Value.ToString(Extensions.Constants.DateTimeFomatFull) : "01/01/1900", isRemember);
                AddCookies(Extensions.Constants.GlobalEndDate, customer.EndDate != null ? customer.EndDate.Value.ToString(Extensions.Constants.DateTimeFomatFull) : "01/01/1900", isRemember);
                CurrentDate = DateTime.Now.Date;
            }
        }

        [HttpPost, ValidateInput(false)]
        [Url("khach-hang-dang-nhap")]
        public virtual ActionResult CustomerLogin()
        {
            var email = Request.Form["txtEmail"];
            var password = Request.Form["txtPassword"];
            var remember = Request.Form["ckbRemember"];
            var result = new DataViewerModel();
            if (!Utilities.ValidateUserName(email))
            {
                result = new DataViewerModel
                {
                    Status = false,
                    Data = T("Tài khoản là email hoặc tên không dấu và không có ký tự đặc biệt.")
                };

                return Json(result);
            }

            var service = WorkContext.Resolve<ICustomerService>();
            var user = service.Login(Utilities.RemoveInjection(email),
                EncryptionExtensions.Encrypt(KeyConfiguration.PublishKey, Utilities.RemoveInjection(password)));
            if (user != null)
            {
                var isRemember = false;
                if (!string.IsNullOrEmpty(remember))
                {
                    isRemember = bool.Parse(remember.Trim().TrimEnd(','));
                }
                SetCustomerState(user, isRemember);
                result = new DataViewerModel
                {
                    Status = true,
                    Url = UrlLogin
                };

                return Json(result);
            }

            result = new DataViewerModel
            {
                Status = false,
                Data = "Người dùng không tồn tại. Vui lòng kiểm tra lại."
            };

            return Json(result);
        }

        [HttpPost, ValidateInput(false)]
        [Url("check-login")]
        public virtual ActionResult CheckLogin()
        {
            var result = new DataViewerModel();
            if (!IsLogin)
            {
                result = new DataViewerModel
                {
                    Status = false,
                    Data = ""
                };

                return Json(result);
            }

            result = new DataViewerModel
            {
                Status = true,
                Data = ""
            };

            return Json(result);
        }

        [HttpPost, ValidateInput(false)]
        [Url("get-vip-informations")]
        public virtual ActionResult GetVip()
        {
            var result = new DataViewerModel();
            if (!IsLogin || !IsVip)
            {
                result = new DataViewerModel
                {
                    Status = false
                };

                return Json(result);
            }

            result = new DataViewerModel
            {
                Status = IsVip
            };

            if (EndDate == Utilities.DateNull())
            {
                result.Data = string.Empty;
            }
            else
            {
                result.Data = EndDate.ToString(Extensions.Constants.DateTimeFomatFull2);
            }

            return Json(result);
        }

        [HttpGet]
        [Url("nguoi-dung/dang-xuat")]
        public virtual ActionResult UserLogout()
        {
            RemoveCookies(Extensions.Constants.GlobalCustomerCode);
            RemoveCookies(Extensions.Constants.GlobalUserName);
            RemoveCookies(Extensions.Constants.GlobalFullName);
            RemoveCookies(Extensions.Constants.GlobalUserId);
            RemoveCookies(Extensions.Constants.GlobalVipXu);
            RemoveCookies(Extensions.Constants.GlobalTotalDay);
            RemoveCookies(Extensions.Constants.GlobalStartDate);
            RemoveCookies(Extensions.Constants.GlobalEndDate);
            Session.RemoveAll();

            return RedirectToLocal(Url.Action("Index", "Home"));
        }
        #endregion

        #region News
        [HttpPost, ValidateInput(false)]
        [Url("list-news-by-category")]
        public virtual ActionResult GetNewsByCategory(string category)
        {
            var id = category == "all" ? 0 : int.Parse(category);
            var service = WorkContext.Resolve<IArticlesService>();
            service.LanguageCode = WorkContext.CurrentCulture;
            service.CategoryId = id;
            service.SiteId = SiteId;
            var data = service.BuildNewsByCategory();
            var html = new StringBuilder();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    var url = Url.Action("ViewDetails", "HomeNews", new { @alias = item.Alias, @id = item.Id });
                    html.Append("<li>");
                    html.Append("<p>");
                    html.Append("<a title='" + item.Title + "' href='" + url + "'>" + item.Title + "</a>");
                    if (item.ViewCount > 10)
                    {
                        html.Append(" <span class='icon-nnews'></span>");
                    }
                    html.Append("</p>");
                    html.Append("<span class='time-tem'>" + item.TextPublishedDate + "</span>");
                    html.Append("</li>");
                }
                html.Append("<li class='li-see_a-link'><a title='Xem thêm' href='" + Url.Action("Index", "HomeNews") + "'>Xem thêm >></a></li>");
            }
            var result = new DataViewerModel();
            result = new DataViewerModel
            {
                Status = true,
                Data = html.ToString()
            };

            return Json(result);
        }
        #endregion

        #region Capcha
        [HttpPost, ValidateInput(false)]
        [Url("capcha/auto-generate")]
        public virtual ActionResult CapchaGenerate(string token)
        {
            Session[Extensions.Constants.GlobalCapcha] = CaptchaImage.GenerateRandomCode();
            var capchaImage = new CaptchaImage(GlobalCapcha).GetCapchaImage();
            var result = new DataViewerModel
            {
                Status = true,
                Data = capchaImage
            };

            return Json(result);
        }
        #endregion

        #region Send Emails
        public bool SendEmailForgetPassword(string subject, string customerCode, string username, string password, string toEmailReceive)
        {
            try
            {
                string html = System.IO.File.ReadAllText(Server.MapPath("~/Media/Default/EmailTemplates/MemberForgetPassword.html"));
                html = html.Replace("[CODE]", customerCode);
                html = html.Replace("[EMAIL]", toEmailReceive.ToLower().Trim());
                html = html.Replace("[USERNAME]", username.ToLower().Trim());
                html = html.Replace("[PASSWORD]", password.Trim());
                SendEmail(subject, html, toEmailReceive, string.Empty);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendEmailRegister(string subject, string customerCode, string username, string password, string toEmailReceive)
        {
            try
            {
                string html = System.IO.File.ReadAllText(Server.MapPath("~/Media/Default/EmailTemplates/MemberRegister.html"));
                html = html.Replace("[CODE]", customerCode);
                html = html.Replace("[EMAIL]", toEmailReceive.ToLower().Trim());
                html = html.Replace("[USERNAME]", username.ToLower().Trim());
                html = html.Replace("[PASSWORD]", password.Trim());
                SendEmail(subject, html, toEmailReceive, "hotro@viphd.vn");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SendEmail(string subject, string body, string toEmailReceive, string ccEmail)
        {
            var service = WorkContext.Resolve<IEmailSender>();
            var mailMessage = new MailMessage
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            
            mailMessage.Sender = new MailAddress(toEmailReceive);
            mailMessage.To.Add(toEmailReceive);
            if (!string.IsNullOrEmpty(ccEmail))
            {
                mailMessage.CC.Add(ccEmail);
                mailMessage.Bcc.Add("api.viphd@gmail.com");
            }

            service.Send(mailMessage);
        }
        #endregion

        #region Films
        [HttpPost, ValidateInput(false)]
        [Url("list-films-by-type")]
        public virtual ActionResult GetFilmByType(int type, int statistical = 0)
        {
            var serviceFilms = WorkContext.Resolve<IFilmService>();
            serviceFilms.SiteId = SiteId;
            serviceFilms.LanguageCode = WorkContext.CurrentCulture;
            var result = new DataViewerModel{Status = false, Data = ""};
            switch ((HomeDisplayFilmType)type)
            {
                #region FilmHot
                case HomeDisplayFilmType.FilmHot:
                    {
                        var listFilmHot = serviceFilms.BuildFilmHot();
                        var html = new StringBuilder();
                        if (listFilmHot != null && listFilmHot.Count > 0)
                        {
                            var count = listFilmHot.Count % 8;
                            var total = listFilmHot.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {

                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1

                                var index = i - 7;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 2

                                index = i - 6;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 3

                                index = i - 5;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 4

                                index = i - 4;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 5

                                index = i - 3;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 6

                                index = i - 2;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 7

                                index = i - 1;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 8

                                index = i;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmHot[index], index, true));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            } 
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region FilmRetail
                case HomeDisplayFilmType.FilmRetail:
                    {
                        var listFilmRetail = serviceFilms.BuildFilmRetail();
                        var html = new StringBuilder();
                        if (listFilmRetail != null && listFilmRetail.Count > 0)
                        {
                            var count = listFilmRetail.Count % 8;
                            var total = listFilmRetail.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmRetail[index], index, false));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region FilmLengthEpisodes
                case HomeDisplayFilmType.FilmLengthEpisodes:
                    {
                        var listFilmLengthEpisodes = serviceFilms.BuildFilmManyEpisodes();
                        var html = new StringBuilder();
                        if (listFilmLengthEpisodes != null && listFilmLengthEpisodes.Count > 0)
                        {
                            var count = listFilmLengthEpisodes.Count % 8;
                            var total = listFilmLengthEpisodes.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region FilmJJChannelIntroduce
                case HomeDisplayFilmType.FilmJJChannelIntroduce:
                    {
                        serviceFilms.CategoryId = 0;
                        var listFilmJJChannelIntroduce = serviceFilms.BuildFilmJJChannelIntroduce();
                        var html = new StringBuilder();
                        if (listFilmJJChannelIntroduce != null && listFilmJJChannelIntroduce.Count > 0)
                        {
                            var count = listFilmJJChannelIntroduce.Count % 8;
                            var total = listFilmJJChannelIntroduce.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region FilmTheater
                case HomeDisplayFilmType.FilmTheater:
                    {
                        var listFilmTheater = serviceFilms.BuildFilmTheater();
                        var html = new StringBuilder();
                        if (listFilmTheater != null && listFilmTheater.Count > 0)
                        {
                            var count = listFilmTheater.Count % 8;
                            var total = listFilmTheater.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmTheater[index], index, false));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region TVShow
                case HomeDisplayFilmType.TVShow:
                    {
                        serviceFilms.CategoryId = 0;
                        var listTVShows = serviceFilms.BuildTVShow();
                        var html = new StringBuilder();
                        if (listTVShows != null && listTVShows.Count > 0)
                        {
                            var count = listTVShows.Count%2;
                            var total = listTVShows.Count - count;
                            for (int i = 1; i < total; i += 2)
                            {
                                var item = listTVShows[i - 1];
                                var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");
                                html.Append("<div class=\"slide_child_div_dt2col item\">");
                                html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
                                html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item.ImageIcon, item.FilmNameEnglish, item.FilmName);
                                html.Append("<p class=\"title_film_child\">");
                                html.AppendFormat("<span class=\"orange_color\">{0}</span>", item.FilmNameEnglish);
                                html.AppendFormat("<span>{0}</span>", item.FilmName);
                                html.Append("</p>");
                                html.AppendFormat("<span class=\"mask-num-film\">Tập <p>{0}</p></span>", item.EpisodeCount);
                                html.Append("</a>");

                                html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", i);
                                html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item.FilmNameEnglish) ? item.FilmName : item.FilmNameEnglish), url, item.FilmName);
                                html.Append("<div class=\"film-info\">");
                                html.AppendFormat("<span>Số tập: </span><label>{0}</label></br>", item.EpisodeCount);
                                html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item.FilmTypeNames);
                                html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item.CountryName);
                                html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item.Time);
                                html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item.Capacity);
                                html.Append("</div>");
                                html.Append("<div class=\"film-body\">");
                                html.AppendFormat("<p>{0}</p>", item.Summary);
                                html.Append("</div>");
                                html.Append("</div>");

                                html.Append("</div>");
                                html.Append("</li> ");

                                var item2 = listTVShows[i];
                                var url2 = Url.Action("FilmView", "HomeView", new { @alias = item2.FilmAlias, @id = item2.Id });
                                html.Append("<li>");
                                html.Append("<div class=\"slide_child_div_dt2col item\">");
                                html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url2, item2.FilmName);
                                html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item2.ImageIcon, item2.FilmNameEnglish, item2.FilmName);
                                html.Append("<p class=\"title_film_child\">");
                                html.AppendFormat("<span class=\"orange_color\">{0}</span>", item2.FilmNameEnglish);
                                html.AppendFormat("<span>{0}</span>", item2.FilmName);
                                html.Append("</p>");
                                html.AppendFormat("<span class=\"mask-num-film\">Tập <p>{0}</p></span>", item2.EpisodeCount);
                                html.Append("</a>");
                                
                                html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", i);
                                html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item2.FilmNameEnglish) ? item2.FilmName : item2.FilmNameEnglish), url2, item2.FilmName);
                                html.Append("<div class=\"film-info\">");
                                html.AppendFormat("<span>Số tập: </span><label>{0}</label></br>", item2.EpisodeCount);
                                html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item2.FilmTypeNames);
                                html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item2.CountryName);
                                html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item2.Time);
                                html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item2.Capacity);
                                html.Append("</div>");
                                html.Append("<div class=\"film-body\">");
                                html.AppendFormat("<p>{0}</p>", item2.Summary);
                                html.Append("</div>");
                                html.Append("</div>");

                                html.Append("</div>");
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var item = listTVShows[i];
                                    var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });
                                    if (total > 0)
                                    {
                                        item = listTVShows[total + i];
                                    }

                                    html.Append("<div class=\"slide_child_div_dt2col item\">");
                                    html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
                                    html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item.ImageIcon, item.FilmNameEnglish, item.FilmName);
                                    html.Append("<p class=\"title_film_child\">");
                                    html.AppendFormat("<span class=\"orange_color\">{0}</span>", item.FilmNameEnglish);
                                    html.AppendFormat("<span>{0}</span>", item.FilmName);
                                    html.Append("</p>");
                                    html.AppendFormat("<span class=\"mask-num-film\">Tập <p>{0}</p></span>", item.EpisodeCount);
                                    html.Append("</a>");

                                    html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", listTVShows.Count - 1);
                                    html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item.FilmNameEnglish) ? item.FilmName : item.FilmNameEnglish), url, item.FilmName);
                                    html.Append("<div class=\"film-info\">");
                                    html.AppendFormat("<span>Số tập: </span><label>{0}</label></br>", item.EpisodeCount);
                                    html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item.FilmTypeNames);
                                    html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item.CountryName);
                                    html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item.Time);
                                    html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item.Capacity);
                                    html.Append("</div>");
                                    html.Append("<div class=\"film-body\">");
                                    html.AppendFormat("<p>{0}</p>", item.Summary);
                                    html.Append("</div>");
                                    html.Append("</div>");
                                    html.Append("</div>");
                                }
                                
                                html.Append("</li> ");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region Clips
                case HomeDisplayFilmType.Clip:
                    {
                        serviceFilms.CategoryId = 5;
                        var listClips = serviceFilms.BuildClips();
                        var html = new StringBuilder();
                        if (listClips != null && listClips.Count > 0)
                        {
                            var count = listClips.Count%8;
                            var total = listClips.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");

                                var index = i - 7;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 6;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 5;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 4;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 3;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 2;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 1;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i;
                                html.Append(BuildItemClip(listClips[index], index));

                                html.Append("</li> ");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItemClip(listClips[index], index));
                                }
                                
                                html.Append("</li> ");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region Statistical FilmRetail
                case HomeDisplayFilmType.StatisticalFilmRetail:
                    {
                        var listStatisticalFilmRetail = serviceFilms.BuildStatistical1(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalFilmRetail != null && listStatisticalFilmRetail.Count > 0)
                        {
                            foreach (var item in listStatisticalFilmRetail)
                            {
                                html.Append(BuildItemStatistical(item));
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region Statistical FilmLengthEpisodes
                case HomeDisplayFilmType.StatisticalLengthEpisodes:
                    {
                        var listStatisticalFilmRetail = serviceFilms.BuildStatistical2(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalFilmRetail != null && listStatisticalFilmRetail.Count > 0)
                        {
                            foreach (var item in listStatisticalFilmRetail)
                            {
                                html.Append(BuildItemStatistical(item));
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region Statistical Shows
                case HomeDisplayFilmType.StatisticalShows:
                    {
                        var listStatisticalShows = serviceFilms.BuildStatistical3(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalShows != null && listStatisticalShows.Count > 0)
                        {
                            foreach (var item in listStatisticalShows)
                            {
                                html.Append(BuildItemStatistical(item));
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region Statistical Clips
                case HomeDisplayFilmType.StatisticalClips:
                    {
                        var listStatisticalClips = serviceFilms.BuildStatistical4(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalClips != null && listStatisticalClips.Count > 0)
                        {
                            foreach (var item in listStatisticalClips)
                            {
                                html.Append(BuildClip(item));
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region Statistical Trailer
                case HomeDisplayFilmType.StatisticalTrailer:
                    {
                        var listStatisticalTrailer = serviceFilms.BuildStatistical5(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalTrailer != null && listStatisticalTrailer.Count > 0)
                        {
                            foreach (var item in listStatisticalTrailer)
                            {
                                html.Append(BuildTrailer(item));
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion

                #region Statistical Trailer
                case HomeDisplayFilmType.StatisticalNextFilm:
                    {
                        var listStatisticalTrailer = serviceFilms.BuildStatistical5(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalTrailer != null && listStatisticalTrailer.Count > 0)
                        {
                            foreach (var item in listStatisticalTrailer)
                            {
                                html.Append(BuildTrailer(item));
                            }
                        }

                        result = new DataViewerModel
                        {
                            Status = true,
                            Data = html.ToString()
                        };
                    }
                    break;
                #endregion
            }
           
            return Json(result);
        }

        public string GetFilmHtml(int type, int statistical = 0)
        {
            var serviceFilms = WorkContext.Resolve<IFilmService>();
            serviceFilms.SiteId = SiteId;
            serviceFilms.LanguageCode = WorkContext.CurrentCulture;
            switch ((HomeDisplayFilmType)type)
            {
                #region FilmHot
                case HomeDisplayFilmType.FilmHot:
                    {
                        var listFilmHot = serviceFilms.BuildFilmHot();
                        var html = new StringBuilder();
                        if (listFilmHot != null && listFilmHot.Count > 0)
                        {
                            var count = listFilmHot.Count % 8;
                            var total = listFilmHot.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {

                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1

                                var index = i - 7;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 2

                                index = i - 6;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 3

                                index = i - 5;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 4

                                index = i - 4;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 5

                                index = i - 3;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 6

                                index = i - 2;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 7

                                index = i - 1;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                #region Item 8

                                index = i;
                                html.Append(BuildItem(listFilmHot[index], index, true));

                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmHot[index], index, true));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region FilmRetail
                case HomeDisplayFilmType.FilmRetail:
                    {
                        var listFilmRetail = serviceFilms.BuildFilmRetail();
                        var html = new StringBuilder();
                        if (listFilmRetail != null && listFilmRetail.Count > 0)
                        {
                            var count = listFilmRetail.Count % 8;
                            var total = listFilmRetail.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmRetail[index], index, false));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmRetail[index], index, false));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region FilmLengthEpisodes
                case HomeDisplayFilmType.FilmLengthEpisodes:
                    {
                        var listFilmLengthEpisodes = serviceFilms.BuildFilmManyEpisodes();
                        var html = new StringBuilder();
                        if (listFilmLengthEpisodes != null && listFilmLengthEpisodes.Count > 0)
                        {
                            var count = listFilmLengthEpisodes.Count % 8;
                            var total = listFilmLengthEpisodes.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmLengthEpisodes[index], index, true));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region FilmJJChannelIntroduce
                case HomeDisplayFilmType.FilmJJChannelIntroduce:
                    {
                        serviceFilms.CategoryId = 0;
                        var listFilmJJChannelIntroduce = serviceFilms.BuildFilmJJChannelIntroduce();
                        var html = new StringBuilder();
                        if (listFilmJJChannelIntroduce != null && listFilmJJChannelIntroduce.Count > 0)
                        {
                            var count = listFilmJJChannelIntroduce.Count % 8;
                            var total = listFilmJJChannelIntroduce.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmJJChannelIntroduce[index], index, true));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region FilmTheater
                case HomeDisplayFilmType.FilmTheater:
                    {
                        var listFilmTheater = serviceFilms.BuildFilmTheater();
                        var html = new StringBuilder();
                        if (listFilmTheater != null && listFilmTheater.Count > 0)
                        {
                            var count = listFilmTheater.Count % 8;
                            var total = listFilmTheater.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");

                                #region Item 1
                                var index = i - 7;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 2
                                index = i - 6;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 3
                                index = i - 5;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 4
                                index = i - 4;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 5
                                index = i - 3;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 6
                                index = i - 2;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 7
                                index = i - 1;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                #region Item 8
                                index = i;
                                html.Append(BuildItem(listFilmTheater[index], index, false));
                                #endregion

                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }

                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango fix-margin-li\" id=\"first-carousel\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItem(listFilmTheater[index], index, false));
                                }
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region TVShow
                case HomeDisplayFilmType.TVShow:
                    {
                        serviceFilms.CategoryId = 0;
                        var listTVShows = serviceFilms.BuildTVShow();
                        var html = new StringBuilder();
                        if (listTVShows != null && listTVShows.Count > 0)
                        {
                            var count = listTVShows.Count % 2;
                            var total = listTVShows.Count - count;
                            for (int i = 1; i < total; i += 2)
                            {
                                var item = listTVShows[i - 1];
                                var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");
                                html.Append("<div class=\"slide_child_div_dt2col item\">");
                                html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
                                html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item.ImageIcon, item.FilmNameEnglish, item.FilmName);
                                html.Append("<p class=\"title_film_child\">");
                                html.AppendFormat("<span class=\"orange_color\">{0}</span>", item.FilmNameEnglish);
                                html.AppendFormat("<span>{0}</span>", item.FilmName);
                                html.Append("</p>");
                                html.AppendFormat("<span class=\"mask-num-film\">Tập <p>{0}</p></span>", item.EpisodeCount);
                                html.Append("</a>");

                                html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", i);
                                html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item.FilmNameEnglish) ? item.FilmName : item.FilmNameEnglish), url, item.FilmName);
                                html.Append("<div class=\"film-info\">");
                                html.AppendFormat("<span>Số tập: </span><label>{0}</label></br>", item.EpisodeCount);
                                html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item.FilmTypeNames);
                                html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item.CountryName);
                                html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item.Time);
                                html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item.Capacity);
                                html.Append("</div>");
                                html.Append("<div class=\"film-body\">");
                                html.AppendFormat("<p>{0}</p>", item.Summary);
                                html.Append("</div>");
                                html.Append("</div>");

                                html.Append("</div>");
                                html.Append("</li> ");

                                var item2 = listTVShows[i];
                                var url2 = Url.Action("FilmView", "HomeView", new { @alias = item2.FilmAlias, @id = item2.Id });
                                html.Append("<li>");
                                html.Append("<div class=\"slide_child_div_dt2col item\">");
                                html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url2, item2.FilmName);
                                html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item2.ImageIcon, item2.FilmNameEnglish, item2.FilmName);
                                html.Append("<p class=\"title_film_child\">");
                                html.AppendFormat("<span class=\"orange_color\">{0}</span>", item2.FilmNameEnglish);
                                html.AppendFormat("<span>{0}</span>", item2.FilmName);
                                html.Append("</p>");
                                html.AppendFormat("<span class=\"mask-num-film\">Tập <p>{0}</p></span>", item2.EpisodeCount);
                                html.Append("</a>");

                                html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", i);
                                html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item2.FilmNameEnglish) ? item2.FilmName : item2.FilmNameEnglish), url2, item2.FilmName);
                                html.Append("<div class=\"film-info\">");
                                html.AppendFormat("<span>Số tập: </span><label>{0}</label></br>", item2.EpisodeCount);
                                html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item2.FilmTypeNames);
                                html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item2.CountryName);
                                html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item2.Time);
                                html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item2.Capacity);
                                html.Append("</div>");
                                html.Append("<div class=\"film-body\">");
                                html.AppendFormat("<p>{0}</p>", item2.Summary);
                                html.Append("</div>");
                                html.Append("</div>");

                                html.Append("</div>");
                                html.Append("</li>");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var item = listTVShows[i];
                                    var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });
                                    if (total > 0)
                                    {
                                        item = listTVShows[total + i];
                                    }

                                    html.Append("<div class=\"slide_child_div_dt2col item\">");
                                    html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
                                    html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item.ImageIcon, item.FilmNameEnglish, item.FilmName);
                                    html.Append("<p class=\"title_film_child\">");
                                    html.AppendFormat("<span class=\"orange_color\">{0}</span>", item.FilmNameEnglish);
                                    html.AppendFormat("<span>{0}</span>", item.FilmName);
                                    html.Append("</p>");
                                    html.AppendFormat("<span class=\"mask-num-film\">Tập <p>{0}</p></span>", item.EpisodeCount);
                                    html.Append("</a>");

                                    html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", listTVShows.Count - 1);
                                    html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item.FilmNameEnglish) ? item.FilmName : item.FilmNameEnglish), url, item.FilmName);
                                    html.Append("<div class=\"film-info\">");
                                    html.AppendFormat("<span>Số tập: </span><label>{0}</label></br>", item.EpisodeCount);
                                    html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item.FilmTypeNames);
                                    html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item.CountryName);
                                    html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item.Time);
                                    html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item.Capacity);
                                    html.Append("</div>");
                                    html.Append("<div class=\"film-body\">");
                                    html.AppendFormat("<p>{0}</p>", item.Summary);
                                    html.Append("</div>");
                                    html.Append("</div>");
                                    html.Append("</div>");
                                }

                                html.Append("</li> ");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region Clips
                case HomeDisplayFilmType.Clip:
                    {
                        serviceFilms.CategoryId = 5;
                        var listClips = serviceFilms.BuildClips();
                        var html = new StringBuilder();
                        if (listClips != null && listClips.Count > 0)
                        {
                            var count = listClips.Count % 8;
                            var total = listClips.Count - count;
                            for (int i = 7; i < total; i += 8)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");

                                var index = i - 7;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 6;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 5;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 4;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 3;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 2;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i - 1;
                                html.Append(BuildItemClip(listClips[index], index));

                                index = i;
                                html.Append(BuildItemClip(listClips[index], index));

                                html.Append("</li> ");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                            if (count > 0)
                            {
                                html.Append("<div>");
                                html.Append("<ul class=\"first-and-second-carousel jcarousel-skin-tango member-item\">");
                                html.Append("<li>");
                                for (int i = 0; i < count; i++)
                                {
                                    var index = i;
                                    if (total > 0)
                                    {
                                        index = total + i;
                                    }
                                    html.Append(BuildItemClip(listClips[index], index));
                                }

                                html.Append("</li> ");
                                html.Append("</ul>");
                                html.Append("</div>");
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region Statistical FilmRetail
                case HomeDisplayFilmType.StatisticalFilmRetail:
                    {
                        var listStatisticalFilmRetail = serviceFilms.BuildStatistical1(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalFilmRetail != null && listStatisticalFilmRetail.Count > 0)
                        {
                            foreach (var item in listStatisticalFilmRetail)
                            {
                                html.Append(BuildItemStatistical(item));
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region Statistical FilmLengthEpisodes
                case HomeDisplayFilmType.StatisticalLengthEpisodes:
                    {
                        var listStatisticalFilmRetail = serviceFilms.BuildStatistical2(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalFilmRetail != null && listStatisticalFilmRetail.Count > 0)
                        {
                            foreach (var item in listStatisticalFilmRetail)
                            {
                                html.Append(BuildItemStatistical(item));
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region Statistical Shows
                case HomeDisplayFilmType.StatisticalShows:
                    {
                        var listStatisticalShows = serviceFilms.BuildStatistical3(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalShows != null && listStatisticalShows.Count > 0)
                        {
                            foreach (var item in listStatisticalShows)
                            {
                                html.Append(BuildItemStatistical(item));
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region Statistical Clips
                case HomeDisplayFilmType.StatisticalClips:
                    {
                        var listStatisticalClips = serviceFilms.BuildStatistical4(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalClips != null && listStatisticalClips.Count > 0)
                        {
                            foreach (var item in listStatisticalClips)
                            {
                                html.Append(BuildClip(item));
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region Statistical Trailer
                case HomeDisplayFilmType.StatisticalTrailer:
                    {
                        var listStatisticalTrailer = serviceFilms.BuildStatistical5(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalTrailer != null && listStatisticalTrailer.Count > 0)
                        {
                            foreach (var item in listStatisticalTrailer)
                            {
                                html.Append(BuildTrailer(item));
                            }
                        }

                        return html.ToString();
                    }
                #endregion

                #region Statistical Trailer
                case HomeDisplayFilmType.StatisticalNextFilm:
                    {
                        var listStatisticalTrailer = serviceFilms.BuildStatistical5(30, statistical);
                        var html = new StringBuilder();
                        if (listStatisticalTrailer != null && listStatisticalTrailer.Count > 0)
                        {
                            foreach (var item in listStatisticalTrailer)
                            {
                                html.Append(BuildTrailer(item));
                            }
                        }

                        return html.ToString();
                    }
                #endregion
            }

            return string.Empty;
        }

        [HttpPost, ValidateInput(false)]
        [Url("list-films-next")]
        public virtual ActionResult GetNextFilm(long filmId, string categoryIds, bool isShow, bool isClip, bool isTrailer,bool isFilm)
        {
            var service = WorkContext.Resolve<ISearchService>();
            service.SiteId = (int)Site.Home;
            var condition = new List<SearchCondition>
            {
                new SearchCondition(new[]
                {
                    SearchField.Title.ToString(),
                    SearchField.CategoryIds.ToString()
                }, categoryIds)
            };

            if (isShow)
            {
                condition.Add(new SearchCondition(new[]
                {
                    SearchField.IsShow.ToString()
                }, isShow.ToString())); 
            }

            if (isClip)
            {
                condition.Add(new SearchCondition(new[]
                {
                    SearchField.IsClip.ToString()
                }, isClip.ToString()));
            }

            if (isTrailer)
            {
                condition.Add(new SearchCondition(new[]
                {
                    SearchField.IsTrailer.ToString()
                }, isTrailer.ToString()));
            }

            if (isFilm)
            {
                condition.Add(new SearchCondition(new[]
                {
                    SearchField.IsFilm.ToString()
                }, isFilm.ToString()));
            }

            var total = 0;
            var data = service.Search(condition, 1, 40, ref total);
            var html = new StringBuilder();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    if (item.SearchId == filmId.ToString())
                    {
                        continue;
                    }

                    html.Append(BuildFilmSearch(item));
                }
            }

            var result = new DataViewerModel
            {
                Status = true,
                Data = html.ToString()
            };

            return Json(result);
        }

        public StringBuilder BuildItemStatistical(FilmInfo item)
        {
            var html = new StringBuilder();
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });
            html.Append("<div class=\"list_category-child\">");
            html.Append("<div class=\"list_category-img\">");
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\">", item.FilmName, url);
            html.AppendFormat("<img alt=\"{0}\" width=\"106\" height=\"80\" src=\"{1}\">",item.FilmAlias, item.ImageIcon);
            html.Append("</a>");
            html.Append("</div>");
            html.Append("<div class=\"list_category-txt\">");
            html.Append("<p class=\"name-en\">");
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", item.FilmNameEnglish, url, item.FilmNameEnglish);
            html.Append("</p>");
            html.Append("<p class=\"name-vi\">");    
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", item.FilmName, url, item.FilmName);    
            html.Append("</p>");
            html.AppendFormat("<p>Lượt xem phim: {0}</p>", item.ViewCount);    
            html.Append("</div>");
            html.Append("<br class=\"clear_both\">");
            html.Append("</div>"); 
            return html;
        }

        public StringBuilder BuildItem(FilmInfo item, int index, bool showEpisodes)
        {
            var html = new StringBuilder();
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });
            html.Append("<div class=\"slide_child_div_dt item\">");
            html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
            html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item.ImageIcon.Replace("'", string.Empty), item.FilmNameEnglish, item.FilmName);
            html.Append("<p class=\"title_film_child\">");
            html.AppendFormat("<span class=\"orange_color\">{0}</span>", item.FilmNameEnglish);
            html.AppendFormat("<span>{0}</span>", item.FilmName);
            html.Append("</p>");
            if (showEpisodes && item.IsFilmLengthEpisodes)
            {
                html.AppendFormat("<span class=\"mask-num-film\">Tập <p>{0}</p></span>", item.EpisodeCount);
            }
            html.Append("</a>");

            html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", index);
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item.FilmNameEnglish) ? item.FilmName : item.FilmNameEnglish), url, item.FilmName);
            html.Append("<div class=\"film-info\">");
            if (showEpisodes && item.IsFilmLengthEpisodes)
            {
                html.AppendFormat("<span>Số tập: </span><label>{0}</label></br>", item.EpisodeCount);
            }
            html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item.FilmTypeNames);
            html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item.CountryName);
            html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item.Time);
            html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item.Capacity);
            html.Append("</div>");
            html.Append("<div class=\"film-body\">");
            html.AppendFormat("<p>{0}</p>", item.Summary.Replace("'", string.Empty));
            html.Append("</div>");
            html.Append("</div>");

            html.Append("</div>");
            return html;
        }

        private StringBuilder BuildItemClip(FilmInfo item, int index)
        {
            var html = new StringBuilder();
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });

            html.Append("<div class=\"slide_childs item\">");
            html.AppendFormat("<a href=\"{0}\" title=\"{1}\">", url, item.FilmName);
            html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\">", item.ImageIcon, item.FilmNameEnglish, item.FilmName);
            html.Append("<p class=\"title_film_child\">");
            html.AppendFormat("<span class=\"orange_color\">{0}</span>", item.FilmNameEnglish);
            html.AppendFormat("<span>{0}</span>", item.FilmName);
            html.Append("</p>");
            html.Append("</a>");

            html.AppendFormat("<div class=\"tooltip_description\" style=\"display:none\" title=\"Item {0} Description\">", index);
            html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{2}</a>", (string.IsNullOrEmpty(item.FilmNameEnglish) ? item.FilmName : item.FilmNameEnglish), url, item.FilmName);
            html.Append("<div class=\"film-info\">");
            html.AppendFormat("<span>Thể loại phim: </span><label>{0}</label></br>", item.FilmTypeNames);
            html.AppendFormat("<span>Quốc gia: </span><label>{0}</label></br>", item.CountryName);
            html.AppendFormat("<span>Thời lượng: </span><label>{0}</label></br>", item.Time);
            html.AppendFormat("<span>Dung lượng: </span><label>{0}</label>", item.Capacity);
            html.Append("</div>");
            html.Append("<div class=\"film-body\">");
            html.AppendFormat("<p>{0}</p>", item.Summary);
            html.Append("</div>");
            html.Append("</div>");

            html.Append("</div>");
            return html;
        }

        private StringBuilder BuildClip(FilmInfo item)
        {
            var html = new StringBuilder();
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });

            html.Append("<div class=\"list_category-child\">");
            html.Append("<div class=\"list_category-img\">");
            html.AppendFormat("<a href=\"{0}\">", url);
            html.AppendFormat("<img alt=\"{0}\" width=\"106\" height=\"80\" src=\"{1}\">", item.FilmName, item.ImageIcon);
            html.Append("</a>");
            html.Append("</div>");
            html.Append("<div class=\"list_category-txt\">");
            html.AppendFormat("<p class=\"name-en\"><a href=\"{0}\">{1}</a></p>", url, item.FilmNameEnglish);
            html.AppendFormat("<p class=\"name-vi\"><a href=\"{0}\">{1}</a></p>", url, item.FilmName);
            html.AppendFormat("<p>Tổng lượt xem: {0}</p>", item.ViewCount);
            html.Append("</div>");
            html.Append("<br class=\"clear_both\">");
            html.Append("</div>");

            return html;
        }

        private StringBuilder BuildTrailer(FilmInfo item)
        {
            var html = new StringBuilder();
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.FilmAlias, @id = item.Id });

            html.Append("<div class=\"list_category-child\">");
            html.Append("<div class=\"list_category-img\">");
            html.AppendFormat("<a href=\"{0}\">", url);
            html.AppendFormat("<img alt=\"{0}\" width=\"106\" height=\"80\" src=\"{1}\">", item.FilmName, item.ImageIcon);
            html.Append("</a>");
            html.Append("</div>");
            html.Append("<div class=\"list_category-txt\">");
            html.AppendFormat("<p class=\"name-en\"><a href=\"{0}\">{1}</a></p>", url, item.FilmNameEnglish);
            html.AppendFormat("<p class=\"name-vi\"><a href=\"{0}\">{1}</a></p>", url, item.FilmName);
            html.Append("</div>");
            html.Append("<br class=\"clear_both\">");
            html.Append("</div>");

            return html;
        }

        private StringBuilder BuildFilmSearch(SearchInfo item)
        {
            var html = new StringBuilder();
            var url = Url.Action("FilmView", "HomeView", new { @alias = item.Alias, @id = item.SearchId });

            html.Append("<div class=\"list_category-child\">");
            html.Append("<div class=\"list_category-img\">");
            html.AppendFormat("<a href=\"{0}\">", url);
            html.AppendFormat("<img alt=\"{0}\" width=\"106\" height=\"80\" src=\"{1}\">", item.Title, item.Images);
            html.Append("</a>");
            html.Append("</div>");
            html.Append("<div class=\"list_category-txt\">");
            html.AppendFormat("<p class=\"name-en\"><a href=\"{0}\">{1}</a></p>", url, item.TitleEnglish);
            html.AppendFormat("<p class=\"name-vi\"><a href=\"{0}\">{1}</a></p>", url, item.Title);
            html.AppendFormat("<p>Lượt xem: {0}</p>", item.ViewCount);
            html.Append("</div>");
            html.Append("<br class=\"clear_both\">");
            html.Append("</div>");

            return html;
        }
        #endregion

        public virtual void BuildModules()
        {

        }

        #region Sliders
        public virtual string BuildSlider(int siteId, int pageId)
        {
            var service = WorkContext.Resolve<ISliderService>();
            var seviceFilm = WorkContext.Resolve<IFilmService>();
            seviceFilm.LanguageCode = WorkContext.CurrentCulture;
            seviceFilm.SiteId = siteId;
            service.LanguageCode = WorkContext.CurrentCulture;
            service.SiteId = siteId;
            var list = service.GetCacheByPageId(pageId);

            var html = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                html.Append("<div class=\"slider-home\" id=\"slider1_container\">");
                html.Append("<div class=\"slider-border\" u=\"slides\">");
                foreach (var slider in list)
                {
                    var film = seviceFilm.GetFilmDetails(slider.FilmId);
                    if (film != null)
                    {
                        var url = Url.Action("FilmView", "HomeView", new { @alias = film.FilmAlias, @id = film.Id });

                        html.Append("<div onmouseout=\"javascript: event_mouseout();\" onmouseover=\"javascript: event_mouseover();\">");
                        html.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" u=\"image\" />", film.ImageThumb, film.FilmAlias, film.FilmNameEnglish);
                        html.Append("<div class=\"caption\">");
                        html.Append("<h3>");
                        html.AppendFormat("<a title=\"{0}\" href=\"{1}\">{0}</a>", film.FilmNameEnglish, url);
                        html.Append("</h3>");
                        html.AppendFormat("<h4>{0}</h4>", film.FilmName);
                        html.Append("<p class=\"description_detail_p\">");
                        html.Append(film.Summary);
                        html.AppendFormat("<a href=\"{0}\" style=\"color:#e74c3c\">Xem thêm</a>", url);
                        html.Append("</p>");
                        html.Append("<ul class=\"description_detail_ul\">");
                        html.AppendFormat("<li><span>Quốc gia</span> {0}</li>", film.CountryName);
                        html.AppendFormat("<li><span>Thể loại</span> {0}</li>", film.FilmTypeNames);
                        html.AppendFormat("<li><span>Diễn viên</span> {0}</li>", film.ActorNames);
                        html.AppendFormat("<li><span>Thời lượng</span> {0}</li>", film.Time);
                        html.Append("</ul>");
                        html.AppendFormat("<a href=\"{0}\" class=\"see_a-link\">Xem phim</a>", url);
                        html.Append("</div>");
                        html.Append("</div>");
                    }
                }

                html.Append("</div>");
                html.Append("<div style=\"bottom: 16px; right: 6px;\" class=\"jssorb03\" u=\"navigator\">");
                html.Append("<div u=\"prototype\">");
                html.Append("<div u=\"numbertemplate\">&nbsp;</div>");
                html.Append("</div>");
                html.Append("</div>");
                html.Append("<span style=\"top: 123px; left: 8px;\" class=\"jssora20l\" onmouseout=\"javascript: event_mouseout();\" onmouseover=\"javascript: event_mouseover();\" u=\"arrowleft\"></span>");
                html.Append("<span style=\"top: 123px; right: 8px;\" class=\"jssora20r\" onmouseout=\"javascript: event_mouseout();\" onmouseover=\"javascript: event_mouseover();\" u=\"arrowright\"></span>");
                html.Append("</div>");
            }
             
            return html.ToString();
        }
        #endregion

        [HttpPost, ValidateInput(false)]
        [Url("get-film-details")]
        public ActionResult FilmDetails()
        {
            var model = new DataViewCategoryModel { HtmlData = string.Empty };
            if (!string.IsNullOrEmpty(Request.Form["txtFilmId"]))
            {
                var id = long.Parse(Request.Form["txtFilmId"]);
                var service = WorkContext.Resolve<IFilmService>();
                service.LanguageCode = WorkContext.CurrentCulture;
                service.SiteId = (int) Site.Home;
                var obj = service.GetFilmDetails(id);
                var html = new StringBuilder();
                if (obj != null)
                {
                    var url = Url.Action("FilmView", "HomeView", new {@alias = obj.FilmAlias, @id = obj.Id});
                    html.Append("<p class=\"name-en\">");
                    html.AppendFormat("<a href=\"{0}\" title=\"{1}\">{1}</a>", url, obj.FilmNameEnglish);
                    html.Append("</p>");

                    html.Append("<p class=\"name-vi\">");
                    html.AppendFormat("<a href=\"{0}\" title=\"{1}\">{1}</a>", url, obj.FilmName);
                    html.Append("</p>");
                    html.AppendFormat("<p>{0}</p>", obj.Summary);

                    html.Append("<ul>");
                    html.AppendFormat("<li><span>Năm phát hành:</span> {0}</li>", obj.ReleaseYear);
                    html.AppendFormat("<li><span>Đạo diễn:</span> {0}</li>", obj.DirectorName);
                    html.AppendFormat("<li><span>Diễn viên:</span> {0}</li>", obj.ActorNames);
                    html.AppendFormat("<li><span>Quốc gia:</span> {0}</li>", obj.CountryName);
                    html.AppendFormat("<li><span>Thể loại:</span> {0}</li>", obj.CategoryNames);
                    html.AppendFormat("<li><span>IMDB:</span> {0}</li>", obj.Capacity);
                    html.AppendFormat("<li><span>Liên quan (tag):</span> {0}</li>", obj.Tags);
                    html.Append("</ul>");

                    html.AppendFormat("<a href=\"{0}\" class=\"see_a-link\">Xem phim<span></span></a>", url);
                    html.Append("<br class=\"clear_both\">");
                }

                model.HtmlData = html.ToString();
            }
            
            return Json(model);
        }

        [HttpPost, ValidateInput(false)]
        [Url("get-data/tim-kiem")]
        public ActionResult Search()
        {
            var keyword = Request.Form["keyword"];
            var service = WorkContext.Resolve<ISearchService>();
            service.SiteId = (int)Site.Home;
            var condition = new List<SearchCondition>
            {
                new SearchCondition(new[]
                {
                    SearchField.Title.ToString(), 
                    SearchField.Keyword.ToString(), 
                    SearchField.KeywordEN.ToString()
                }, keyword)
            };

            var total = 0;
            var data = service.Search(condition, 1, 40, ref total);
            return Json(data);
        }

        #region Rates
        [HttpPost, ValidateInput(false)]
        [Url("danh-gia-bao-loi-phim")]
        public ActionResult RateFilm(int type, long filmId, int rate, int title, string messsages)
        {
            var result = new DataViewerModel();
            if (!IsLogin)
            {
                result = new DataViewerModel
                {
                    Status = false,
                    Data = "Bạn chưa đăng nhập."
                };

                return Json(result);
            }

            var service = WorkContext.Resolve<IRateService>();
            var entity = service.GetByFilmCustomer(filmId, CustomerCode) ?? new RateInfo();
            switch (type)
            {
                case 1: // Đánh giá
                    entity.Rate += rate;
                    entity.Messages = GetRateMessages(rate) + ", ";
                    result.Status = true;
                    result.Data = "Cảm ơn bạn đã đánh giá viphd chúc các bạn xem phim vui vẻ.";
                    var filmService = WorkContext.Resolve<IFilmService>();
                    filmService.UpdateCommentCount(filmId);
                    break;
                case 2: //Báo giật lag
                    entity.AlertLag = messsages + ", ";
                    entity.Messages = "Phim này đang bị khách hàng thông báo giật lag.";
                    result.Status = true;
                    result.Data = "Thông báo lỗi của bạn đã được gửi đi. Chúng tôi đang kiểm tra lại. Cảm ơn bạn đã ủng hộ và hỗ trợ viphd nhé.";
                    break;
                case 3: //Báo lỗi
                    entity.AlertError = EnumExtensions.GetDisplayName((AlertErrorVideo) title);
                    entity.Messages += messsages + ", ";
                    result.Status = true;
                    result.Data = "Thông báo lỗi của bạn đã được gửi đi. Chúng tôi đang kiểm tra lại . Cảm ơn bạn đã ủng hộ và hỗ trợ viphd nhé.";
                    break;
                case 4: //Thích phim
                    entity.LikeFilm = true;
                    entity.Messages = messsages + ", ";
                    result.Status = false;
                    result.Data = "";
                    break;
            }

            entity.Status = 1;
            entity.LanguageCode = WorkContext.CurrentCulture;
            entity.SiteId = (int)Site.Home;
            entity.CustomerId = UserId;
            entity.CustomerCode = CustomerCode;
            entity.FilmId = filmId;

            service.Save(entity);
            return Json(result);
        }

        private string GetRateMessages(int rate) {
            var text = string.Empty;
            if (rate == 1) {
                text = "Hay Chết Liền";
            }

            if (rate == 2) {
                text = "Hay chỗ nào";
            }

            if (rate == 3) {
                text = "Ai Bảo Hay";
            }

            if (rate == 4) {
                text = "Xém nữa thì Hay";
            }

            if (rate == 5) {
                text = "Có vẻ Hay";
            }

            if (rate == 6) {
                text = "Ờ Cũng Hay";
            }

            if (rate == 7) {
                text = "Hơi bị Hay";
            }

            if (rate == 8) {
                text = "Quá Là Hay";
            }

            if (rate == 9) {
                text = "Hay Quá Xá";
            }

            if (rate == 10) {
                text = "Tuyệt đỉnh Hay";
            }

            return text;
        }

        [HttpPost, ValidateInput(false)]
        [Url("download-games")]
        public virtual ActionResult CheckDownload(int id)
        {
            var result = new DataViewerModel();
            result.Status = true;
            result.Data = "Bạn chưa đăng nhập.";
            if (IsLogin)
            {
                result.Data = "Nhiệm vụ đang chờ bạn thực hiện.";
                var service = WorkContext.Resolve<IDownloadGameService>();
                var obj = service.GetById(id);
                if (obj != null)
                {
                    var customerService = WorkContext.Resolve<ICustomerService>();
                    var status = customerService.AddVipXu(UserId, obj.Id, obj.VipXu);
                    if (status == 1)
                    {
                        result.Data = "Bạn cần tải về, cài đặt và mở ứng dụng để nhận được " + obj.VipXu + " VIPXU.";
                    }
                    else
                    {
                        var downloadService = WorkContext.Resolve<IDownloadCustomerService>();
                        var item = downloadService.GetItem(UserId, id);
                        if (item != null)
                        {
                            var displayDay = Utilities.GetStatusDownload(item);
                            result.Data = "Bạn cần tải về, cài đặt và mở ứng dụng để nhận được " + obj.VipXu + " VIPXU (sau " + displayDay + ")";
                        }
                    }
                }
            }

            return Json(result);
        }
        #endregion
    }
}
