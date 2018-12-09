using System.Collections.Generic;
using System.Data.SqlClient;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IFilmVideoService : IGenericService<FilmVideoInfo, long>, IDependency
    {
        IList<FilmVideoInfo> GetByFilm(long filmId, int pageIndex, int pageSize, out int totalRecord);

        FilmVideoInfo GetByFilmFile(long filmId, long fileId);

        IList<FilmVideoInfo> GetFilesByFile(string folderPath);
    }

    public class FilmVideoService : GenericService<FilmVideoInfo, long>, IFilmVideoService
    {
        public FilmVideoService(IRepository<FilmVideoInfo, long> repository, IEventBus eventBus)
            : base(repository, eventBus)
        {

        }

        public IList<FilmVideoInfo> GetByFilm(long filmId, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FilmId", filmId),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<FilmVideoInfo>("sp_FilmVideos_GetByFilm", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public FilmVideoInfo GetByFilmFile(long filmId, long fileId)
        {
             var list = new List<SqlParameter>
            {
                AddInputParameter("@FilmId", filmId),
                AddInputParameter("@FileId", fileId)
            };

            return ExecuteReaderRecord<FilmVideoInfo>("sp_FilmVideos_GetByFilmFile", list.ToArray());
        }

        public IList<FilmVideoInfo> GetFilesByFile(string folderPath)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@FolderPath", folderPath)
            };

            return ExecuteReader<FilmVideoInfo>("sp_FilmVideos_GetFilesByFolder", list.ToArray());
        }
    }
}
