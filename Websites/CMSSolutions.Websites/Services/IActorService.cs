using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{ 
    public interface IActorService : IGenericService<ActorInfo, int>, IDependency
    {
        bool CheckExist(int id, string keyword);

        List<ActorInfo> SearchPaged(
            string searchText,
            int status,
            int pageIndex,
            int pageSize,
            out int totalRecord);
    }

    public class ActorService : GenericService<ActorInfo, int>, IActorService
    {
        public ActorService(IRepository<ActorInfo, int> repository, IEventBus eventBus)
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
            var result = (int)ExecuteReaderResult("sp_Actors_CheckName", list.ToArray());

            return result > 0;
        }

        public List<ActorInfo> SearchPaged(string searchText, int status,int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Keyword", searchText),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<ActorInfo>("sp_Actors_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}
