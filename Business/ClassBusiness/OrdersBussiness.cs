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
    public class OrdersBusiness : AbstractBaseBusiness<Order, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public OrdersBusiness()
            : base(new OrdersDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Order obj)
        {
            try
            {
                CheckValidate(obj);
                obj.CreateDate = DateTime.Now;
                obj.ModifyDate = DateTime.Now;
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

        public override void Edit(Order obj)
        {
            try
            {
                CheckValidate(obj);
                obj.ModifyDate = DateTime.Now;
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

        public List<Order> GetList(long? shopid, int? state, int? paid)
        {
            var listResults = GetDynamicQuery().OrderByDescending(x => x.Id).ToList();
            if (shopid != -1 && shopid != null)
            {
                listResults = listResults.Where(x => x.IdShop == shopid).ToList();
            }
            if (state != null && state != -1)
            {
                listResults = listResults.Where(x => x.Status == state).ToList();
            }
            if (paid != null && paid != -1)
            {
                listResults = listResults.Where(x => x.Paid == paid).ToList();
            }
            return listResults;
        }

        public List<Common.Order> GetByKey(string key)
        {
            IQueryable<Common.Order> query = GetDynamicQuery();
            if (string.IsNullOrEmpty(key))
            {
                key = "";
                return query.ToList();
            }

            try
            {
                var number = double.Parse(key);
                return query.Where(p => p.IdShop == number || p.GramGood == number || p.FeeOfTranspot == number || p.IdPayForm == number).ToList();
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

        public void CheckValidate(Order obj)
        {
        }
    }
}