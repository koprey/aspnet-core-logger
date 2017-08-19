using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Koprey.Extensions.Logging.SqlServer.Internal
{
    public class SqlServerLoggerProcessor : IDisposable
    {
        private const int _maxQueuedMessages = 1024;

        private readonly BlockingCollection<LogMessageEntry> _messageQueue = new BlockingCollection<LogMessageEntry>(_maxQueuedMessages);
        private readonly Task _outputTask;

        public SqlServerLoggerProcessor()
        {
            // Start SqlServer message queue processor
            _outputTask = Task.Factory.StartNew(
                ProcessLogQueue,
                this,
                TaskCreationOptions.LongRunning);
        }

        public virtual void EnqueueMessage(LogMessageEntry message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message);
                    return;
                }
                catch (InvalidOperationException) { }
            }

            // Adding is completed so just log the message
            WriteMessage(message);
        }

        // for testing
        internal virtual void WriteMessage(LogMessageEntry message)
        {
            //if (message.LevelString != null)
            //{
            //    SqlServer.Write(message.LevelString, message.LevelBackground, message.LevelForeground);
            //}

            //SqlServer.Write(message.Message, message.MessageColor, message.MessageColor);
            //SqlServer.Flush();
        }

        private void ProcessLogQueue()
        {
            foreach (var message in _messageQueue.GetConsumingEnumerable())
            {
                WriteMessage(message);
            }
        }

        private static void ProcessLogQueue(object state)
        {
            var SqlServerLogger = (SqlServerLoggerProcessor)state;

            SqlServerLogger.ProcessLogQueue();
        }

        public void Dispose()
        {
            _messageQueue.CompleteAdding();

            try
            {
                _outputTask.Wait(1500); // with timeout in-case SqlServer is locked by user input
            }
            catch (TaskCanceledException) { }
            catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException) { }
        }
    }
}