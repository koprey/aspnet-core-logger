using Koprey.Extensions.Logging.SqlServer.Internal;
using Microsoft.EntityFrameworkCore;
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
        /// <param name="configuration">The <see cref="IConfiguration"/> to use for <see cref="LoggingDBContext"/>.</param>
        public static ILoggingBuilder AddSqlServer(this ILoggingBuilder builder, IConfiguration configuration)
        {
            var settings = new ConfigurationSqlServerLoggerSettings(configuration.GetSection("Logging"));
            //builder.Services.AddDbContext<LoggingDBContext>(o=>o.UseSqlServer(settings.ConnectionString));
            //builder.Services.AddScoped<LoggingDBContext>();
            //builder.Services.AddSingleton<ILoggerProvider, SqlServerLoggerProvider>();
            
            builder.AddProvider(new SqlServerLoggerProvider(settings));

            return builder;
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
