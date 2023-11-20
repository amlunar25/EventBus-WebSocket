using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace WebSocket_V4.WebSocketHandler
{
    public class WebSocketManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        
        public void AddSocket(string id, WebSocket socket)
        {
            _sockets.TryAdd(id, socket);
        }

        public void RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out _);
        }

        public async Task SendToAllAsync(string message)
        {
            foreach (var socket in _sockets.Values)
            {
                await SendMessageAsync(socket, message);
            }
        }

        public async Task SendAsync(string connectionId, string message)
        {
            if (_sockets.TryGetValue(connectionId, out var socket))
            {
                await SendMessageAsync(socket, message);
            }
        }

        private async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

    }

}
