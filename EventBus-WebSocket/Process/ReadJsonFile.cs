using EventBus_WebSocket.EventBusHandler;

namespace EventBus_WebSocket.Process
{
    public class ReadJsonFile : IProcess
    {
        private readonly EventBus _eventBus;

        public ReadJsonFile(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public void Run()
        {
            var result = ReadJsonFileAsync().GetAwaiter().GetResult();

            Console.WriteLine(result);
        }

        private async Task<string> ReadJsonFileAsync()
        {
            try
            {
                var jsonFilePath = "D:\\Projects\\EventBus-WebSocket\\EventBus-WebSocket\\test.json";
                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                Console.WriteLine(jsonContent);

                return jsonContent;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
