using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IEpisodesService : IGenericService<EpisodeInfo, int>, IDependency
    {
        bool CheckExist(int id, string keyword);

        IList<EpisodeInfo> GetPaged(string languageCode, int siteId, string keyword, int status, int pageIndex, int pageSize, out int totals);

        IList<EpisodeInfo> GetAll(string languageCode, int siteId, int status);
    }
    
    public class EpisodeService : GenericService<EpisodeInfo, int>, IEpisodesService
    {
        public EpisodeService(IRepository<EpisodeInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public bool CheckExist(int id, string keyword)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Id", id),
                AddInputParameter("@Keyword", keyword)
            };
            var result = (int)ExecuteReaderResult("sp_Episodes_CheckName", list.ToArray());

            return result > 0;
        }

        public IList<EpisodeInfo> GetPaged(string languageCode, int siteId, string keyword, int status, int pageIndex, int pageSize, out int totals)
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

            return ExecuteReader<EpisodeInfo>("sp_Episodes_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public IList<EpisodeInfo> GetAll(string languageCode, int siteId, int status)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@Status", status)
            };

            return ExecuteReader<EpisodeInfo>("sp_Episodes_GetAll", list.ToArray());
        }
    }
}
