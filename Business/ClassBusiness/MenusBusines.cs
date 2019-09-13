using Business.bases;
using Common;
using DataAccess.DAO;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Business.ClassBusiness
{
    public class MenusBusiness : AbstractBaseBusiness<Menus, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Hàm tạo
        /// </summary>
        public MenusBusiness()
            : base(new MenusDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Menus obj)
        {
            base.AddNew(obj);
            DataContext.SaveChanges();
        }

        public override void Edit(Menus obj)
        {
            base.Edit(obj);
            DataContext.SaveChanges();
        }

        public override void Remove(long id)
        {
            base.Remove(id);
            DataContext.SaveChanges();
        }

        public List<Common.Menus> GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<Common.Menus> query = GetDynamicQuery();
            return query.Where(p => p.MenuName.Contains(key)).OrderByDescending(p => p.Order).ToList();
        }

        public List<Menus> GetList(string key)
        {
            IQueryable<Menus> query = GetDynamicQuery();
            var temp = query.Where(p => p.MenuName.Contains(key)).OrderByDescending(p => p.Order);
            return temp.ToList();
        }

        public List<Menus> GetList()
        {
            IQueryable<Menus> query = GetDynamicQuery();
            var temp = query.OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Menus> SearchMenusByParentId(long Id)
        {
            IQueryable<Menus> query = GetDynamicQuery();
            List<Menus> list = query.Where(x => x.ParentId == Id).ToList();

            return list;
        }

        public List<Menus> SearchMenusByParentId(long Id, int typemenu)
        {
            IQueryable<Menus> query = GetDynamicQuery();
            List<Menus> list = query.Where(x => x.ParentId == Id && x.TypeMenu == typemenu).OrderByDescending(p => p.Order).ToList();

            return list;
        }
    }
}