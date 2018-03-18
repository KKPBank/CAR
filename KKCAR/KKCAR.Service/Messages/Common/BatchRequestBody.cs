using Newtonsoft.Json;
using System;

///<summary>
/// Class Name : BatchRequestBody
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
    public class BatchRequestBody
    {
        [JsonProperty("ReferenceNo")]
        public string ReferenceNo { get; set; }

        [JsonProperty("ChannelId")]
        public string ChannelId { get; set; }

        [JsonProperty("StatusDateTime")]
        public DateTime StatusDateTime { get; set; }

        [JsonProperty("SubscriptionId")]
        public string SubscriptionId { get; set; }

        [JsonProperty("SubscriptionCusType")]
        public string SubscriptionCusType { get; set; }

        [JsonProperty("SubscriptionCardType")]
        public string SubscriptionCardType { get; set; }

        [JsonProperty("PDMProductGroupId")]
        public string PDMProductGroupId { get; set; }

        [JsonProperty("PDMProductGroupDesc")]
        public string PDMProductGroupDesc { get; set; }

        [JsonProperty("PDMProductSubGroupId")]
        public string PDMProductSubGroupId { get; set; }

        [JsonProperty("PDMProductSubGroupDesc")]
        public string PDMProductSubGroupDesc { get; set; }

        [JsonProperty("PDMProductId")]
        public string PDMProductId { get; set; }

        [JsonProperty("PDMProductDesc")]
        public string PDMProductDesc { get; set; }

        [JsonProperty("PDMCampaignId")]
        public string PDMCampaignId { get; set; }

        [JsonProperty("PDMCampaignDesc")]
        public string PDMCampaignDesc { get; set; }

        [JsonProperty("CMTProductGroupID")]
        public string CMTProductGroupID { get; set; }

        [JsonProperty("CMTProductGroupDesc")]
        public string CMTProductGroupDesc { get; set; }

        [JsonProperty("CMTProductID")]
        public string CMTProductID { get; set; }

        [JsonProperty("CMTProductDesc")]
        public string CMTProductDesc { get; set; }

        [JsonProperty("CMTCampaignID")]
        public string CMTCampaignID { get; set; }

        [JsonProperty("CMTCampaignDesc")]
        public string CMTCampaignDesc { get; set; }

        [JsonProperty("OwnerSystemId")]
        public string OwnerSystemId { get; set; }

        [JsonProperty("OwnerSystemCode")]
        public string OwnerSystemCode { get; set; }

        [JsonProperty("RefSystemId")]
        public string RefSystemId { get; set; }

        [JsonProperty("RefSystemCode")]
        public string RefSystemCode { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("StatusName")]
        public string StatusName { get; set; }

        [JsonProperty("SubStatus")]
        public string SubStatus { get; set; }

        [JsonProperty("SubStatusName")]
        public string SubStatusName { get; set; }

        [JsonProperty("UpdatedBranch")]
        public string UpdatedBranch { get; set; }

        [JsonProperty("UpdatedTeam")]
        public string UpdatedTeam { get; set; }

        [JsonProperty("UpdatedId")]
        public string UpdatedId { get; set; }

        [JsonProperty("UpdatedName")]
        public string UpdatedName { get; set; }

        [JsonProperty("UpdatedPosition")]
        public string UpdatedPosition { get; set; }
        [JsonProperty("Remark")]
        public string Remark { get; set; }
    }
}
