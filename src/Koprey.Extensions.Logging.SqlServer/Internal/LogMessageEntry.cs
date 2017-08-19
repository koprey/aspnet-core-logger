using System;

namespace Koprey.Extensions.Logging.SqlServer.Internal
{
    public struct LogMessageEntry
    {
        public string LevelString;
        public string Application;
        public string Event;
        public string Message;
        public DateTime Timestamp;
    }
}