using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ICardTypeService : IGenericService<CardTypeInfo, int>, IDependency
    {
        IList<CardTypeInfo> GetPaged(int status, int pageIndex, int pageSize, out int totals);

        CardTypeInfo GetByCode(string code, int hasSerial);
    }

    public class CardTypeService : GenericService<CardTypeInfo, int>, ICardTypeService
    {
        public CardTypeService(IRepository<CardTypeInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<CardTypeInfo> GetPaged(int status, int pageIndex, int pageSize, out int totals)
        {
            var results = Repository.Table.Where(x => x.Status == status).ToList();
            {
                totals = results.Count();
                return (from x in results select x).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public CardTypeInfo GetByCode(string code, int hasSerial)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Code", code),
                AddInputParameter("@HasSerial", hasSerial)
            };

            return ExecuteReaderRecord<CardTypeInfo>("sp_CardTypes_GetByCode", list.ToArray());
        }
    }
}
