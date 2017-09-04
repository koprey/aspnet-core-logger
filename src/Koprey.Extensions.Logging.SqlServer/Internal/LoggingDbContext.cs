using Microsoft.EntityFrameworkCore;

namespace Koprey.Extensions.Logging.SqlServer.Internal
{
    public class LoggingDBContext : DbContext
    {
        public LoggingDBContext(DbContextOptions<LoggingDBContext> options)
            : base(options)
        {
        }

        public DbSet<LogMessageEntry> LogMessageEntries { get; set; }

    }
}
