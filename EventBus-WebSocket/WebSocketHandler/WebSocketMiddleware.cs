using EventBus_WebSocket.EventBusHandler;
using EventBus_WebSocket.WebSocketHandler;
using System.Net.WebSockets;
using System.Text;

namespace WebSocket_V4.WebSocketHandler
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EventBus _eventBus;
        public WebSocketMiddleware(RequestDelegate next, EventBus eventBus)
        {
            _next = next;
            _eventBus = eventBus;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                var webSocketServer = new WebSocketServer(webSocket, _eventBus);

                await webSocketServer.StartReceiving();
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}

