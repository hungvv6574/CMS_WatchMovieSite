using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IVIPCardService : IGenericService<VIPCardInfo, int>, IDependency
    {
        bool CheckVipCode(int id, string vipCode);

        List<VIPCardInfo> SearchPaged(
            string searchText,
            string languageCode,
            int siteId,
            int serverId,
            int pageIndex,
            int pageSize,
            out int totalRecord);
    }

    public class VIPCardService : GenericService<VIPCardInfo, int>, IVIPCardService
    {
        public VIPCardService(IRepository<VIPCardInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public bool CheckVipCode(int id, string vipCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@VIPCode", vipCode),
                AddInputParameter("@Id", id)
            };

            var result = (int)ExecuteReaderResult("sp_VIPCards_CheckAlias", list.ToArray());
            return result > 0;
        }

        public List<VIPCardInfo> SearchPaged(string searchText, string languageCode, int siteId, int serverId, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@ServerId", serverId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<VIPCardInfo>("sp_VIPCards_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}
