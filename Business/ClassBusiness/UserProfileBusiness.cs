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
    public class UserProfileBusiness : AbstractBaseBusiness<UserProfile, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserProfileBusiness()
           : base(new UserProfileDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(UserProfile obj)
        {
            try
            {
                base.AddNew(obj);
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

        public override void Edit(UserProfile obj)
        {
            try
            {
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
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
            }
        }

        /// <summary>
        /// Phương thức lọc theo từ khóa
        /// </summary>
        /// <param name="key">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách userGroup</returns>
        public List<UserProfile> GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<UserProfile> query = GetDynamicQuery();
            return query.Where(p => p.Name.Contains(key)).ToList();
        }

        /// <summary>
        /// Phương thức tìm các group không chứa User chỉ định
        /// </summary>
        /// <param name="userId">UserId chỉ định</param>
        /// <returns>Danh sách các group không chứa User</returns>
        //public List<UserGroup> GetGroupByNotInUser(long userId)
        //{
        //    try
        //    {
        //        var userUserGroupBusiness = new UserUserGroupBusiness();
        //        var listGroupsInUser = userUserGroupBusiness.GetByUserId_(userId);

        //        var arrayIdGroupsInUser = new long[listGroupsInUser.Count()];

        //        int i = 0;
        //        foreach (var re in listGroupsInUser)
        //        {
        //            arrayIdGroupsInUser[i] = re.UserUsergroup1;
        //            i++;
        //        }

        //        IQueryable<UserGroup> query = GetDynamicQuery();
        //        return query.Where(p => arrayIdGroupsInUser.Contains(p.Id) == false).ToList();
        //    }
        //    catch (SqlException ex)
        //    {
        //        _logger.Error("", ex);
        //        throw ObjectUtil.CreateFaultException(CodedException.SqlExceptionUnhandler, "Sql Exeption");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("", ex);
        //        throw ObjectUtil.CreateFaultException(CodedException.Unhandler, "Unhandler Exception");
        //    }

        //}
    }
}