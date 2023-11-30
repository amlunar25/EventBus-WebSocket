using EventBus_WebSocket.EventBusHandler;
using EventBus_WebSocket.Models.Adtran;
using Newtonsoft.Json;

namespace EventBus_WebSocket.Process
{
    public class AuthenticationGetAdtransNEs : IProcess
    {
        private readonly EventBus _eventBus;
        private const string Username = "******";
        private const string Password = "******";

        //private readonly ILogger _logger;
        public AuthenticationGetAdtransNEs(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public void Run()
        {
            var result1 = AuthenticationAsync();
            var result2 = GetNetworkEquipmentsAsync(result1.GetAwaiter().GetResult());

            //foreach (var Equipment in result2.Result)
            //{

            //}

            //Console.WriteLine($"Combined result: {combinedResult}");
            //_eventBus.Publish(combinedResult);
        }

        private async Task<string> AuthenticationAsync()
        {
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var client = new HttpClient(handler);
                var request = new HttpRequestMessage(HttpMethod.Get, "https://192.168.210.112/aoe/Login.action?&view=json");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(Username), "username");
                content.Add(new StringContent(Password), "password");
                request.Content = content;

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var res = await response.Content.ReadAsStringAsync();

                    // Console.WriteLine(res);

                    if (response.Headers.TryGetValues("Set-Cookie", out var cookieValues))
                    {
                        var cookie = cookieValues.FirstOrDefault();
                        Console.WriteLine($"Received Cookie: {cookie}");
                        return cookie;
                    }
                    else
                    {
                        return "error";
                    }
                }
            }
        }

        private async Task<NN> GetNetworkEquipmentsAsync(string jsessionId)
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(handler);
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://192.168.210.112/aoe/RetrieveNEs.action?view=json");
                    request.Headers.Add("Cookie", jsessionId);
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent("1"), "pageNumber");
                    content.Add(new StringContent("25"), "pageSize");
                    content.Add(new StringContent("All Locations"), "locations");
                    content.Add(new StringContent("false"), "isCircuitIDQuery");
                    content.Add(new StringContent("Adtran.AdtranSystem,Adtran.AdtranShelf,RedCell.Config.DiscoveredEntities"), "classesToQuery");
                    content.Add(new StringContent("1701199924616"), "nochache");
                    content.Add(new StringContent("Main"), "applicationName");
                    request.Content = content;

                    var startTime = DateTime.Now;

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();

                        // Read and return the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        //Deserialize Response
                        try
                        {
                            var responseContentDeserialized1 = JsonConvert.DeserializeObject<NN>(responseContent.ToString());

                            Console.WriteLine($"Response: {responseContentDeserialized1.ToString()}");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        var responseContentDeserialized = JsonConvert.DeserializeObject<NN>(responseContent.ToString());

                        //Console.WriteLine($"Response: {JsonConvert.SerializeObject(responseContentDeserialized)}");

                        foreach ( var equipment in responseContentDeserialized.collectionOfNEs )
                        {
                            //Console.WriteLine($"Equipment: {equipment.sqlDbId.ToString()} , {equipment.key.ToString()}" );

                            var shelfs = GetNetworkShelfsAsync(jsessionId, equipment.sqlDbId.ToString(), equipment.key).GetAwaiter().GetResult();

                            if (shelfs.collectionOfNEs.Count > 0)
                            {
                                foreach (var shelf in shelfs.collectionOfNEs)
                                {
                                    //Console.WriteLine($"Shelf: {shelf.sqlDbId.ToString()} , {shelf.key.ToString()}");

                                    var tarjets = GetNetworkShelfsAsync(jsessionId, shelf.sqlDbId.ToString(), shelf.key).GetAwaiter().GetResult();
                                    
                                    if (tarjets.collectionOfNEs.Count > 0)
                                    {
                                        foreach (var tarjet in tarjets.collectionOfNEs)
                                        {
                                            //Console.WriteLine($"Tarjet: {tarjet.sqlDbId.ToString()} , {tarjet.key.ToString()}");

                                            var ports = GetNetworkShelfsAsync(jsessionId, tarjet.sqlDbId.ToString(), tarjet.key).GetAwaiter().GetResult();

                                            //string keyPorts = "";
                                            Console.WriteLine(ports.collectionOfNEs.Count);
                                            if (ports.collectionOfNEs.Count > 0)
                                            {
                                                foreach (var port in ports.collectionOfNEs)
                                                {
                                                    var portStatus = GetPortStatusAsync(jsessionId, port.key).GetAwaiter().GetResult();
                                                    if (portStatus.serviceStateList.Count > 0)
                                                    {
                                                        if (portStatus.serviceStateList[0].state == 1008)
                                                        {
                                                            Console.WriteLine($"Equipment: {equipment.sqlDbId.ToString()} , {equipment.key.ToString()} , {equipment.titleName.ToString()}");
                                                            Console.WriteLine($"Shelf: {shelf.sqlDbId.ToString()} , {shelf.key.ToString()} , {shelf.titleName.ToString()}");
                                                            Console.WriteLine($"Tarjet: {tarjet.sqlDbId.ToString()} , {tarjet.key.ToString()} , {tarjet.titleName.ToString()}");
                                                            Console.WriteLine($"Port: {port.state}, {port.name}, {portStatus.serviceStateList[0].state}");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            Console.WriteLine("Finished");
                        }
                        var endTime = DateTime.Now;
                        Console.WriteLine($"Time taken: {endTime - startTime}");
                        return responseContentDeserialized;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<NN> GetNetworkShelfsAsync(string jsessionId, string sqlDbId, string equipmentKey)
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(handler);
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://192.168.210.112/aoe/ExpandNE.action?view=json");
                    request.Headers.Add("Cookie", jsessionId);
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(sqlDbId), "sqlDbId");
                    content.Add(new StringContent(equipmentKey), "equipmentKey");
                    request.Content = content;

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();

                        // Read and return the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var responseContentDeserialized1 = JsonConvert.DeserializeObject<NN>(responseContent.ToString());

                            Console.WriteLine($"Response: {responseContentDeserialized1.ToString()}");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        var responseContentDeserialized = JsonConvert.DeserializeObject<NN>(responseContent.ToString());

                        return responseContentDeserialized;

                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<Ports> GetPortStatusAsync(string jsessionId, string keysForServiceState)
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(handler);
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://192.168.210.112/aoe/GetPortServiceStatus.action?view=json");
                    request.Headers.Add("Cookie", jsessionId);
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(keysForServiceState), "keysForServiceState");
                    request.Content = content;

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();

                        // Read and return the response content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var responseContentDeserialized1 = JsonConvert.DeserializeObject<Ports>(responseContent.ToString());

                            Console.WriteLine($"Response: {responseContentDeserialized1.ToString()}");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        var responseContentDeserialized = JsonConvert.DeserializeObject<Ports>(responseContent.ToString());

                        return responseContentDeserialized;

                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
