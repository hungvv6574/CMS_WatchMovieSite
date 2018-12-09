using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IPromotionService : IGenericService<PromotionInfo, int>, IDependency
    {
        IList<PromotionInfo> SearchPaged(
            int status,
            int pageIndex,
            int pageSize,
            out int totalRecord);
    }

    public class PromotionService : GenericService<PromotionInfo, int>, IPromotionService
    {
        public PromotionService(IRepository<PromotionInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<PromotionInfo> SearchPaged(int status, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<PromotionInfo>("sp_Promotions_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}