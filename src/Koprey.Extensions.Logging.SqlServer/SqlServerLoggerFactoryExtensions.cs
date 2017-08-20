using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Koprey.Extensions.Logging.SqlServer
{
    public static class SqlServerLoggerExtensions
    {
        /// <summary>
        /// Adds a SqlServer logger named 'SqlServer' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to use for <see cref="LoggingContext"/>.</param>
        public static ILoggingBuilder AddSqlServer(this ILoggingBuilder builder, IConfiguration configuration)
        {
            //builder.Services.AddDbContext<LoggingContext>(options => options.UseSqlServer(configuration["ConnectionString"]));
            //builder.Services.AddSingleton<ILoggerProvider, SqlServerLoggerProvider>();
            var settings = new ConfigurationSqlServerLoggerSettings(configuration);
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
