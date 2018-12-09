using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ITransactionCardService : IGenericService<TransactionCardInfo, long>, IDependency
    {
        int CardProcessing(TransactionCardInfo entity, int vipXu);

        IList<TransactionCardInfo> GetPaged(string keyword, string cardCode, int status, int isLock,
            DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, out int totals);

        IList<TransactionCardInfo> ExportExcelAtm(DateTime fromDate, DateTime toDate);

        int CardClosed(DateTime toDate);
    }

    public class TransactionCardService : GenericService<TransactionCardInfo, long>, ITransactionCardService
    {
        public TransactionCardService(IRepository<TransactionCardInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public int CardProcessing(TransactionCardInfo entity, int vipXu)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerCode", entity.CustomerCode),
                AddInputParameter("@TransactionCode", entity.TransactionCode),
                AddInputParameter("@CreateDate", entity.CreateDate),
                AddInputParameter("@CardCode", entity.CardCode),
                AddInputParameter("@Amount", entity.Amount),
                AddInputParameter("@SeriNumber", entity.SeriNumber),
                AddInputParameter("@PinCode", entity.PinCode),
                AddInputParameter("@Description", entity.Description),
                AddInputParameter("@Status", entity.Status),
                AddInputParameter("@VipXu", vipXu)
            };

            return ExecuteNonQuery("sp_TransactionCard_CardProcessing", list.ToArray());
        }

        public IList<TransactionCardInfo> GetPaged(string keyword, string cardCode, int status, 
            int isLock, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", keyword),
                AddInputParameter("@CardCode", cardCode),
                AddInputParameter("@Status", status),
                AddInputParameter("@IsLock", isLock),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<TransactionCardInfo>("sp_TransactionCard_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public IList<TransactionCardInfo> ExportExcelAtm(DateTime fromDate, DateTime toDate)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate)
            };

            return ExecuteReader<TransactionCardInfo>("sp_TransactionBank_ReportCard", list.ToArray());
        }

        public int CardClosed(DateTime toDate)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@ToDate", toDate)
            };

            return ExecuteNonQuery("sp_TransactionCard_Closed", list.ToArray());
        }
    }
}