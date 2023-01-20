﻿using log4net;
using log4net.Config;
using log4net.Repository;
using Mirai.CSharp.Framework.Models.General;
using Mirai.CSharp.HttpApi.Models.ChatMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Theresa3rd_Bot.Common;
using Theresa3rd_Bot.Exceptions;
using Theresa3rd_Bot.Model.Error;

namespace Theresa3rd_Bot.Util
{
    public static class LogHelper
    {
        private static readonly string RepositoryName = "NETCoreRepository";
        private static readonly string ConfigFile = "log4net.config";

        private static ILog RollingLog { get; set; }
        private static ILog ConsoleLog { get; set; }
        private static ILog FileLog { get; set; }
        private static ILoggerRepository repository { get; set; }

        /// <summary>
        /// 初始化日志
        /// </summary>
        public static void ConfigureLog()
        {
            repository = LogManager.CreateRepository(RepositoryName);
            XmlConfigurator.Configure(repository, new FileInfo(ConfigFile));
            RollingLog = LogManager.GetLogger(RepositoryName, "RollingLog");
            ConsoleLog = LogManager.GetLogger(RepositoryName, "ConsoleLog");
            FileLog = LogManager.GetLogger(RepositoryName, "FileLog");
        }

        /// <summary>
        /// 记录Info级别的日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            FileLog.Info(message);
            ConsoleLog.Info(message);
        }

        /// <summary>
        /// 记录Error级别的日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            ConsoleLog.Error(ex.Message);
            RollingLog.Error("", ex);
        }

        /// <summary>
        /// 记录Error级别的日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public static void Error(Exception ex, string message)
        {
            ConsoleLog.Error($"{message}：{ex.Message}");
            RollingLog.Error(message, ex);
        }

        /// <summary>
        /// 记录FATAL级别的日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public static void FATAL(Exception ex, string message)
        {
            FileLog.Info(message);
            ConsoleLog.Error(message, ex);
            RollingLog.Error(message, ex);
        }

    }
}
