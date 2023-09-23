namespace WorkerAndApiPoc.Worker;

public class ConsumerWorker : BackgroundService
{
    private readonly ILogger<ConsumerWorker> _logger;
    private readonly WorkerSettings _workerSettings;
    private IHostApplicationLifetime _hostApplicationLifetime;

    public string WorkerName { get => _workerSettings.Name; }
    public bool IsRunning { get => ExecuteTask != null? !ExecuteTask.IsCompleted : false; }

    public ConsumerWorker(ILogger<ConsumerWorker> logger, IHostApplicationLifetime hostApplicationLifetime, WorkerSettings workerSettings)
    {
        _logger = logger;
        _workerSettings = workerSettings;
        _hostApplicationLifetime = hostApplicationLifetime;
        _hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
        _hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
        _hostApplicationLifetime.ApplicationStopped.Register(OnStopped);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Service: {WorkerName} is running at: {DateTimeOffset.Now}");        
            await Task.Delay(10000, stoppingToken);
        }
    }

    private void OnStarted(){
        _logger.LogInformation($"[{WorkerName}]: IHostApplicationLifetime - OnStarted");
    }

    private void OnStopping(){
        _logger.LogInformation($"[{WorkerName}]: IHostApplicationLifetime - OnStopping");
    }

    private void OnStopped(){
        _logger.LogInformation($"[{WorkerName}]: IHostApplicationLifetime - OnStopped");
    }
}