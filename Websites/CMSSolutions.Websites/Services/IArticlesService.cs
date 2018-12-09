using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Caching;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IArticlesService : ICacheService<ArticlesInfo, int>, IGenericService<ArticlesInfo, int>, IDependency
    {
        bool CheckAlias(string alias);

        string LanguageCode { get; set; }

        int CategoryId { get; set; }

        int SiteId { get; set; }

        List<ArticlesInfo> SearchPaged(
            string searchText, 
            int siteId,
            int userId,
            DateTime fromDate, 
            DateTime toDate, 
            int status,
            int pageIndex, 
            int pageSize,
            out int totalRecord);

        IList<ArticlesInfo> BuildNewsByCategory();

        IList<ArticlesInfo> GetByCategory();

        IList<ArticlesInfo> GetByCategoryPaged(int pageIndex, int pageSize, out int totalRecord);
    }

    public class ArticlesService : GenericService<ArticlesInfo, int>, IArticlesService
    {
        private readonly ICacheInfo cacheManager;
        public string LanguageCode { get; set; }
        public int CategoryId { get; set; }
        public int SiteId { get; set; }

        public ArticlesService(
            IRepository<ArticlesInfo, int> repository, 
            IEventBus eventBus, ICacheInfo cacheManager) 
            : base(repository, eventBus)
        {
            this.cacheManager = cacheManager;
        }

        public bool CheckAlias(string alias)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Alias", alias)
            };
            var result = (int)ExecuteReaderResult("sp_Articles_CheckAlias", list.ToArray());

            return result > 0;
        }

        public List<ArticlesInfo> SearchPaged(string searchText, int siteId, int userId, DateTime fromDate, DateTime toDate, int status, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", searchText),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@CategoryId", CategoryId),
                AddInputParameter("@UserId", userId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@Todate", toDate),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<ArticlesInfo>("sp_Articles_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<ArticlesInfo> GetByCategory()
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", CategoryId)
            };

            return ExecuteReader<ArticlesInfo>("sp_Articles_GetByCategory", list.ToArray());
        }

        public IList<ArticlesInfo> GetByCategoryPaged(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                 AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", CategoryId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<ArticlesInfo>("sp_Articles_GetByCategory_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<ArticlesInfo> BuildNewsByCategory()
        {
            var list = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.HOME_CATEGORY_ID, CategoryId));
            if (list == null)
            {
                list = ResetCache();
            }

            return (IList<ArticlesInfo>)list;
        }

        public ArticlesInfo GetByIdCache(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ArticlesInfo> GetAllCache()
        {
            throw new NotImplementedException();
        }

        public IList<ArticlesInfo> ResetCache()
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_CATEGORY_ID, CategoryId));
            var list = GetByCategory();
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_CATEGORY_ID, CategoryId), list);

            return list;
        }
    }
}
