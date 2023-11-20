using System;
using System.Collections.Generic;

namespace EventBus_WebSocket.EventBusHandler
{
    public class EventBus
    {
        private readonly Dictionary<string, EventHandler<MessageEventArgs>> subscribers = new Dictionary<string, EventHandler<MessageEventArgs>>();

        public void Subscribe(string clientId, EventHandler<MessageEventArgs> handler)
        {
            if (!subscribers.ContainsKey(clientId))
            {
                subscribers[clientId] = handler;
            }
        }

        public void Unsubscribe(string clientId)
        {
            subscribers.Remove(clientId);
        }

        public void Publish(string message)
        {
            var args = new MessageEventArgs { Message = message };

            foreach (var subscriber in subscribers)
            {
                // Ensure the handler is not null before invoking
                subscriber.Value?.Invoke(this, args);
            }
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
