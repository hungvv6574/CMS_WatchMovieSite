using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Caching;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ISliderService : ICacheService<SliderInfo, int>, IGenericService<SliderInfo, int>, IDependency
    {
        string LanguageCode { get; set; }

        int SiteId { get; set; }

        IList<SliderInfo> GetPaged(int pageId, int pageIndex, int pageSize, out int totals);

        IList<SliderInfo> GetByPageId(int pageId);

        void RefreshByPage(int pageId);

        IList<SliderInfo> GetCacheByPageId(int pageId);
    }

    public class SliderService : GenericService<SliderInfo, int>, ISliderService
    {
        private readonly ICacheInfo cacheManager;
        private readonly ICategoryService categoryService;
        public SliderService(IRepository<SliderInfo, int> repository,
            IEventBus eventBus, ICacheInfo cacheManager,
            ICategoryService categoryService)
            : base(repository, eventBus)
        {
            this.cacheManager = cacheManager;
            this.categoryService = categoryService;
        }

        public string LanguageCode { get; set; }

        public int SiteId { get; set; }

        public IList<SliderInfo> GetPaged(int pageId, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@PageId", pageId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<SliderInfo>("sp_Slider_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }

        public IList<SliderInfo> GetByPageId(int pageId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@PageId", pageId)
            };

            return ExecuteReader<SliderInfo>("sp_Slider_GetByPageId", list.ToArray());
        }

        public IList<SliderInfo> GetCacheByPageId(int pageId)
        {
            var list = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.HOME_SLIDER_PAGE, pageId));
            if (list == null)
            {
                list = GetByPageId(pageId);
                cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_SLIDER_PAGE, pageId));
                cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_SLIDER_PAGE, pageId), list);
            }

            return (IList<SliderInfo>)list;
        }

        public void RefreshByPage(int pageId)
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_SLIDER_PAGE, pageId));
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_SLIDER_PAGE, pageId), GetByPageId(pageId));
        }

        public SliderInfo GetByIdCache(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<SliderInfo> GetAllCache()
        {
            throw new System.NotImplementedException();
        }

        public IList<SliderInfo> ResetCache()
        {
            throw new System.NotImplementedException();
        }
    }
}