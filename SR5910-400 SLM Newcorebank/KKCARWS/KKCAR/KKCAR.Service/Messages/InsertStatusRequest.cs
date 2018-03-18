using KKCAR.Common.Utilities;
using KKCAR.Service.Messages.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel;

///<summary>
/// Class Name : InsertStatusRequest
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
    [MessageContract]
    public class InsertStatusRequest
    {
        [MessageHeader]
        public Header Header { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_StatusDateTime", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        public DateTime StatusDateTime { get; set; }
        
        [MessageBodyMember]
        [Display(Name = "Lbl_SubsID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.SubscriptionID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string SubscriptionID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_SubsCusType", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.SubscriptionCusType, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string SubscriptionCusType { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_SubsCardType", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.SubscriptionCardType, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string SubscriptionCardType { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMProductGroupID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMProductGroupID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMProductGroupID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMProductGroupDesc", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMProductGroupDesc, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMProductGroupDesc { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMProductSubGroupID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMProductSubGroupID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMProductSubGroupID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMProductSubGroupDesc", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMProductSubGroupDesc, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMProductSubGroupDesc { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMProductID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMProductID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMProductID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMProductDesc", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMProductDesc, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMProductDesc { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMCampaignID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMPCampaignID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMCampaignID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_PDMCampaignDesc", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMPCampaignDesc, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string PDMCampaignDesc { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_CMTProductGroupID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.CMTProductGroupID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string CMTProductGroupID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_CMTProductGroupDesc", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.PDMPCampaignDesc, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string CMTProductGroupDesc { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_CMTProductID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.CMTProductID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string CMTProductID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_CMTProductDesc", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.CMTProductDesc, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string CMTProductDesc { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_CMTCampaignID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.CMTCampaignID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string CMTCampaignID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_CMTCampaignDesc", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.CMTCampaignDesc, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string CMTCampaignDesc { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_OwnerSystemId", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.OwnerSystemId, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string OwnerSystemId { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_OwnerSystemCode", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.OwnerSystemCode, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string OwnerSystemCode { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_RefSystemId", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.RefSystemId, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string RefSystemId { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_RefSystemCode", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.RefSystemCode, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string RefSystemCode { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_Status", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.Status, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string Status { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_StatusName", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.StatusName, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string StatusName { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_SubStatus", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.SubStatus, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string SubStatus { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_SubStatusName", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedRequired(ErrorMessage = "ValErr_RequiredField")]
        [LocalizedStringLength(Constants.MaxLength.SubStatusName, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string SubStatusName { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_UpdatedBranch", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.UpdatedBRANCH, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string UpdatedBRANCH { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_UpdatedTeam", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.UpdatedTeam, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string UpdatedTeam { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_UpdatedID", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.UpdatedID, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string UpdatedID { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_UpdatedName", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.UpdatedName, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string UpdatedName { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_UpdatedPosition", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.UpdatedPosition, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string UpdatedPosition { get; set; }

        //[MessageBodyMember]
        //public string StatusNextActionBy { get; set; }

        //[MessageBodyMember]
        //[Display(Name = "Lbl_StatusNextBranch", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedStringLength(Constants.MaxLength.StatusNextBranch, ErrorMessageResourceName = "ValErr_StringLength",
        //    ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //public string StatusNextBranch { get; set; }

        //[MessageBodyMember]
        //[Display(Name = "Lbl_StatusNextTeam", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //[LocalizedStringLength(Constants.MaxLength.StatusNextTeam, ErrorMessageResourceName = "ValErr_StringLength",
        //    ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        //public string StatusNextTeam { get; set; }

        [MessageBodyMember]
        [Display(Name = "Lbl_Remark", ResourceType = typeof(KKCAR.Common.Resources.Resource))]
        [LocalizedStringLength(Constants.MaxLength.Remark, ErrorMessageResourceName = "ValErr_StringLength",
            ErrorMessageResourceType = typeof(KKCAR.Common.Resources.Resource))]
        public string Remark { get; set; }

        //[MessageBodyMember]
        //public List<DataItem> CustomerInfoList { get; set; }

        //[MessageBodyMember]
        //public List<DataItem> OfficerInfoList { get; set; }

        //[MessageBodyMember]
        //public List<DataItem> ApplicationInfoList { get; set; }

        //[MessageBodyMember]
        //public List<DataItem> OtherInfoList { get; set; }
    }
}
