using Hangfire;
using Jobs;

namespace WorkerService;

public class Worker : BackgroundService
{
        private readonly ILogger<Worker> _logger;
        private readonly IRecurringJobManager _jobManager;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IRecurringJobManager jobManager, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _jobManager = jobManager;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _jobManager.AddOrUpdate<WeatherUpdateJob>(
                "MultiCityWeatherUpdateJob",
                job => job.ExecuteAsync(),
                Cron.Hourly);

            _logger.LogInformation("WeatherUpdateJob scheduled to run hourly for all configured cities.");

            return Task.CompletedTask;
        }
    }
