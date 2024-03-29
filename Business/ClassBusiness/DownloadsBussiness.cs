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
using System.ServiceModel;

namespace Business.ClassBusiness
{
    public class DownloadsBusiness : AbstractBaseBusiness<Downloads, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DownloadsBusiness()
            : base(new DownloadsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Downloads obj)
        {
            try
            {
                CheckValidate(obj);
                obj.CreateDate = DateTime.Now;
                obj.Modifydate = DateTime.Now;
                base.AddNew(obj);
                DataContext.SaveChanges();
            }
            catch (FaultException ex)
            {
                //write log
                _logger.Error("", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                if (ex is SqlException)
                {
                    throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, Constants.Error.SqlException);
                }
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, Constants.Error.SqlUnhandler);
                //write log
            }
        }

        public override void Edit(Downloads obj)
        {
            try
            {
                CheckValidate(obj);
                obj.Modifydate = DateTime.Now;
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
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public List<Downloads> GetList(string key)
        {
            IQueryable<Downloads> query = GetDynamicQuery();
            var temp = query.Where(p => p.FileName.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Downloads> GetListID(long id, string Searchtext, int size)
        {
            IQueryable<Downloads> query = GetDynamicQuery();
            List<Downloads> temp = new List<Downloads>();
            if (id != 0)
            {
                temp = query.Where(p => p.CatalogDownloadID == id & p.FileName.Contains(Searchtext)).OrderByDescending(p => p.Id).Take(size).ToList();
            }
            else
            {
                temp = query.Where(p => p.FileName.Contains(Searchtext)).OrderByDescending(p => p.Id).Take(size).ToList();
            }
            return temp;
        }

        public List<Downloads> GetList()
        {
            IQueryable<Downloads> query = GetDynamicQuery();
            var temp = query.OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Downloads> GetByArrayId(long[] arrayId)
        {
            IQueryable<Downloads> query = GetDynamicQuery();
            var temp = query.Where(p => arrayId.Contains(p.Id));
            return temp.ToList();
        }

        public List<Downloads> ListAllDownload(ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            IQueryable<Common.Downloads> query = GetDynamicQuery();

            totalRecord = query.Count();
            var model = query.ToList();
            model = model.OrderByDescending(x => x.Modifydate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model.ToList();
        }

        public void CheckValidate(Downloads obj)
        {
        }
    }
}