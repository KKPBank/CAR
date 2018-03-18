using KKCAR.Service.Messages.Common;
using Newtonsoft.Json;

///<summary>
/// Class Name : BatchCreateStatusRequest
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Service.Messages.Batch
{
    public class BulkInsertStatusRequest
    {
        [JsonProperty("header")]
        public BatchHeader Header { get; set; }

        [JsonProperty("body")]
        public BatchRequestBody[] Body { get; set; }
    }
}
