using Newtonsoft.Json;

///<summary>
/// Class Name : InsertRequest
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Service.Messages
{
    public class BatchInsertStatusRequest
    {
        [JsonProperty("SystemCode")]
        public string SystemCode { get; set; }

        [JsonProperty("ServiceName")]
        public string ServiceName { get; set; }

        [JsonProperty("DataDate")]
        public string DataDate { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("GetResult")]
        public bool GetResult { get; set; }
    }
}
