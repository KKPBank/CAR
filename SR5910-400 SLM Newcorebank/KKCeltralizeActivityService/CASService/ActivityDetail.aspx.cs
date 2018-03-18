using Cas.Biz;
using Cas.Common;
using Cas.Dal;
using Cas.Dal.Data;
using Cas.LogServce.Controls;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cas.LogServce
{
    public partial class ActivityDetail : System.Web.UI.Page
    {
        const string ServiceName = "ActivityDetail";
        private static readonly ILog log = LogManager.GetLogger(typeof(ActivityDetail));
        private string SystemCode
        {
            get { return ViewState["CAS.SYSTEM"] as string; }
            set { ViewState["CAS.SYSTEM"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            try
			{
                log.Debug("OnInit");
                base.OnInit(e);
                Appz.BuildCombo(cmbSSubscriptionType, "CAS_SUBSCRIPTION_CARDTYPE", "SUBSCRIPTION_CARDTYPE_TH", "SUBSCRIPTION_CARDTYPE_CODE", "", "", "ระบุประเภทบัตร", "", false);
                Appz.BuildCombo(cmbSChannel, "CAS_CHANNEL", "CHANNEL_NAME||' ('||CHANNEL_ID||')'", "CHANNEL_ID", "", "", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbSProductGroup, "CAS_PRODUCT_GROUP", "PRODUCT_GROUP_NAME", "PRODUCT_GROUP_ID", "", "PRODUCT_GROUP_NAME", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbSProduct, "CAS_PRODUCT", "PRODUCT_NAME", "PRODUCT_ID", "", "PRODUCT_NAME", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbSCampaign, "CAS_CAMPAIGN", "CAMPAIGN_NAME", "CAMPAIGN_ID", "", "CAMPAIGN_NAME", "ทั้งหมด", "", false);

                //Kug SR6007-323 Type Area SubArea   2017-11-10
                Appz.BuildDistinctCombo(cmbSType, "CAS_ACTIVITY_HEADER", "TYPE_NAME", "TYPE_NAME", "TYPE_NAME is not null", "", "ทั้งหมด", "");
                Appz.BuildDistinctCombo(cmbSArea, "CAS_ACTIVITY_HEADER", "AREA_NAME", "AREA_NAME", "AREA_NAME is not null", "NAME", "ทั้งหมด", "");
                Appz.BuildDistinctCombo(cmbSSubArea, "CAS_ACTIVITY_HEADER", "SUBAREA_NAME", "SUBAREA_NAME", "SUBAREA_NAME is not null", "NAME", "ทั้งหมด", "");
                Appz.BuildDistinctCombo(cmbSActivityType, "CAS_ACTIVITY_HEADER", "ACTIVITY_TYPE_NAME", "ACTIVITY_TYPE_NAME", "ACTIVITY_TYPE_NAME is not null", "NAME", "ทั้งหมด", "");

                //Appz.BuildCombo(cmbSType, "CAS_TYPE", "TYPE_NAME", "TYPE_ID", "", "", "ทั้งหมด", "0", false);
                //Appz.BuildCombo(cmbSArea, "CAS_AREA", "AREA_NAME", "AREA_ID", "", "AREA_NAME", "ทั้งหมด", "", false);
                //Appz.BuildCombo(cmbSSubArea, "CAS_SUBAREA", "SUBAREA_NAME", "SUBAREA_ID", "", "SUBAREA_NAME", "ทั้งหมด", "", false);
                //Appz.BuildCombo(cmbSActivityType, "CAS_ACTIVITY_TYPE", "ACTIVITY_TYPE_NAME", "ACTIVITY_TYPE_ID", "", "ACTIVITY_TYPE_NAME", "ทั้งหมด", "", false);

                ctlPageControlBot.PageChange += CtlPageControl_PageChange;
                ctlPageControlTop.PageChange += CtlPageControl_PageChange;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                divSearch.Visible = false;
                lblUnauthorize.Visible = true;
                lblUnauthorize.Text = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;

            //Session["Authen"] = true;//**********************
            //Session["UserId"] = Request.Form["userid"];
            log.Debug("OnLoad");
            if (!IsPostBack)
            {
                try
               {
                    if ((Session["Authen"] == null && Request.Form["userid"] != null) || 
                        (Session["Authen"] != null && Request.Form["userid"] != null && Request.Form["userid"] != ConvertSession(Session["UserId"])))
                    {
                        log.Debug("OnLoad Authorize Fail");
                        log.Debug("UserId=" + Request.Form["userid"].ToString());
                        log.Debug("ticket=" + ConvertSession(Request.Form["ticket"]));

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<html>");
                        sb.Append("@<body onload='document.forms[\"form\"].submit()'>");
                        sb.AppendFormat("<form name='form' action='{0}' method='post'>", "ActivityLogin.aspx");
                        sb.AppendFormat("<input type='hidden' name='system' value='{0}'>", ConvertSession(Request.Form["system"]));
                        sb.AppendFormat("<input type='hidden' name='userid' value='{0}'>", ConvertSession(Request.Form["userid"]));
                        sb.AppendFormat("<input type='hidden' name='sr' value='{0}'>", ConvertSession(Request.Form["sr"]));
                        sb.AppendFormat("<input type='hidden' name='lead' value='{0}'>", ConvertSession(Request.Form["lead"]));
                        sb.AppendFormat("<input type='hidden' name='ticket' value='{0}'>", ConvertSession(Request.Form["ticket"]));
                        sb.AppendFormat("<input type='hidden' name='contract' value='{0}'>", ConvertSession(Request.Form["contract"]));
                        sb.AppendFormat("<input type='hidden' name='noncustomer' value='{0}'>", ConvertSession(Request.Form["noncustomer"]));
                        sb.AppendFormat("<input type='hidden' name='subscription' value='{0}'>", ConvertSession(Request.Form["subscription"]));
                        sb.AppendFormat("<input type='hidden' name='subscriptiontype' value='{0}'>", ConvertSession(Request.Form["subscriptiontype"]));
                        sb.AppendFormat("<input type='hidden' name='refapp' value='{0}'>", ConvertSession(Request.Form["refapp"]));
                        sb.Append("</form>");
                        sb.Append("</body>");
                        sb.Append("</html>");
                        Response.Write(sb.ToString());
                        Response.End();
                    }

                    log.Debug("Validate");
                    log.Debug("system=" + Request.Form["system"].ToString());

                    AuthenBiz aBiz = new AuthenBiz();
                    SystemCode = Request.Form["system"];                    
                    string err;
                    CAS_SERVICE_ACTIVITYLOG actlog = new CAS_SERVICE_ACTIVITYLOG()
                    {
                        REFERENCE_NO = "0",
                        REQUEST_DATETIME = DateTime.Now,
                        REQUEST_IPADDRESS = Request.UserHostAddress,
                        REQUEST_URL = Request.Url.AbsoluteUri,
                        SYSTEM_CODE = SystemCode,
                        SERVICE_NAME = ServiceName
                    };

                    if (String.IsNullOrEmpty(SystemCode))
                    {
                        actlog.RESPONSE_CODE = "CAS-E-101";
                        actlog.RESPONSE_MESSAGE = "Parameter required: system";
                    }
                    else if (!aBiz.CheckAuth(SystemCode, ServiceName))
                    {
                        actlog.RESPONSE_CODE = "CAS-E-202";
                        actlog.RESPONSE_MESSAGE = "No Permission";
                    }
                    else
                    {
                        var scope = aBiz.GetScope(SystemCode, ServiceName);
                        for (int i = 0; i < scope.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(scope[i]))
                            {
                                scope[i] = "'" + scope[i] + "'";
                            }
                        }
                        Appz.BuildCombo(cmbSSystem, "CAS_SYSTEM", "SYSTEM_NAME||' ('||SYSTEM_ID||')'", "SYSTEM_ID", string.IsNullOrEmpty(scope[0]) ? "" : " SYSTEM_ID IN (" + string.Join(",", scope) + ") ", "SYSTEM_NAME", "ทั้งหมด", "", false);

                        hidUserId.Value = DataUtil.GetHashString(Request.Form["userid"]);
                        
                        // set default
                        string sr, lead, ticket, contract, noncustomer, subscription, subscriptiontype, refapp;
                        sr = Request.Form["sr"];
                        lead = Request.Form["lead"];
                        ticket = Request.Form["ticket"];
                        contract = Request.Form["contract"];
                        noncustomer = Request.Form["noncustomer"];
                        subscription = Request.Form["subscription"];
                        subscriptiontype = Request.Form["subscriptiontype"];
                        refapp = Request.Form["referenceapp"];

                        log.Debug("Validate OK Set Default");
                        log.Debug("ticket=" + ticket);

                        if (!String.IsNullOrEmpty(subscription) && !String.IsNullOrEmpty(subscriptiontype))
                        {
                            txtSSubscription.Text = subscription;
                            Appz.SetComboIndexByValue(cmbSSubscriptionType, subscriptiontype);
                            GetUserDetailCombo();
                            Appz.SetComboIndexByValue(cmbSrId, sr);
                            Appz.SetComboIndexByValue(cmbLeadId, lead);
                            Appz.SetComboIndexByValue(cmbTicketId, ticket);
                            Appz.SetComboIndexByValue(cmbContractId, contract);
                            Appz.SetComboIndexByValue(cmbNoncusId, noncustomer);
                            Appz.SetComboIndexByValue(cmbReferenceAppID, refapp);

                            actlog.RESPONSE_CODE = "CAS-I-000";
                            actlog.RESPONSE_MESSAGE = "Success";
                        }
                        else if (!String.IsNullOrEmpty(subscription) && String.IsNullOrEmpty(subscriptiontype))
                        {
                            actlog.RESPONSE_CODE = "CAS-E-101";
                            actlog.RESPONSE_MESSAGE = "Paramenter reqired: subscriptiontype";
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(sr)) { cmbSrId.Items.Clear(); cmbSrId.Items.Add(sr); }
                            if (!String.IsNullOrEmpty(lead)) { cmbLeadId.Items.Clear(); cmbLeadId.Items.Add(lead); }
                            if (!String.IsNullOrEmpty(contract)) { cmbContractId.Items.Clear(); cmbContractId.Items.Add(contract); }
                            if (!String.IsNullOrEmpty(ticket)) { cmbTicketId.Items.Clear(); cmbTicketId.Items.Add(ticket); }
                            if (!String.IsNullOrEmpty(noncustomer)) { cmbNoncusId.Items.Clear(); cmbNoncusId.Items.Add(noncustomer); }
                            if (!String.IsNullOrEmpty(refapp)) { cmbReferenceAppID.Items.Clear(); cmbReferenceAppID.Items.Add(refapp); }

                            cmbSrId.Enabled = false;
                            cmbLeadId.Enabled = false;
                            cmbContractId.Enabled = false;
                            cmbTicketId.Enabled = false;
                            cmbNoncusId.Enabled = false;
                            cmbReferenceAppID.Enabled = false;

                            if (String.IsNullOrEmpty(sr) && String.IsNullOrEmpty(lead) && String.IsNullOrEmpty(contract) && String.IsNullOrEmpty(ticket) && String.IsNullOrEmpty(noncustomer) && string.IsNullOrEmpty(refapp))
                            {
                                actlog.RESPONSE_CODE = "CAS-E-102";
                                actlog.RESPONSE_MESSAGE = "One of these parameter required: subscription, lead, ticket, contract, sr, noncustomer, reference app";
                            }
                            else
                            {
                                actlog.RESPONSE_CODE = "CAS-I-000";
                                actlog.RESPONSE_MESSAGE = "Success";
                            }
                        }
                        txtSSubscription.ReadOnly = true;
                        cmbSSubscriptionType.Enabled = false;
                    }

                    if (actlog.RESPONSE_MESSAGE != "Success" || System.Configuration.ConfigurationManager.AppSettings["CreateServiceActivityLogDB"].ToString() == "Y")
                    {
                        ActivityLogBiz.InsertServiceActivityLog(actlog, out err);
                    }

                    if (actlog.RESPONSE_CODE != "CAS-I-000")
                    {
                        divSearch.Visible = false;
                        lblUnauthorize.Visible = true;
                        lblUnauthorize.Text = string.Format("{0} - {1}", actlog.RESPONSE_CODE, actlog.RESPONSE_MESSAGE);
                        log.Error(lblUnauthorize.Text);
                    }

                    tdmTo.DateValue = DateTime.Now.Date;
                    tdmFrom.DateValue = DateTime.Now.Date;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    divSearch.Visible = false;
                    lblUnauthorize.Visible = true;
                    lblUnauthorize.Text = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                }
            }
        }

        private void DoSearch(int pageIdx)
        {
            BindGridView(ctlPageControlTop, ctlPageControlBot, CacheItem.ToArray(), pageIdx);
        }
        private void CtlPageControl_PageChange(object sender, EventArgs e)
        {
            var pg = sender as GridviewPageController;
            if (pg != null)
                DoSearch(pg.SelectedPageIndex);
        }

        private void BindGridView(GridviewPageController pgTop, GridviewPageController pgBot, object[] itms, int pageIdx)
        {
            int pagesize = Appz.SafeInt(System.Configuration.ConfigurationManager.AppSettings["pagesize"]);
            if (pagesize == 0) pagesize = 10;

            pgTop.SetGridview(gvMain);
            pgBot.SetGridview(gvMain);
            pgTop.Update(itms, pageIdx, pagesize);
            pgBot.Update(itms, pageIdx, pagesize);

            bool vis = gvMain.Rows.Count > 0;
            pgTop.Visible = vis;
            pgBot.Visible = vis;

            pgTop.GenerateRecordNumber(0, pageIdx, pagesize);

            gvMain.Visible = true;
            tabDetails.Visible = false;

        }
        
        private void GetUserDetailCombo()
        {
            if (!string.IsNullOrEmpty(txtSSubscription.Text.Trim()) && !string.IsNullOrEmpty(cmbSSubscriptionType.SelectedValue))
            {
                Appz.BuildCombo(cmbLeadId, "CAS_SEARCH_LEAD", "LEAD_ID", "LEAD_ID", String.Format("SUBSCRIPTION_ID = '{0}' AND SUBSCRIPTION_TYPE_ID = '{1}'", txtSSubscription.Text, cmbSSubscriptionType.SelectedValue), "LEAD_ID", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbTicketId, "CAS_SEARCH_TICKET", "TICKET_ID", "TICKET_ID", String.Format("SUBSCRIPTION_ID = '{0}' AND SUBSCRIPTION_TYPE_ID = '{1}'", txtSSubscription.Text, cmbSSubscriptionType.SelectedValue), "TICKET_ID", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbContractId, "CAS_SEARCH_CONTRACT", "CONTRACT_ID", "CONTRACT_ID", String.Format("SUBSCRIPTION_ID = '{0}' AND SUBSCRIPTION_TYPE_ID = '{1}'", txtSSubscription.Text, cmbSSubscriptionType.SelectedValue), "CONTRACT_ID", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbSrId, "CAS_SEARCH_SR", "SR_ID", "SR_ID", String.Format("SUBSCRIPTION_ID = '{0}' AND SUBSCRIPTION_TYPE_ID = '{1}'", txtSSubscription.Text, cmbSSubscriptionType.SelectedValue), "SR_ID", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbNoncusId, "CAS_SEARCH_NON_CUSTOMER", "NON_CUSTOMER_ID", "NON_CUSTOMER_ID", String.Format("SUBSCRIPTION_ID = '{0}' AND SUBSCRIPTION_TYPE_ID = '{1}'", txtSSubscription.Text, cmbSSubscriptionType.SelectedValue), "NON_CUSTOMER_ID", "ทั้งหมด", "", false);
                Appz.BuildCombo(cmbReferenceAppID, "CAR_SEARCH_REFAPP", "REFERENCE_APP_ID", "REFERENCE_APP_ID", String.Format("SUBSCRIPTION_ID = '{0}' AND SUBSCRIPTION_TYPE_ID = '{1}'", txtSSubscription.Text, cmbSSubscriptionType.SelectedValue), "REFERENCE_APP_ID", "ทั้งหมด", "", false);
            }
            else {
                lblError.Text = "ข้อมูลไม่ครบถ้วน";
                btnSearch.Enabled = false;
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            if (hidUserId.Value != DataUtil.GetHashString(ConvertSession(Session["UserId"])))
            {
                divSearch.Visible = false;
                lblUnauthorize.Visible = true;
                lblUnauthorize.Text = "CAS-E-202 : No Permission";
            }
            else
	        {
	            try 
				{
	                InquiryActivityLogData inq = new InquiryActivityLogData();
	                inq.SubscriptionID = txtSSubscription.Text;
	                inq.TicketID = cmbTicketId.SelectedValue;
	                inq.ContractID = cmbContractId.SelectedValue;
	                inq.LeadID = cmbLeadId.SelectedValue;
	                inq.SrID = cmbSrId.SelectedValue;
	                inq.NoncustomerID = cmbNoncusId.SelectedValue;
	                inq.SubscriptionTypeID = cmbSSubscriptionType.SelectedValue;
	                inq.ProductID = cmbSProduct.SelectedValue;
	                inq.ProductGroupID = cmbSProductGroup.SelectedValue;
                    inq.CampaignID = cmbSCampaign.SelectedValue;

                    //Kug SR6007-323 Type Area SubArea  2017-11-10
                    //inq.TypeID = Appz.SafeDecimal(cmbSType.SelectedValue);
                    //inq.AreaID = Appz.SafeDecimal(cmbSArea.SelectedValue);
	                //inq.SubAreaID = Appz.SafeDecimal(cmbSSubArea.SelectedValue);
                    //inq.ActivityTypeID = Appz.SafeDecimal(cmbSActivityType.SelectedValue);
                    inq.TypeName = cmbSType.SelectedValue;
                    inq.AreaName = cmbSArea.SelectedValue;
                    inq.SubAreaName = cmbSSubArea.SelectedValue;
                    inq.ActivityTypeName = cmbSActivityType.SelectedValue;
                    //############################################################################

                    inq.ReferenceAppID = cmbReferenceAppID.SelectedValue;
	                inq.ActivityStartDateTime = tdmFrom.DateValue;
	                inq.ActivityEndDateTime = tdmTo.DateValue;
	                inq.ChannelID = cmbSChannel.SelectedValue;
	                inq.SystemID = cmbSSystem.SelectedValue;
	
	                inq.Status = txtSStatus.Text;
	                inq.SubStatus = txtSSubStatus.Text;
	                
	                string err = "";
	                // validate
	                if (String.IsNullOrEmpty(inq.SubscriptionID) &&
	                    String.IsNullOrEmpty(inq.TicketID) &&
	                    String.IsNullOrEmpty(inq.ContractID) &&
	                    String.IsNullOrEmpty(inq.LeadID) &&
	                    String.IsNullOrEmpty(inq.SrID) &&
	                    String.IsNullOrEmpty(inq.NoncustomerID) &&
	                    string.IsNullOrEmpty(inq.ReferenceAppID))
	                    err = "Required one of these parameters: SubscriptionID, TicketID, ContractID, LeadID, SrID, NoncustomerID, ReferenceAppID";
	
	                if (!String.IsNullOrEmpty(inq.SubscriptionID) && String.IsNullOrEmpty(inq.SubscriptionTypeID))
	                    err = "Subscription Type ID Required for Subscription ID";
	                 	
	                if (!string.IsNullOrEmpty(err))
	                {
	                    lblError.Text = err;
	                    return;
	                }
	
	                var scope = new AuthenBiz().GetScope(SystemCode, ServiceName);
		
	                ServiceLogBiz bz = new ServiceLogBiz();
	                var lst = bz.SearchActivity(inq, scope, true);
	                CacheItem = lst.OrderByDescending(d => d.ActivityDateTime).ToList();
	                DoSearch(0);
	            }
	            catch (Exception ex)
	            {
	                log.Error(ex.Message, ex);
	                lblError.Text = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
	
	            }
			}
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSLead.Text = "";
                txtSTicket.Text = "";
                txtSContract.Text = "";
                txtSNonCustomer.Text = "";
                txtSSR.Text = "";
                
                cmbSChannel.SelectedIndex = 0;
                cmbSSystem.SelectedIndex = 0;
                cmbSType.SelectedIndex = 0;
                cmbSCampaign.SelectedIndex = 0;
                cmbSProduct.SelectedIndex = 0;
                cmbSProductGroup.SelectedIndex = 0;
                cmbSActivityType.SelectedIndex = 0;
                cmbSArea.SelectedIndex = 0;
                cmbSSubArea.SelectedIndex = 0;
                cmbSrId.SelectedIndex = 0;
                cmbLeadId.SelectedIndex = 0;
                cmbTicketId.SelectedIndex = 0;
                cmbNoncusId.SelectedIndex = 0;
                cmbReferenceAppID.SelectedIndex = 0;

                tdmFrom.DateValue = DateTime.Now.Date;
                tdmTo.DateValue = DateTime.Now.Date;

                BindGridView(ctlPageControlTop, ctlPageControlBot, new object[] { }, 0);
                gvMain.Visible = false;
                tabDetails.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                lblError.Text = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
        }

        protected void LnbViewDetail_Click(object sender, EventArgs e)
        {
            try {
                // get data
                var id = Appz.SafeDecimal(((LinkButton)sender).CommandArgument);
                var lst = CacheItem;
                if (lst != null)
                {
                    var itm = lst.Where(a => a.ActivityID == id).FirstOrDefault();
                    if (itm != null)
                    {
                        gvActivity.DataSource = itm.ActivityInfoList;
                        gvActivity.DataBind();
                        gvCustomer.DataSource = itm.CustomerInfoList;
                        gvCustomer.DataBind();
                        gvContract.DataSource = itm.ContractInfoList;
                        gvContract.DataBind();
                        gvProduct.DataSource = itm.ProductInfoList;
                        gvProduct.DataBind();
                        gvOfficer.DataSource = itm.OfficerInfoList;
                        gvOfficer.DataBind();
                    }
                }


                // set visible
                string strText = string.Empty;
                foreach (GridViewRow r in gvMain.Rows)
                {
                    r.BackColor = default(System.Drawing.Color);
                    strText = r.Cells[0].Text;
                    r.Cells[0].Text = strText;
                }

                var row = ((Control)sender).Parent.Parent as GridViewRow;
                row.BackColor = System.Drawing.Color.FromArgb(255, 255, 200);
                tabDetails.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                lblError.Text = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
        }

        protected void GvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;
            if (row.RowType == DataControlRowType.DataRow)
            {
                var lb = row.Cells[15].FindControl("lnbViewDetail") as LinkButton;
                if (lb != null)
                {
                    row.Attributes.Add("onclick", "showWait();" + Page.GetPostBackEventReference(lb));
                }
            }
        }
        
        private List<ActivityDataItem> CacheItem
        {
            get { return Session["CAS.ACTIVITY.ITEM"] as List<ActivityDataItem>; }
            set { Session["CAS.ACTIVITY.ITEM"] = value; }
        }

        private string ConvertSession(object objSession)
        {
            if(objSession != null)
            {
                return objSession.ToString();
            }
            return string.Empty;
        }
    }
}