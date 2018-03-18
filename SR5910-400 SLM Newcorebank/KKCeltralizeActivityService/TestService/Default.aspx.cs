using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Serialization;

namespace TestService
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            CASLogService.CASLogServiceSoapClient cl = new CASLogService.CASLogServiceSoapClient();
            var res = cl.CreateActivityLog(new CASLogService.LogServiceHeader()
            {
                ReferenceNo = "000",
                ServiceName = "CreateActivityLog",
                SystemCode = "OBT",
                SecurityKey = "12345",
                TransactionDateTime = DateTime.Now
            },
             new CASLogService.CreateActivityLogData()
             {
                 ActivityDateTime = DateTime.Now,
                 AreaID = 1,
                 CampaignID = "001",
                 TicketID = "0002",
                 ChannelID = "CALLCENTER",
                 TypeID = 1,
                 TypeName = "",
                 SubscriptionTypeID = 1,
                 SubscriptionID = "1234567890124",
                 SubAreaID = 0,
                 SubAreaName = "การขาย",
                 AreaName = ""
            });

            txtResult.Text = res.ResponseStatus.ResponseCode + " : " + res.ResponseStatus.ResponseMessage;
        }

        protected void btnTestInqurey_Click(object sender, EventArgs e)
        {
            CASLogService.CASLogServiceSoapClient cl = new CASLogService.CASLogServiceSoapClient();
            var res = cl.InquiryActivityLog(new CASLogService.LogServiceHeader()
            {
                ReferenceNo = "000",
                ServiceName = "InquiryActivityLog",
                SystemCode = "SLM",
                SecurityKey = "12345",
                TransactionDateTime = DateTime.Now
            },
            new CASLogService.InqueryActivityLogData()
            {
                ActivityStartDateTime = new DateTime(2016, 1, 1),
                ActivityEndDateTime = new DateTime(2016, 6, 30),
                ChannelID = "",
                TicketID = "0002",
                ProductID = "999"

            });
            txtResult.Text = res.ResponseStatus.ResponseCode + " : " + res.ResponseStatus.ResponseMessage;
            if (res.InquiryActivityDataList != null)
                gvMain.DataSource = res.InquiryActivityDataList.OrderByDescending(i => i.ActivityDateTime);
            else
                gvMain.DataSource = null;

            gvMain.DataBind();

         }

        protected void btnGenTxt_Click(object sender, EventArgs e)
        {
            var lst = new CASBatchData()
            {
                BatchDate = DateTime.Now,
                SystemCode = "string",
                TotalItems = 0,
                Items = new List<CASBatchItem>()
                {
                new CASBatchItem()
                {
                     ActivityDateTime = DateTime.Now,
                     AreaID = 0,
                     AreaName = "string",
                     CampaignID = "string",
                     ChannelID = "string",
                     CISID = "string",
                     ContractID = "string",
                     KKCISID = "string",
                     LeadID = "string",
                     NoncustomerID = "string",
                     ProductGroupID = "string",
                     ProductID = "string",
                     ReferenceNo = "string",
                     SrID = "string",
                     Status = "string",
                     SubAreaID = 0,
                     SubAreaName = "string",
                     SubscriptionID = "string",
                     SubscriptionTypeID = 0,
                     SubStatus = "string",
                     TicketID= "string",
                     TrxSeqID = "string",
                     TypeID = 0,
                     TypeName = "string",
                     ActivityInfoList  = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     },
                     ContractInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } ,
                     CustomerInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } ,
                     ProductInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } ,
                     OfficerInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } 
                }
                ,
                new CASBatchItem()
                {
                     ActivityDateTime = DateTime.Now,
                     AreaID = 0,
                     AreaName = "string",
                     CampaignID = "string",
                     ChannelID = "string",
                     CISID = "string",
                     ContractID = "string",
                     KKCISID = "string",
                     LeadID = "string",
                     NoncustomerID = "string",
                     ProductGroupID = "string",
                     ProductID = "string",
                     ReferenceNo = "string",
                     SrID = "string",
                     Status = "string",
                     SubAreaID = 0,
                     SubAreaName = "string",
                     SubscriptionID = "string",
                     SubscriptionTypeID = 0,
                     SubStatus = "string",
                     TicketID= "string",
                     TrxSeqID = "string",
                     TypeID = 0,
                     TypeName = "string",
                     ActivityInfoList  = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     },
                     ContractInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } ,
                     CustomerInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } ,
                     ProductInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } ,
                     OfficerInfoList = new List<DataItem>()
                     {
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         },
                         new DataItem()
                         {
                            SeqNo = 1,
                             DataLabel = "string",
                             DataValue = "string"
                         }
                     } 
                }
                }
            };

            //var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            //txtResult.Text = jss.Serialize(lst);

            XmlSerializer xx = new XmlSerializer(typeof(CASBatchData));
            using (StringWriter wt = new StringWriter())
            {
                xx.Serialize(wt, lst);
                txtResult.Text = wt.ToString();
            }
            
        }

        [Serializable]
        public partial class CASBatchItem 
        {
            public string ReferenceNo { get; set; }
            // required
            public DateTime ActivityDateTime { get; set; }
            public string ChannelID { get; set; }

            // required once
            public int SubscriptionTypeID { get; set; }
            public string SubscriptionID { get; set; }
            public string LeadID { get; set; }
            public string TicketID { get; set; }
            public string SrID { get; set; }
            public string ContractID { get; set; }

            // not required

            public string ProductGroupID { get; set; }
            public string ProductID { get; set; }
            public string CampaignID { get; set; }
            public decimal TypeID { get; set; }
            public decimal AreaID { get; set; }
            public decimal SubAreaID { get; set; }
            public string TypeName { get; set; }
            public string AreaName { get; set; }
            public string SubAreaName { get; set; }

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

        [Serializable]
        public class DataItem
        {
            public int SeqNo { get; set; }
            public string DataLabel { get; set; }
            public string DataValue { get; set; }
        }


        [Serializable]
        public partial class CASBatchData
        {
            public string SystemCode { get; set; }
            public DateTime BatchDate { get; set; }
            public int TotalItems { get; set; }

            public List<CASBatchItem> Items { get; set; }
        }

        protected void btnGenObj_Click(object sender, EventArgs e)
        {
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var obj = jss.Deserialize<CASBatchData>(txtResult.Text);

            txtResult.Text = obj.GetType().ToString() + " : " + (obj == null ? "0" : obj.Items.Count().ToString());
        }
    }
}