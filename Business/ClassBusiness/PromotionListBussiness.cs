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
    public class PromotionListBusiness : AbstractBaseBusiness<PromotionList, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PromotionListBusiness()
            : base(new PromotionListDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(PromotionList obj)
        {
            try
            {
                //   CheckValidate(obj);
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

        public override void Edit(PromotionList obj)
        {
            try
            {
                // CheckValidate(obj);
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

        public List<PromotionList> GetList(string key)
        {
            IQueryable<PromotionList> query = GetDynamicQuery();
            var temp = query.Where(p => p.Title.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<PromotionList> GetList()
        {
            IQueryable<PromotionList> query = GetDynamicQuery();
            var temp = query.Where(p => p.Status == 1).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        //khi bấm vào nút xóa sẽ thay đổi trạng thái danh mục
        public int ChangeStatusCatalog(long id)
        {
            var catalog = GetById(id);
            if (catalog.Status == 0)
            {
                catalog.Status = 1;
            }
            else
            {
                catalog.Status = 0;
            }
            base.Edit(catalog);
            DataContext.SaveChanges();
            return catalog.Status;
        }

        public void CheckValidate(PromotionList obj)
        {
        }
    }
}