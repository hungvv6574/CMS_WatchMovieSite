using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IAdvertisementGroupService : IGenericService<AdvertisementGroupInfo, int>, IDependency
    {
        List<AdvertisementGroupInfo> SearchPaged(
            string languageCode,
            int siteId,
            int categoryId,
            int pageIndex,
            int pageSize,
            out int totalRecord);

        AdvertisementGroupInfo GetAdByCategories(string languageCode, int siteId, string categoryIds);

        DataTable GetDataGenerateXml(string languageCode);

        IList<AdvertisementGroupInfo> GetGroupGenerateXml(string languageCode);
    }
    
    public class AdvertisementGroupService : GenericService<AdvertisementGroupInfo, int>, IAdvertisementGroupService
    {
        public AdvertisementGroupService(IRepository<AdvertisementGroupInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public List<AdvertisementGroupInfo> SearchPaged(string languageCode, int siteId, int categoryId, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@CategoryId", categoryId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<AdvertisementGroupInfo>("sp_AdvertisementGroup_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public AdvertisementGroupInfo GetAdByCategories(string languageCode, int siteId, string categoryIds)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@CategoryIds", categoryIds)
            };

            return ExecuteReaderRecord<AdvertisementGroupInfo>("sp_AdvertisementGroup_GetAdByCategory", list.ToArray());
        }

        public DataTable GetDataGenerateXml(string languageCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", languageCode)
            };

            return ExecuteReader("sp_Advertisement_GetGenerateXml", list.ToArray()).Tables[0];
        }

        public IList<AdvertisementGroupInfo> GetGroupGenerateXml(string languageCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", languageCode)
            };

            return ExecuteReader<AdvertisementGroupInfo>("sp_AdvertisementGroup_GetGenerateXml", list.ToArray());
        }
    }
}
