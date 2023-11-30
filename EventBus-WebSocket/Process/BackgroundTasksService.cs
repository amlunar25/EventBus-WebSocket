using EventBus_WebSocket.EventBusHandler;

namespace EventBus_WebSocket.Process
{  

    public class BackgroundTasksService : BackgroundService
    {
        private readonly ILogger<BackgroundTasksService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly List<IProcess> _processes = new List<IProcess>(); 

        public BackgroundTasksService(ILogger<BackgroundTasksService> logger, IHttpClientFactory httpClientFactory, EventBus eventBus)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _processes.Add(new AuthenticationGetAdtransNEs(eventBus));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Execute Task in Parallel
                foreach (var process in _processes)
                {
                    Task task = Task.Run(() => { process.Run(); });
                }

                // Delay before the next iteration
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
