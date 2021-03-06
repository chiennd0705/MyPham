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
    public class TagsBusiness : AbstractBaseBusiness<Tag, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TagsBusiness()
            : base(new TagsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Tag obj)
        {
            try
            {
                CheckValidate(obj);
                obj.DateCreate = DateTime.Now;
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

        public override void Edit(Tag obj)
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

        public List<Tag> GetList(string key)
        {
            IQueryable<Tag> query = GetDynamicQuery();
            var temp = query.Where(p => p.TagName.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Tag> GetList(int type, string key)
        {
            IQueryable<Tag> query = GetDynamicQuery();
            var temp = query.Where(p => p.Type == type && p.TagName.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Tag> GetList()
        {
            IQueryable<Tag> query = GetDynamicQuery();
            var temp = query.OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Tag> GetByArrayId(long[] arrayId)
        {
            IQueryable<Tag> query = GetDynamicQuery();
            var temp = query.Where(p => arrayId.Contains(p.Id));
            return temp.ToList();
        }

        public Tag GetByUrl(string tagUrl)
        {
            IQueryable<Tag> query = GetDynamicQuery();
            var temp = query.Where(p => p.TagUrl == tagUrl);
            return temp.First();
        }

        public void CheckValidate(Tag obj)
        {
        }
    }
}