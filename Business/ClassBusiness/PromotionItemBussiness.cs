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
    public class PromotionItemBusiness : AbstractBaseBusiness<PromotionItem, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PromotionItemBusiness()
            : base(new PromotionItemDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public List<PromotionItem> GetList(string key)
        {
            IQueryable<PromotionItem> query = GetDynamicQuery();
            var temp = query.Where(p => p.Title.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<PromotionItem> GetListByAlbum(string key)
        {
            try
            {
                IQueryable<PromotionItem> query = GetDynamicQuery();
                var temp = query.Where(p => p.Title.Contains(key)).OrderByDescending(p => p.Id);
                return temp.ToList();
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

        public override void AddNew(PromotionItem obj)
        {
            try
            {
                CheckValidate(obj);

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

        public override void Edit(PromotionItem obj)
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

        public void CheckValidate(PromotionItem obj)
        {
        }
    }
}