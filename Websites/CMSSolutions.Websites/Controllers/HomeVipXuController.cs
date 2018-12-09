using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using CMSSolutions.ContentManagement.Widgets.Services;
using CMSSolutions.DisplayManagement;
using CMSSolutions.Extensions;
using CMSSolutions.Web.Mvc;
using CMSSolutions.Web.Themes;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Models;
using CMSSolutions.Websites.Payments;
using CMSSolutions.Websites.Services;
using CMSSolutions.Websites.VipHDBankCardServices;

namespace CMSSolutions.Websites.Controllers
{
    [Themed(IsDashboard = false)]
    public class HomeVipXuController : BaseHomeController
    {
        private readonly dynamic shapeFactory;
        public HomeVipXuController(IWorkContextAccessor workContextAccessor, IShapeFactory shapeFactory) 
            : base(workContextAccessor)
        {
            this.shapeFactory = shapeFactory;
        }

        [Url("tai-tro.html")]
        public ActionResult Index()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var model = new PaymentModel();
            model.PageName = 1;
            if (!IsLogin)
            {
                return Redirect(Url.Action("Index", "Home"));
            }

            var customerService = WorkContext.Resolve<ICustomerService>();
            CustomerInfo customer = customerService.GetCustomerByCacheId(UserId);
            SetCustomerState(customer);

            #region Payments
            var transid = Request.QueryString["?transid"];
            var responCode = Request.QueryString["responCode"];
            var mac = Request.QueryString["mac"];
            var amount = Request.QueryString["amount"];
            var bankCode = Request.QueryString["bankCode"];

