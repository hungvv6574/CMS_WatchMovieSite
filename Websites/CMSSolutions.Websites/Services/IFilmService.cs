using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CMSSolutions.Caching;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IFilmService : ICacheService<FilmInfo, long>, IGenericService<FilmInfo, long>, IDependency
    {
        string LanguageCode { get; set; }

        int SiteId { get; set; }

        int CategoryId { get; set; }

        bool CheckAlias(long id, string alias);

        bool CheckName(string filmName);

        List<FilmInfo> SearchPaged(
            string searchText,
            string languageCode,
            int siteId,
            int categoryId,
	        int filmTypeId,
	        int countryId,
	        int directorId,
	        int actorId,
	        int collectionId,
	        int articlesId,
	        long createByUserId,
	        int serverId,
	        int releaseYear,
	        DateTime fromDate,
	        DateTime toDate,
	        DateTime createDate,
	        DateTime publishedDate,
	        int isPublished,
	        int isHot,
	        int isHome,
            int isFilmRetail,
            int isFilmLengthEpisodes,
	        int status,
	        int pageIndex,
	        int pageSize,
            out int totalRecord);

        string GetLatestFilmCode();

        IList<FilmInfo> BuildFilmHot();

        IList<FilmInfo> BuildFilmRetail();

        IList<FilmInfo> BuildFilmManyEpisodes();

        IList<FilmInfo> BuildFilmJJChannelIntroduce();

        IList<FilmInfo> BuildFilmTheater();

        IList<FilmInfo> BuildTVShow();

        IList<FilmInfo> BuildClips();

        IList<FilmInfo> GetFilmHot(int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetFilmRetail(int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetFilmManyEpisodes(int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetFilmJJChannelIntroduce(int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetFilmTheater(int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetTVShow(int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetClips(int pageIndex, int pageSize, out int totalRecord);

        void RefreshFilmJJChannelIntroduce(int categoryId);

        void RefreshTVShows(int categoryId);

        void RefreshClips(int categoryId);

        FilmInfo GetFilmDetails(long filmId);

        void RefreshStatistical1(int top, int type);

        IList<FilmInfo> GetStatistical1(int top, int type);

        IList<FilmInfo> BuildStatistical1(int top, int type);

        IList<FilmInfo> GetStatistical2(int top, int type);

        IList<FilmInfo> BuildStatistical2(int top, int type);

        void RefreshStatistical2(int top, int type);

        IList<FilmInfo> BuildStatistical3(int top, int type);

        IList<FilmInfo> GetStatistical3(int top, int type);

        void RefreshStatistical3(int top, int type);

        IList<FilmInfo> BuildStatistical4(int top, int type);

        IList<FilmInfo> GetStatistical4(int top, int type);

        void RefreshStatistical4(int top, int type);

        IList<FilmInfo> BuildStatistical5(int top, int type);

        IList<FilmInfo> GetStatistical5(int top, int type);

        void RefreshStatistical5(int top, int type);

        IList<FilmInfo> GetByCategoryId(int siteId, string languageCode,
                                        int categoryId, int parentId, int filmType, int countryId, int type, int orderBy,
                                        int sortBy,
                                        int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetByCategoryJJChannelPaged(int siteId, string languageCode,
                                                    int categoryId, int parentId, int filmType, int countryId,
                                                    int orderBy, int sortBy,
                                                    int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetByFilmTheater(int siteId, string languageCode,
            int filmType, int countryId, int orderBy, int sortBy,
            int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetByShows(int siteId, string languageCode,
            int categoryId, int parentId, int showType, int countryId, int orderBy, int sortBy,
            int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetByClips(int siteId, string languageCode,
            int categoryId, int parentId, int showType, int orderBy, int sortBy,
            int pageIndex, int pageSize, out int totalRecord);

        IList<FilmInfo> GetByTrailer(int siteId, string languageCode, int pageIndex, int pageSize, out int totalRecord);

        FilmInfo GetFilmsMovie(long filmId, int episodeId);

        FilmInfo GetTrailerMovie(long filmId);

        IList<FilmInfo> GetFilmEpisodes(long filmId, int pageIndex, int pageSize, out int totalRecord);

        int UpdateViewCount(long filmId);

        IList<FilmInfo> GetAllByCategoryId();

        int UpdateCommentCount(long filmId);
    }

    public class FilmService : GenericService<FilmInfo, long>, IFilmService
    {
        private readonly ICacheInfo cacheManager;
        private readonly ICategoryService categoryService;
        public string LanguageCode { get; set; }
        public int SiteId { get; set; }
        public int CategoryId { get; set; }

        public FilmService(IRepository<FilmInfo, long> repository, 
            IEventBus eventBus, ICacheInfo cacheManager, 
            ICategoryService categoryService) 
            : base(repository, eventBus)
        {
            this.cacheManager = cacheManager;
            this.categoryService = categoryService;
        }

        public bool CheckAlias(long id, string alias)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FilmAlias", alias),
                AddInputParameter("@Id", id)
            };

            var result = (int)ExecuteReaderResult("sp_Film_CheckAlias", list.ToArray());
            return result > 0;
        }

        public bool CheckName(string filmName)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FilmName", filmName)
            };

            var result = (int)ExecuteReaderResult("sp_Film_CheckName", list.ToArray());
            return result > 0;
        }

        public List<FilmInfo> SearchPaged(
            string searchText,
            string languageCode,
            int siteId,
            int categoryId,
            int filmTypeId,
            int countryId,
            int directorId,
            int actorId,
            int collectionId,
            int articlesId,
            long createByUserId,
            int serverId,
            int releaseYear,
            DateTime fromDate,
            DateTime toDate,
            DateTime createDate,
            DateTime publishedDate,
            int isPublished,
            int isHot,
            int isHome,
            int isFilmRetail,
            int isFilmLengthEpisodes,
            int status,
            int pageIndex,
            int pageSize,
            out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", searchText),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@CategoryId", categoryId),
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@FilmTypeId", filmTypeId),
                AddInputParameter("@CountryId", countryId),
                AddInputParameter("@DirectorId", directorId),
                AddInputParameter("@ActorId", actorId),
                AddInputParameter("@CollectionId", collectionId),
                AddInputParameter("@ArticlesId", articlesId),
                AddInputParameter("@CreateByUserId", createByUserId),
                AddInputParameter("@ServerId", serverId),
                AddInputParameter("@ReleaseYear", releaseYear),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate),
                AddInputParameter("@CreateDate", createDate),
                AddInputParameter("@PublishedDate", publishedDate),
                AddInputParameter("@IsPublished", isPublished),
                AddInputParameter("@IsHot", isHot),
                AddInputParameter("@IsHome", isHome),
                AddInputParameter("@IsFilmRetail", isFilmRetail),
                AddInputParameter("@IsFilmLengthEpisodes", isFilmLengthEpisodes),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Film_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public string GetLatestFilmCode()
        {
            return (string)ExecuteReaderResult("NextFilmCode");
        }

        public IList<FilmInfo> GetFilmHot(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetFilmHot_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> BuildFilmHot()
        {
            var list = cacheManager.Get(Extensions.Constants.CacheKeys.HOME_FILM_HOT);
            if (list == null)
            {
                int totalRecord;
                list = GetFilmHot(1, 16, out totalRecord);
                cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_HOT);
                cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_HOT, list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetFilmRetail(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetFilmRetail_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> BuildFilmRetail()
        {
            var list = cacheManager.Get(Extensions.Constants.CacheKeys.HOME_FILM_RETAIL);
            if (list == null)
            {
                int totalRecord;
                list = GetFilmRetail(1, 16, out totalRecord);
                cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_RETAIL);
                cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_RETAIL, list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> BuildFilmManyEpisodes()
        {
            var list = cacheManager.Get(Extensions.Constants.CacheKeys.HOME_FILM_MANY_EPISODES);
            if (list == null)
            {
                int totalRecord;
                list = GetFilmManyEpisodes(1, 16, out totalRecord);
                cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_MANY_EPISODES);
                cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_MANY_EPISODES, list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetFilmManyEpisodes(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetFilmManyEpisodes_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> BuildFilmJJChannelIntroduce()
        {
            var list = cacheManager.Get(Extensions.Constants.CacheKeys.HOME_FILM_JJ_CHANNEL_INTRODUCE);
            if (list == null)
            {
                int totalRecord;
                list = GetFilmJJChannelIntroduce(1, 16, out totalRecord);
                cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_JJ_CHANNEL_INTRODUCE);
                cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_JJ_CHANNEL_INTRODUCE, list);
            }

            return (IList<FilmInfo>)list;
        }
        
        public IList<FilmInfo> GetFilmJJChannelIntroduce(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", CategoryId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetFilmJJChannelIntroduce_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> BuildFilmTheater()
        {
            var list = cacheManager.Get(Extensions.Constants.CacheKeys.HOME_FILM_THEATER);
            if (list == null)
            {
                int totalRecord;
                list = GetFilmTheater(1, 16, out totalRecord);
                cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_THEATER);
                cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_THEATER, list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetFilmTheater(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetFilmTheater_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> BuildTVShow()
        {
            var list = cacheManager.Get(Extensions.Constants.CacheKeys.HOME_FILM_TV_SHOWS);
            if (list == null)
            {
                int totalRecord;
                list = GetTVShow(1, 10, out totalRecord);
                cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_TV_SHOWS);
                cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_TV_SHOWS, list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetTVShow(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", CategoryId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetTVShows_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> BuildClips()
        {
            var list = cacheManager.Get(Extensions.Constants.CacheKeys.HOME_FILM_TV_CLIPS);
            if (list == null)
            {
                int totalRecord;
                list = GetClips(1, 16, out totalRecord);
                cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_TV_CLIPS);
                cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_TV_CLIPS, list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetClips(int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", 0),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetClips_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public void RefreshFilmJJChannelIntroduce(int categoryId)
        {
            CategoryId = categoryId;
            var totalRecord = 0;
            cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_JJ_CHANNEL_INTRODUCE);
            cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_JJ_CHANNEL_INTRODUCE, GetFilmJJChannelIntroduce(1, 16, out totalRecord));
        }

        public void RefreshTVShows(int categoryId)
        {
            CategoryId = categoryId;
            var totalRecord = 0;
            cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_TV_SHOWS);
            cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_TV_SHOWS, GetTVShow(1, 10, out totalRecord));
        }

        public void RefreshClips(int categoryId)
        {
            CategoryId = categoryId;
            var totalRecord = 0;
            cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_TV_CLIPS);
            cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_TV_CLIPS, GetClips(1, 16, out totalRecord));
        }

        public FilmInfo GetByIdCache(long id)
        {
            throw new NotImplementedException();
        }

        public IList<FilmInfo> GetAllCache()
        {
            throw new NotImplementedException();
        }

        public IList<FilmInfo> ResetCache()
        {
            int totalRecord;
            cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_HOT);
            cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_HOT, GetFilmHot(1, 16, out totalRecord));
            cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_RETAIL);
            cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_RETAIL, GetFilmRetail(1, 16, out totalRecord));
            cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_MANY_EPISODES);
            cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_MANY_EPISODES, GetFilmManyEpisodes(1, 16, out totalRecord));
            cacheManager.Remove(Extensions.Constants.CacheKeys.HOME_FILM_THEATER);
            cacheManager.Add(Extensions.Constants.CacheKeys.HOME_FILM_THEATER, GetFilmTheater(1, 16, out totalRecord));
            return null;
        }

        public FilmInfo GetFilmDetails(long filmId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@FilmId", filmId)
            };

            return ExecuteReaderRecord<FilmInfo>("sp_Films_GetFilmDetails", list.ToArray());
        }

        public void RefreshStatistical1(int top, int type)
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_1_TYPE, type));
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_1_TYPE, type), GetStatistical1(top, type));
        }

        public IList<FilmInfo> BuildStatistical1(int top, int type)
        {
            var list = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_1_TYPE, type));
            if (list == null)
            {
                list = GetStatistical1(top, type);
                cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_1_TYPE, type));
                cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_1_TYPE, type), list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetStatistical1(int top, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@TopRecord", top),
                AddInputParameter("@Type", type)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetStatistical", list.ToArray());
        }


        public void RefreshStatistical2(int top, int type)
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_2_TYPE, type));
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_2_TYPE, type), GetStatistical2(top, type));
        }

        public void RefreshStatistical3(int top, int type)
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_3_TYPE, type));
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_3_TYPE, type), GetStatistical3(top, type));
        }

        public IList<FilmInfo> GetStatistical4(int top, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@TopRecord", top),
                AddInputParameter("@Type", type)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetStatisticalClips", list.ToArray());
        }

        public void RefreshStatistical4(int top, int type)
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_4_TYPE, type));
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_4_TYPE, type), GetStatistical4(top, type));
        }

        public IList<FilmInfo> BuildStatistical5(int top, int type)
        {
            var list = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_5_TYPE, type));
            if (list == null)
            {
                list = GetStatistical5(top, type);
                cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_5_TYPE, type));
                cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_5_TYPE, type), list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetStatistical5(int top, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@TopRecord", top),
                AddInputParameter("@Type", type)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetStatisticalTrailer", list.ToArray());
        }

        public void RefreshStatistical5(int top, int type)
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_5_TYPE, type));
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_5_TYPE, type), GetStatistical5(top, type));
        }

        public IList<FilmInfo> BuildStatistical4(int top, int type)
        {
            var list = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_4_TYPE, type));
            if (list == null)
            {
                list = GetStatistical4(top, type);
                cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_4_TYPE, type));
                cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_4_TYPE, type), list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetStatistical2(int top, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@TopRecord", top),
                AddInputParameter("@Type", type)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetStatisticalLengthEpisodes", list.ToArray());
        }

        public IList<FilmInfo> BuildStatistical2(int top, int type)
        {
            var list = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_2_TYPE, type));
            if (list == null)
            {
                list = GetStatistical2(top, type);
                cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_2_TYPE, type));
                cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_2_TYPE, type), list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> BuildStatistical3(int top, int type)
        {
            var list = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_3_TYPE, type));
            if (list == null)
            {
                list = GetStatistical3(top, type);
                cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_3_TYPE, type));
                cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.HOME_STATISTICAL_3_TYPE, type), list);
            }

            return (IList<FilmInfo>)list;
        }

        public IList<FilmInfo> GetStatistical3(int top, int type)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@TopRecord", top),
                AddInputParameter("@Type", type)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetStatisticalShows", list.ToArray());
        }

        public IList<FilmInfo> GetByCategoryId(int siteId, string languageCode,
            int categoryId, int parentId, int filmType, int countryId, int type,int orderBy, int sortBy, 
            int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", categoryId),
                AddInputParameter("@ParentId", parentId),
                AddInputParameter("@FilmTypeId", filmType),
                AddInputParameter("@CountryId", countryId),
                AddInputParameter("@Type", type),
                AddInputParameter("@OrderBy", orderBy),
                AddInputParameter("@SortBy", sortBy),               
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Film_GetByCategoryId", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> GetByCategoryJJChannelPaged(int siteId, string languageCode,
            int categoryId, int parentId, int filmType, int countryId, int orderBy, int sortBy,
            int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", categoryId),
                AddInputParameter("@ParentId", parentId),
                AddInputParameter("@FilmTypeId", filmType),
                AddInputParameter("@CountryId", countryId),
                AddInputParameter("@OrderBy", orderBy),
                AddInputParameter("@SortBy", sortBy),               
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Film_GetByCategoryJJChannel_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> GetByFilmTheater(int siteId, string languageCode,
            int filmType, int countryId, int orderBy, int sortBy,
            int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@FilmTypeId", filmType),
                AddInputParameter("@CountryId", countryId),
                AddInputParameter("@OrderBy", orderBy),
                AddInputParameter("@SortBy", sortBy),               
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Film_GetFilmsTheater", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> GetByShows(int siteId, string languageCode, 
            int categoryId, int parentId, int showType, 
            int countryId, int orderBy, int sortBy, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", categoryId),
                AddInputParameter("@ParentId", parentId),
                AddInputParameter("@ShowTypeId", showType),
                AddInputParameter("@CountryId", countryId),
                AddInputParameter("@OrderBy", orderBy),
                AddInputParameter("@SortBy", sortBy),               
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Film_GetShows", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> GetByClips(int siteId, string languageCode, int categoryId, int parentId, int showType, int orderBy, int sortBy, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", categoryId),
                AddInputParameter("@ParentId", parentId),
                AddInputParameter("@ShowTypeId", showType),
                AddInputParameter("@OrderBy", orderBy),
                AddInputParameter("@SortBy", sortBy),               
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Film_GetClips", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public IList<FilmInfo> GetByTrailer(int siteId, string languageCode, 
            int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),              
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Film_GetTrailer", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public FilmInfo GetFilmsMovie(long filmId, int episodeId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", LanguageCode), 
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@FilmId", filmId),
                AddInputParameter("@EpisodeId", episodeId)
            };

            return ExecuteReaderRecord<FilmInfo>("sp_Films_Movies", list.ToArray());
        }

        public FilmInfo GetTrailerMovie(long filmId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@LanguageCode", LanguageCode), 
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@FilmId", filmId)
            };

            return ExecuteReaderRecord<FilmInfo>("sp_Films_TrailerMovies", list.ToArray());
        }

        public IList<FilmInfo> GetFilmEpisodes(long filmId, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@FilmId", filmId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetFilmEpisodes", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public int UpdateViewCount(long filmId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FilmId", filmId)
            };

            return ExecuteNonQuery("sp_Films_UpdateViewCount", list.ToArray());
        }

        public int UpdateCommentCount(long filmId)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FilmId", filmId)
            };

            return ExecuteNonQuery("sp_Films_UpdateCommentCount", list.ToArray());
        }

        public IList<FilmInfo> GetAllByCategoryId()
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SiteId", SiteId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@CategoryId", CategoryId)
            };

            return ExecuteReader<FilmInfo>("sp_Films_GetAllByCategoryId", list.ToArray());
        }
    }
}
