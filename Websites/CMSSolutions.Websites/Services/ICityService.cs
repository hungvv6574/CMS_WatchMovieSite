using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ICityService : IGenericService<CityInfo, int>, IDependency
    {

    }

    public class CityService : GenericService<CityInfo, int>, ICityService
    {
        public CityService(IRepository<CityInfo, int> repository, IEventBus eventBus) : base(repository, eventBus)
        {

        }
    }
}