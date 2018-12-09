using System;
using System.Web;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;
using CMSSolutions.Websites.Payments;
using CMSSolutions.Websites.Services;

namespace CMSSolutions.Websites.API
{
    public class SmsService : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string partnerid = context.Request["partnerid"];
            string moid = context.Request["moid"];
            string userid = context.Request["userid"];
            string shortcode = context.Request["shortcode"];
            string keyword = context.Request["keyword"];
            string subkeyword = context.Request["subkeyword"];
            string content = context.Request["content"];
            string transdate = context.Request["transdate"];
            string checksum = context.Request["checksum"];
            string amount = context.Request["amount"];
            var service = new APISmsService();
            var request = new APISms();
            string url = request.UrlSMSMT;
            var transactionCode = Utilities.GenerateUniqueNumber();
            var code = HttpUtility.UrlDecode(content);
            var entityMO = new TransactionSmsInfo
            {
                Id = 0,
                PartnerId = partnerid,
                MoId = moid,
                UserId = userid,
                ShortCode = shortcode,
                Keyword = keyword,
                Subkeyword = subkeyword,
                Content = content,
                TransDate = transdate,
                CheckSum = checksum,
                Amount = amount,
                CreateDate = DateTime.Now,
                Status = (int)RequestStatus.Messages01,
                Type = (int)SMSType.MO,
                MtId = "",
                CustomerCode = "",
                TransactionCode = transactionCode,
                StartDate = new DateTime(1900,01,01),
                EndDate = new DateTime(1900, 01, 01),
                IsLock = false
            };
            try
            {
                LogFiles.WriteLogSms(string.Format("MO URL: {0}", context.Request.Url.AbsoluteUri));
                service.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = string.Format("MO URL: {0}", context.Request.Url.AbsoluteUri),
                    Keyword = userid,
                    Contents = code + " " + shortcode,
                    Type = (int)LogType.SMS,
                    Status = (int)LogStatus.Information
                });


                int totalDate;
                DateTime? startDate;
                DateTime? endDate;
                var totalMoney = service.Amount(shortcode, out totalDate, out startDate, out endDate);
                entityMO.TotalDay = totalDate;
                entityMO.StartDate = startDate;
                entityMO.EndDate = endDate;

                var list = code.Split(' ');
                if (list.Length <= 1)
                {
                    LogFiles.WriteLogSms(string.Format("MO: CUSTOMERCODE NOT FOUND IN CONTENT: {0}", content));
                    entityMO.IsLock = true;
                    service.SmsProcessing(entityMO);
                    request.SentMT(moid, shortcode, keyword, userid, string.Format(Extensions.Constants.SMSCustomerNotFound , transactionCode), ref url);
                    service.InsertLog(new LogInfo
                    {
                        CreateDate = DateTime.Now, 
                        Messages = string.Format("MO: CUSTOMERCODE NOT FOUND IN CONTENT: {0}", content),
                        Keyword = userid,
                        Contents = code + " " + shortcode,
                        Type = (int)LogType.SMS,
                        Status = (int)LogStatus.Error
                    });
                    context.Response.Write(string.Format("message:requeststatus=200"));
                    return;
                }

                var helpKey = list[1].Trim().ToUpper();
                var customerCode = list[1];
                var rejectShortCode = (ShortCode)int.Parse(shortcode);
                if (rejectShortCode == ShortCode.DauSo8376 && helpKey.ToUpper() == "HD")
                {
                    LogFiles.WriteLogSms(string.Format("MO: HELP USER: {0}", Extensions.Constants.SMSUserHelp));
                    service.SmsProcessing(entityMO);
                    request.SentMT(moid, shortcode, keyword, userid, Extensions.Constants.SMSUserHelp, ref url);
                    service.InsertLog(new LogInfo
                    {
                        CreateDate = DateTime.Now,
                        Messages = string.Format("MO: HELP USER: {0}", Extensions.Constants.SMSUserHelp),
                        Keyword = userid,
                        Contents = code + " " + shortcode,
                        Type = (int)LogType.SMS,
                        Status = (int)LogStatus.Error
                    });
                    context.Response.Write(string.Format("message:requeststatus=200"));
                    return;
                }

                var customer = service.GetByCustomerCode(customerCode);
                if (customer == null)
                {
                    entityMO.IsLock = true;
                    service.SmsProcessing(entityMO);
                    request.SentMT(moid, shortcode, keyword, userid, string.Format(Extensions.Constants.SMSCustomerNotFound, transactionCode), ref url);
                    service.InsertLog(new LogInfo
                    {
                        CreateDate = DateTime.Now,
                        Messages = string.Format("MO: CUSTOMERCODE NOT FOUND: {0}", customerCode),
                        Keyword = userid,
                        Contents = code + " " + shortcode,
                        Type = (int)LogType.SMS,
                        Status = (int)LogStatus.Error
                    });
                    LogFiles.WriteLogSms(string.Format("SYSTEM SAVE SUCCESS"));
                    context.Response.Write(string.Format("message:requeststatus=200"));
                    return;
                }

                var currentSum = request.Md5(moid + shortcode + keyword + content + transdate + request.Password).ToLower();
                if (checksum.Length != currentSum.Length)
                {
                    LogFiles.WriteLogSms(string.Format("MO: CHECKSUM FAILED {0} - {1}", checksum.Length, currentSum.Length));
                    service.SmsProcessing(entityMO);
                    request.SentMT(moid, shortcode, keyword, userid, Extensions.Constants.SMSSyntax, ref url);
                    service.InsertLog(new LogInfo
                    {
                        CreateDate = DateTime.Now,
                        Messages = string.Format("MO: CHECKSUM FAILED {0} - {1}", checksum.Length, currentSum.Length),
                        Keyword = userid,
                        Contents = code + " " + shortcode,
                        Type = (int)LogType.SMS,
                        Status = (int)LogStatus.Error
                    });
                    context.Response.Write(string.Format("message:requeststatus=200"));
                    return;
                }

                if (rejectShortCode != ShortCode.DauSo8576 
                    && rejectShortCode != ShortCode.DauSo8676 
                    && rejectShortCode != ShortCode.DauSo8776)
                {
                    LogFiles.WriteLogSms(string.Format("MO: SHORTCODE {0}", shortcode));
                    request.SentMT(moid, shortcode, keyword, userid, Extensions.Constants.SMSSyntax, ref url);
                    service.InsertLog(new LogInfo
                    {
                        CreateDate = DateTime.Now,
                        Messages = string.Format("MO: SHORTCODE {0}", shortcode),
                        Keyword = userid,
                        Contents = code + " " + shortcode,
                        Type = (int)LogType.SMS,
                        Status = (int)LogStatus.Error
                    });
                    context.Response.Write(string.Format("message:requeststatus=200"));
                    return;
                }

                var status = Send(url, moid, userid, shortcode, keyword, entityMO.Amount, subkeyword, transdate, checksum, customer, totalDate, startDate, endDate, transactionCode);
                if (status == RequestStatus.Messages200)
                {
                    entityMO.Amount = totalMoney;
                    entityMO.Status = (int)RequestStatus.Messages200;
                    entityMO.CustomerCode = customer.CustomerCode;
                    service.SmsProcessing(entityMO);
                }
                else
                {
                    entityMO.Status = (int)RequestStatus.Messages01;
                    service.SmsProcessing(entityMO);
                }
                LogFiles.WriteLogSms(string.Format("MO INSERT: {0} SUCCESSFUL", (int)status));
                service.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = string.Format("MO: INSERT: {0} SUCCESSFUL", (int)status),
                    Keyword = userid,
                    Contents = code + " " + shortcode,
                    Type = (int)LogType.SMS,
                    Status = (int)LogStatus.Information
                });
                context.Response.Write(string.Format("message:requeststatus=200"));
            }
            catch (Exception ex)
            {
                entityMO.Status = (int)RequestStatus.Messages01;
                service.SmsProcessing(entityMO);
                var st = request.SentMT(moid, shortcode, keyword, userid, Extensions.Constants.SMSSystemError, ref url);
                LogFiles.WriteLogSms(string.Format("Error: {0} - {1} ", ex.Message, st));
                service.InsertLog(new LogInfo
                {
                    CreateDate = DateTime.Now,
                    Messages = string.Format("Error: {0}", ex.Message),
                    Keyword = userid,
                    Contents = code + " " + shortcode,
                    Type = (int)LogType.SMS,
                    Status = (int)LogStatus.Error
                });
                context.Response.Write(string.Format("message:requeststatus=200"));
            }
        }

        private RequestStatus Send(string url, string moid, string userid, string shortcode,
            string keyword, string amount,
            string subkeyword, string transdate, string checksum, CustomerInfo customer,
            int totalDate, DateTime? startDate, DateTime? endDate, string transactionCode)
        {
            var service = new APISmsService();
            var request = new APISms();
            var messages = string.Format("VIPHD.VN: Xin chuc mung ban da dang ky thanh cong goi tai tro {0} ngay cho tai khoan {1} tren VIPHD! Chan thanh cam on!", totalDate, customer.CustomerCode);
            var status = (RequestStatus)request.SentMT(moid, shortcode, keyword, userid, messages, ref url);
            LogFiles.WriteLogSms(string.Format("Day: {0} - StartDate: {1} - EndDate: {2} - MT URL - {3}: {4}", totalDate, startDate != null ? startDate.Value.ToString() : "", endDate != null ? endDate.Value.ToString() : "", url, customer.CustomerCode));
            service.InsertLog(new LogInfo
            {
                CreateDate = DateTime.Now,
                Messages = string.Format("MT: REQUEST SUCCESSFUL: Day: {0} - StartDate: {1} - EndDate: {2} - CUSTOMERCODE: {3} - MT URL - {4}", totalDate, startDate != null ? startDate.Value.ToString() : "", endDate != null ? endDate.Value.ToString() : "", customer.CustomerCode, url),
                Keyword = userid,
                Contents = keyword + " " + shortcode,
                Type = (int)LogType.SMS,
                Status = (int)LogStatus.Success
            });

            var entityMT = new TransactionSmsInfo
            {
                Id = 0,
                PartnerId = request.PartnerId,
                MoId = moid,
                MtId = request.CreateMTId(),
                UserId = userid,
                ShortCode = shortcode,
                Keyword = keyword,
                Subkeyword = subkeyword,
                Content = messages,
                CreateDate = DateTime.Now,
                Status = (int)status,
                Type = (int)SMSType.MT,
                TransDate = transdate,
                CheckSum = checksum,
                Amount = amount,
                CustomerCode = customer.CustomerCode,
                TotalDay = totalDate,
                StartDate = startDate,
                EndDate = endDate,
                TransactionCode = transactionCode
            };

            service.SmsProcessing(entityMT);
            service.InsertLog(new LogInfo
            {
                CreateDate = DateTime.Now,
                Messages = string.Format("MT: INSERT {0} SUCCESSFUL", (int)status),
                Keyword = userid,
                Contents = keyword + " " + shortcode,
                Type = (int)LogType.SMS,
                Status = (int)LogStatus.Information
            });

            return status;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}