using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ISupportService : IGenericService<SupportInfo, int>, IDependency
    {
        string LanguageCode { get; set; }

        int SiteId { get; set; }

        IList<SupportInfo> GetAllParents(int status);

        IList<SupportInfo> GetPaged(string keyword, int parentId, int status, int pageIndex, int pageSize, out int totals);

        IList<SupportInfo> GetByParent(int parentId);

        IList<SupportInfo> GetAllChildren(int status);
    }

    public class SupportService : GenericService<SupportInfo, int>, ISupportService
    {
        public SupportService(IRepository<SupportInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public string LanguageCode { get; set; }
        public int SiteId { get; set; }

        public IList<SupportInfo> GetAllParents(int status)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@Status", status),
            };

            return ExecuteReader<SupportInfo>("sp_Supports_GetAllParents", list.ToArray());
        }

        public IList<SupportInfo> GetPaged(string keyword, int parentId, int status, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Keyword", keyword),
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@ParentId", parentId),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<SupportInfo>("sp_Supports_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public IList<SupportInfo> GetByParent(int parentId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@ParentId", parentId)
            };

            return ExecuteReader<SupportInfo>("sp_Supports_GetByParent", list.ToArray());
        }

        public IList<SupportInfo> GetAllChildren(int status)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@Status", status),
            };

            return ExecuteReader<SupportInfo>("sp_Supports_GetAllChildren", list.ToArray());
        }
    }
}