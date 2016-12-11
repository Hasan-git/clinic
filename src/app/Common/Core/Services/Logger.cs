using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace Clinic.Common.Core.Services
{
    public static class Logger
    {
        private static bool _logInitialized;
        public const string ConfigFileName = "Log4Net.config";
        private static readonly Dictionary<Type, ILog> Loggers = new Dictionary<Type, ILog>();

        private static void Initialize()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(GetConfigFilePath()));
        }

        private static string GetConfigFilePath()
        {
            var basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var configPath = Path.Combine(basePath, "bin");
            configPath = Path.Combine(configPath, ConfigFileName);

            if (File.Exists(configPath)) return configPath;

            configPath = Path.Combine(basePath, ConfigFileName);

            if (!File.Exists(configPath))
            {
                configPath = Path.Combine(basePath, @"..\" + ConfigFileName);
            }

            return configPath;
        }

        public static void EnsureInitialized()
        {
            if (_logInitialized) return;

            Initialize();
            _logInitialized = true;
        }

        public static string SerializeException(Exception e)
        {
            return SerializeException(e, string.Empty);
        }

        private static string SerializeException(Exception e, string exceptionMessage)
        {
            if (e == null) return string.Empty;

            exceptionMessage = string.Format(
                "{0}{1}{2}\n{3}",
                exceptionMessage,
                (exceptionMessage == string.Empty) ? string.Empty : "\n\n",
                e.Message,
                e.StackTrace);

            if (e.InnerException != null)
                exceptionMessage = SerializeException(e.InnerException, exceptionMessage);

            return exceptionMessage;
        }

        private static ILog GetLogger(Type source)
        {
            if (!Loggers.ContainsKey(source))
            {
                lock (Loggers)
                {
                    if (!Loggers.ContainsKey(source))
                    {
                        ILog logger = LogManager.GetLogger(source);
                        Loggers.Add(source, logger);
                    }
                }
            }


            return Loggers[source];
        }

        public static void Debug(object source, object message)
        {
            Debug(source.GetType(), message);
        }

        public static void Debug(Type source, object message)
        {
            new Task(() =>
            {
                GetLogger(source).Debug(message);
            }).Start();
        }

        public static void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        public static void Info(Type source, object message)
        {
            new Task(() =>
            {
                GetLogger(source).Info(message);
            }).Start();
        }

        public static void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        public static void Warn(Type source, object message)
        {
            new Task(() =>
            {
                GetLogger(source).Warn(message);
            }).Start();
        }

        public static void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        public static void Error(Type source, object message)
        {
            new Task(() =>
            {
                GetLogger(source).Error(message);
            }).Start();
        }

        public static void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        public static void Fatal(Type source, object message)
        {
            new Task(() =>
            {
                GetLogger(source).Fatal(message);
            }).Start();
        }

        public static void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        public static void Debug(Type source, object message, Exception exception)
        {
            new Task(() =>
            {
                GetLogger(source).Debug(message, exception);

            }).Start();
        }

        public static void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        public static void Info(Type source, object message, Exception exception)
        {
            new Task(() =>
            {
                GetLogger(source).Info(message, exception);
            }).Start();
        }

        public static void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        public static void Warn(Type source, object message, Exception exception)
        {
            new Task(() =>
            {
                GetLogger(source).Warn(message, exception);
            }).Start();
        }

        public static void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        public static void Error(Type source, object message, Exception exception)
        {
            new Task(() =>
            {
                GetLogger(source).Error(message, exception);
            }).Start();
        }

        public static void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        public static void Fatal(Type source, object message, Exception exception)
        {
            new Task(() =>
            {
                GetLogger(source).Fatal(message, exception);
            }).Start();
        }
    }
}
