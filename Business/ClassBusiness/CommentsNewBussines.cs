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
    public class CommentsNewsBusiness : AbstractBaseBusiness<CommentsNew, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CommentsNewsBusiness()
            : base(new CommentsNewDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(CommentsNew obj)
        {
            try
            {
                CheckValidate(obj);
                obj.CreateDate = DateTime.Now;
                obj.MofifyDate = DateTime.Now;
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

        public override void Edit(CommentsNew obj)
        {
            try
            {
                CheckValidate(obj);
                obj.MofifyDate = DateTime.Now;
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

        public int CalculatorByProductId(long NewId, ref int count)
        {
            IQueryable<CommentsNew> query = GetDynamicQuery();
            var temp = query.Where(p => p.NewId == NewId);
            int tt = 0;
            foreach (var item in temp)
            {
                count++;
                tt = tt + item.Rate;
            }
            if (count > 0)
                return tt / count;
            else
                return 3;
        }

        public CommentsNew GetCommentsByParentID(long parentID)
        {
            try
            {
                IQueryable<CommentsNew> query = GetDynamicQuery();
                var temp = query.Where(p => p.ParentId == parentID).SingleOrDefault();

                return temp;
            }
            catch
            {
                return null;
            }
        }

        public List<News> GetListComments(string key)
        {
            try
            {
                List<News> listresults = new List<News>();
                var listnewid = GetDynamicQuery().Select(x => x.NewId).Distinct();
                if (listnewid.Any())
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        foreach (var l in listnewid)
                        {
                            listresults.Add(new NewsBusiness().GetById(l.Value));
                        }
                    }
                    else
                    {
                        foreach (var l in listnewid)
                        {
                            var item = new NewsBusiness().GetById(l.Value);
                            if (item.Title.Contains(key))
                            {
                                listresults.Add(item);
                            }
                        }
                    }
                }
                return listresults;
            }
            catch
            {
                return null;
            }
        }

        public int CountCommentByNewID(long newID)
        {
            List<CommentsNew> listresults = new List<CommentsNew>();
            int i = GetDynamicQuery().Where(x => x.NewId == newID && x.Status == 2).Count();

            return i;
        }

        public List<CommentsNew> GetListCommentsNew(string key)
        {
            List<CommentsNew> listresults = new List<CommentsNew>();
            listresults = GetDynamicQuery().Where(x => x.ParentId == null).OrderByDescending(p => p.CreateDate).ToList();

            return listresults;
        }

        public CommentsNew GetContentComment(long parentID)
        {
            CommentsNew commentNew = null;
            try
            {
                IQueryable<Common.CommentsNew> query = GetDynamicQuery();
                commentNew = query.Where(x => x.ParentId == parentID).OrderByDescending(p => p.CreateDate).FirstOrDefault();
            }
            catch { }
            return commentNew;
        }

        public void CheckValidate(CommentsNew obj)
        {
        }

        public List<CommentsNew> ListByCommentsNew(long? ParentId, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            IQueryable<Common.CommentsNew> query = GetDynamicQuery();
            var getListById = query.Where(p => p.Status == 2 && p.ParentId == ParentId);

            totalRecord = getListById.Count();
            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model.ToList();
        }

        public List<CommentsNew> ListByCommentsNewByParentID(long? ParentId, int pageIndex = 1, int pageSize = 2)
        {
            IQueryable<Common.CommentsNew> query = GetDynamicQuery();
            var getListById = query.Where(p => p.Status == 2 && p.ParentId == ParentId);
            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model.ToList();
        }

        public List<CommentsNew> ListByCommentsNewID(long NewID, int pageIndex = 1, int pageSize = 2)
        {
            IQueryable<Common.CommentsNew> query = GetDynamicQuery();
            var getListById = query.Where(p => p.Status == 2 && p.NewId == NewID && p.ParentId == null);

            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model.ToList();
        }
    }
}