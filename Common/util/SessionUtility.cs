using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace Common.util
{
    /// <summary>
    /// Summary description for SessionUtility
    /// </summary>
    public static class SessionUtility
    {
        /////////LOGIN - STAFF///////////////
        public static bool CheckLogin(HttpSessionStateBase httpSessionState)
        {
            if (GetSessionUserName(httpSessionState) != string.Empty && GetSessionUserId(httpSessionState) != "0" && GetSessionLocation(httpSessionState) != string.Empty)
            {
                return true;
            }
            return false;
        }

        public static bool CheckLoginMember(HttpSessionStateBase httpSessionState)
        {
            if (GetSessionMember(httpSessionState) != null)
            {
                return true;
            }
            return false;
        }

        public static void LogOut(HttpSessionStateBase httpSessionState)
        {
            // httpSessionState["ACCOUNT_USERID"] = 0;
            //   httpSessionState.Abandon();//["ACCOUNT_USERNAME"].A
            //httpSessionState["ACCOUNT_NAME"] = string.Empty;
            //httpSessionState["ACCOUNT_LOCATION"] = string.Empty; ;
            //    httpSessionState["MEMBERREG"] = null;

            SetSessionMember(null, httpSessionState);
        }

        public static void LogOutAdmin(HttpSessionStateBase httpSessionState)
        {
            SetSessionUser(null, null, null, null, null, false);
        }

        public static void SetSessionUser(string userId, string userName, string location, HttpSessionStateBase httpSessionState, bool isSupperUser)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ACCOUNT_USERID"] = userId;
                httpSessionState["ACCOUNT_USERNAME"] = userName;
                httpSessionState["ACCOUNT_LOCATION"] = location;

                SetListModule(httpSessionState, long.Parse(userId), isSupperUser);
            }
        }

        public static void SetSessionUser(string userId, string userName, string name, string location, HttpSessionStateBase httpSessionState, bool isSupperUser)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ACCOUNT_USERID"] = userId;
                httpSessionState["ACCOUNT_USERNAME"] = userName;
                httpSessionState["ACCOUNT_NAME"] = name;
                httpSessionState["ACCOUNT_LOCATION"] = location;
                SetListModule(httpSessionState, long.Parse(userId), isSupperUser);
            }
        }

        #region USER NAME

        public static void SetSessionUserName(string userName, HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ACCOUNT_USERNAME"] = userName;
            }
        }

        public static string GetSessionUserName(HttpSessionStateBase httpSessionState)
        {
            string username = string.Empty;

            try
            {
                if (httpSessionState != null)
                    username = httpSessionState["ACCOUNT_USERNAME"].ToString();
            }
            catch
            {
                username = string.Empty;
            }
            return username;
        }

        #endregion USER NAME

        #region Order Reciver

        public static void SetSessionOrderReciver(OrderReciver orderReciver, HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ORDER_RECIVER"] = orderReciver;
            }
        }

        public static OrderReciver GetSessionOrderReciver(HttpSessionStateBase httpSessionState)
        {
            OrderReciver orderReciver = new OrderReciver();

            try
            {
                if (httpSessionState != null)
                    orderReciver = (OrderReciver)httpSessionState["ORDER_RECIVER"];
            }
            catch
            {
            }
            return orderReciver;
        }

        #endregion Order Reciver

        #region USER NAME

        public static void SetSessionName(string userName, HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ACCOUNT_NAME"] = userName;
            }
        }

        public static string GetSessionName(HttpSessionStateBase httpSessionState)
        {
            string username = string.Empty;

            try
            {
                if (httpSessionState != null)
                    username = httpSessionState["ACCOUNT_NAME"].ToString();
            }
            catch
            {
                username = string.Empty;
            }
            return username;
        }

        #endregion USER NAME

        #region USER ID

        public static void SetSessionUserId(string userId, HttpSessionStateBase httpSessionState, bool isSupperUser)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ACCOUNT_USERID"] = userId;
                SetListModule(httpSessionState, long.Parse(userId), isSupperUser);
            }
        }

        public static string GetSessionUserId(HttpSessionStateBase httpSessionState)
        {
            string userId = "0";

            try
            {
                if (httpSessionState != null)
                    userId = httpSessionState["ACCOUNT_USERID"].ToString();
            }
            catch
            {
                userId = "0";
            }
            return userId;
        }

        #endregion USER ID

        #region LOCATION

        public static void SetSessionLocation(string location, HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ACCOUNT_LOCATION"] = location;
            }
        }

        public static string GetSessionLocation(HttpSessionStateBase httpSessionState)
        {
            string location = string.Empty;

            try
            {
                if (httpSessionState != null)
                    location = httpSessionState["ACCOUNT_LOCATION"].ToString();
            }
            catch
            {
                location = string.Empty;
            }
            return location;
        }

        #endregion LOCATION

        #region Member

        public static void SetSessionMember(Member obj, HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState != null)
            {
                httpSessionState["MEMBER"] = obj;
            }
        }

        public static void SetSessionMemberReg(Member obj, HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState != null)
            {
                httpSessionState["MEMBERREG"] = obj;
            }
        }

        public static Member GetSessionMember(HttpSessionStateBase httpSessionState)
        {
            Member member = null;

            try
            {
                if (httpSessionState != null)
                    member = (Member)httpSessionState["MEMBER"];
            }
            catch
            {
                member = null;
            }
            return member;
        }

        #endregion Member

        #region LOCATION

        public static void SetSessionModule(string listModule, HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState != null)
            {
                httpSessionState["ACCOUNT_MODULE"] = listModule;
            }
        }

        public static string GetSessionModule(HttpSessionStateBase httpSessionState)
        {
            string listModule = string.Empty;

            try
            {
                if (httpSessionState != null)
                    listModule = httpSessionState["ACCOUNT_MODULE"].ToString();
            }
            catch
            {
                listModule = string.Empty;
            }
            return listModule;
        }

        public static void SetListModule(HttpSessionStateBase httpSessionState, long userId, bool isSupperUser)
        {
            if (!isSupperUser)
            {
                var userEntities = new BuyGroup365Entities();
                var listResult = userEntities.GetAllModuleByUserId(userId).ToList();

                string listModule = listResult.Aggregate("", (current, re) => current + (re.WebMethod + ";"));
                listModule += "/User/Info;/User/ChangePassword;/User/UpdateInfo";
                SetSessionModule(listModule, httpSessionState);
            }
            else
                SetSessionModule("IsSupperUser", httpSessionState);
        }

        public static bool CheckPermission(HttpSessionStateBase httpSessionState, string url)
        {
            string listModule = GetSessionModule(httpSessionState);
            if (!listModule.Equals("IsSupperUser"))
            {
                string[] arrayModule = listModule.Split(';');

                /* if (arrayModule.Contains(url))
                 return true;*/
                for (int i = 0; i < arrayModule.Length; i++)
                {
                    url = url.ToLower();
                    if (url.Contains(arrayModule[i].ToLower()))
                        return true;
                }

                return false;
            }
            return true;
        }

        #endregion LOCATION

        #region Cookies

        public static void WriteCookie(string strCookieName, object obj, int hour)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            var json = JsonConvert.SerializeObject(obj);

            var userCookie = new HttpCookie(strCookieName, json);
            userCookie.Expires.AddHours(hour);
            HttpContext.Current.Response.Cookies.Set(userCookie);
            //Response.Cookies.Add(userCookie);
        }

        public static string ReadCookie(string strCookieName)
        {
            foreach (string strCookie in HttpContext.Current.Response.Cookies.AllKeys)
            {
                if (strCookie == strCookieName)
                {
                    return HttpContext.Current.Response.Cookies[strCookie].Value;
                }
            }

            foreach (string strCookie in HttpContext.Current.Request.Cookies.AllKeys)
            {
                if (strCookie == strCookieName)
                {
                    return HttpContext.Current.Request.Cookies[strCookie].Value;
                }
            }

            return null;
        }

        public static void ClearCoookie(string strCookieName)
        {
            var cookies = HttpContext.Current.Response.Cookies[strCookieName];
            if (cookies != null)
            {
                //   HttpCookie myCookie = new HttpCookie("UserSettings");
                cookies.Expires = DateTime.Now.AddDays(-1d);
                cookies.Value = null;
                HttpContext.Current.Response.Cookies.Add(cookies);
            }
        }

        #endregion Cookies
    }
}