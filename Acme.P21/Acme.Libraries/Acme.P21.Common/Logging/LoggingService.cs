using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using Serilog.Sinks.Elasticsearch;

namespace Acme.P21.Common.Logging
{
    public class LoggingService<T> : ILoggingService
    {
        #region Fields

        private readonly ILogger _logger;
       
        #endregion

        #region Constructors

        public LoggingService(AppConfiguration configuration)
        {
            var levelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = GetMinimumLogLevel(configuration.LogLevel)
            };

            if (Log.Logger == Logger.None)
            {
                if (configuration.LogInternal) Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(File.CreateText("c://temp/logs/selflog.txt")));

                AppDomain.CurrentDomain.DomainUnload += (s, e) => Log.CloseAndFlush();

                //Log.Logger = InitializeFileSink(levelSwitch, configuration.Application, configuration.LogFile);
                Log.Logger = InitializeDatabaseSink(levelSwitch, configuration);
                //Log.Logger = InitializeElasticSearchSink(levelSwitch, configuration);
                //Log.Logger = InitializeSeqSink(levelSwitch, configuration);
                
                //Log.Information("Initializing the logger on the call for type: {0}", typeof(T).ToString());
            }
            
            //Log.Information("Instantiating a new Log instance for type: {0}", typeof(T).ToString());
            _logger = Log.ForContext<T>();
        }

        #endregion

        #region Interface Members

        public void Debug(string messageTemplate) => _logger.Debug(messageTemplate);

        public void Debug<T1>(string messageTemplate, T1 propertyValue) => _logger.Debug(messageTemplate, propertyValue);

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Debug(messageTemplate, propertyValue0, propertyValue1);

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Debug(string messageTemplate, params object[] propertyValues) => _logger.Debug(messageTemplate, propertyValues);

        public void Debug(Exception exception, string messageTemplate) => _logger.Debug(exception, messageTemplate);

        public void Debug<T1>(Exception exception, string messageTemplate, T1 propertyValue) => _logger.Debug(exception, messageTemplate, propertyValue);

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1);

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues) => _logger.Debug(exception, messageTemplate, propertyValues);

        public void Error(string messageTemplate) => _logger.Error(messageTemplate);

        public void Error<T1>(string messageTemplate, T1 propertyValue) => _logger.Error(messageTemplate, propertyValue);

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Error(messageTemplate, propertyValue0, propertyValue1);

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Error(string messageTemplate, params object[] propertyValues) => _logger.Error(messageTemplate, propertyValues);

        public void Error(Exception exception, string messageTemplate) => _logger.Error(exception, messageTemplate);

        public void Error<T1>(Exception exception, string messageTemplate, T1 propertyValue) => _logger.Error(exception, messageTemplate, propertyValue);

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues) => _logger.Error(exception, messageTemplate, propertyValues);

        public void Fatal(string messageTemplate) => _logger.Fatal(messageTemplate);

        public void Fatal<T1>(string messageTemplate, T1 propertyValue) => _logger.Fatal(messageTemplate, propertyValue);

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Fatal(messageTemplate, propertyValue0, propertyValue1);

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Fatal(string messageTemplate, params object[] propertyValues) => _logger.Fatal(messageTemplate, propertyValues);

        public void Fatal(Exception exception, string messageTemplate) => _logger.Fatal(exception, messageTemplate);

        public void Fatal<T1>(Exception exception, string messageTemplate, T1 propertyValue) => _logger.Fatal(exception, messageTemplate, propertyValue);

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        
        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues) => _logger.Fatal(exception, messageTemplate, propertyValues);

        public void Information(string messageTemplate) => _logger.Information(messageTemplate);

        public void Information<T1>(string messageTemplate, T1 propertyValue) => _logger.Information(messageTemplate, propertyValue);

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Information(messageTemplate, propertyValue0, propertyValue1);

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Information(string messageTemplate, params object[] propertyValues) => _logger.Information(messageTemplate, propertyValues);

        public void Information(Exception exception, string messageTemplate) => _logger.Information(exception, messageTemplate);

        public void Information<T1>(Exception exception, string messageTemplate, T1 propertyValue) => _logger.Information(exception, messageTemplate, propertyValue);

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1);

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues) => _logger.Information(exception, messageTemplate, propertyValues);

        public bool IsEnabled(LogEventLevel level) => _logger.IsEnabled(level);

        public void Verbose(string messageTemplate) => _logger.Verbose(messageTemplate);

        public void Verbose<T1>(string messageTemplate, T1 propertyValue) => _logger.Verbose(messageTemplate, propertyValue);

        public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Verbose(messageTemplate, propertyValue0, propertyValue1);

        public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Verbose(string messageTemplate, params object[] propertyValues) => _logger.Verbose(messageTemplate, propertyValues);

        public void Verbose(Exception exception, string messageTemplate) => _logger.Verbose(exception, messageTemplate);

        public void Verbose<T1>(Exception exception, string messageTemplate, T1 propertyValue) => _logger.Verbose(exception, messageTemplate, propertyValue);

        public void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);

        public void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues) => _logger.Verbose(exception, messageTemplate, propertyValues);

        public void Warning(string messageTemplate) => _logger.Warning(messageTemplate);

        public void Warning<T1>(string messageTemplate, T1 propertyValue) => _logger.Warning(messageTemplate, propertyValue);

        public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Warning(messageTemplate, propertyValue0, propertyValue1);

        public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Warning(string messageTemplate, params object[] propertyValues) => _logger.Warning(messageTemplate, propertyValues);

        public void Warning(Exception exception, string messageTemplate) => _logger.Warning(exception, messageTemplate);

        public void Warning<T1>(Exception exception, string messageTemplate, T1 propertyValue) => _logger.Warning(exception, messageTemplate, propertyValue);

        public void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) => _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1);

        public void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) => _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues) => _logger.Warning(exception, messageTemplate, propertyValues);

        #endregion

        #region Private Methods

        private Logger InitializeElasticSearchSink(LoggingLevelSwitch levelSwitch, AppConfiguration configuration)
        {
            try
            {
                return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration.ElasticUrl))
                    {
                        IndexFormat = "p21-logs-{0:yyyy.MM}",
                        //DetectElasticsearchVersion = true
                    })
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithProperty("Application", configuration.Application)
                    .CreateLogger();
            }
            catch
            {
                throw new Exception("Cannot Initialize Elastic Search Logger");
            }
            
        }

        private Logger InitializeDatabaseSink(LoggingLevelSwitch levelSwitch, AppConfiguration configuration)
        {
            try
            {
                var columnOptions = new ColumnOptions
                {
                    AdditionalColumns = new Collection<SqlColumn>
                    {
                        new SqlColumn
                            {ColumnName = "Application", PropertyName = "Application", DataType = SqlDbType.VarChar, DataLength = 150, AllowNull = true},
                        new SqlColumn
                            {ColumnName = "SourceContext",  PropertyName = "SourceContext", DataType = SqlDbType.VarChar, DataLength = 300, AllowNull = true},
                        new SqlColumn
                            {ColumnName = "MachineName",  PropertyName = "MachineName", DataType = SqlDbType.VarChar, DataLength = 300, AllowNull = true},
                        new SqlColumn
                            {ColumnName = "EnvironmentUserName",  PropertyName = "EnvironmentUserName", DataType = SqlDbType.VarChar, DataLength = 300, AllowNull = true}
                    },
                    Properties =
                    {
                        ExcludeAdditionalProperties = true
                    }
                };

                columnOptions.Store.Remove(StandardColumn.Properties);
                columnOptions.Store.Add(StandardColumn.LogEvent);
                columnOptions.LogEvent.DataLength = 2048;
                columnOptions.PrimaryKey = columnOptions.TimeStamp;
                columnOptions.TimeStamp.NonClusteredIndex = true;

                return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.MSSqlServer(
                        connectionString: configuration.LogSqlConnection,
                        sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true },
                        columnOptions: columnOptions)
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithProperty("Application", configuration.Application)
                    //)
                    .CreateLogger();
            }
            catch
            {
                throw new Exception("Cannot Initialize Database Logger");
            }
        }

        private Logger InitializeFileSink(LoggingLevelSwitch levelSwitch, string application, string logFile)
        {
            try
            {
                const string template = "[{Timestamp:HH:mm:ss} {Level:u3}] |{Properties} |{SourceContext}| {Message:lj}{Exception}{NewLine}";

                return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, outputTemplate: template)
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithProperty("Application", application)
                    .CreateLogger();
            }
            catch
            {
                throw new Exception("Cannot Initialize File Logger");
            }
        }

        private static Logger InitializeSeqSink(LoggingLevelSwitch levelSwitch, AppConfiguration configuration)
        {
            try
            {
                return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.Seq(configuration.SeqUrl)
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithProperty("Application", configuration.Application)
                    .CreateLogger();
            }
            catch
            {
                throw new Exception("Cannot Initialize Seq Logger");
            }
        }

        private LogEventLevel GetMinimumLogLevel(string logLevel)
        {
            switch (logLevel)
            {
                case "Info":
                    return LogEventLevel.Information;
                case "Debug":
                    return LogEventLevel.Debug;
                case "Error":
                    return LogEventLevel.Error;
                case "Fatal":
                    return LogEventLevel.Fatal;
                case "Warning":
                    return LogEventLevel.Warning;
                case "Verbose":
                    return LogEventLevel.Verbose;
                default:
                    return LogEventLevel.Information;

            }
        }

        #endregion
    }
}
