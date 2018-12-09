using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ITagService : IGenericService<TagInfo, int>, IDependency
    {
        bool CheckExist(int id, string keyword);

        IList<TagInfo> GetPaged(string keyword, int status, int pageIndex, int pageSize, out int totals);

        IList<TagInfo> GetDisplayTags();
    }

    public class TagService : GenericService<TagInfo, int>, ITagService
    {
        public TagService(IRepository<TagInfo, int> repository, IEventBus eventBus) 
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
            var result = (int)ExecuteReaderResult("sp_Tags_CheckName", list.ToArray());

            return result > 0;
        }

        public IList<TagInfo> GetPaged(string keyword, int status, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Keyword", keyword),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<TagInfo>("sp_Tags_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public IList<TagInfo> GetDisplayTags()
        {
            return ExecuteReader<TagInfo>("sp_Tags_GetDisplay");
        }
    }
}