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
    public class VideosBusiness : AbstractBaseBusiness<Videos, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public VideosBusiness()
            : base(new VideosDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Videos obj)
        {
            try
            {
                CheckValidate(obj);
                obj.DateCreate = DateTime.Now;

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

        public override void Edit(Videos obj)
        {
            try
            {
                CheckValidate(obj);

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

        public List<Videos> GetList(string key)
        {
            IQueryable<Videos> query = GetDynamicQuery();
            var temp = query.Where(p => p.VideoName.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Videos> GetList()
        {
            IQueryable<Videos> query = GetDynamicQuery();
            var temp = query.OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Videos> GetByArrayId(long[] arrayId)
        {
            IQueryable<Videos> query = GetDynamicQuery();
            var temp = query.Where(p => arrayId.Contains(p.Id));
            return temp.ToList();
        }

        public List<Videos> ListAllVideo(ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            IQueryable<Common.Videos> query = GetDynamicQuery();

            totalRecord = query.Count();
            var model = query.ToList();
            model = model.OrderByDescending(x => x.DateCreate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model.ToList();
        }

        public void CheckValidate(Videos obj)
        {
        }
    }
}