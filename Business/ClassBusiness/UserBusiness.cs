﻿using Business.bases;
using Common;
using Common.exception;
using Common.util;
using DataAccess.DAO;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Business.ClassBusiness
{
    public class UserBusiness : AbstractBaseBusiness<Common.User, long>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Hàm tạo
        /// </summary>
        public UserBusiness()
            : base(new UserDao())
        {
            daoLayer.DataContext = DataContext;
        }

        public override void AddNew(Common.User obj)
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

        public override void Edit(Common.User obj)
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

        public bool CheckExistUserName(string userName)
        {
            try
            {
                var userEntities = new BuyGroup365Entities();
                var user = userEntities.GetUserByUserName(userName);
                if (user != null && user.Any()) return true;

                return false;
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
        /// Phương thức kiểm tra thông tin đăng nhập
        /// </summary>
        /// <param name="userName">UserNamw</param>
        /// <param name="passWord">Password</param>
        /// <param name="locationId">ID Rạp</param>
        /// <param name="computerIp">IP máy</param>
        /// <param name="computerName">Tên máy</param>
        /// <returns>UserInfo</returns>
        public UserInfo CheckLogin(string userName, string passWord, string locationId, string computerIp, string computerName)
        {
            try
            {
                var userInfo = new UserInfo();
                var userEntities = new BuyGroup365Entities();
                var user = userEntities.GetUserByUserNameAndPassword(userName, Common.util.Common.GetMd5Sum(passWord)).ToList();
                if (user.Any())
                {
                    GetUserByUserNameAndPassword_Result userLogin = user.ElementAt(0);
                    userInfo.ScreenName = userLogin.Screenname;
                    userInfo.UserId = userLogin.Id;
                    userInfo.UserName = userLogin.Username;
                    userInfo.IsSuperUser = userLogin.IsSuperUser;
                    if (userLogin.DiscountPercent != null) userInfo.DisountPercent = (float)userLogin.DiscountPercent;
                    userInfo.ModifyDate = userLogin.Modifydate;
                    if (userLogin.Status == (int)Common.util.Common.USER_STATUS.ACTIVE)
                    {
                        if (userLogin.Expireddate == null)
                        {
                            userInfo.Status = true;
                            userInfo.StatusMessage = "Trạng thái tốt.";
                        }
                        else
                        {
                            if (((DateTime)userLogin.Expireddate) >= DateTime.Now)
                            {
                                userInfo.Status = true;
                                userInfo.StatusMessage = "Trạng thái tốt.";
                            }
                            else
                            {
                                userInfo.Status = false;
                                userInfo.StatusMessage = "Tài khoản đã hết hạn.";
                            }
                        }
                    }
                    else
                    {
                        userInfo.Status = false;

                        userInfo.StatusMessage = userLogin.Status == (int)Common.util.Common.USER_STATUS.NOACTIVE ? "Tài khoản chưa được active." : "Tài khoản đang bị khóa.";
                    }
                }
                else
                {
                    userInfo.Status = false;
                    userInfo.StatusMessage = "Tên đăng nhập hoặc mật khẩu không hợp lệ.";
                }
                _logger.Info("userInfo: " + JsonConvert.SerializeObject(userInfo));
                return userInfo;
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
        /// Phương thức kiểm tra thông tin đăng nhập dành cho branch
        /// </summary>
        /// <param name="userName">UserNamw</param>
        /// <param name="passWord">Password</param>
        /// <param name="locationId">ID Rạp</param>
        /// <param name="computerIp">IP máy</param>
        /// <param name="computerName">Tên máy</param>
        /// <returns>UserInfo</returns>
        public UserInfo CheckLoginBranch(string userName, string passWord, string locationId, string computerIp, string computerName)
        {
            try
            {
                var userInfo = new UserInfo();
                var userEntities = new BuyGroup365Entities();
                List<GetUserByUserNameAndPassword_Result> user = userEntities.GetUserByUserNameAndPassword(userName, Common.util.Common.GetMd5Sum(passWord)).ToList();
                if (user.Any())
                {
                    GetUserByUserNameAndPassword_Result userLogin = user.ElementAt(0);
                    //Kiểm tra xem có phải nhân viên rạp này không
                    if (userLogin.CompanyId == 0 || userLogin.CompanyId == long.Parse(locationId) || userLogin.IsSuperUser)
                    {
                        userInfo.ScreenName = userLogin.Screenname;
                        userInfo.UserId = userLogin.Id;
                        userInfo.UserName = userLogin.Username;

                        //Nếu đã là supper admin thì gán luôn
                        if (userLogin.IsSuperUser)
                        {
                            userInfo.IsSuperUser = true;
                        }
                        else //Ngược lại xem có thuộc Group BRANCH_ADMIN không (Admin BRANCH_ADMIN ~ Supperadmin)
                        {
                            List<GroupInfo> listGroup = new UserUserGroupBusiness().GetByUserId(userInfo.UserId);
                            foreach (GroupInfo group in listGroup)
                            {
                                if (group.Code.Equals("BRANCH_ADMIN"))
                                {
                                    userInfo.IsSuperUser = true;
                                    break;
                                }
                            }
                        }
                        if (userLogin.DiscountPercent != null)
                            userInfo.DisountPercent = (float)userLogin.DiscountPercent;
                        if (userLogin.Status == (int)Common.util.Common.USER_STATUS.ACTIVE)
                        {
                            if (userLogin.Expireddate == null)
                            {
                                userInfo.Status = true;
                                userInfo.StatusMessage = "Trạng thái tốt.";
                            }
                            else
                            {
                                if (((DateTime)userLogin.Expireddate) >= DateTime.Now)
                                {
                                    userInfo.Status = true;
                                    userInfo.StatusMessage = "Trạng thái tốt.";
                                }
                                else
                                {
                                    userInfo.Status = false;
                                    userInfo.StatusMessage = "Tài khoản đã hết hạn.";
                                }
                            }
                        }
                        else
                        {
                            userInfo.Status = false;

                            userInfo.StatusMessage = userLogin.Status == (int)Common.util.Common.USER_STATUS.NOACTIVE ? "Tài khoản chưa được active." : "Tài khoản đang bị khóa.";
                        }
                    }
                    else
                    {
                        userInfo.Status = false;
                        userInfo.StatusMessage = "Bạn không được cấp quyền truy cập vào rạp này .";
                    }
                }
                else
                {
                    userInfo.Status = false;
                    userInfo.StatusMessage = "Tên đăng nhập hoặc mật khẩu không hợp lệ.";
                }

                _logger.Info("userInfo: " + JsonConvert.SerializeObject(userInfo));

                return userInfo;
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
        /// Phương thức lấy về UserID
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="passWord">Password</param>
        /// <returns>UserId</returns>
        public long GetUserId(string userName, string passWord)
        {
            try
            {
                var userEntities = new BuyGroup365Entities();

                List<GetUserByUserNameAndPassword_Result> user = userEntities.GetUserByUserNameAndPassword(userName, Common.util.Common.GetMd5Sum(passWord)).ToList();
                if (user.Any())
                {
                    return user.First().Id;
                }
                else
                {
                    return 0;
                }
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
        /// Phương thức kiểm tra password
        /// </summary>
        /// <param name="userName">userName</param>
        /// <param name="passWord">password</param>
        /// <returns>
        /// True: Nếu thông tin hợp lệ
        /// False: Nếu thông tin không hợp lệ
        /// </returns>
        public bool CheckPassword(string userName, string passWord)
        {
            try
            {
                var userEntities = new BuyGroup365Entities();

                List<GetUserByUserNameAndPassword_Result> user = userEntities.GetUserByUserNameAndPassword(userName, Common.util.Common.GetMd5Sum(passWord)).ToList();
                if (user.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        /// Phương thức cập nhật mật khẩu
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="password">Mật khẩu mới</param>
        /// <returns>
        /// True: Nếu cập nhật thành công
        /// False: Nếu không thành công
        /// </returns>
        public bool ChangePassword(long userId, string password)
        {
            try
            {
                var user = GetById(userId);
                user.Password = Common.util.Common.GetMd5Sum(password);
                user.PasswordModify_date = DateTime.Now;
                user.Modifydate = DateTime.Now;
                Edit(user);
                return true;
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
        /// Phương thức lọc User
        /// </summary>
        /// <param name="key">Khóa tìm kiếm</param>
        /// <returns>Danh sách user </returns>
        public List<Common.User> GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<Common.User> query = GetDynamicQuery();
            return query.Where(p => p.Username.Contains(key) || p.UserProfile.Phone.Contains(key) || p.UserProfile.Email.Contains(key)).ToList();
        }

        public List<Common.User> GetByKey(string key, long location)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }
            IQueryable<Common.User> query = GetDynamicQuery();
            return query.Where(p =>
                (location == -10 || p.CompanyId == location) &&
                (p.Username.Contains(key) || p.UserProfile.Phone.Contains(key) || p.UserProfile.Email.Contains(key))).ToList();
        }

        /// <summary>
        /// Phương thức tìm kiếm tất cả user
        /// </summary>
        /// <param name="locationId">ID rạp</param>
        /// <param name="computerIp">IP máy</param>
        /// <param name="computerName">Tên máy</param>
        /// <returns>Danh sách user theo khóa tìm kiếm</returns>
        public List<UserInfo> GetAllUser(long locationId, string computerIp, string computerName)
        {
            try
            {
                List<Common.User> query = GetDynamicQuery().ToList();
                var temp = query.Where(p => (p.CompanyId == 0 || p.CompanyId == locationId) && (p.Expireddate == null || p.Expireddate >= DateTime.Now) && p.Status == (int)Common.util.Common.USER_STATUS.ACTIVE).ToList();
                var listUser = new List<UserInfo>();
                foreach (var u in temp)
                {
                    var userInfo = new UserInfo
                    {
                        DisountPercent = (float)u.DiscountPercent,
                        IsSuperUser = u.IsSuperUser,
                        ScreenName = u.Screenname,
                        Status = true,
                        UserId = u.Id,
                        UserName = u.Username
                    };
                    listUser.Add(userInfo);
                }
                return listUser;
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

        public List<UserInfoFull> GetAllUser(long locationId)
        {
            try
            {
                IQueryable<Common.User> query = GetDynamicQuery();
                var temp = query.Where(p => (p.CompanyId == 0 || p.CompanyId == locationId)).ToList();
                var listUser = new List<UserInfoFull>();
                foreach (var u in temp)
                {
                    var userInfo = new UserInfoFull
                    {
                        UserId = u.Id,
                        UserName = u.Username,
                        Password = u.Password,
                        CompanyId = u.CompanyId,
                        IsSuperUser = u.IsSuperUser,
                        CreateDate = u.Createdate,
                        ModifyDate = u.Modifydate,
                        ExpiredDate = u.Expireddate,
                        ScreenName = u.Screenname,
                        LoginDate = u.Logindate,
                        FailedLoginAttemp = u.FailedLoginAttemp,
                        LockoutDate = u.LockoutDate,
                        Status = u.Status,
                        Description = u.Description,
                        DisountPercent = u.DiscountPercent
                    };

                    if (u.UserProfile != null)
                    {
                        userInfo.Code = u.UserProfile.Code;
                        userInfo.Name = u.UserProfile.Name;
                        userInfo.Phone = u.UserProfile.Phone;
                        userInfo.Address = u.UserProfile.Address;
                        userInfo.Email = u.UserProfile.Email;
                        userInfo.Dob = u.UserProfile.Dob;
                    }

                    listUser.Add(userInfo);
                }
                return listUser;
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
    }
}