using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace Common
{
    /// <summary>
	/// 日志
	/// </summary>
	public class LogRecord
    {

        private static readonly ConcurrentDictionary<string, object> _FileLock = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// DEBUG INFO WARN ERROR FATAL 优先级由低到高
        /// </summary>      
        private static readonly string LogLevel = "debug";
        private static readonly string LogPath = @"d:\logrecord\";
        private static readonly bool IsConsoleEnabled = false;

        public static bool IsWarnEnabled { get; private set; }
        public static bool IsInfoEnabled { get; private set; }
        public static bool IsFatalEnabled { get; private set; }
        public static bool IsErrorEnabled { get; private set; }
        public static bool IsDebugEnabled { get; private set; }

        static LogRecord()
        {
            try
            {
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ResetLog();
        }

        private static void ResetLog()
        {
            #region LogLevel

            //DEBUG INFO WARN ERROR FATAL 优先级由低到高
            if (LogLevel.ToUpper() == "DEBUG")
            {
                IsDebugEnabled = true;
                IsInfoEnabled = true;
                IsWarnEnabled = true;
                IsErrorEnabled = true;
                IsFatalEnabled = true;
            }
            else if (LogLevel.ToUpper() == "INFO")
            {
                IsDebugEnabled = false;
                IsInfoEnabled = true;
                IsWarnEnabled = true;
                IsErrorEnabled = true;
                IsFatalEnabled = true;
            }
            else if (LogLevel.ToUpper() == "WARN")
            {
                IsDebugEnabled = false;
                IsInfoEnabled = false;
                IsWarnEnabled = true;
                IsErrorEnabled = true;
                IsFatalEnabled = true;
            }
            else if (LogLevel.ToUpper() == "ERROR")
            {
                IsDebugEnabled = false;
                IsInfoEnabled = false;
                IsWarnEnabled = false;
                IsErrorEnabled = true;
                IsFatalEnabled = true;
            }
            else if (LogLevel.ToUpper() == "FATAL")
            {
                IsDebugEnabled = false;
                IsInfoEnabled = false;
                IsWarnEnabled = false;
                IsErrorEnabled = false;
                IsFatalEnabled = true;
            }

            #endregion
        }

        private static void writeLogsingle(string fileName, string logMessage, string size = "h")
        {
            object _lock;
            if (_FileLock.ContainsKey(fileName))
            {
                _lock = _FileLock[fileName];
            }
            else
            {
                _lock = new object();
                _FileLock[fileName] = _lock;
            }
            lock (_lock)
            {
                logMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {logMessage}{Environment.NewLine}";
                string logName = string.Format("{0}{1}_{2}", LogPath, DateTime.Now.ToString("yyyyMMdd_HH"), fileName);
                if (size == "d")
                {
                    logName = string.Format("{0}{1}_{2}", LogPath, DateTime.Now.ToString("yyyyMMdd_99"), fileName);
                }

                try
                {
                    if (!logName.EndsWith(".log"))
                    {
                        logName += ".log";
                    }

                    using (FileStream fileStream = new FileStream(logName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, System.Text.Encoding.UTF8))
                        {
                            binaryWriter.Write(logMessage.ToCharArray());
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="size">h 小时 d 天</param>
        public static void Debug(string fileName, string logMessage, string size = "h")
        {
            if (!IsDebugEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            writeLogsingle(fileName + "_debug", logMessage, size);
            if (IsConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Debug: " + logMessage);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="size">h 小时 d 天</param>
        public static void Info(string fileName, string logMessage, string size = "h")
        {
            if (!IsInfoEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            writeLogsingle(fileName + "_info", logMessage, size);
            if (IsConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Info: " + logMessage);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Warn日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="size">h 小时 d 天</param>
        public static void Warn(string fileName, string logMessage, string size = "h")
        {
            if (!IsWarnEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            writeLogsingle(fileName + "_warn", logMessage, size);
            if (IsConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Warn: " + logMessage);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Error 日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="size">h 小时 d 天</param>
        public static void Error(string fileName, string logMessage, string size = "h")
        {
            if (!IsErrorEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            writeLogsingle(fileName + "_error", logMessage, size);
            if (IsConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-Error: " + logMessage);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Fatal 日志
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <param name="logMessage">日志内容</param>
        /// <param name="size">h 小时 d 天</param>
        public static void Fatal(string fileName, string logMessage, string size = "h")
        {
            if (!IsFatalEnabled)
            {
                return;
            }

            logMessage = $"[{Thread.CurrentThread.ManagedThreadId}] {logMessage}";
            writeLogsingle(fileName + "_fatal", logMessage, size);
            if (IsConsoleEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "-Fatal: " + logMessage);
                Console.WriteLine();
            }
        }
    }
}
