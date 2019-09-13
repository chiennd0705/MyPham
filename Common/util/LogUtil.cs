using log4net;
using System;

namespace BHDCommon.util
{
    public class LogUtil
    {
        public static void fatal(string classname, string content)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Fatal(content);
        }

        public static void fatal(string classname, string content, Exception ex)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Fatal(content, ex);
        }

        public static void error(string classname, string content)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Error(content);
        }

        public static void error(string classname, string content, Exception ex)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Error(content, ex);
        }

        public static void warn(string classname, string content)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Warn(content);
        }

        public static void warn(string classname, string content, Exception ex)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Warn(content, ex);
        }

        public static void info(string classname, string content)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Info(content);
        }

        public static void info(string classname, string content, Exception ex)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Info(content, ex);
        }

        public static void debug(string classname, string content)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Debug(content);
        }

        public static void debug(string classname, string content, Exception ex)
        {
            ILog _logger = LogManager.GetLogger(classname);
            _logger.Debug(content, ex);
        }
    }
}