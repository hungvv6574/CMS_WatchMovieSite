using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IPromotionCustomersService : IGenericService<PromotionCustomerInfo, long>, IDependency
    {
        IList<PromotionCustomerInfo> GetByPromotion(int promotionId);

        PromotionCustomerInfo GetCode(int promotionId, int customerId);

        PromotionCustomerInfo GetCodeCustomer(int customerId, string code);

        int ActiveCode(int customerId, string code);
    }

    public class PromotionCustomersService : GenericService<PromotionCustomerInfo, long>, IPromotionCustomersService
    {
        public PromotionCustomersService(IRepository<PromotionCustomerInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<PromotionCustomerInfo> GetByPromotion(int promotionId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@PromotionId", promotionId)
            };

            return ExecuteReader<PromotionCustomerInfo>("sp_PromotionCustomers_GetByPromotionId", list.ToArray());
        }

        public PromotionCustomerInfo GetCode(int promotionId, int customerId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@PromotionId", promotionId),
                AddInputParameter("@CustomerId", customerId)
            };

            return ExecuteReaderRecord<PromotionCustomerInfo>("sp_PromotionCustomers_GetCode", list.ToArray());
        }

        public PromotionCustomerInfo GetCodeCustomer(int customerId, string code)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId),
                AddInputParameter("@Code", code)
            };

            return ExecuteReaderRecord<PromotionCustomerInfo>("sp_PromotionCustomers_GetCodeCustomer", list.ToArray());
        }

        public int ActiveCode(int customerId, string code)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@CustomerId", customerId),
                AddInputParameter("@Code", code)
            };

            return (int)ExecuteReaderResult("sp_TransactionCode_Active", list.ToArray());
        }
    }
}