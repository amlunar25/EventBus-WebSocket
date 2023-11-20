using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket_V4.WebSocketHandler
{
    public class WebSocketBackgroundService : BackgroundService
    {
        private readonly WebSocketManager _webSocketManager;

        public WebSocketBackgroundService(WebSocketManager webSocketManager)
        {
            _webSocketManager = webSocketManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Send a broadcast message every 30 seconds
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                await _webSocketManager.SendToAllAsync("Broadcast message from the server!");
            }
        }
    }
}

