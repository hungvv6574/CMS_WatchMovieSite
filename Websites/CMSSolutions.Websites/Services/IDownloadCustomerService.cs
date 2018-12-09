using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IDownloadCustomerService : IGenericService<DownloadCustomerInfo, long>, IDependency
    {
        List<DownloadCustomerInfo> GetByCustomer(int customerId);

        DownloadCustomerInfo GetItem(int customerId, int downloadId);

        DataTable GetHistory(int customerId);
    }

    public class DownloadCustomerService : GenericService<DownloadCustomerInfo, long>, IDownloadCustomerService
    {
        public DownloadCustomerService(IRepository<DownloadCustomerInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public List<DownloadCustomerInfo> GetByCustomer(int customerId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId)
            };

            return ExecuteReader<DownloadCustomerInfo>("sp_DownloadCustomers_GetByCustomer", list.ToArray());
        }

        public DownloadCustomerInfo GetItem(int customerId, int downloadId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId),
                AddInputParameter("@DownloadId", downloadId)
            };

            return ExecuteReaderRecord<DownloadCustomerInfo>("sp_DownloadCustomers_GetItem", list.ToArray());
        }

        public DataTable GetHistory(int customerId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId)
            };

            return ExecuteReader("sp_DownloadCustomers_GetHistory", list.ToArray()).Tables[0];
        }
    }
}