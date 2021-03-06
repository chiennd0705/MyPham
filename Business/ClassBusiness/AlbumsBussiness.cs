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
    public class AlbumsBusiness : AbstractBaseBusiness<Album, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AlbumsBusiness()
            : base(new AlbumsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Album obj)
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

        public override void Edit(Album obj)
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

        public List<Album> GetList(string key)
        {
            IQueryable<Album> query = GetDynamicQuery();
            var temp = query.Where(p => p.Title.Contains(key)).OrderBy(p => p.Order).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<Album> GetList()
        {
            IQueryable<Album> query = GetDynamicQuery();
            var temp = query.Where(p => p.Status == 1).OrderBy(p => p.Order).OrderByDescending(p => p.Id);
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

        public void CheckValidate(Album obj)
        {
        }

        public List<Album> GetListSlider()
        {
            IQueryable<Album> list = GetDynamicQuery();
            var tem = list.Where(p => p.Title != "Slider").OrderBy(p => p.Order).OrderByDescending(p => p.Id);
            return tem.ToList();
        }
    }
}