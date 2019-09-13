using Common;
using Common.entity.bases;
using DataAccess.bases;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Business.bases
{
    public abstract class AbstractBaseBusiness<T, TId> where T : AbstractBaseEntity<TId>, new() where TId : IEquatable<TId>
    {
        protected DbContext Context = null;
        protected AbstractBaseDao<T, TId> daoLayer = null;

        protected AbstractBaseBusiness(AbstractBaseDao<T, TId> daoLayer)
        {
            this.daoLayer = daoLayer;
        }

        public AbstractBaseDao<T, TId> DaoLayer
        {
            get { return daoLayer; }
        }

        public virtual void AddNew(T obj)
        {
            DaoLayer.Insert(obj);
        }

        public virtual void Edit(T obj)
        {
            DaoLayer.Update(obj);
        }

        public virtual void Remove(TId id)
        {
            DaoLayer.Delete(id);
        }

        public virtual T GetById(TId id)
        {
            return DaoLayer.GetById(id);
        }

        public virtual IQueryable<T> GetDynamicQuery()
        {
            IQueryable<T> query = DaoLayer.GetDynamicQuery();
            return query;
        }

        public List<T> FindProperty(Func<T, bool> func, List<string[]> orderby, int startIndex, int pageSize)
        {
            return DaoLayer.FindByProperties(func, orderby, startIndex, pageSize);
        }

        public List<T> FindProperty(List<object[]> conditions, List<string[]> orderby, string operation, int startIndex, int pageSize)
        {
            return DaoLayer.FindByPropertiesByconditions(conditions, orderby, operation, startIndex, pageSize);
        }

        public long CountProperty(List<object[]> conditions, string operation)
        {
            return DaoLayer.CountByProperties(conditions, operation);
        }

        public DbContext DataContext
        {
            get
            {
                if (Context == null)
                {
                    Context = new BuyGroup365Entities();
                }
                return Context;
            }
        }
    }
}