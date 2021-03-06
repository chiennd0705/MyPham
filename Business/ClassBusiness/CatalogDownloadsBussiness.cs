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
    public class CatalogDownloadsBusiness : AbstractBaseBusiness<CatalogDownloads, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CatalogDownloadsBusiness()
            : base(new CatalogDownloadsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(CatalogDownloads obj)
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

        public override void Edit(CatalogDownloads obj)
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

        public CatalogDownloads SearchCategoryByFriendUrl(string url)
        {
            List<CatalogDownloads> list = GetDynamicQuery().Where(x => x.FriendlyUrl == url).ToList();
            return list.First();
        }

        public List<CatalogDownloads> SearchCategoryByName(List<CatalogDownloads> listDropdowCate)
        {
            List<CatalogDownloads> obj = new List<CatalogDownloads>();
            var list = SearchCategoryByParentId("", -1, listDropdowCate, ref obj);

            return list;
        }

        public List<CatalogDownloads> SearchCategoryByParentId(string sp, long paId, List<CatalogDownloads> listcate, ref List<CatalogDownloads> listDropdowCate)
        {
            //List<CatalogDownloads>  listDropdowCate=new List<CatalogDownloads>();
            List<CatalogDownloads> list = GetDynamicQuery().Where(x => x.ParentId == paId).ToList();
            //List<CatalogDownloads> list = listcate.Where(x => x.ParentId == paId).ToList();
            if (list.Any())
            {
                foreach (var CatalogDownloads in list)
                {
                    var str = sp + CatalogDownloads.CatalogDownloadName;
                    //  CatalogDownloads = str;
                    //   var dropdowCate = new LoadDropdown.DropdowCate { Id = CatalogDownloads.Id, Name = sp + CatalogDownloads.CatalogDownloadsName };
                    listDropdowCate.Add(CatalogDownloads);
                    SearchCategoryByParentId(sp + "-----", CatalogDownloads.Id, listcate, ref listDropdowCate);
                }
            }

            return listDropdowCate;
        }

        public List<CatalogDownloads> SearchCatalogDownloadsByParentId(long ParentId)
        {
            IQueryable<CatalogDownloads> query = GetDynamicQuery();
            var listCatalogDownloads = query.Where(p => p.ParentId == ParentId && p.Status == 1).OrderBy(p => p.ModifyDate).ToList();
            return listCatalogDownloads;
        }

        public List<CatalogDownloads> SearchCatalogDownloadsALL()
        {
            IQueryable<CatalogDownloads> query = GetDynamicQuery();
            var listCatalogDownloads = query.Where(p => p.Status == 1).OrderBy(p => p.ModifyDate).ToList();
            return listCatalogDownloads;
        }

        public List<CatalogDownloads> SearchCatalogDownloadsByParentIdActive(long ParentId)
        {
            IQueryable<CatalogDownloads> query = GetDynamicQuery();
            var listCatalogDownloads = query.Where(p => p.ParentId == ParentId && p.Status == 1).OrderBy(p => p.ModifyDate).ToList();
            return listCatalogDownloads;
        }

        public List<CatalogDownloads> GetList(string key)
        {
            IQueryable<CatalogDownloads> query = GetDynamicQuery();
            var temp = query.Where(p => p.CatalogDownloadName.Contains(key)).OrderByDescending(p => p.Id);
            return temp.ToList();
        }

        public List<long> GetListCateByParent(long parentid, ref List<long> listResults, List<CatalogDownloads> listCat)
        {
            listResults.Add(parentid);
            var obj = listCat.Where(x => x.ParentId == parentid);// Sữ dụng biến cache

            if (obj.Any())
            {
                foreach (var item in obj)
                {
                    listResults.Add(item.Id);
                    //         GetListCateByParent(item.Id, ref listResults);
                    GetListCateByParent(item.Id, ref listResults, listCat);
                }
            }
            return listResults;
        }

        //khi bấm vào nút xóa sẽ thay đổi trạng thái danh mục
        public int ChangeStatusCatalogDownloads(long id)
        {
            var CatalogDownloads = GetById(id);
            if (CatalogDownloads.Status == 0)
            {
                CatalogDownloads.Status = 1;
            }
            else
            {
                CatalogDownloads.Status = 0;
            }
            base.Edit(CatalogDownloads);
            DataContext.SaveChanges();
            return CatalogDownloads.Status;
        }

        public void CheckValidate(CatalogDownloads obj)
        {
        }
    }
}