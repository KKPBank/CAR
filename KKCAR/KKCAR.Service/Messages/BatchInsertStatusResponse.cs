using System;
using Newtonsoft.Json;

///<summary>
/// Class Name : InsertResponse
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
    public class BatchInsertStatusResponse
    {
        [JsonProperty("ResponseCode")]
        public string ResponseCode { get; set; }

        [JsonProperty("ResponseMessage")]
        public string ResponseMessage { get; set; }
    }
}
