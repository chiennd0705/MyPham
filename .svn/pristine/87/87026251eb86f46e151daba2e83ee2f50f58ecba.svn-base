using BHDBranchServer.exception;
using BHDCommon;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
namespace BHDBusiness.Cache
{
    public abstract class SessionCache
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string KEY_USERNAME = "username";
        public static string KEY_ID = "userId";
        public static string KEY_PASSWORD = "password";
        public static string KEY_LOGIN_DATE = "loginDate";
        public static string KEY_LAST_ACTIVE_DATE = "lastActiveDate";
        public static string KEY_USER = "user";
        public static string KEY_USERGROUP = "usergroup";
        public static string KEY_ROLE = "role";
        public static string KEY_MODULE = "module";
        public static string KEY_SHIFTSALE = "shiftId";
        public static string KEY_SESSION_ID = "sessionId";
        public static string KEY_NAME_COMPUTER = "nameComputer";
        public static string KEY_COMPUTER_IP = "computerId";
        private static Dictionary<string, Dictionary<string, object>> sessionDic = null;

        /// <summary>
        /// key: username
        /// value: dictionary[session_key] = value
        /// </summary>
        protected static Dictionary<string, Dictionary<string, object>> SessionDic
        {
            get
            {
                if (sessionDic == null)
                {
                   sessionDic = new Dictionary<string, Dictionary<string, object>>();
                }
                return sessionDic;
            }
        }

        public static Dictionary<string, Dictionary<string, object>> GetSession()
        {
            return SessionDic;
        }

        public static Dictionary<string, object> GetSessionAttributes(string username, bool autoCreateSession = true)
        {
            Dictionary<string, object> rs = null;
            if (!SessionDic.ContainsKey(username) && autoCreateSession)
            {
                SessionDic.Add(username, new Dictionary<string, object>());
                rs = SessionDic[username];
            }
            else if (!SessionDic.ContainsKey(username) && !autoCreateSession)
            {
                rs = null;
            }
            else
            {
                rs = SessionDic[username];
            }
            return rs;
        }

        public static IEnumerator<KeyValuePair<string, Dictionary<string, object>>> GetSessionAttributeEnum()
        {
            return SessionCache.SessionDic.GetEnumerator();
        }

        public static Dictionary<string, object> GetSessionInfo(String SessionId)
        {
            Dictionary<string, object> rs = null;
            if (SessionDic.ContainsKey(SessionId))
            {
                rs = SessionDic[SessionId];
            }
            else
            {
                rs = null;
            }
            return rs;
        }

        public static void RemoveSession(String key)
        {
            SessionDic.Remove(key);
        }

        public bool Authentication(Header header, String modules)
        {
            try
            {
                if (!ObjectUtil.IsEqualNull(header))
                {
                    // Check username có login hay không
                    bool existsUserName = SessionCache.GetSession().ToList().Exists(x => x.Key == header.USERNAME);
                    if (existsUserName)
                    {
                        // Check session có trùng với server không
                        String sessionId = SessionCache.GetSessionAttributes(header.USERNAME)[SessionCache.KEY_SESSION_ID].ToString();
                        if (sessionId != header.SESSION_ID)
                        {
                            throw ObjectUtil.CreateFaultException(CodedException.USERNAME_LOGIN_ANOTHER_COMPUTER, Constants.Error.USERNAME_LOGIN_ANOTHER_COMPUTER);
                        }
                        else
                        {
                            List<ModuleInfo> moduleInfos = new List<ModuleInfo>();
                            moduleInfos = (List<ModuleInfo>)SessionCache.GetSessionAttributes(header.USERNAME)[SessionCache.KEY_MODULE];
                            if (moduleInfos.Any())
                            {
                                modules = "BHDBO." + modules;
                                bool existsModule = moduleInfos.Exists(x => x.ModuleName == modules);
                                if (existsModule)
                                {
                                    return true;
                                }
                                else
                                {
                                    throw ObjectUtil.CreateFaultException(CodedException.YOU_CAN_NOT_ACCESS, Constants.Error.YOU_CAN_NOT_ACCESS);
                                }
                            }
                            else
                            {
                                throw ObjectUtil.CreateFaultException(CodedException.DATA_NOT_EXITANCE, Constants.Error.NO_DATA);
                            }
                        }
                    }
                    else
                    {
                        throw ObjectUtil.CreateFaultException(CodedException.NOT_YET_LOGIN, Constants.Error.NOT_YET_LOGIN);
                    }
                }
                else
                {
                    throw ObjectUtil.CreateFaultException(CodedException.UNKNOW_HEADER, Constants.Error.CAN_NOT_READ_HEADER);
                }
                _logger.Info("End");
            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
                throw ex;
            }
        }
    }
}
