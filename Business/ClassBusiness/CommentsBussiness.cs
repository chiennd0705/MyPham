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
    public class CommentsBusiness : AbstractBaseBusiness<Comment, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CommentsBusiness()
            : base(new CommentsDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Comment obj)
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

        public override void Edit(Comment obj)
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

        public double CalculatorByProductId(long ProductId, ref int count)
        {
            IQueryable<Comment> query = GetDynamicQuery();
            var temp = query.Where(p => p.ProductId == ProductId && p.Status == 2 && p.ParentId == null);
            decimal tt = 0;
            decimal t;
            foreach (var item in temp)
            {
                count++;
                tt = tt + item.Rate;
            }
            if (count > 0)
            {
                t = tt / count;
                return (double)Math.Round(t, 1);
            }
            else
                return 3;
        }

        public List<Product> GetListComments(string key)
        {
            List<Product> listresults = new List<Product>();
            var listproducid = GetDynamicQuery().Select(x => x.ProductId).Distinct();
            if (listproducid.Any())
            {
                if (string.IsNullOrEmpty(key))
                {
                    foreach (var l in listproducid)
                    {
                        listresults.Add(new ProductsBusiness().GetById(l));
                    }
                }
                else
                {
                    foreach (var l in listproducid)
                    {
                        var item = new ProductsBusiness().GetById(l);
                        if (item.ProductName.Contains(key))
                        {
                            listresults.Add(item);
                        }
                    }
                }
            }
            return listresults;
        }

        public Comment GetContentComment(long parentID)
        {
            Comment Comment = null;
            try
            {
                IQueryable<Common.Comment> query = GetDynamicQuery();
                Comment = query.Where(p => p.Status == 2 && p.ParentId == parentID).OrderByDescending(p => p.CreateDate).FirstOrDefault();
            }
            catch { }
            return Comment;
        }

        public List<Comment> ListByCommentsByParentID(long? ParentId, int pageIndex = 1, int pageSize = 2)
        {
            IQueryable<Common.Comment> query = GetDynamicQuery();
            var getListById = query.Where(p => p.Status == 2 && p.ParentId == ParentId);
            var model = getListById.ToList();
            model = model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model.ToList();
        }

        public List<Comment> GetListComment(long id)
        {
            List<Comment> LComment = new List<Comment>();
            try
            {
                IQueryable<Common.Comment> query = GetDynamicQuery();
                LComment = query.Where(p => p.Status == 2 && p.ProductId == id && p.ParentId == null).OrderByDescending(p => p.CreateDate).ToList();
            }
            catch { }
            return LComment;
        }

        public string Getphantramsao(int sao, long productID)
        {
            Double phantram = 0;
            int result = 0;
            try
            {
                IQueryable<Comment> query = GetDynamicQuery();
                var temp = query.Where(p => p.ProductId == productID && p.Status == 2 && p.ParentId == null);
                int count = 0;
                int tt = temp.Count();
                foreach (var item in temp)
                {
                    if (item.Rate == sao)
                        count = count + 1;
                }
                phantram = ((double)count / tt) * 100;
                result = (int)phantram;
            }
            catch
            {
            }
            return result.ToString().Replace(',', '.');
        }

        public void CheckValidate(Comment obj)
        {
        }
    }
}