using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface IBankCardService : IGenericService<BankCardInfo, int>, IDependency
    {
        IList<BankCardInfo> GetPaged(int status, int pageIndex, int pageSize, out int totals);

        BankCardInfo GetByCode(string code);
    }

    public class BankCardService : GenericService<BankCardInfo, int>, IBankCardService
    {
        public BankCardService(IRepository<BankCardInfo, int> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        public IList<BankCardInfo> GetPaged(int status, int pageIndex, int pageSize, out int totals)
        {
            var results = Repository.Table.Where(x => x.Status == status).ToList();
            {
                totals = results.Count();
                return (from x in results select x).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public BankCardInfo GetByCode(string code)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Code", code)
            };

            return ExecuteReaderRecord<BankCardInfo>("sp_BankCards_GetByCode", list.ToArray());
        }
    }
}
