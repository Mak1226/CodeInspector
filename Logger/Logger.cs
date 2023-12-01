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
        private readonly LogLevel _level;
        private readonly bool _logCallerFile;
        private readonly bool _logCallerFunction;

        public Logger(
            string teamName, 
            LogLevel level = LogLevel.DEBUG, 
            bool logCallerFile = false, 
            bool logCallerFunction = false,
            string logFilePath = "Analyzer.log")
        {
            _teamName = teamName;
            _level = level;
            _logCallerFile = logCallerFile;
            _logCallerFunction = logCallerFunction;

            LogWriter.SubscribeLogger(); // Initialize thread for writing log
        }

        public void Debug(string message) => Log(message, LogLevel.DEBUG);
        public void Inform(string message) => Log(message, LogLevel.INFO );
        public void Warn(string message) => Log(message , LogLevel.WARNING );
        public void Error(string message) => Log(message , LogLevel.ERROR );
        public void Critical( string message ) => Log( message , LogLevel.CRITICAL );

        public void Log(string message, LogLevel level) 
        {
            if (level < _level)
            {
                return; //Log mask
            }
            string logMessage = $"[{nameof(level)}][{DateTime.Now}][{_teamName}]";
            if (_logCallerFile)
            {
                logMessage += $"[{GetCallerFile()}]";
            }
            if (_logCallerFunction)
            {
                logMessage += $"[{GetCallerFunction()}]";
            }

            LogWriter.WriteLog(logMessage);
        }

        private static string GetCallerFunction()
        {
            StackTrace stackTrace = new();
            MethodBase? method = stackTrace.GetFrame( 2 )?.GetMethod();
            return method?.Name ?? "UnknownFunction";
        }

        private static string GetCallerFile()
        {
            StackTrace stackTrace = new();
            StackFrame? frame = stackTrace.GetFrame( 2 );
            return frame?.GetMethod()?.DeclaringType?.FullName ?? "UnknownFile";
        }
    }
}
