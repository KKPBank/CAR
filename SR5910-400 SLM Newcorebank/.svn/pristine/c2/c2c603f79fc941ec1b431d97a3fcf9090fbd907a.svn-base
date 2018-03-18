using KKCAR.Service.Messages.Common;
using Newtonsoft.Json;

///<summary>
/// Class Name : BatchCreateStatusResponse
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
    public class BulkInsertStatusResponse
    {
        [JsonProperty("header")]
        public BatchHeader Header { get; set; }

        [JsonProperty("body")]
        public BatchResponseBody[] Body { get; set; }

        public BulkInsertStatusResponse()
        {
        }

        public BulkInsertStatusResponse(BatchHeader header)
        {
            this.Header = header;
        }
    }
}
