using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IAdvertisementService : IGenericService<AdvertisementInfo, int>, IDependency
    {
        List<AdvertisementInfo> SearchPaged(
            string searchText,
            string languageCode,
            int siteId,
            DateTime fromDate,
            DateTime toDate,
            int pageIndex,
            int pageSize,
            out int totalRecord);

        List<AdvertisementInfo> GetAdBySite(string languageCode, int siteId);
    }

    public class AdvertisementService : GenericService<AdvertisementInfo, int>, IAdvertisementService
    {
        public AdvertisementService(IRepository<AdvertisementInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public List<AdvertisementInfo> SearchPaged(string searchText, string languageCode, int siteId, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", searchText),
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@Todate", toDate),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<AdvertisementInfo>("sp_Advertisement_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public List<AdvertisementInfo> GetAdBySite(string languageCode, int siteId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@SiteId", siteId)
            };

            return ExecuteReader<AdvertisementInfo>("sp_Advertisement_GetAdBySite", list.ToArray());
        }
    }
}