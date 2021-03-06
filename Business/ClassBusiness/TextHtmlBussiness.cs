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
    public class TextHtmlBusiness : AbstractBaseBusiness<TextHtml, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TextHtmlBusiness()
            : base(new TextHtmlDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(TextHtml obj)
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

                throw ex;
                //write log
            }
        }

        public override void Edit(TextHtml obj)
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

        public TextHtml GetByCode(string code)
        {
            try
            {
                IQueryable<TextHtml> query = GetDynamicQuery();
                var temp = query.Where(p => p.Code == code).First();
                return temp;
            }
            catch
            {
                TextHtml text = new TextHtml();
                return text;
            }
        }

        public List<TextHtml> GetList(string key)
        {
            IQueryable<TextHtml> query = GetDynamicQuery();
            var temp = query.Where(p => p.Title.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        //khi bấm vào nút xóa sẽ thay đổi trạng thái danh mục
        public int ChangeStatusCatalog(long id, int _status)
        {
            var catalog = GetById(id);

            catalog.Status = _status;

            base.Edit(catalog);
            DataContext.SaveChanges();
            return catalog.Status;
        }

        public void CheckValidate(TextHtml obj)
        {
        }
    }
}