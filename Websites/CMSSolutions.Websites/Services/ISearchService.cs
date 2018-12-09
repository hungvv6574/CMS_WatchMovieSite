using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ISearchService : IGenericService<SearchInfo, long>, IDependency
    {
        int SiteId { get; set; }

        SearchInfo GetBySearchId(string searchId, int type);

        IList<SearchInfo> Search(List<SearchCondition> conditions, int pageIndex, int pageSize, ref int total);

        IList<SearchInfo> ResetCache();

        void SearchRestore(int type);
    }

    public class SearchService : GenericService<SearchInfo, long>, ISearchService
    {
        public SearchService(IRepository<SearchInfo, long> repository, 
            IEventBus eventBus) 
            : base(repository, eventBus)
        {
            
        }

        public IList<SearchInfo> Search(List<SearchCondition> conditions, int pageIndex, int pageSize, ref int total)
        {
            var service = new LuceneService();
            return service.Search(conditions, true, pageIndex, pageSize, ref total);
        }

        public int SiteId { get; set; }

        public SearchInfo GetBySearchId(string searchId, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchId", searchId),
                AddInputParameter("@Type", type)
            };

            return ExecuteReaderRecord<SearchInfo>("sp_Search_GetDetails", list.ToArray());
        }

        public void SearchRestore(int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Type", type)
            };

            ExecuteNonQuery("sp_AutoSearh", list.ToArray());
        }

        public IList<SearchInfo> ResetCache()
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId)
            };

            var data = ExecuteReader("sp_Search_BuildJson", list.ToArray());
            var service = new LuceneService {SiteId = SiteId};
            service.AddUpdateLuceneIndex(data.Tables[0]);
            return null;
        }
    }
}
