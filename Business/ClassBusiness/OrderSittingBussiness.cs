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
    public class OrderSittingBusiness : AbstractBaseBusiness<OrderSitting, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public OrderSittingBusiness()
            : base(new OrderSittingDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(OrderSitting obj)
        {
            try
            {
                CheckValidate(obj);

                base.AddNew(obj);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //write log
                _logger.Error("", ex);
                //throw ex;
            }/*
            catch (Exception ex)
            {
                _logger.Error("", ex);
                if (ex is SqlException)
                {
                    throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, Constants.Error.SqlException);
                }
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, Constants.Error.SqlUnhandler);
                //write log
            }*/
        }

        public override void Edit(OrderSitting obj)
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

        public List<OrderSitting> GetList(string key)
        {
            var listResults = GetDynamicQuery().OrderByDescending(x => x.Id).ToList();

            return listResults;
        }

        public List<Common.OrderSitting> GetByKey(string key)
        {
            IQueryable<Common.OrderSitting> query = GetDynamicQuery();
            if (string.IsNullOrEmpty(key))
            {
                key = "";
                return query.ToList();
            }

            try
            {
                var number = double.Parse(key);
                return query.ToList();
            }
            catch (FormatException)
            {
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "FormatException");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        public void CheckValidate(OrderSitting obj)
        {
        }
    }
}