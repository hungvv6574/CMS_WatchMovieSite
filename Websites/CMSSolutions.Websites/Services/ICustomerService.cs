using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using CMSSolutions.Caching;
using CMSSolutions.ContentManagement.Messages.Domain;
using CMSSolutions.ContentManagement.Messages.Services;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Services
{
    public interface ICustomerService : IGenericService<CustomerInfo, int>, IDependency
    {
        string GetLatestCustomerCode();

        List<CustomerInfo> SearchPaged(
            string searchText,
            int filmTypeId,
            int countryId,
            DateTime fromDate,
            DateTime toDate,
            int isBlock,
            int status,
            int pageIndex,
            int pageSize,
            out int totalRecord);

        CustomerInfo Login(string userName, string password);

        CustomerInfo CheckRegister(string userName);

        CustomerInfo GetByCustomerCode(string customerCode);

        int ExchangeDay(int customerId, int day, int vipXu, string transactionCode, string package);

        IList<CustomerInfo> GetPromotions(int promotionId);

        int ChangePassword(string userName, string passwordOld, string password);

        CustomerInfo ResetTotalDay(int customerId);

        void ResetCacheCustomers();

        CustomerInfo GetCustomerByCacheId(int customerId);

        int AddVipXu(int customerId, int downloadId, int vipXu);

        IList<CustomerInfo> ExportExcel();

        CustomerInfo GetMemberForgetPassword(string newPassword, string email);

        void AddEmailMessages();

        List<CustomerInfo> GetPaged(int pageIndex, int pageSize, out int totalRecord);

        IList<QueuedEmail> GetQueuedEmails(int sentTries, DateTime sentOnUtc, int pageIndex, int pageSize, out int totalRecord);
    }

    public class CustomerService : GenericService<CustomerInfo, int>, ICustomerService
    {
        private readonly ICacheInfo cacheManager;
        private readonly IMessageService messageService;
        private readonly IFilmService filmService;
        public CustomerService(IRepository<CustomerInfo, int> repository, 
            IEventBus eventBus, ICacheInfo cacheManager, 
            IMessageService messageService, IFilmService filmService) 
            : base(repository, eventBus)
        {
            this.cacheManager = cacheManager;
            this.messageService = messageService;
            this.filmService = filmService;
        }

        public string GetLatestCustomerCode()
        {
            return (string)ExecuteReaderResult("NextCustomerCode");
        }

        public List<CustomerInfo> SearchPaged(
            string searchText, int filmTypeId, int countryId, 
            DateTime fromDate, DateTime toDate, int isBlock,
            int status, int pageIndex, int pageSize, 
            out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", searchText),
                AddInputParameter("@FilmTypeId", filmTypeId),
                AddInputParameter("@CountryId", countryId),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate),
                AddInputParameter("@IsBlock", isBlock),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<CustomerInfo>("sp_Customers_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public CustomerInfo Login(string userName, string password)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@UserName", userName),
                AddInputParameter("@Password", password),
            };

            return ExecuteReaderRecord<CustomerInfo>("sp_Customers_Login", list.ToArray());
        }

        public CustomerInfo CheckRegister(string userName)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@UserName", userName),
            };

            return ExecuteReaderRecord<CustomerInfo>("sp_Customers_CheckRegister", list.ToArray());
        }

        public CustomerInfo GetByCustomerCode(string customerCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerCode", customerCode),
            };

            return ExecuteReaderRecord<CustomerInfo>("sp_Customers_GetByCustomerCode", list.ToArray());
        }

        public int ExchangeDay(int customerId, int day, int vipXu, string transactionCode, string package)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId),
                AddInputParameter("@Day", day),
                AddInputParameter("@VipXu", vipXu),
                AddInputParameter("@TransactionCode", transactionCode),
                AddInputParameter("@Package", package)
            };

            return (int)ExecuteReaderResult("sp_Customers_ExchangeDay", list.ToArray());
        }

        public IList<CustomerInfo> GetPromotions(int promotionId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@PromotionId", promotionId)
            };

            return ExecuteReader<CustomerInfo>("sp_Customers_GetPromotions", list.ToArray());
        }

        public int ChangePassword(string userName, string passwordOld, string password)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@UserName", userName),
                AddInputParameter("@PasswordOld", passwordOld),
                AddInputParameter("@Password", password)
            };

            return (int)ExecuteReaderResult("sp_Customers_ChangePassword", list.ToArray());
        }

        public CustomerInfo ResetTotalDay(int customerId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId),
            };

            return ExecuteReaderRecord<CustomerInfo>("sp_Customers_ResetTotalDay", list.ToArray());
        }

        public void ResetCacheCustomers()
        {
            cacheManager.Remove(Extensions.Constants.CacheKeys.CUSTOMERS_DATA_CARD);
            cacheManager.Add(Extensions.Constants.CacheKeys.CUSTOMERS_DATA_CARD, GetAllCustomers());
        }

        public CustomerInfo GetCustomerByCacheId(int customerId)
        {
            var data = cacheManager.Get(Extensions.Constants.CacheKeys.CUSTOMERS_DATA_CARD);
            if (data == null)
            {
                ResetCacheCustomers();
                data = cacheManager.Get(Extensions.Constants.CacheKeys.CUSTOMERS_DATA_CARD);
            }

            var items = ((IList<CustomerInfo>) data);
            return items.FirstOrDefault(x => x.Id == customerId);
        }

        private IList<CustomerInfo> GetAllCustomers()
        {
            return ExecuteReader<CustomerInfo>("sp_Customers_GetAllCustomerCache");
        }


        private string BuildHtmlEmail()
        {
            var html = new StringBuilder();
            filmService.SiteId = (int)Site.Home;
            filmService.LanguageCode = "vi-VN";
            var listFilms = filmService.BuildFilmHot();
            if (listFilms != null && listFilms.Count > 0)
            {
                foreach (var filmInfo in listFilms)
                {
                    var url = string.Format("http://viphd.vn/xem-phim/{0}/f{1}.html", filmInfo.FilmAlias, filmInfo.Id);
                    var urlImage = string.Format("http://viphd.vn{0}", filmInfo.ImageIcon);
                   
                    html.Append("<div style=\"margin-right: 15px;padding: 8px 0px;width: 250px;float: left;\">");
                    html.Append("<div style=\"float: left;background-color: #FFF;box-shadow: 0px 0px 5px #999;-moz-box-shadow: 0px 0px 5px #999;-webkit-box-shadow: 0px 0px 5px #999;margin-right: 8px;\">");
                    html.AppendFormat("<a style=\"text-decoration: none;\" title=\"{0}\" href=\"{1}\">", filmInfo.FilmNameEnglish, url);
                    html.AppendFormat("<img alt=\"{0}\" width=\"106\" height=\"80\" src=\"{1}\">", filmInfo.FilmName, urlImage);
                    html.Append("</a>");
                    html.Append("</div>");
                    html.Append("<div style=\"float: left;\">");
                    html.Append("<p style=\"max-height: 35px;overflow: hidden;color: #44619d;font-family: 'SFUGillSansRegular';font-size: 14px;font-style: normal;font-weight: 200;text-transform: capitalize;line-height: 16px;\">");
                    html.AppendFormat("<a style=\"color: #44619d;font-family: 'SFUGillSansRegular';font-size: 14px;font-style: normal;font-weight: 200;text-transform: capitalize;line-height: 16px;text-transform: uppercase;\" title=\"{0}\" href=\"{1}\">{0}</a>", filmInfo.FilmNameEnglish, url);
                    html.Append("</p>");
                    html.Append("<p style=\"max-height: 36px;overflow: hidden;line-height: 18px;color: #666666;font-family: 'SFUGillSansRegular';font-size: 12px;font-style: normal;font-weight: 200;text-transform: capitalize;\">");
                    html.AppendFormat("<a style=\"color: #666666;font-family: 'SFUGillSansRegular';font-size: 12px;font-style: normal;font-weight: 200;text-transform: capitalize;line-height: 14px;\" title=\"{0}\" href=\"{1}\">{0}</a>", filmInfo.FilmName, url);
                    html.Append("</p>");
                    html.Append("<p style=\"color: #696969;font-size: 11px;font-family: Arial, Helvetica, sans-serif;line-height: 14px;overflow: hidden;\">");
                    html.Append(filmInfo.Summary);
                    html.Append("</p>");
                    html.Append("</div>");
                    html.Append("<br class=\"clear_both\">");
                    html.Append("</div>");
                }
            }

            return html.ToString();
        }

        public int AddVipXu(int customerId, int downloadId, int vipXu)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId),
                AddInputParameter("@DownloadId", downloadId),
                AddInputParameter("@VipXu", vipXu)
            };

            return (int)ExecuteReaderResult("sp_Customers_UpdateVipXu", list.ToArray());
        }

        public IList<CustomerInfo> ExportExcel()
        {
            return ExecuteReader<CustomerInfo>("sp_Customers_GetAllCustomers");
        }

        public CustomerInfo GetMemberForgetPassword(string newPassword, string email)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@NewPassword", newPassword),
                AddInputParameter("@Email", email)
            };

            return ExecuteReaderRecord<CustomerInfo>("sp_Customers_ForgetPassword", list.ToArray());
        }

        public List<CustomerInfo> GetPaged(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<CustomerInfo>("sp_Customers_GetPaged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public void AddEmailMessages()
        {
            try
            {
                int pageIndex = 1;
                int pageSize = 200;
                int totalRecord = 0;
                int totalPage = 1;

                var htmlBody = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Media/Default/EmailTemplates/FilmHot.html"));
                var customers = GetPaged(pageIndex, pageSize, out totalRecord);
                if (customers != null && totalRecord > 0)
                {
                    if (totalRecord <= pageSize)
                    {
                        totalPage = 1;
                    }
                    else
                    {
                        var count = totalRecord % pageSize;
                        if ((count == 0))
                        {
                            totalPage = totalRecord / pageSize;
                        }
                        else
                        {
                            totalPage = ((totalRecord - count) / pageSize) + 1;
                        }
                    }

                    for (int i = 1; i <= totalPage; i++)
                    {
                        try
                        {
                            if (i > 1)
                            {
                                customers = GetPaged(pageIndex, pageSize, out totalRecord);
                            }

                            htmlBody = htmlBody.Replace("[LIST_FILM]", BuildHtmlEmail());
                            foreach (var customer in customers)
                            {
                                try
                                {
                                    var mailMessage = new MailMessage
                                    {
                                        Subject = "Phim hot nhất trong ngày",
                                        SubjectEncoding = Encoding.UTF8,
                                        Body = htmlBody,
                                        BodyEncoding = Encoding.UTF8,
                                        IsBodyHtml = true,
                                        Sender = new MailAddress(customer.Email)
                                    };

                                    mailMessage.To.Add(customer.Email);
                                    mailMessage.Bcc.Add("api.viphd@gmail.com");
                                    messageService.SendEmailMessage(mailMessage);
                                }
                                catch (Exception)
                                {
                                    
                                }
                                
                            }
                        }
                        catch (Exception)
                        {
                           
                        }

                        pageIndex++;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public IList<QueuedEmail> GetQueuedEmails(int sentTries, DateTime sentOnUtc, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SentTries", sentTries),
                AddInputParameter("@SentOnUtc", sentOnUtc),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<QueuedEmail>("sp_QueuedEmails_GetMessages_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}
