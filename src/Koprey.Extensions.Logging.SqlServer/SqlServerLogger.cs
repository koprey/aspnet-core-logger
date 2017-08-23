using Koprey.Extensions.Logging.SqlServer.Internal;
using Microsoft.Extensions.Logging;
using System;

namespace Koprey.Extensions.Logging.SqlServer
{
    public class SqlServerLogger : ILogger
    {     
        private readonly SqlServerLoggerProcessor _queueProcessor;
        private Func<string, LogLevel, bool> _filter;
        

        static SqlServerLogger()
        {
            var logLevelString = GetLogLevelString(LogLevel.Information);
        }

        public SqlServerLogger(string name, Func<string, LogLevel, bool> filter, bool includeScopes)
            : this(name, filter, includeScopes, new SqlServerLoggerProcessor())
        {
        }

        internal SqlServerLogger(string name, Func<string, LogLevel, bool> filter, bool includeScopes, SqlServerLoggerProcessor loggerProcessor)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Filter = filter ?? ((category, logLevel) => true);
            IncludeScopes = includeScopes;

            _queueProcessor = loggerProcessor;
            
        }

        public Func<string, LogLevel, bool> Filter
        {
            get { return _filter; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _filter = value;
            }
        }

        public bool IncludeScopes { get; set; }

        public string Name { get; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, Name, eventId.Id, message, exception);
            }
        }

        public virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception)
        {
            var entry = new LogMessageEntry
            {
                Timestamp = DateTime.Now,
                LevelString = GetLogLevelString(logLevel),
                Message = message
            };
            // scope information
            if (IncludeScopes)
            {
            }

            // Example:
            // System.InvalidOperationException
            //    at Namespace.Class.Function() in File:line X
            if (exception != null)
            {
                // exception message
                entry.Exception = exception.ToString();
            }

            _queueProcessor.EnqueueMessage(entry);                        
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }

            return Filter(Name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return SqlServerLogScope.Push(Name, state);
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            return logLevel.ToString();
        }
        
        
    }
}