            var encrypter = new DesSecurity();
            var apiBank = new APIBankCardService();
            var transactionBankService = WorkContext.Resolve<ITransactionBankService>();
            var serviceLog = new APISmsService();
            if (mac != encrypter.DESMAC(transid + responCode, apiBank.ReceiverKey) && !String.IsNullOrEmpty(transid))
            {
                model.Messages = "Giao dịch nạp VIPXU qua thẻ ngân hàng thất bại.";
                model.Status = false;
                model.Url = "";
                var transaction = new TransactionBankInfo
                {
                    CustomerCode = CustomerCode,
                    TransactionCode = TransactionCode,
                    CreateDate = DateTime.Now,
                    BankCode = bankCode,
                    Amount = amount,
                    ResponCode = responCode,
                    Mac = mac,
                    TransId = transid,
                    Type = (int)TransferType.Rereceive,
                    Description = "Giao dịch nạp VIPXU qua thẻ ngân hàng thất bại.",
                    Status = (int)PaymentStatus.Close
                };

                transactionBankService.BankProcessing(transaction, 0);
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Giao dịch nạp VIPXU qua thẻ ngân hàng thất bại.",
                    Keyword = bankCode,
                    Contents = "Mã GD: " + TransactionCode + " - Mã KH: " + CustomerCode + " - Số tiền nạp: " + amount,
                    Type = (int)LogType.BankCard,
                    Status = (int)LogStatus.Error
                });
                TransactionCode = null;
            }

            if (!string.IsNullOrEmpty(transid) && !string.IsNullOrEmpty(responCode) && Convert.ToInt32(responCode) != (int)BankErrorMessages.Success)
            {
                model.Messages = "Giao dịch nạp VIPXU qua thẻ ngân hàng thất bại.";
                model.Status = false;
                model.Url = "";
                var transaction = new TransactionBankInfo
                {
                    CustomerCode = CustomerCode,
                    TransactionCode = TransactionCode,
                    CreateDate = DateTime.Now,
                    BankCode = bankCode,
                    Amount = amount,
                    ResponCode = responCode,
                    Mac = mac,
                    TransId = transid,
                    Type = (int)TransferType.Rereceive,
                    Description = "Giao dịch thất bại",
                    Status = (int)PaymentStatus.Close
                };

                transactionBankService.BankProcessing(transaction, 0);
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Giao dịch nạp VIPXU qua thẻ ngân hàng thất bại.",
                    Keyword = bankCode,
                    Contents = TransactionCode + " " + CustomerCode + " " + amount,
                    Type = (int)LogType.BankCard,
                    Status = (int)LogStatus.Error
                });

                TransactionCode = null;
            }

            if (!string.IsNullOrEmpty(TransactionCode) && !string.IsNullOrEmpty(transid) && Convert.ToInt32(responCode) == (int)BankErrorMessages.Success)
            {
                var vipXu = TotalVipXu(int.Parse(amount));
                model.Messages = string.Format("Bạn đã nạp {0} VIPXU vào tài khoản {1} thành công.", vipXu, CustomerCode);
                model.Status = true;
                model.Amount = amount;
                model.Url = string.Format("http://{0}", Extensions.Constants.HomeDomainName);

                var transaction = new TransactionBankInfo
                {
                    CustomerCode = CustomerCode,
                    TransactionCode = TransactionCode,
                    CreateDate = DateTime.Now,
                    BankCode = bankCode,
                    Amount = amount,
                    ResponCode = responCode,
                    Mac = mac,
                    TransId = transid,
                    Type = (int)TransferType.Rereceive,
                    Description = string.Format("Đã nạp {0} VIPXU vào tài khoản {1} thành công.", vipXu, CustomerCode),
                    Status = (int)PaymentStatus.Close
                };

                transactionBankService.BankProcessing(transaction, vipXu);
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Giao dịch nạp VIPXU qua thẻ ngân hàng thành công.",
                    Keyword = bankCode,
                    Contents = TransactionCode + " " + CustomerCode + " " + amount,
                    Type = (int)LogType.BankCard,
                    Status = (int)LogStatus.Success
                });

                TransactionCode = null;
                customerService.ResetCacheCustomers();
                customer = customerService.GetCustomerByCacheId(UserId);
                SetCustomerState(customer);
            }

            #endregion

            #region Init
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            serviceCategory.SiteId = SiteId;
            var cate = serviceCategory.GetByIdCache((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = cate.Name;
            ViewData[Extensions.Constants.HeaderDescription] = cate.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = cate.Tags;

            BindVipModules();

            var serviceBankCard = WorkContext.Resolve<IBankCardService>();
            model.ListBankCards = serviceBankCard.GetRecords(x => x.Status == (int)Status.Approved).ToList();
            var serviceCard = WorkContext.Resolve<ICardTypeService>();
            model.ListCardTypes = serviceCard.GetRecords(x => x.Status == (int)Status.Approved).ToList();
            model.CustomerCode = CustomerCode;
            #endregion

            return View(model);
        }

        [Url("tai-tro/0/thong-tin.html")]
        public ActionResult VipXuInfo()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var model = new PaymentModel();
            model.PageName = 0;
            model.CustomerName = FullName;
            model.VipXu = VipXu.ToString();
            if (!IsLogin)
            {
                model.Messages = "Bạn chưa đăng nhập.";
                model.Status = false;
                return Redirect(Url.Action("Index", "Home"));
            }

            #region Init
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            serviceCategory.SiteId = SiteId;
            var cate = serviceCategory.GetByIdCache((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = cate.Name;
            ViewData[Extensions.Constants.HeaderDescription] = cate.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = cate.Tags;

            BindVipModules();

            model.CustomerCode = CustomerCode;
            #endregion

            return View(model);
        }

        [Url("tai-tro/2/log-doi-xu.html")]
        public ActionResult LogDoiXu()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var model = new PaymentModel();
            model.PageName = 2;
            if (!IsLogin)
            {
                model.Messages = "Bạn chưa đăng nhập.";
                model.Status = false;
                return Redirect(Url.Action("Index", "Home"));
            }

            #region Init
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            serviceCategory.SiteId = SiteId;
            var cate = serviceCategory.GetByIdCache((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = cate.Name;
            ViewData[Extensions.Constants.HeaderDescription] = cate.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = cate.Tags;

            BindVipModules();

            model.CustomerCode = CustomerCode;
            var historyService = WorkContext.Resolve<ICustomerHistoriesService>();
            model.CustomerDoiXuHistories = historyService.GetDoiXuByCustomer(CustomerCode);
            #endregion

            return View("LogDoiXu", model);
        }

        [Url("tai-tro/3/log-nap-vip.html")]
        public ActionResult LogNapVip()
        {
            UrlLogin = Request.Url != null ? Request.Url.AbsoluteUri : Url.Action("Index", "Home");
            var model = new PaymentModel();
            model.PageName = 3;
            #region Init
            var serviceCategory = WorkContext.Resolve<ICategoryService>();
            serviceCategory.LanguageCode = WorkContext.CurrentCulture;

            SiteId = (int)Site.Home;
            serviceCategory.SiteId = SiteId;
            var cate = serviceCategory.GetByIdCache((int)FixCategories.NapVIP);
            ViewData[Extensions.Constants.HeaderTitle] = cate.Name;
            ViewData[Extensions.Constants.HeaderDescription] = cate.Description;
            ViewData[Extensions.Constants.HeaderKeywords] = cate.Tags;

            BindVipModules();

            model.CustomerCode = CustomerCode;
            var historyService = WorkContext.Resolve<ICustomerHistoriesService>();
            model.CustomerNapVipHistories = historyService.GetNapVipByCustomer(UserId);
            #endregion

            return View("LogNapVip", model);
        }

        private void BindVipModules()
        {
            var widget = WorkContext.Resolve<IWidgetService>();
            var viewRenderer = new ViewRenderer { Context = ControllerContext };

            #region Customer
            var customerService = WorkContext.Resolve<ICustomerService>();
            var customer = customerService.GetById(UserId);
            if (customer == null)
            {
                customer = new CustomerInfo();
            }
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

        [HttpPost, ValidateInput(false)]
        [Url("thanh-toan/the-atm")]
        public ActionResult BankPayment()
        {
            var redirect = new DataViewerModel();
            if (!IsLogin)
            {
                redirect.Data = "Bạn chưa đăng nhập.";
                redirect.Status = false;
                return Json(redirect);
            }

            var amount = Request.Form["Amount"];
            var bankCode = Request.Form["BankCode"];
            var captcha = Request.Form["txtCaptcha"];
            if (string.IsNullOrEmpty(captcha) || captcha.ToUpper() != GlobalCapcha.ToUpper())
            {
                redirect.Status = false;
                redirect.Data = "Mã xác thực không đúng.";
                return Json(redirect);
            }

            if (string.IsNullOrEmpty(amount.Trim()))
            {
                redirect.Status = false;
                redirect.Data = "Số tiền không để trống.";
                return Json(redirect);
            }
            
            if (double.Parse(amount) < 50000)
            {
                redirect.Status = false;
                redirect.Data = "Số tiền phải lớn hơn hoặc bằng 50.000VNĐ.";
                return Json(redirect);
            }

            var apiBank = new APIBankCardService();
            apiBank.ReponseUrl = "http://viphd.vn" + Url.Action("Index") + "?amount=" + amount + "&bankCode=" + bankCode + "&";
            //apiBank.ReponseUrl = "http://localhost:1500" + Url.Action("Index") + "?amount=" + amount + "&bankCode=" + bankCode + "&";
            var encrypter = new DesSecurity();

            string stan = "720527";
            string termtxndatetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fee = "0";
            string userName = "DuManhTan";
            string IssuerID = "EPAY";
            string tranID = DateTime.Now.ToString("yyyyMMddHHmmss");

            string mac = apiBank.MerchantId + stan + termtxndatetime + amount + fee + userName + IssuerID + tranID + bankCode + apiBank.ReponseUrl;
            mac = encrypter.DESMAC(mac, apiBank.SenderKey);
            var service = new Service();
            var result = service.Deposit(apiBank.MerchantId, stan, termtxndatetime, amount, fee, userName,
                                               IssuerID, tranID,
                                               bankCode, mac,
                                               apiBank.ReponseUrl);
            
            TransactionCode = Utilities.GenerateUniqueNumber();
            var transaction = new TransactionBankInfo
            {
                CustomerCode = CustomerCode,
                TransactionCode = TransactionCode,
                CreateDate = DateTime.Now,
                BankCode = bankCode,
                Amount = amount,
                ResponCode = "",
                Mac = "",
                TransId = "",
                Type = (int) TransferType.Send,
                Description = "",
                Status = (int) PaymentStatus.Open
            };

            var transactionBankService = WorkContext.Resolve<ITransactionBankService>();
            transactionBankService.Insert(transaction);

            var serviceLog = new APISmsService();
            serviceLog.InsertLog(new LogInfo
            {
                CreateDate = DateTime.Now,
                Messages = "Gửi giao dịch nạp thẻ ngân hàng.",
                Keyword = bankCode,
                Contents = TransactionCode + " " + CustomerCode + " " + amount,
                Type = (int)LogType.BankCard,
                Status = (int)LogStatus.Information
            });

            redirect = new DataViewerModel
            {
                Status = true,
                Url = !string.IsNullOrEmpty(result.url) ? result.url : string.Empty
            };

            return Json(redirect);
        }

        [HttpPost, ValidateInput(false)]
        [Url("thanh-toan/the-nap")]
        public ActionResult CardPayment()
        {
            if (string.IsNullOrEmpty(TransactionCode))
            {
                TransactionCode = Utilities.GenerateUniqueNumber();
            }

            var redirect = new DataViewerModel();
            if (!IsLogin)
            {
                redirect.Data = "Bạn chưa đăng nhập.";
                redirect.Status = false;
                return Json(redirect);
            }

            var captcha = Request.Form["Captcha"];
            if (string.IsNullOrEmpty(captcha) || captcha.ToUpper() != GlobalCapcha.ToUpper())
            {
                redirect.Status = false;
                redirect.Data = "Mã xác thực không đúng.";
                return Json(redirect);
            }

            var service = new APICardCharging();
            string sessionId;
            string messages = service.Login(out sessionId);
            if (string.IsNullOrEmpty(sessionId) || messages != Utilities.GetMessages((int)ErrorMessages.Success))
            {
                redirect.Status = false;
                redirect.Data = messages;
                return Json(redirect);
            }

            var serviceCardTypes = WorkContext.Resolve<ICardTypeService>();
            var cardTypes = Request.Form["CardTypes"];
            if (string.IsNullOrEmpty(cardTypes))
            {
                redirect.Status = false;
                redirect.Data = "Bạn chưa chọn mạng cho thẻ nạp";
                return Json(redirect);
            }

            var seriNumber = Request.Form["SeriNumber"];
            if (cardTypes != "MGC" && string.IsNullOrEmpty(seriNumber))
            {
                redirect.Status = false;
                redirect.Data = "Bạn chưa nhập số seri của thẻ.";
                return Json(redirect);
            }

            var pinCode = Request.Form["PinCode"];
            if (string.IsNullOrEmpty(pinCode))
            {
                redirect.Status = false;
                redirect.Data = "Bạn chưa nhập mã thẻ cào.";
                return Json(redirect);
            }
            
            var cardType = serviceCardTypes.GetByCode(cardTypes, -1);
            if (cardType.HasSerial)
            {
                service.CardData = seriNumber + ":" + pinCode + "::" + cardTypes;
            }
            else if (!cardType.HasSerial)
            {
                service.CardData = pinCode + ":" + cardTypes;
            }

            string txttarget = "account";
            string amount;
            var status = service.CardCharging(sessionId, service.CardData, txttarget, out amount);
            if (status == ErrorMessages.Success)
            {
                var vipXu = TotalVipXu(int.Parse(amount));
                redirect.Status = true;
                redirect.Data = string.Format("Bạn đã nạp {0} VIPXU vào tài khoản {1} thành công.", vipXu, CustomerCode);
                redirect.Url = string.Format("http://{0}", Extensions.Constants.HomeDomainName);

                var transaction = new TransactionCardInfo
                {
                    CustomerCode = CustomerCode,
                    TransactionCode = TransactionCode,
                    CreateDate = DateTime.Now,
                    Amount = amount,
                    CardCode = cardTypes,
                    SeriNumber = seriNumber,
                    PinCode = pinCode,
                    Description = "Nạp thẻ thành công",
                    Status = (int)CardStatus.Success
                };

                var transactionCardService = WorkContext.Resolve<ITransactionCardService>();
                transactionCardService.CardProcessing(transaction, vipXu);
                var serviceLog = new APISmsService();
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Nạp tiền theo nhà mạng thành công",
                    Keyword = cardTypes,
                    Contents = "Mã GD: " + TransactionCode + " - Mã KH: " + CustomerCode + " - Số tiền nạp: " + amount + " - VIPXU: " + vipXu,
                    Type = (int)LogType.ScratchCard,
                    Status = (int)LogStatus.Success
                });

                TransactionCode = null;
                var customerService = WorkContext.Resolve<ICustomerService>();
                customerService.ResetCacheCustomers();
                var customer = customerService.GetCustomerByCacheId(UserId);
                SetCustomerState(customer);
            }
            else
            {
                 redirect.Status = true;
                 redirect.Data = Utilities.GetMessages((int)status);
                 var transaction = new TransactionCardInfo
                 {
                     CustomerCode = CustomerCode,
                     TransactionCode = TransactionCode,
                     CreateDate = DateTime.Now,
                     Amount = amount,
                     CardCode = cardTypes,
                     SeriNumber = seriNumber,
                     PinCode = pinCode,
                     Description = "Nạp tiền theo nhà mạng thất bại",
                     Status = (int)CardStatus.Error
                 };

                 var transactionCardService = WorkContext.Resolve<ITransactionCardService>();
                 transactionCardService.CardProcessing(transaction, 0);

                 var serviceLog = new APISmsService();
                 serviceLog.InsertLog(new LogInfo
                 {
                     CreateDate = DateTime.Now,
                     Messages = "Nạp tiền theo nhà mạng",
                     Keyword = cardTypes,
                     Contents = "Mã GD: " + TransactionCode + " - Mã KH: " + CustomerCode + " - Số tiền nạp: " + amount,
                     Type = (int)LogType.ScratchCard,
                     Status = (int)LogStatus.Error
                 });
            }
           
            return Json(redirect);
        }

        [HttpPost, ValidateInput(false)]
        [Url("thanh-toan/doi-ngay")]
        public ActionResult ExchangeDay()
        {
            var redirect = new DataViewerModel();
            if (!IsLogin)
            {
                redirect.Data = "Bạn chưa đăng nhập.";
                redirect.Status = false;
                return Json(redirect);
            }

            var day = Request.Form["txtExchangeDay"];
            if (string.IsNullOrEmpty(day))
            {
                redirect.Data = "Số ngày bạn chọn không đúng.";
                redirect.Status = false;
                return Json(redirect);
            }
            
            var vipXu = 0;
            var package = (ExchangeDay) int.Parse(day);
            switch (package)
            {
                case Extensions.ExchangeDay.Package15:
                    vipXu = 200;
                    break;
                case Extensions.ExchangeDay.Package50:
                    vipXu = 500;
                    break;
                case Extensions.ExchangeDay.Package100:
                    vipXu = 1000;
                    break;
                case Extensions.ExchangeDay.Package300:
                    vipXu = 3000;
                    break;
            }
           
            TransactionCode = Utilities.GenerateUniqueNumber();
            var customerService = WorkContext.Resolve<ICustomerService>();
            var status = customerService.ExchangeDay(UserId, int.Parse(day), vipXu, TransactionCode, EnumExtensions.GetDisplayName(package));
            var serviceLog = new APISmsService();
            if (status == 0)
            {
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Đổi VIPXU thành công",
                    Keyword = day,
                    Contents = "Mã GD: " + TransactionCode + " - Mã KH: " + CustomerCode + " - VIPXU: " + vipXu + " - Số ngày:" + day,
                    Type = (int)LogType.Other,
                    Status = (int)LogStatus.Success
                });

                TransactionCode = null;
                customerService.ResetCacheCustomers();
                var customer = customerService.GetCustomerByCacheId(UserId);
                SetCustomerState(customer);
                redirect.Data = string.Format("Bạn đã đổi {0} VIPXU lấy {1} ngày sử dụng thành công. Sử dụng từ ngày {2} đến hết ngày {3}", vipXu, day,
                    customer.StartDate != null ? customer.StartDate.Value.ToString(Extensions.Constants.DateTimeFomat) : StartDate.ToString(Extensions.Constants.DateTimeFomat),
                    customer.EndDate != null ? customer.EndDate.Value.ToString(Extensions.Constants.DateTimeFomat) : EndDate.ToString(Extensions.Constants.DateTimeFomat));
                redirect.Status = true;
            }
            else
            {
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Đổi VIPXU thất bại",
                    Keyword = day,
                    Contents = "Mã GD: " + TransactionCode + " - Mã KH: " + CustomerCode + " - VIPXU: " + vipXu + " - Số ngày:" + day,
                    Type = (int)LogType.Other,
                    Status = (int)LogStatus.Success
                });

                TransactionCode = null;
                redirect.Data = "Tài khoản của bạn không đủ VIPXU để thực hiện giao dịch. Vui lòng quay lại bước 1.";
                redirect.Status = false;
            }

            return Json(redirect);
        }

        private int ExchangeVipXu(int money)
        {
            var vipXu = 0;
            foreach (ExchangeMoney item in Enum.GetValues(typeof(ExchangeMoney)))
            {
                var name = EnumExtensions.GetDisplayName(item);
                var value = int.Parse(name.Replace(".", ""));
                if (value == money)
                {
                    vipXu = (int) item;
                    break;
                }
            }

            return vipXu;
        }

        private int TotalVipXu(int money)
        {
            var vipXu = ExchangeVipXu(money);
            if (vipXu == 0)
            {
                foreach (ExchangeMoney item in Enum.GetValues(typeof(ExchangeMoney)))
                {
                    var name = EnumExtensions.GetDisplayName(item);
                    var value = int.Parse(name.Replace(".", ""));
                    if (money > value)
                    {
                        var newMoney = money - value;
                        vipXu += ExchangeVipXu(value);
                        vipXu += ExchangeVipXu(newMoney);
                        break;
                    }
                }
            }

            return vipXu;
        }

        [HttpPost, ValidateInput(false)]
        [Url("thanh-toan/kich-hoat-khuyenmai")]
        public ActionResult ActiveCode()
        {
            var redirect = new DataViewerModel();
            if (!IsLogin)
            {
                redirect.Data = "Bạn chưa đăng nhập.";
                redirect.Status = false;
                return Json(redirect);
            }

            var freeCode = Request.Form["txtFreeCode"];
            if (string.IsNullOrEmpty(freeCode))
            {
                redirect.Data = "Mã khuyến mãi không tồn tại.";
                redirect.Status = false;
                return Json(redirect);
            }

            var servicePromotionCustomers = WorkContext.Resolve<IPromotionCustomersService>();
            var promotionCustomers = servicePromotionCustomers.GetCodeCustomer(UserId, freeCode);
            if (promotionCustomers == null)
            {
                redirect.Data = "Mã khuyến mãi không tồn tại.";
                redirect.Status = false;
                return Json(redirect);
            }

            var status = servicePromotionCustomers.ActiveCode(UserId, freeCode);
            var serviceLog = new APISmsService();
            if (status == 0)
            {
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Kích hoạt mã khuyến mãi thành công.",
                    Keyword = freeCode,
                    Contents = string.Format("Kích hoạt mã khuyến mãi {0} thành công. Đã nhận được {1} VIPXU từ mã khuyến mãi.", freeCode, promotionCustomers.Value),
                    Type = (int)LogType.Other,
                    Status = (int)LogStatus.Success
                });

                var customerService = WorkContext.Resolve<ICustomerService>();
                customerService.ResetCacheCustomers();
                var customer = customerService.GetCustomerByCacheId(UserId);
                SetCustomerState(customer);
                redirect.Data = string.Format("Bạn đã kích hoạt thành công mã khuyến mãi {0}. Bạn nhận được {1} VIPXU.", freeCode, promotionCustomers.Value);
                redirect.Status = true;
                redirect.Url = string.Format("http://{0}", Extensions.Constants.HomeDomainName);
            }
            else
            {
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Kích hoạt mã khuyến mãi thất bại.",
                    Keyword = freeCode,
                    Contents = string.Format("Mã khuyến mãi {0} có thể đã được sử dụng hoặc đã quá hạn sử dụng.", freeCode),
                    Type = (int)LogType.Other,
                    Status = (int)LogStatus.Error
                });

                redirect.Data = string.Format("Mã khuyến mãi {0} có thể đã được sử dụng hoặc đã quá hạn sử dụng.", freeCode);
                redirect.Status = false;
            }
           
            return Json(redirect);
        }

        [HttpPost, ValidateInput(false)]
        [Url("vip-xu/cap-nhat")]
        public ActionResult LoadVipXu()
        {
            var redirect = new DataViewerModel();
            var customerService = WorkContext.Resolve<ICustomerService>();
            customerService.ResetCacheCustomers();
            var customer = customerService.GetCustomerByCacheId(UserId);
            SetCustomerState(customer);
            redirect.Url = Url.Action("Index");
            redirect.Status = true;

            return Json(redirect);
        }

        [HttpPost, ValidateInput(false)]
        [Url("thanh-toan/kich-hoat/sms")]
        public ActionResult ActiveSmsCode()
        {
            var redirect = new DataViewerModel();
            if (!IsLogin)
            {
                redirect.Data = "Bạn chưa đăng nhập.";
                redirect.Status = false;
                return Json(redirect);
            }

            var transactionCode = Request.Form["TransactionCode"];
            if (string.IsNullOrEmpty(transactionCode))
            {
                redirect.Data = "Mã giao dịch không tồn tại.";
                redirect.Status = false;
                return Json(redirect);
            }

            var service = WorkContext.Resolve<ITransactionSmsService>();
            var transaction = service.GetByMOCode(transactionCode);
            if (transaction == null)
            {
                redirect.Data = "Mã giao dịch không tồn tại.";
                redirect.Status = false;
                return Json(redirect);
            }

            var status = service.ActiveSms(transactionCode, CustomerCode);
            var serviceLog = new APISmsService();
            if (status == 0)
            {
                var mo = service.GetByMOCode(transactionCode);
                mo.Id = 0;
                mo.Status = (int) RequestStatus.Messages200;
                mo.Type = (int) SMSType.MT;
                mo.StartDate = transaction.StartDate;
                mo.EndDate = transaction.EndDate;
                service.Insert(mo);

                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Kích hoạt mã giao dịch SMS thành công.",
                    Keyword = transactionCode,
                    Contents = string.Format("Kích hoạt mã giao dịch SMS {0} thành công. Đã nhận được {1} ngày từ mã giao dịch. Bắt đầu từ ngày {2} đến hết ngày {3}", 
                        transactionCode, transaction.TotalDay, transaction.StartDate != null ? transaction.StartDate.Value.ToString(Extensions.Constants.DateTimeFomat) : "", 
                        transaction.EndDate != null ? transaction.EndDate.Value.ToString(Extensions.Constants.DateTimeFomat): ""),
                    Type = (int)LogType.SMS,
                    Status = (int)LogStatus.Success
                });

                var customerService = WorkContext.Resolve<ICustomerService>();
                customerService.ResetCacheCustomers();
                var customer = customerService.GetCustomerByCacheId(UserId);
                SetCustomerState(customer);
                redirect.Data = string.Format("Kích hoạt mã giao dịch SMS {0} thành công. Đã nhận được {1} ngày từ mã giao dịch. Bắt đầu từ ngày {2} đến hết ngày {3}",
                        transactionCode, transaction.TotalDay, transaction.StartDate != null ? transaction.StartDate.Value.ToString(Extensions.Constants.DateTimeFomat) : "",
                        transaction.EndDate != null ? transaction.EndDate.Value.ToString(Extensions.Constants.DateTimeFomat) : "");
                redirect.Status = true;
                redirect.Url = string.Format("http://{0}", Extensions.Constants.HomeDomainName);
            }
            else
            {
                serviceLog.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = "Kích hoạt mã giao dịch thất bại.",
                    Keyword = transactionCode,
                    Contents = string.Format("Mã giao dịch {0} có thể đã được sử dụng hoặc đã bị khóa. Vui lòng liên hệ với quản trị để được trợ giúp.", transactionCode),
                    Type = (int)LogType.SMS,
                    Status = (int)LogStatus.Error
                });

                redirect.Data = string.Format("Mã giao dịch {0} có thể đã được sử dụng hoặc đã bị khóa. Vui lòng liên hệ với quản trị để được trợ giúp.", transactionCode);
                redirect.Status = false;
            }

            return Json(redirect);
        }
    }
}
