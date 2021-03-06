using Business.bases;
using Common;
using Common.exception;
using DataAccess.DAO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Business.ClassBusiness
{
    public class ShopsBusiness : AbstractBaseBusiness<Shop, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ShopsBusiness()
            : base(new ShopsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Shop obj)
        {
            try
            {
                CheckValidate(obj);
                obj.CreateDate = DateTime.Now;
                obj.ModifyDate = DateTime.Now;
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

        public override void Edit(Shop obj)
        {
            try
            {
                CheckValidate(obj);
                obj.ModifyDate = DateTime.Now;
                base.Edit(obj);
                DataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw ObjectUtil.CreateFaultException(CodedException.NullEntity, "Vui lòng nhập các ô có dấu *");
            }
            catch (MissingFieldException ms)
            {
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
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

        /// <summary>
        /// Phương thức lọc Shop
        /// </summary>
        /// <param name="key">Khóa tìm kiếm</param>
        /// <returns>Danh sách Shop </returns>
        public List<Common.Shop> GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<Common.Shop> query = GetDynamicQuery();
            return query.Where(p => p.ShopName.Contains(key) || p.Phone.Contains(key)).ToList();
        }

        public void CheckValidate(Shop obj)
        {
        }
    }
}