using Microsoft.EntityFrameworkCore;

namespace Koprey.Extensions.Logging.SqlServer.Internal
{
    public class LoggingContext : DbContext
    {
        public LoggingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LogMessageEntry> Entries { get; set; }

    }
}
