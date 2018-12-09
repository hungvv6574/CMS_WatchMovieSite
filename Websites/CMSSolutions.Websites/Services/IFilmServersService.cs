using System.Collections.Generic;
using System.Linq;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IFilmServersService : IGenericService<FilmServerInfo, int>, IDependency
    {
        IList<FilmServerInfo> GetPaged(string languageCode, int siteId, int status, int pageIndex, int pageSize, out int totals);
    }

    public class FilmServersService : GenericService<FilmServerInfo, int>, IFilmServersService
    {
        public FilmServersService(IRepository<FilmServerInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<FilmServerInfo> GetPaged(string languageCode, int siteId, int status, int pageIndex, int pageSize, out int totals)
        {
            var results = Repository.Table.Where(x => x.Status == status && x.LanguageCode == languageCode && x.SiteId == siteId).ToList();
            {
                totals = results.Count();
                return (from x in results select x).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}
