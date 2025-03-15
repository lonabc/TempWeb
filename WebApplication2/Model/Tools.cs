using NLog;
using NLog.Config;
using System.Diagnostics;
using System.Reflection;

using System;

using System.IO;

using System.Xml;


namespace WebApplication2.Model
{
    public class Tools
    {
        public static void WirteLogTest(string content)
        {
           var config=new NLog.Config.LoggingConfiguration();
           var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"logs/{DateTime.Now.ToString("yyyy-MM-dd")}.txt" };
           config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
           LogManager.Configuration = config;
           Logger logger=LogManager.GetCurrentClassLogger();
           logger.Info(content);//将内容写入日志文件
        }

    }

    public class NLogService
    {
        private static NLog.ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        static NLogService()
        {
            //判断配置文件NLog.config是否存在
            bool flag = File.Exists(Path.Combine(Path.GetDirectoryName(typeof(NLogService).Assembly.Location), "NLog.config"));
            //获取配置信息
            LogManager.Configuration = GetXmlLoggingConfiguration(!flag);
        }
        /// <summary>
        /// 
        /// </summary>
        private NLogService()
        {
        }
        /// <summary>
        /// logger 初始化
        /// </summary>
        private static void SetLogger()
        {
            StackFrame stackFrame = new StackFrame(2, false);
            MethodBase method = stackFrame.GetMethod();
            _logger = LogManager.GetLogger(string.Format("{0}.{1}", method.DeclaringType, method.Name));
        }
        /// <summary>
        /// 获取logger 配置信息
        /// </summary>
        /// <param name="fromManifestResource"></param>
        /// <returns></returns>
        public static XmlLoggingConfiguration GetXmlLoggingConfiguration(bool fromManifestResource = true)
        {
            XmlLoggingConfiguration result;
            if (fromManifestResource)
            {
                Type declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                string text = ((declaringType != null) ? declaringType.Namespace : null) ?? "Logging.NLog";
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(text + ".nlog.config");
                XmlTextReader xmlTextReader = new XmlTextReader(manifestResourceStream);
                result = new XmlLoggingConfiguration(xmlTextReader);
            }
            else
            {
                string text2 = Path.Combine(Path.GetDirectoryName(typeof(NLogService).Assembly.Location), "nlog.config");
                bool flag = !File.Exists(text2);
                if (flag)
                {
                    throw new FileNotFoundException("nlog.config文件未找到。");
                }
                result = new XmlLoggingConfiguration(new FileInfo(text2).FullName);
            }
            return result;
        }
        /// <summary>
        /// write info log
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            SetLogger();
            _logger.Info(msg);
        }
        /// <summary>
        /// write warn log
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(string msg)
        {
            SetLogger();
            _logger.Warn(msg);
        }
        /// <summary>
        /// write debug log
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            SetLogger();
            _logger.Debug(msg);
        }
        /// <summary>
        /// write error log
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            SetLogger();
            _logger.Error(msg);
        }
        /// <summary>
        /// write exception log
        /// </summary>
        /// <param name="e"></param>
        public static void Exception(Exception e)
        {
            SetLogger();
            _logger.Error<Exception>(e);
        }
    }
}
