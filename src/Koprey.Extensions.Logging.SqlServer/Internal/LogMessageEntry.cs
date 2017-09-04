using System;

namespace Koprey.Extensions.Logging.SqlServer.Internal
{
    public class LogMessageEntry
    {
        public int Id { get; set; }
        public string LevelString { get; set; }
        public string Application { get; set; }
        public string Event { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string Exception { get; set; }
    }
}