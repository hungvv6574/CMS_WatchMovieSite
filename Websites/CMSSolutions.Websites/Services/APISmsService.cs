using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Extensions;
using CMSSolutions.Websites.Entities;
using CMSSolutions.Websites.Extensions;

namespace CMSSolutions.Websites.Services
{
    public class APISmsService : DataContexService
    {
        public string Amount(string shortCode, out int totalDate, out DateTime? startDate, out DateTime? endDate)
        {
            startDate = Utilities.DateNull();
            endDate = Utilities.DateNull();
            totalDate = 0;
            string amount = "0";
            var code = EnumExtensions.Parse<ShortCode>(shortCode);
            switch (code)
            {
                case ShortCode.DauSo8076:
                    amount = "500";
                    break;
                case ShortCode.DauSo8176:
                    amount = "1000";
                    break;
                case ShortCode.DauSo8276:
                    amount = "2000";
                    break;
                case ShortCode.DauSo8376:
                    amount = "3000";
                    break;
                case ShortCode.DauSo8476:
                    amount = "4000";
                    break;
                case ShortCode.DauSo8576:
                    startDate = DateTime.Now;
                    endDate = DateTime.Now.AddDays(1);
                    totalDate = 1;
                    amount = "5000";
                    break;
                case ShortCode.DauSo8676:
                    startDate = DateTime.Now;
                    endDate = DateTime.Now.AddDays(3);
                    totalDate = 3;
                    amount = "10000";
                    break;
                case ShortCode.DauSo8776:
                    startDate = DateTime.Now;
                    endDate = DateTime.Now.AddDays(5);
                    totalDate = 5;
                    amount = "15000";
                    break;
            }

            return amount;
        }

        public CustomerInfo GetByCustomerCode(string customerCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerCode", customerCode),
            };

            return ExecuteReaderRecord<CustomerInfo>("sp_Customers_GetByCustomerCode", list.ToArray());
        }

        public int SmsProcessing(TransactionSmsInfo entity)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@PartnerId", entity.PartnerId),
                AddInputParameter("@MoId", entity.MoId),
                AddInputParameter("@UserId", entity.UserId),
                AddInputParameter("@ShortCode", entity.ShortCode),
                AddInputParameter("@Keyword", entity.Keyword),
                AddInputParameter("@Subkeyword", entity.Subkeyword),
                AddInputParameter("@Content", entity.Content),
                AddInputParameter("@TransDate", entity.TransDate),
                AddInputParameter("@CheckSum", entity.CheckSum),
                AddInputParameter("@Amount", entity.Amount),
                AddInputParameter("@CreateDate", entity.CreateDate),
                AddInputParameter("@Status", entity.Status),
                AddInputParameter("@Type", entity.Type),
                AddInputParameter("@MtId", entity.MtId),
                AddInputParameter("@CustomerCode", entity.CustomerCode),
                AddInputParameter("@TotalDay", entity.TotalDay),
                AddInputParameter("@StartDate", entity.StartDate),
                AddInputParameter("@EndDate", entity.EndDate),
                AddInputParameter("@TransactionCode", entity.TransactionCode),
                AddInputParameter("@IsLock", entity.IsLock)
            };

            return ExecuteNonQuery("sp_TransactionSms_SmsProcessing", list.ToArray());
        }

        public int InsertLog(LogInfo entity)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CreateDate", entity.CreateDate),
                AddInputParameter("@Type", entity.Type),
                AddInputParameter("@Messages", entity.Messages),
                AddInputParameter("@Keyword", entity.Keyword),
                AddInputParameter("@Contents", entity.Contents),
                AddInputParameter("@Status", entity.Status)
            };

            return ExecuteNonQuery("sp_Logs_Insert", list.ToArray());
        }
    }
}