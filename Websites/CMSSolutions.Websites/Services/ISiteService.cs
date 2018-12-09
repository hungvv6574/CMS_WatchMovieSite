using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ISiteService : IGenericService<SiteInfo, int>, IDependency
    {

    }

    public class SiteService : GenericService<SiteInfo, int>, ISiteService
    {
        public SiteService(IRepository<SiteInfo, int> repository,
            IEventBus eventBus)
            : base(repository, eventBus)
        {

        }
    }
}
