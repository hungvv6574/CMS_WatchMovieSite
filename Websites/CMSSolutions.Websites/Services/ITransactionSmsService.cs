using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ITransactionSmsService : IGenericService<TransactionSmsInfo, long>, IDependency
    {
        int ActiveSms(string transactionCode, string customerCode);

        TransactionSmsInfo GetByMOCode(string transactionCode);

        IList<TransactionSmsInfo> GetPaged(string keyword, int type, int status, int isClosed,
            DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, out int totals);

        IList<TransactionSmsInfo> ExportExcelSms(DateTime fromDate, DateTime toDate, int type);

        int SmsClosed(DateTime toDate);
    }

    public class TransactionSmsService : GenericService<TransactionSmsInfo, long>, ITransactionSmsService
    {
        public TransactionSmsService(IRepository<TransactionSmsInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public int ActiveSms(string transactionCode, string customerCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@TransactionCode", transactionCode),
                AddInputParameter("@CustomerCode", customerCode)
            };

            return (int)ExecuteReaderResult("sp_TransactionSms_Active", list.ToArray());
        }

        public TransactionSmsInfo GetByMOCode(string transactionCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@TransactionCode", transactionCode)
            };

            return ExecuteReaderRecord<TransactionSmsInfo>("sp_TransactionSms_GetMOCode", list.ToArray());
        }

        public IList<TransactionSmsInfo> GetPaged(string keyword, int type,
            int status, int isClosed, DateTime fromDate, DateTime toDate, 
            int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", keyword),
                AddInputParameter("@Type", type),
                AddInputParameter("@Status", status),
                AddInputParameter("@IsClosed", isClosed),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<TransactionSmsInfo>("sp_TransactionSms_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public IList<TransactionSmsInfo> ExportExcelSms(DateTime fromDate, DateTime toDate, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Type", type),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate)
            };

            return ExecuteReader<TransactionSmsInfo>("sp_TransactionSms_ReportSms", list.ToArray());
        }

        public int SmsClosed(DateTime toDate)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@ToDate", toDate)
            };

            return ExecuteNonQuery("sp_TransactionSms_Closed", list.ToArray());
        }
    }
}