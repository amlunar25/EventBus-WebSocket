namespace EventBus_WebSocket.Models.Adtran
{
    public class NN
    {
        public string circuitIDMatchErrorString { get; set; }
        public List<NE> collectionOfNEs { get; set; }
        public int? id { get; set; }
        public bool isCircuitIDSearchResult { get; set; }
        public string matchingCircuitIDKeyHeirarchy { get; set; }
        public int matchingCircuitIDPageNumber { get; set; }
        public bool maxRecordsFound { get; set; }
        public string maxRecordsFoundString { get; set; }
        public string maxRecordsFoundToolTipString { get; set; }
        public bool showHighestAlarmSeverityFlag { get; set; }
        public int totalMatchedCount { get; set; }
    }

    public class NE
    {
        public string IPAddress { get; set; }
        public string KEY { get; set; }
        public int accessLevel { get; set; }
        public bool alarmResyncEnable { get; set; }
        public bool alarmsEnable { get; set; }
        public bool allowCapacityManager { get; set; }
        public bool allowListServices { get; set; }
        public bool allowReplaceONT { get; set; }
        public bool allowServiceMonitor { get; set; }
        public bool assignLocation { get; set; }
        public string bridgeName { get; set; }
        public int bridgeOperState { get; set; }
        public string businessClassName { get; set; }
        public bool canBeSelectedForExportInGui { get; set; }
        public bool canRequestServiceStatus { get; set; }
        public string cardName { get; set; }
        public string cardSpecificType { get; set; }
        public bool checkHeartBeat { get; set; }
        public List<string> children { get; set; }
        public string circuitID { get; set; }
        public bool circuitIdSetEnable { get; set; }
        public bool communityStringSetEnable { get; set; }
        public bool dataCollectorConfigurationEnable { get; set; }
        public string description { get; set; }
        public bool editEquipmentEnable { get; set; }
        public string emsType { get; set; }
        public string equipmentName { get; set; }
        public int highestAlarmSeverity { get; set; }
        public bool isBridge { get; set; }
        public bool isProvisioned { get; set; }
        public string key { get; set; }
        public string level { get; set; }
        public bool logical { get; set; }
        public bool manageEvcEnable { get; set; }
        public string name { get; set; }
        public string neName { get; set; }
        public int numComponents { get; set; }
        public string ontBatteryStatus { get; set; }
        public string parentPortName { get; set; }
        public int? parentPortNumber { get; set; }
        public string portName { get; set; }
        public string portNumber { get; set; }
        public string portNumberAsInteger { get; set; }
        public string portSpecificType { get; set; }
        public string provState { get; set; }
        public bool scaRestore { get; set; }
        public string serviceType { get; set; }
        public string shelfName { get; set; }
        public int? shelfNumber { get; set; }
        public bool showDetailEnable { get; set; }
        public bool showLinkExplorerView { get; set; }
        public string slotNumber { get; set; }
        public string slotNumberAsInteger { get; set; }
        public int sqlDbId { get; set; }
        public string state { get; set; }
        public string subPortName { get; set; }
        public int? subPortNumber { get; set; }
        public int sysObjectId { get; set; }
        public string titleName { get; set; }
        public string tooltipMsg { get; set; }
    }
}