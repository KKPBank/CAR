using Newtonsoft.Json;
using System;

///<summary>
/// Class Name : BatchHeader
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
    public class BatchHeader
    {
        [JsonProperty("ReferenceNo")]
        public string ReferenceNo { get; set; }

        [JsonProperty("Filename")]
        public string Filename { get; set; }

        [JsonProperty("CreateDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("CurrentSequence")]
        public string CurrentSequence { get; set; }

        [JsonProperty("TotalSequence")]
        public string TotalSequence { get; set; }

        [JsonProperty("TotalRecord")]
        public string TotalRecord { get; set; }

        [JsonProperty("SystemCode")]
        public string SystemCode { get; set; }

        [JsonProperty("SecurityKey")]
        public string SecurityKey { get; set; }
    }
}
