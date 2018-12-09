using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IDownloadGameService : IGenericService<DownloadGameInfo, int>, IDependency
    {
        IList<DownloadGameInfo> SearchPaged(string keyword, int pageIndex, int pageSize, out int totalRecord);

        List<DownloadGameInfo> GetPaged(int pageIndex, int pageSize, out int totalRecord);
    }

    public class DownloadGameService : GenericService<DownloadGameInfo, int>, IDownloadGameService
    {
        public DownloadGameService(IRepository<DownloadGameInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<DownloadGameInfo> SearchPaged(string keyword, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Keyword", keyword),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<DownloadGameInfo>("sp_DownloadGames_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public List<DownloadGameInfo> GetPaged(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<DownloadGameInfo>("sp_DownloadGames_GetPaged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}