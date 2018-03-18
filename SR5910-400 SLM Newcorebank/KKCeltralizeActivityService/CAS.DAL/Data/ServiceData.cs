using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cas.Dal.Data
{
    [Serializable]
    [XmlType("DataItem", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService", IncludeInSchema = true)]
    [SoapType("DataItem")]
    public class DataItem
    {
        public int SeqNo { get; set; }
        public string DataLabel { get; set; }
        public string DataValue { get; set; }
    }

    [Serializable]
    [XmlType("LogServiceHeader", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService", IncludeInSchema = true)]
    [SoapType("LogServiceHeader")]
    public class LogServiceHeader
    {
        public string ReferenceNo { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string ServiceName { get; set; }
        public string SystemCode { get; set; }
        public string SecurityKey { get; set; }
    }
    
    // Request
    [Serializable]
    [XmlType("InqueryActivityLogData", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService")]
    public class InquiryActivityLogData
    {
        // -- require 
        public DateTime ActivityStartDateTime { get; set; }
        public DateTime ActivityEndDateTime { get; set; }
        public string ChannelID { get; set; }

        // -- require at least 1
        public string SubscriptionTypeID { get; set; }
        public string SubscriptionID { get; set; }
        public string LeadID { get; set; }
        public string TicketID { get; set; }
        public string SrID { get; set; }
        public string ContractID { get; set; }
        public string ReferenceAppID { get; set; }
        // -- ID
        public string PDMProductGroupID { get; set; }
        public string PDMProductSubGroupID { get; set; }
        public string PDMProductID { get; set; }
        public string PDMCampaignID { get; set; }
        public string ProductGroupID { get; set; }
        public string ProductID { get; set; }
        public string CampaignID { get; set; }
        public decimal TypeID { get; set; }
        public decimal AreaID { get; set; }
        public decimal SubAreaID { get; set; }
        public decimal ActivityTypeID { get; set; }
        public string SystemID { get; set; }

        // -- Name
        public string TypeName { get; set; }
        public string AreaName { get; set; }
        public string SubAreaName { get; set; }
        public string ActivityTypeName { get; set; }

        // Others
        public string KKCISID { get; set; }
        public string CISID { get; set; }
        public string TrxSeqID { get; set; }
        public string NoncustomerID { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }

    }

    [Serializable]
    [XmlType("CreateActivityLogData", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService")]
    public class CreateActivityLogData
    {
        public string ReferenceNo { get; set; }
        // required
        public DateTime ActivityDateTime { get; set; }
        public string ChannelID { get; set; }

        // required once
        public string SubscriptionTypeID { get; set; }
        public string SubscriptionID { get; set; }
        public string LeadID { get; set; }
        public string TicketID { get; set; }
        public string SrID { get; set; }
        public string ContractID { get; set; }
        public string ReferenceAppID { get; set; }

        // not 
        public string PDMProductGroupID { get; set; }
        public string PDMProductSubGroupID { get; set; }
        public string PDMProductID { get; set; }
        public string PDMCampaignID { get; set; }
        public string ProductGroupID { get; set; }
        public string ProductID { get; set; }
        public string CampaignID { get; set; }
        public decimal TypeID { get; set; }
        public decimal AreaID { get; set; }
        public decimal SubAreaID { get; set; }
        public decimal ActivityTypeID { get; set; }
        public string TypeName { get; set; }
        public string AreaName { get; set; }
        public string SubAreaName { get; set; }
        public string ActivityTypeName { get; set; }

        // Others
        public string KKCISID { get; set; }
        public string CISID { get; set; }
        public string TrxSeqID { get; set; }
        public string NoncustomerID { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }

        // list of data item
        public List<DataItem> OfficerInfoList { get; set; }
        public List<DataItem> ContractInfoList { get; set; }
        public List<DataItem> ProductInfoList { get; set; }
        public List<DataItem> CustomerInfoList { get; set; }
        public List<DataItem> ActivityInfoList { get; set; }        
    }
    
    // Response
    [Serializable]
    [XmlType("CreateActivityLogResponse", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService")]
    public class CreateActivityLogResponse
    {
        public LogServiceHeader Header { get; set; }
        public ResponseData ResponseStatus { get; set; }
    }

    [Serializable]
    [XmlType("InqueryActivytyLogResponse", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService")]
    public class InquiryActivityLogResponse
    {
        public LogServiceHeader Header { get; set; }
        public List<ActivityDataItem> InquiryActivityDataList { get; set; }
        public ResponseData ResponseStatus { get; set; }

    }

    [Serializable]
    [XmlType("ResponseData", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService")]
    public class ResponseData
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
    
    [Serializable]
    [XmlType("ActivityDataItem", Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService")]
    public class ActivityDataItem
    {
        public decimal ActivityID { get; set; }
        public string SystemID { get; set; }
        public string SystemName { get; set; }
        public string CISID { get; set; }
        public DateTime ActivityDateTime { get; set; }

        // id value
        public string PDMProductGroupID { get; set; }
        public string PDMProductSubGroupID { get; set; }
        public string PDMProductID { get; set; }
        public string PDMCampaignID { get; set; }
        public string ProductGroupID { get; set; }
        public string ProductID { get; set; }
        public string CampaignID { get; set; }
        public decimal TypeID { get; set; }
        public decimal AreaID { get; set; }
        public decimal SubAreaID { get; set; }
        public string ChannelID { get; set; }
        public string SubscriptionTypeID { get; set; }
        public decimal ActivityTypeID { get; set; }

        // name value
        public string ProductGroupName { get; set; }
        public string ProductName { get; set; }
        public string CampaignName { get; set; }
        public string TypeName { get; set; }
        public string AreaName { get; set; }
        public string SubAreaName { get; set; }
        public string ChannelName { get; set; }
        public string SubscriptionTypeName { get; set; }
        public string ActivityTypeName { get; set; }

        // person id info
        public string SubscriptonID { get; set; }
        public string LeadID { get; set; }
        public string TicketID { get; set; }
        public string SrID { get; set; }
        public string ContractID { get; set; }
        public string ReferenceAppID { get; set; }
        // kk info
        public string KKCISID { get; set; }
        public string TrxSeqID { get; set; }
        public string NoneCustomerID { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }


        // array item
        public List<DataItem> OfficerInfoList { get; set; }
        public List<DataItem> ContractInfoList { get; set; }
        public List<DataItem> ProductInfoList { get; set; }
        public List<DataItem> CustomerInfoList { get; set; }
        public List<DataItem> ActivityInfoList { get; set; }

    }
}
