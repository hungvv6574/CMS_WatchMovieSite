using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ISmsMessageService : IGenericService<SmsMessageInfo, int>, IDependency
    {

    }

    public class SmsMessageService : GenericService<SmsMessageInfo, int>, ISmsMessageService
    {
        public SmsMessageService(IRepository<SmsMessageInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }
    }
}