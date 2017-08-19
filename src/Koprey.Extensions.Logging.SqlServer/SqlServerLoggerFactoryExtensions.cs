using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Koprey.Extensions.Logging.SqlServer
{
    public static class SqlServerLoggerExtensions
    {
        /// <summary>
        /// Adds a SqlServer logger named 'SqlServer' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        public static ILoggingBuilder AddSqlServer(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, SqlServerLoggerProvider>();

            return builder;
        }

        /// <summary>
        /// Adds a SqlServer logger named 'SqlServer' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configure"></param>
        public static ILoggingBuilder AddSqlServer(this ILoggingBuilder builder, Action<SqlServerLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddSqlServer();
            builder.Services.Configure(configure);

            return builder;
        }

        /// <summary>
        /// Adds a SqlServer logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        public static ILoggerFactory AddSqlServer(this ILoggerFactory factory)
        {
            return factory.AddSqlServer(includeScopes: false);
        }

        /// <summary>
        /// Adds a SqlServer logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        public static ILoggerFactory AddSqlServer(this ILoggerFactory factory, bool includeScopes)
        {
            factory.AddSqlServer((n, l) => l >= LogLevel.Information, includeScopes);
            return factory;
        }

        /// <summary>
        /// Adds a SqlServer logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        public static ILoggerFactory AddSqlServer(this ILoggerFactory factory, LogLevel minLevel)
        {
            factory.AddSqlServer(minLevel, includeScopes: false);
            return factory;
        }

        /// <summary>
        /// Adds a SqlServer logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        public static ILoggerFactory AddSqlServer(
            this ILoggerFactory factory,
            LogLevel minLevel,
            bool includeScopes)
        {
            factory.AddSqlServer((category, logLevel) => logLevel >= minLevel, includeScopes);
            return factory;
        }

        /// <summary>
        /// Adds a SqlServer logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="filter">The category filter to apply to logs.</param>
        public static ILoggerFactory AddSqlServer(
            this ILoggerFactory factory,
            Func<string, LogLevel, bool> filter)
        {
            factory.AddSqlServer(filter, includeScopes: false);
            return factory;
        }

        /// <summary>
        /// Adds a SqlServer logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="filter">The category filter to apply to logs.</param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        public static ILoggerFactory AddSqlServer(
            this ILoggerFactory factory,
            Func<string, LogLevel, bool> filter,
            bool includeScopes)
        {
            factory.AddProvider(new SqlServerLoggerProvider(filter, includeScopes));
            return factory;
        }


        /// <summary>
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="settings">The settings to apply to created <see cref="SqlServerLogger"/>'s.</param>
        /// <returns></returns>
        public static ILoggerFactory AddSqlServer(
            this ILoggerFactory factory,
            ISqlServerLoggerSettings settings)
        {
            factory.AddProvider(new SqlServerLoggerProvider(settings));
            return factory;
        }

        /// <summary>
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to use for <see cref="ISqlServerLoggerSettings"/>.</param>
        /// <returns></returns>
        public static ILoggerFactory AddSqlServer(this ILoggerFactory factory, IConfiguration configuration)
        {
            var settings = new ConfigurationSqlServerLoggerSettings(configuration);
            return factory.AddSqlServer(settings);
        }
    }

}
