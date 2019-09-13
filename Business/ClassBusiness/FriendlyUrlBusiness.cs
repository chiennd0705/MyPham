using Business.bases;
using Common;
using Common.exception;
using DataAccess.DAO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Business.ClassBusiness
{
    public class FriendlyUrlBusiness : AbstractBaseBusiness<FriendlyUrl, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FriendlyUrlBusiness()
            : base(new FriendlyUrlDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(FriendlyUrl obj)
        {
            try
            {
                base.AddNew(obj);
                DataContext.SaveChanges();
            }
            catch (SqlException ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public override void Edit(FriendlyUrl obj)
        {
            try
            {
                base.Edit(obj);
                DataContext.SaveChanges();
            }
            catch (SqlException ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public override void Remove(long id)
        {
            try
            {
                base.Remove(id);
                DataContext.SaveChanges();
            }
            catch (SqlException ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public bool GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<Common.FriendlyUrl> query = GetDynamicQuery();
            if (query.Where(p => p.Link == key).ToList().Count < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public long InsertLink(FriendlyUrl aluplink)
        {
            if (!GetByKey(aluplink.NameLink))
            {
                AddNew(aluplink);
            }
            return aluplink.Id;
        }

        public void UpdateLink(string linkcu, FriendlyUrl aluplink)
        {
            if (linkcu != "")
            {
                IQueryable<Common.FriendlyUrl> query = GetDynamicQuery();
                var getList = query.Where(p => p.Link.Contains(linkcu)).ToList();
                foreach (var q in getList)
                {
                    base.Remove(q.Id);
                }
                DataContext.SaveChanges();
            }

            if (!GetByKey(aluplink.Link))
            {
                AddNew(aluplink);
            }
        }

        public List<FriendlyUrl> searchAllLink()
        {
            IQueryable<Common.FriendlyUrl> query = GetDynamicQuery();
            var getList = query.OrderByDescending(p => p.Order);

            return getList.ToList();
        }

        public List<FriendlyUrl> searchByName(string name)
        {
            IQueryable<Common.FriendlyUrl> query = GetDynamicQuery();
            var listResults = GetDynamicQuery().OrderByDescending(x => x.Id).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                listResults = query.Where(p => p.Link.Contains(name)).OrderByDescending(p => p.Order).ToList();
            }

            return listResults;
        }
    }
}