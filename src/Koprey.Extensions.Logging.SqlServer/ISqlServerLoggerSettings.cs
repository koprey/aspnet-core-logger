using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Koprey.Extensions.Logging.SqlServer
{
    public interface ISqlServerLoggerSettings
    {
        string ConnectionString { get; }

        bool IncludeScopes { get; }

        IChangeToken ChangeToken { get; }

        bool TryGetSwitch(string name, out LogLevel level);

        ISqlServerLoggerSettings Reload();
    }
}