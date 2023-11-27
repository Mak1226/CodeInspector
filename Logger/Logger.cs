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

        private readonly string _logFilePath;

        public Logger(
            string teamName, 
            LogLevel level = LogLevel.DEBUG, 
            bool logCallerFile = true, 
            bool logCallerFunction = true,
            string logFilePath = "Analyzer.log")
        {
            _teamName = teamName;
            _level = level;
            _logCallerFile = logCallerFile;
            _logCallerFunction = logCallerFunction;
            _logFilePath = logFilePath;
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
            string logMessage = $"[{nameof( level )}][{DateTime.Now}]";
            if (_logCallerFile)
            {
                logMessage += $"[{GetCallerFile()}]";
            }
            if (_logCallerFunction)
            {
                logMessage += $"[{GetCallerFunction()}]";
            }

            WriteToLogFile(logMessage);
        }

        private void WriteToLogFile( string logMessage )
        {
            try
            {
                using StreamWriter writer = new(_logFilePath, true );
                writer.WriteLine( logMessage );
            }
            catch (Exception ex)
            {
                Trace.WriteLine( $"Error writing to log file: {ex.Message}" );
                Trace.WriteLine( logMessage );
            }
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
