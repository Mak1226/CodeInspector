using System.Diagnostics;

namespace Logger
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
    public class Logger
    {
        private readonly string _teamName;

        /// <summary>
        /// Start a new logger instance with given team name
        /// </summary>
        /// <param name="teamName">Name of team to be logged</param>
        public Logger(string teamName)
        {
            _teamName = teamName;

            LogWriter.SubscribeLogger(); // Initialize thread for writing log
        }

        /// <summary>
        /// Write a log of level DEBUG.
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message) => Log(message, LogLevel.DEBUG);
        /// <summary>
        /// Write a log of level INFO.
        /// </summary>
        /// <param name="message"></param>
        public void Inform(string message) => Log(message, LogLevel.INFO );
        /// <summary>
        /// Write a log of level WARNING.
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message) => Log(message , LogLevel.WARNING );
        /// <summary>
        /// Write a log of level ERROR.
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message) => Log(message , LogLevel.ERROR );
        /// <summary>
        /// Write a log of level CRITICAL.
        /// </summary>
        /// <param name="message"></param>
        public void Critical( string message ) => Log( message , LogLevel.CRITICAL );

        /// <summary>
        /// Write a log of given level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void Log(string message, LogLevel level) 
        {
            try
            {
                string logMessage = $"[{LogLevelName(level)}][{DateTime.Now}][{_teamName}]";
                logMessage += " ";
                logMessage += message;
                LogWriter.WriteLog(logMessage, level);
            }
            catch (Exception e)
            {
                Trace.TraceError( $"Logger for {_teamName} and message {message} failed with error {e}" );
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
            LogWriter.SetLogLevel(level);
        }


        static string? LogLevelName( LogLevel level )
        {
            return Enum.GetName( typeof(LogLevel) , level );
        }

    }
}
