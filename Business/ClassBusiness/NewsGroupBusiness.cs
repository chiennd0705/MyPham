using Business.bases;
using Common;
using DataAccess.DAO;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Business.ClassBusiness
{
    public class NewsGroupBusiness : AbstractBaseBusiness<NewsGroups, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Hàm tạo
        /// </summary>
        public NewsGroupBusiness()
            : base(new NewsGroupDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(NewsGroups obj)
        {
            base.AddNew(obj);
            DataContext.SaveChanges();
        }

        public override void Edit(NewsGroups obj)
        {
            base.Edit(obj);
            DataContext.SaveChanges();
        }

        public override void Remove(long id)
        {
            base.Remove(id);
            DataContext.SaveChanges();
        }

        /// <summary>
        /// Phương thức tìm tất cả các module trong role
        /// </summary>
        /// <param name="roleId">ID role cần tìm</param>
        /// <returns>Danh sách các module nằm trong group</returns>

        public List<NewsGroups> GetListNewsGroup()
        {
            IQueryable<NewsGroups> list = GetDynamicQuery();
            return list.ToList();
        }

        public List<Common.NewsGroups> GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<Common.NewsGroups> query = GetDynamicQuery();
            return query.Where(p => p.NewsGroupName.Contains(key)).ToList();
        }

        public NewsGroups GetNewsGroupById(long id)
        {
            var newsGroup = base.GetById(id);
            return newsGroup;
        }

        public List<Common.NewsGroups> GetByParentId(long id)
        {
            IQueryable<Common.NewsGroups> query = GetDynamicQuery();
            return query.Where(p => p.ParentId.Equals(id) && p.IsView1).ToList();
        }

        public List<Common.NewsGroups> GetViewFooter()
        {
            IQueryable<Common.NewsGroups> query = GetDynamicQuery();
            return query.Where(p => p.IsView2).ToList();
        }

        public List<NewsGroups> SearchCategoryByName()
        {
            List<NewsGroups> obj = new List<NewsGroups>();
            var list = SearchCategoryByParentId("", -1);
            List<NewsGroups> listDropdowCate = new List<NewsGroups>();
            listDropdowCate.AddRange(list);
            return list;
        }

        public List<NewsGroups> SearchNewGroupsByParentId(long ParentId)
        {
            IQueryable<NewsGroups> query = GetDynamicQuery();
            var listCatalog = query.Where(p => p.ParentId == ParentId && p.Status == 1 && p.IsView1 == true).OrderBy(p => p.CreateDate).ToList();
            return listCatalog;
        }

        public List<NewsGroups> SearchListNewGroups(long ParentId)
        {
            IQueryable<NewsGroups> query = GetDynamicQuery();
            var listCatalog = query.Where(p => p.ParentId == ParentId && p.Status == 1).OrderBy(p => p.CreateDate).ToList();
            return listCatalog;
        }

        public List<NewsGroups> SearchCategoryByParentId(string sp, long paId)
        {
            List<NewsGroups> listDropdowCate = new List<NewsGroups>();
            List<NewsGroups> list = GetDynamicQuery().Where(x => x.ParentId == paId).ToList();
            //List<Catalog> list = listcate.Where(x => x.ParentId == paId).ToList();
            if (list.Any())
            {
                foreach (var catalog in list)
                {
                    var str = sp + catalog.NewsGroupName;
                    catalog.NewsGroupName = str;

                    listDropdowCate.Add(catalog);
                    var list1 = SearchCategoryByParentId(sp + "-----", catalog.Id);
                    listDropdowCate.AddRange(list1);
                }
            }

            return listDropdowCate;
        }
    }
}