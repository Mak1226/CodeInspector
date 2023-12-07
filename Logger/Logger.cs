using System.Diagnostics;
using System.Reflection;

namespace Logging
{
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        CRITICAL
    }

    /// <summary>
    /// Simple logger that can log 5 levels, and logs team name
    /// Writing to file is done by a <see cref="LogWriter"/> thread
    /// </summary>
    public static class Logger
    {
        private static LogLevel s_logLevel = 0;

        /// <summary>
        /// Start a new logger instance with given team name
        /// </summary>
        /// <param name="teamName">Name of team to be logged</param>
        static Logger()
        {
            LogWriter.StartThread(); // Initialize thread for writing log
        }

        /// <summary>
        /// Write a log of level DEBUG.
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message) => Log(message, LogLevel.DEBUG);
        /// <summary>
        /// Write a log of level INFO.
        /// </summary>
        /// <param name="message"></param>
        public static void Inform(string message) => Log(message, LogLevel.INFO );
        /// <summary>
        /// Write a log of level WARNING.
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message) => Log(message , LogLevel.WARNING );
        /// <summary>
        /// Write a log of level ERROR.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message) => Log(message , LogLevel.ERROR );
        /// <summary>
        /// Write a log of level CRITICAL.
        /// </summary>
        /// <param name="message"></param>
        public static void Critical( string message ) => Log( message , LogLevel.CRITICAL );

        /// <summary>
        /// Write a log of the given level.
        /// Log Format : [Level][time][namespace] message
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/></param>
        /// <param name="message"></param>
        public static void Log(string message, LogLevel level)
        {
            if (level < s_logLevel)
            {
                return; // Mask log levels
            }

            try
            {
                //Fetch namespace of caller
                MethodBase method = new StackFrame(2).GetMethod();
                string teamName = method?.DeclaringType?.Namespace ?? "Unknown" ?? "Unknown";

                string logMessage = $"[{LogLevelName( level )}]".PadRight(10) +
                    $"[{DateTime.Now}]" +
                    $"[{teamName}]" +
                    $" {message}";
                LogWriter.WriteLog(logMessage);
            }
            catch (Exception e)
            {
                Trace.TraceError( $"Logging message {message} failed with error {e}" );
            }
        }

        /// <summary>
        /// Set log file path. Default is /Analyzer.log
        /// 
        /// Note: This is a global operation.
        /// </summary>
        /// <param name="logFilePath"></param>
        public static void SetGlobalLogFile(string logFilePath)
        {
            LogWriter.SetLogFile(logFilePath);
        }

        /// <summary>
        /// Set log level. Default is DEBUG, ie. all levels will be logged
        /// 
        /// Note: This is a global operation
        /// </summary>
        /// <param name="level"></param>
        public static void SetLogLevel(LogLevel level)
        {
            s_logLevel = level;
        }


        static string? LogLevelName( LogLevel level )
        {
            return Enum.GetName( typeof(LogLevel) , level );
        }

    }
}
