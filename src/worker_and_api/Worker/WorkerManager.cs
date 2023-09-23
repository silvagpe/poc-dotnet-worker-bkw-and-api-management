namespace WorkerAndApiPoc.Worker;

public class WorkerManager : BackgroundService
{
    private readonly ILogger<WorkerManager> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceProvider _serviceProvider;
    private IList<ConsumerWorker> _workers = new List<ConsumerWorker>();
    public IList<ConsumerWorker> GetWorkers() => _workers;

    public WorkerManager(ILogger<WorkerManager> logger, IConfiguration configuration, IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _configuration = configuration;
        _hostApplicationLifetime = hostApplicationLifetime;
        _hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
        _hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
        _hostApplicationLifetime.ApplicationStopped.Register(OnStopped);
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var workersSettings = WorkerSettings.Load(_configuration);

        foreach (var setting in workersSettings)
        {
            ConsumerWorker consumerWorker = new ConsumerWorker(
                _serviceProvider.GetService<ILogger<ConsumerWorker>>(),
                _serviceProvider.GetService<IHostApplicationLifetime>(),
                setting);
            
            _ = consumerWorker.StartAsync(stoppingToken);
            _workers.Add(consumerWorker);
        }

        while (!stoppingToken.IsCancellationRequested)
        {            
            await Task.Delay(10000, stoppingToken);
        }

        _hostApplicationLifetime.StopApplication();
    }

    public bool StopWorker(string workerName){
        ConsumerWorker worker = _workers.FirstOrDefault(w => w.WorkerName == workerName);
        if (worker == null) return false;

        CancellationTokenSource cts = new CancellationTokenSource();
        worker.StopAsync(cts.Token);
        cts.Cancel();
        
        return true;
    }

    public bool StartWorker(string workerName){
        ConsumerWorker worker = _workers.FirstOrDefault(w => w.WorkerName == workerName);
        if (worker == null) return false;

        CancellationTokenSource cts = new CancellationTokenSource();
        worker.StartAsync(cts.Token);
        
        return true;
    }

    private void OnStarted(){
        _logger.LogInformation($"[{typeof(WorkerManager).Name}]: IHostApplicationLifetime  - OnStarted");
    }

    private void OnStopping(){
        _logger.LogInformation($"[{typeof(WorkerManager).Name}]: IHostApplicationLifetime  - OnStopping");
    }

    private void OnStopped(){
        _logger.LogInformation($"[{typeof(WorkerManager).Name}]: IHostApplicationLifetime  - OnStopped");
    }
}