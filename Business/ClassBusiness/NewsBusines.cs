using Business.bases;
using Common;
using DataAccess.DAO;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Business.ClassBusiness
{
    public class NewsBusiness : AbstractBaseBusiness<News, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Hàm tạo
        /// </summary>
        public NewsBusiness()
            : base(new NewsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(News obj)
        {
            base.AddNew(obj);
            DataContext.SaveChanges();
            //    return obj.Id;
        }

        public override void Edit(News obj)
        {
            base.Edit(obj);
            DataContext.SaveChanges();
        }

        public override void Remove(long id)
        {
            base.Remove(id);
            DataContext.SaveChanges();
        }

        public List<Common.News> GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<Common.News> query = GetDynamicQuery();
            return query.Where(p => p.Title.Contains(key) || p.Summary.Contains(key)).OrderByDescending(p => p.Id).ToList();
        }

        public List<SearchNewByGroupID_Result> ListByNewsIdNewsGroup(long? newsGroupId, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {


            var NewEntities = new BuyGroup365Entities();

            var listResults = NewEntities.SearchNewByGroupID(newsGroupId, pageIndex, pageSize).ToList();
            totalRecord = NewEntities.SearchNewByGroupID(newsGroupId, 1, 10000).Count();
            return listResults;


        }

        public List<News> ListByNewsIdNewsGroup(long newsGroupId)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(x => x.NewsGroupId == newsGroupId).OrderByDescending(p => p.ModifyDate);

            return getListById.ToList();
        }

        public List<News> ListByNewsIdNewsGroup(long newsGroupId, int size)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(x => x.NewsGroupId == newsGroupId).OrderByDescending(p => p.ModifyDate);

            return getListById.Take(size).ToList();
        }

        public List<News> ListByNewsIdNewsGroupActive(long newsGroupId, int size)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(x => x.NewsGroupId == newsGroupId && x.IsActive == true).OrderByDescending(p => p.ModifyDate);

            return getListById.Take(size).ToList();
        }
        public List<News> ListByNewsActive( int size)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(x => x.IsActive == true).OrderByDescending(p => p.ModifyDate);

            return getListById.Take(size).ToList();
        }
        public List<News> ListByNews(int size)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(p => p.isPublic == true).OrderByDescending(p => p.ModifyDate);

            return getListById.Take(size).ToList();
        }

        public List<News> ListByNewsMoinhat(int size)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(p => p.isPublic == true).OrderByDescending(p => p.CreateDate);

            return getListById.Take(size).ToList();
        }

        public List<News> GetByTag(string tag, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(x => x.Tags.Contains(tag));

            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.ModifyDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model.ToList();
        }

        public int CountByTag(string tag)
        {
            int obj = GetDynamicQuery().Where(x => x.Tags.Contains(tag)).OrderByDescending(x => x.ModifyDate).Count();
            return obj;
        }

        public List<News> GetByTagAuthor(string tag, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<Common.News> query = GetDynamicQuery();
            var getListById = query.Where(x => x.Author.Contains(tag));

            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.ModifyDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model.ToList();
        }

        public int CountByTagAuthor(string tag)
        {
            int obj = GetDynamicQuery().Where(x => x.Author.Contains(tag)).OrderByDescending(x => x.Id).Count();
            return obj;
        }

        public List<SearchNewByName_Result> SearchListNewByName(string key)
        {
            var ProductEntities = new BuyGroup365Entities();

            var listResults = ProductEntities.SearchNewByName(key).ToList();

            return listResults;
        }
    }
}