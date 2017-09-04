﻿using Koprey.Extensions.Logging.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SampleApp
{
    public class Program
    {
        private readonly ILogger _logger;
        
        public Program()
        {
            var loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("logging.json", optional: false, reloadOnChange: true)
                .Build();

            // A Web App based program would configure logging via the WebHostBuilder.
            // Create a logger factory with filters that can be applied across all logger providers.
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConfiguration(loggingConfiguration.GetSection("Logging"))
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("SampleApp.Program", LogLevel.Debug)
                        .AddSqlServer(loggingConfiguration);
                });
            
            // providers may be added to a LoggerFactory before any loggers are created
            var serviceProvider = serviceCollection.BuildServiceProvider();
            // getting the logger using the class's name is conventional
            _logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Program().Execute(args);
        }

        public void Execute(string[] args)
        {
            _logger.LogInformation("Starting");

            var startTime = DateTimeOffset.Now;
            _logger.LogInformation(1, "Started at '{StartTime}' and 0x{Hello:X} is hex of 42", startTime, 42);
            // or
            _logger.ProgramStarting(startTime, 42);

            using (_logger.PurchaseOrderScope("00655321"))
            {
                try
                {
                    throw new Exception("Boom");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(1, ex, "Unexpected critical error starting application");
                    _logger.LogError(1, ex, "Unexpected error");
                    _logger.LogWarning(1, ex, "Unexpected warning");
                }

                using (_logger.BeginScope("Main"))
                {

                    _logger.LogInformation("Waiting for user input");

                    string input;
                    do
                    {
                        Console.WriteLine("Enter some test to log more, or 'quit' to exit.");
                        input = Console.ReadLine();

                        _logger.LogInformation("User typed '{input}' on the command line", input);
                        _logger.LogWarning("The time is now {Time}, it's getting late!", DateTimeOffset.Now);
                    }
                    while (input != "quit");
                }
            }

            var endTime = DateTimeOffset.Now;
            _logger.LogInformation(2, "Stopping at '{StopTime}'", endTime);
            // or
            _logger.ProgramStopping(endTime);

            _logger.LogInformation("Stopping");
        }



    }
}
