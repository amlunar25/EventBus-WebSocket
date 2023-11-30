namespace EventBus_WebSocket.Models.Adtran
{
    public class Ports
    {
        public int? id { get; set; }
        public List<Port> serviceStateList { get; set; }
    }
    
    public class Port
    {
        public string attachedProfileName { get; set; }
        public int bridgeOperState { get; set; }
        public string circuitId { get; set; }
        public string fxsPortStatus { get; set; }
        public string key { get; set; }
        public bool participatingInBridge { get; set; }
        public int state { get; set; }
    }
}
