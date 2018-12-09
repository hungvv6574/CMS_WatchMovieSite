using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ICustomerHistoriesService : IGenericService<CustomerHistoriesInfo, long>, IDependency
    {
        IList<CustomerHistoriesInfo> GetPaged(int customerId, DateTime fromDate, DateTime toDate, int type, int status, int pageIndex, int pageSize, out int totalRecord);

        List<NapVipCustomerLogs> GetNapVipByCustomer(int customerId);

        List<TransactioCustomerLogs> GetDoiXuByCustomer(string customerCode);
    }

    public class CustomerHistoriesService : GenericService<CustomerHistoriesInfo, long>, ICustomerHistoriesService
    {
        public CustomerHistoriesService(IRepository<CustomerHistoriesInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<CustomerHistoriesInfo> GetPaged(int customerId, DateTime fromDate, DateTime toDate, 
            int type, int status, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId),
                AddInputParameter("@Type", type),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<CustomerHistoriesInfo>("sp_CustomerHistories_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public List<NapVipCustomerLogs> GetNapVipByCustomer(int customerId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId)
            };

            return ExecuteReader<NapVipCustomerLogs>("sp_NapVipLogs", list.ToArray());
        }

        public List<TransactioCustomerLogs> GetDoiXuByCustomer(string customerCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerCode", customerCode)
            };

            return ExecuteReader<TransactioCustomerLogs>("sp_TransactioLogs", list.ToArray());
        }
    }
}
