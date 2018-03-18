using Newtonsoft.Json;

///<summary>
/// Class Name : BatchResponseStatus
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Service.Messages.Common
{
    public class BatchResponseStatus
    {
        [JsonProperty("ResponseCode")]
        public string ResponseCode { get; set; }

        [JsonProperty("ResponseMessage")]
        public string ResponseMessage { get; set; }
    }
}
