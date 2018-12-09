using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IVastService : IGenericService<VastInfo, int>, IDependency
    {
        List<VastInfo> SearchPaged(
                    string languageCode,
                    int siteId,
                    int adId,
                    int pageIndex,
                    int pageSize,
                    out int totalRecord);
    }
    
    public class VastService : GenericService<VastInfo, int>, IVastService
    {
        public VastService(IRepository<VastInfo, int> repository, IEventBus eventBus) : base(repository, eventBus)
        {

        }

        public List<VastInfo> SearchPaged(string languageCode, int siteId, int adId, 
            int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@AdId", adId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<VastInfo>("sp_Vast_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}
