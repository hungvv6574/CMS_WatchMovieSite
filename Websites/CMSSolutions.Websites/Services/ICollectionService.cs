using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ICollectionService : IGenericService<CollectionInfo, int>, IDependency
    {
        bool CheckExist(int id, string keyword);

        IList<CollectionInfo> GetPaged(string languageCode, int siteId, string keyword, int status, int pageIndex, int pageSize, out int totals);
    }
    
    public class CollectionService : GenericService<CollectionInfo, int>, ICollectionService
    {
        public CollectionService(IEventBus eventBus, 
            IRepository<CollectionInfo, int> repository) : 
                base(repository, eventBus)
        {

        }

        public bool CheckExist(int id, string keyword)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Id", id),
                AddInputParameter("@Keyword", keyword)
            };
            var result = (int)ExecuteReaderResult("sp_Collections_CheckName", list.ToArray());

            return result > 0;
        }

        public IList<CollectionInfo> GetPaged(string languageCode, int siteId, string keyword, int status, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@Keyword", keyword),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<CollectionInfo>("sp_Collections_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }
    }
}
