using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IFilmTypesService : IGenericService<FilmTypesInfo, int>, IDependency
    {
        IList<FilmTypesInfo> GetPaged(string languageCode, int siteId, int status, int pageIndex, int pageSize, out int totals);

        IList<FilmTypesInfo> GetByType(string languageCode, int siteId, int type);
    }
    
    public class FilmTypesService : GenericService<FilmTypesInfo, int>, IFilmTypesService
    {
        public FilmTypesService(IRepository<FilmTypesInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<FilmTypesInfo> GetPaged(string languageCode, int siteId, int status, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmTypesInfo>("sp_FilmTypes_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public IList<FilmTypesInfo> GetByType(string languageCode, int siteId, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@Type", type)
            };

            return ExecuteReader<FilmTypesInfo>("sp_FilmTypes_GetByType", list.ToArray());
        }
    }
}
