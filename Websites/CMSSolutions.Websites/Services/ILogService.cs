using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ILogService : IGenericService<LogInfo, long>, IDependency
    {
        IList<LogInfo> GetPaged(string keyword, DateTime fromDate, DateTime toDate, int type, int status, int pageIndex, int pageSize, out int totals);
    }

    public class LogService : GenericService<LogInfo, long>, ILogService
    {
        public LogService(IRepository<LogInfo, long> repository, IEventBus eventBus) 
            : base(repository, eventBus)
        {

        }

        protected override IOrderedQueryable<LogInfo> MakeDefaultOrderBy(IQueryable<LogInfo> queryable)
        {
            return queryable.OrderByDescending(x => x.CreateDate);
        }

        public IList<LogInfo> GetPaged(string keyword, DateTime fromDate, DateTime toDate, 
            int type, int status, int pageIndex, int pageSize, out int totals)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", keyword),
                AddInputParameter("@FromDate", fromDate),
                AddInputParameter("@ToDate", toDate),
                AddInputParameter("@Type", type),
                AddInputParameter("@Status", status),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<LogInfo>("sp_Logs_Search_Paged", "@TotalRecord", out totals, list.ToArray());
        }
    }
}