using System.Diagnostics;
using System.Reflection;

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

    public class Logger
    {
        private readonly string _teamName;

        public Logger(string teamName)
        {
            _teamName = teamName;

            LogWriter.SubscribeLogger(); // Initialize thread for writing log
        }

        public void Debug(string message) => Log(message, LogLevel.DEBUG);
        public void Inform(string message) => Log(message, LogLevel.INFO );
        public void Warn(string message) => Log(message , LogLevel.WARNING );
        public void Error(string message) => Log(message , LogLevel.ERROR );
        public void Critical( string message ) => Log( message , LogLevel.CRITICAL );

        public void Log(string message, LogLevel level) 
        {
            string logMessage = $"[{LogLevelName(level)}][{DateTime.Now}][{_teamName}]";
            logMessage += " ";
            logMessage += message;
            LogWriter.WriteLog(logMessage, level);
        }

        public static void SetGlobalLogFile(string logFilePath)
        {
            LogWriter.SetLogFile(logFilePath);
        }

        public static void SetLogLevel(LogLevel level)
        {
            LogWriter.SetLogLevel(level);
        }


        public static string? LogLevelName( LogLevel level )
        {
            return Enum.GetName( typeof(LogLevel) , level );
        }

    }
}
