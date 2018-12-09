using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ICountryService : IGenericService<CountryInfo, int>, IDependency
    {
        bool CheckExist(int id, string keyword);

        List<CountryInfo> SearchPaged(
            string searchText,
            int pageIndex,
            int pageSize,
            out int totalRecord);
    }

    public class CountryService : GenericService<CountryInfo, int>, ICountryService
    {
        public CountryService(IRepository<CountryInfo, int> repository, IEventBus eventBus) 
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
            var result = (int)ExecuteReaderResult("sp_Countries_CheckName", list.ToArray());

            return result > 0;
        }

        public List<CountryInfo> SearchPaged(string searchText, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Keyword", searchText),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<CountryInfo>("sp_Countries_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}
