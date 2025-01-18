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
            // Agendar o job para rodar a cada hora
            _jobManager.AddOrUpdate<WeatherUpdateJob>(
                "WeatherUpdateJob",
                job => job.ExecuteAsync("São Paulo"),
                Cron.Hourly);

            _logger.LogInformation("WeatherUpdateJob agendado para rodar a cada hora.");

            return Task.CompletedTask;
        }
    }
