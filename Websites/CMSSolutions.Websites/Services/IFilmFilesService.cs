using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IFilmFilesService : IGenericService<FilmFilesInfo, long>, IDependency
    {
        FilmFilesInfo GetByFullPathFile(string fullPath);

        IList<FilmFilesInfo> GetDayFolderByRoot(string folderName);

        IList<FilmFilesInfo> GetChildrenByRootFolders(string folderName);

        List<FilmFilesInfo> SearchPaged(
            string folderName,
            string languageCode,
            int siteId,
            int serverId,
            int pageIndex,
            int pageSize,
            out int totalRecord);
    }

    public class FilmFilesService : GenericService<FilmFilesInfo, long>, IFilmFilesService
    {
        public FilmFilesService(IRepository<FilmFilesInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public FilmFilesInfo GetByFullPathFile(string fullPath)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FullPath", fullPath)
            };

            return ExecuteReaderRecord<FilmFilesInfo>("sp_FilmFiles_GetByFullPathFile", list.ToArray());
        }

        public IList<FilmFilesInfo> GetDayFolderByRoot(string folderName)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FolderRoot", folderName)
            };

            return ExecuteReader<FilmFilesInfo>("sp_FilmFiles_GetDayFolderByRoot", list.ToArray());
        }

        public IList<FilmFilesInfo> GetChildrenByRootFolders(string folderName)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FolderDay", folderName)
            };

            return ExecuteReader<FilmFilesInfo>("sp_FilmFiles_GetChildrenByRootFolders", list.ToArray());
        }

        public List<FilmFilesInfo> SearchPaged(string folderName, string languageCode, int siteId, int serverId, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FolderName", folderName),
                AddInputParameter("@LanguageCode", languageCode),
                AddInputParameter("@SiteId", siteId),
                AddInputParameter("@ServerId", serverId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmFilesInfo>("sp_FilmFiles_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}
