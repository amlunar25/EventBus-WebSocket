using EventBus_WebSocket.EventBusHandler;
using System.Net.WebSockets;
using System.Text;

namespace EventBus_WebSocket.WebSocketHandler
{
    public class WebSocketServer
    {
        private readonly WebSocket _webSocket;
        private readonly EventBus _eventBus;
        private readonly string _clientId;
        private static readonly Dictionary<string, WebSocket> _clientConnections = new Dictionary<string, WebSocket>();

        public WebSocketServer(WebSocket webSocket, EventBus eventBus)
        {
            _webSocket = webSocket;
            _eventBus = eventBus;
            _clientId = Guid.NewGuid().ToString();

            // Add the new WebSocket connection to the dictionary
            _clientConnections[_clientId] = _webSocket;
        }

        public async Task StartReceiving()
        {
            try
            {
                // Subscribe this WebSocket client
                _eventBus.Subscribe(_clientId, HandleMessage);

                while (_webSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Received from {_clientId} the message : {message}");

                        HandleMessage(_clientId, new MessageEventArgs { Message = message });
                        // Handle your business logic here
                        //_eventBus.Publish(message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        // Unsubscribe and close WebSocket connection
                        _eventBus.Unsubscribe(_clientId);
                        _clientConnections.Remove(_clientId);
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket exception: {ex.Message}");
            }
        }

        private void HandleMessage(object sender, MessageEventArgs args)
        {
            // Send a reply to the client who sent the message
            SendAsync(_clientId, $"Client {_clientId} replies: {args.Message}");
        }

        public static async Task SendAsync(string clientId, string message)
        {
            try
            {
                if (_clientConnections.TryGetValue(clientId, out var clientWebSocket))
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    await clientWebSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to WebSocket client {clientId}: {ex.Message}");
            }
        }
    }
}
