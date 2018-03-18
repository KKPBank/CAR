using System;

///<summary>
/// Class Name : CarStatusEntity
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Entity
{
    [Serializable]
    public class CarStatusEntity
    {
        public string CarID { get; set; }
        public DateTime? TranDateTime { get; set; }
        public DateTime? StatusDateTime { get; set; }
        public string RefNo { get; set; }
        public string ChannelID { get; set; }
        public string SubsID { get; set; }
        public string SubsCusType { get; set; }
        public string SubsCardType { get; set; }
        public string PDMProdGrpID { get; set; }
        public string PDMProdGrpDesc { get; set; }
        public string PDMProdSubGrpID { get; set; }
        public string PDMProdSubGrpDesc { get; set; }
        public string PDMProdID { get; set; }
        public string PDMProdDesc { get; set; }
        public string PDMCampaignID { get; set; }
        public string PDMCampaignDesc { get; set; }
        public string CMTProdGrpID { get; set; }
        public string CMTProdGrpDesc { get; set; }
        public string CMTProdID { get; set; }
        public string CMTProdDesc { get; set; }
        public string CMTCampaignID { get; set; }
        public string CMTCampaignDesc { get; set; }
        public string StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public string SubStatusCode { get; set; }
        public string SubStatusDesc { get; set; }
        public string UpdatedBranch { get; set; }
        public string UpdatedTeam { get; set; }
        public string UpdatedID { get; set; }
        public string UpdatedName { get; set; }
        public string UpdatedPosition { get; set; }
        public string StatusNextBranch { get; set; }
        public string StatusNextTeam { get; set; }
        public string Remark { get; set; }
        public string OwnerSystemId { get; set; }
        public string OwnerSystemCode { get; set; }
        public string RefSystemId { get; set; }
        public string RefSystemCode { get; set; }
        public string JsonCusInfo { get; set; }
        public string JsonOfficerInfo { get; set; }
        public string JsonAppInfo { get; set; }
        public string JsonOtherInfo { get; set; }
        public bool SkipUpdate { get; set; }
    }
}
