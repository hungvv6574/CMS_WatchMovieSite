﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CMSSolutions.Caching;
using CMSSolutions.Data;
using CMSSolutions.Events;
using CMSSolutions.Services;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Services
{
    public interface ICategoryService : ICacheService<CategoryInfo, int>, IGenericService<CategoryInfo, int>, IDependency
    {
        bool CheckAlias(int id, string alias);

        string LanguageCode { get; set; }

        int SiteId { get; set; }

        CategoryInfo GetByIdCache(int id);

        CategoryInfo GetHomePage();

        string GetCategoryName(int id);

        IList<CategoryInfo> GetPaged(bool isDeleted, int pageIndex, int pageSize, out int totals);

        IList<CategoryInfo> ResetCache();

        CategoryInfo GetByAlias(string alias);

        List<CategoryInfo> GetChildenByParentId(int parentId);

        List<CategoryInfo> GetAllParent();

        IList<CategoryInfo> GetAllCache();

        List<CategoryInfo> GetTree();
    }

    public class CategoryService : GenericService<CategoryInfo, int>, ICategoryService
    {
        private readonly ICacheInfo cacheManager;

        public string LanguageCode { get; set; }

        public int SiteId { get; set; }

        public CategoryService(
            IRepository<CategoryInfo, int> repository, 
            IEventBus eventBus,
            ICacheInfo cacheManager)
            : base(repository, eventBus)
        {
            this.cacheManager = cacheManager;
        }

        public override void Insert(CategoryInfo record)
        {
            Repository.Insert(record);
            ResetCache();
        }

        public override void Update(CategoryInfo record)
        {
            Repository.Update(record);
            ResetCache();
        }

        public override void Delete(CategoryInfo record)
        {
            Repository.Delete(record);
            ResetCache();
        }

        public override void Save(CategoryInfo record)
        {
            if (record.IsTransient())
            {
                Insert(record);
            }
            else
            {
                Update(record);
            }

            ResetCache();
        }

        public CategoryInfo GetHomePage()
        {
            var list = GetAllCache();
            if (list != null && list.Count > 0)
            {
                return list.FirstOrDefault(x => x.IsHome && x.IsActived);
            }

            return null;
        }

        public string GetCategoryName(int id)
        {
            var list = GetAllCache();
            if (list != null && list.Count > 0)
            {
                return list.Where(x => x.Id == id && !x.IsActived).Select(x => x.Name).FirstOrDefault();
            }

            return string.Empty;
        }

        public bool CheckAlias(int id, string alias)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Alias", alias),
                AddInputParameter("@Id", id)
            };

            var result = (int)ExecuteReaderResult("sp_Categories_CheckAlias", list.ToArray());
            return result > 0;
        }

        public CategoryInfo GetByAlias(string alias)
        {
            var list = GetAllCache();
            if (list != null && list.Count > 0)
            {
                return list.FirstOrDefault(x => x.Alias.ToLower() == alias.ToLower() && x.IsActived);
            }

            return null;
        }

        public List<CategoryInfo> GetTree()
        {
            var listParent = GetAllParent();
            var list = new List<CategoryInfo>();
            if (listParent != null)
            {
                foreach (var parent in listParent)
                {
                    parent.ChildenName = parent.ShortName;
                    parent.HasChilden = true;
                    list.Add(parent);
                    list.AddRange(GetChildenByParentId(parent.Id));
                }
            }

            return list;
        }
       
        public List<CategoryInfo> GetChildenByParentId(int parentId)
        {
            var list = GetAllCache();
            var results = new List<CategoryInfo>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (parentId > 0 && item.ParentId == parentId && item.IsActived)
                    {
                        item.HasChilden = false;
                        item.ChildenName = "--- " + item.ShortName;
                        results.Add(item);
                        results.AddRange(GetChildenByParentId2(item.Id));
                    }
                }

                return results;
            }

            return null;
        }

        public List<CategoryInfo> GetChildenByParentId2(int parentId)
        {
            var list = GetAllCache();
            var results = new List<CategoryInfo>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (parentId > 0 && item.ParentId == parentId && item.IsActived)
                    {
                        item.HasChilden = false;
                        item.ChildenName = "--- --- " + item.ShortName;
                        results.Add(item);
                    }
                }

                results.Sort((foo1, foo2) => foo1.OrderBy.CompareTo(foo2.OrderBy));

                return results;
            }

            return null;
        }

        public List<CategoryInfo> GetAllParent()
        {
            var list = GetAllCache();
            if (list != null && list.Count > 0)
            {
                return list.Where(x => x.ParentId == 0 && x.IsActived).ToList();
            }

            return null;
        }

        public CategoryInfo GetByIdCache(int id)
        {
            var list = GetAllCache();
            if (list != null && list.Count > 0)
            {
                return list.FirstOrDefault(x => x.Id == id && x.IsActived);
            }

            return null;
        }

        public IList<CategoryInfo> GetAllCache()
        {
            var results = cacheManager.Get(string.Format(Extensions.Constants.CacheKeys.CATEGORY_ALL_TABLE, LanguageCode, SiteId));
            if (results == null)
            {
                results = ResetCache();
            }

            var list = (List<CategoryInfo>) results;
            if (results != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    foreach (var check in list)
                    {
                        if (check.Id == item.ParentId)
                        {
                            item.ParentName = check.ShortName;
                            break;
                        }
                    }
                }

                return list;
            }

            return new List<CategoryInfo>();
        }

        public IList<CategoryInfo> GetPaged(bool isDeleted, int pageIndex, int pageSize, out int totals)
        {
            var results = GetAllCache();
            if (results != null)
            {
                var list = results.Where(y => y.IsDeleted == isDeleted);
                totals = list.Count();
                var users = (from x in list select x).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return users;
            }

            totals = 0;
            return new List<CategoryInfo>();
        }

        public IList<CategoryInfo> ResetCache()
        {
            cacheManager.Remove(string.Format(Extensions.Constants.CacheKeys.CATEGORY_ALL_TABLE, LanguageCode, SiteId));
            var table = Repository.Table.Where(x => x.LanguageCode == LanguageCode && x.SiteId == SiteId).ToList();
            cacheManager.Add(string.Format(Extensions.Constants.CacheKeys.CATEGORY_ALL_TABLE, LanguageCode, SiteId), table);

            return table;
        }
    }
}
