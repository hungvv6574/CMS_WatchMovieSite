using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ITransactionBankService : IGenericService<TransactionBankInfo, long>, IDependency
    {
        int BankProcessing(TransactionBankInfo entity, int vipXu);

        IList<TransactionBankInfo> GetPaged(string keyword, string bankCode, int type, int status, int isLock,
            DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, out int totals);

        int ATMClosed(DateTime toDate);

        IList<TransactionBankInfo> ExportExcelAtm(DateTime fromDate, DateTime toDate, int type);
    }

    public class TransactionBankService : GenericService<TransactionBankInfo, long>, ITransactionBankService
    {
        public TransactionBankService(IRepository<TransactionBankInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public int BankProcessing(TransactionBankInfo entity, int vipXu)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerCode", entity.CustomerCode),
                AddInputParameter("@TransactionCode", entity.TransactionCode),
                AddInputParameter("@CreateDate", entity.CreateDate),
                AddInputParameter("@BankCode", entity.BankCode),
                AddInputParameter("@Amount", entity.Amount),
                AddInputParameter("@ResponCode", entity.ResponCode),
                AddInputParameter("@Mac", entity.Mac),
                AddInputParameter("@TransId", entity.TransId),
                AddInputParameter("@Type", entity.Type),
                AddInputParameter("@Description", entity.Description),
                AddInputParameter("@Status", entity.Status),
                AddInputParameter("@VipXu", vipXu)
            };

            return ExecuteNonQuery("sp_TransactionBank_BankProcessing", list.ToArray());
        }

        public IList<TransactionBankInfo> ExportExcelAtm(DateTime fromDate, DateTime toDate, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Type", type),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate)
            };

            return ExecuteReader<TransactionBankInfo>("sp_TransactionBank_ReportATM", list.ToArray());
        }

        public IList<TransactionBankInfo> GetPaged(string keyword, string bankCode, int type, int status, int isLock, 
            DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", keyword),
                AddInputParameter("@BankCode", bankCode),
                AddInputParameter("@Type", type),
                AddInputParameter("@Status", status),
                AddInputParameter("@IsLock", isLock),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<TransactionBankInfo>("sp_TransactionBank_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public int ATMClosed(DateTime toDate)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@ToDate", toDate)
            };

            return ExecuteNonQuery("sp_TransactionAtm_Closed", list.ToArray());
        }
    }
}