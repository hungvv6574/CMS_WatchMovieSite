using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IRateService : IGenericService<RateInfo, long>, IDependency
    {
        IList<RateInfo> SearchPaged(string searchText, int pageIndex, int pageSize, out int totalRecord);

        RateInfo GetByFilmCustomer(long filmId, string customerCode);
    }
    
    public class RateService : GenericService<RateInfo, long>, IRateService
    {
        public RateService(IRepository<RateInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<RateInfo> SearchPaged(string searchText, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Keyword", searchText),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<RateInfo>("sp_Rates_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public RateInfo GetByFilmCustomer(long filmId, string customerCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FilmId", filmId),
                AddInputParameter("@CustomerCode", customerCode),
            };

            return ExecuteReaderRecord<RateInfo>("sp_Rates_GetByFilmAndCustomer", list.ToArray());
        }

        public override RateInfo GetById(long id)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Id", id),
            };

            return ExecuteReaderRecord<RateInfo>("sp_Rates_GetById", list.ToArray());
        }
    }
}
