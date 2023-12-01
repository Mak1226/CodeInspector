using System.Diagnostics;

namespace Logger
{
    /// <summary>
    /// Threaded writer for log messages
    /// </summary>
    internal static class LogWriter
    {
        private static Thread? s_writer;
        static string s_logFilePath = "Analyzer.log";
        static LogLevel s_logLevel = 0;

        static readonly Queue<string> s_logs = new();
        static readonly ManualResetEvent s_queueNotEmpty = new(false);
        static readonly object s_queueLock = new();

        /// <summary>
        /// Start the writer thread if not already started
        /// </summary>
        internal static void SubscribeLogger()
        {
            if (s_writer == null)
            {
                s_writer = new( new ThreadStart( WriterThread ) )
                {
                    IsBackground = true
                };
                s_writer.Start();
            }
        }

        /// <summary>
        /// Writer thread function. Pops log message queue and writes to log file if queue is non empty.
        /// </summary>
        internal static void WriterThread()
        {
            while (true)
            {
                s_queueNotEmpty.WaitOne();

                lock (s_queueLock)
                {
                    string msg = s_logs.Dequeue();
                    WriteToLogFile(msg);

                    if (s_logs.Count == 0)
                    {
                        s_queueNotEmpty.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Set path of log file
        /// </summary>
        /// <param name="logFilePath"></param>
        internal static void SetLogFile(string logFilePath)
        {
            s_logFilePath = logFilePath;
        }

        /// <summary>
        /// Set level of allowed log
        /// </summary>
        /// <param name="logLevel"></param>
        internal static void SetLogLevel(LogLevel logLevel)
        {
            s_logLevel = logLevel;
        }

        /// <summary>
        /// Write to log file a message.
        /// Internally adds the message to log queue for the thread to pop
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <exception cref="NullReferenceException">If writer thread is null (ie. not initiallized)</exception>
        internal static void WriteLog(string message, LogLevel level)
        {
            if (s_writer == null)
            {
                throw new NullReferenceException( "No logger subscribed" );
            }

            if (level < s_logLevel)
            {
                return;
            }

            lock (s_queueLock)
            {
                s_logs.Enqueue(message);
                s_queueNotEmpty.Set();
            }
        }

        static void WriteToLogFile( string logMessage )
        {
            try
            {
                using StreamWriter writer = new( s_logFilePath , true );
                writer.WriteLine( logMessage );
            }
            catch (Exception ex)
            {
                Trace.WriteLine( $"Error writing to log file: {ex.Message}" );
                Trace.WriteLine( logMessage );
            }
        }
    }
}
