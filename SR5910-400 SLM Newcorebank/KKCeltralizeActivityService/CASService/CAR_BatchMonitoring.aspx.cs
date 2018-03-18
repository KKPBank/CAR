using Cas.Biz;
using Cas.Common;
using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Cas.LogServce
{
    public partial class CAR_BatchMonitoring : System.Web.UI.Page
    {
        #region Member
        private CreateActivityLogBiz _biz = null;
        #endregion

        #region Properties
        private CreateActivityLogBiz Business
        {
            get { return _biz ?? new CreateActivityLogBiz(); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            if (!Page.IsPostBack)
            {
                tdmBatchDate.DateValue = DateTime.Now;
            }            
        }

        #region GridView Handler
        protected void GrdBatchMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                HtmlImage imgLog = e.Row.Cells[0].FindControl("imgLog") as HtmlImage;
                ImageButton ibtnRun = e.Row.Cells[5].FindControl("ibtnRun") as ImageButton;
                GridView grdBatchLog = e.Row.FindControl("grdBatchLog") as GridView;
                DataTable dt = Business.LoadBatchLog(Convert.ToDecimal(dv["BATCH_ID"]), tdmBatchDate.DateValue.ToString("yyyyMMdd"));
                grdBatchLog.PageSize = AppConfig.GridMaxRowPerPage;
                grdBatchLog.DataSource = dt;
                grdBatchLog.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    imgLog.Visible = false;
                }
                else
                {
                    Session[grdBatchLog.ClientID] = dt;
                }
                if(dv["STATUS"].ToString() == BatchStatus.Processing || dv["MAIN_RERUN_URL"] == DBNull.Value || string.IsNullOrEmpty(dv["MAIN_RERUN_URL"].ToString()))
                {
                    ibtnRun.Visible = false;
                }
            }
        }
        protected void GrdBatchLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;
                HtmlImage imgLogDetail = e.Row.Cells[0].FindControl("imgLogDetail") as HtmlImage;
                GridView grdBatchLogDetail = e.Row.Cells[0].FindControl("grdBatchLogDetail") as GridView;
                ImageButton ibtnRun = e.Row.Cells[14].FindControl("ibtnRun") as ImageButton;
                DataTable dt = Business.LoadBatchLogDetail(Convert.ToDecimal(dv["BATCH_LOG_ID"]));
                grdBatchLogDetail.PageSize = AppConfig.GridMaxRowPerPage;
                grdBatchLogDetail.DataSource = dt;
                grdBatchLogDetail.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    imgLogDetail.Visible = false;
                }
                else
                {
                    imgLogDetail.Visible = true;
                    Session[grdBatchLogDetail.ClientID] = dt;
                }

                if (dv["RERUN_PATH"] == DBNull.Value || string.IsNullOrEmpty(dv["RERUN_PATH"].ToString()))
                {
                    ibtnRun.Visible = false;
                }
            }
        }        
        protected void GrdBatchMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RunCommand")
            {
                var batch = Business.LoadBatchMasterByCode(e.CommandArgument.ToString());
                if (batch != null)
                {
                    Business.CallGetHttpClient(batch.MAIN_RERUN_URL);
                    LoadBatchMasterData();
                }
            }
        }
        protected void GrdBatchLog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RunLogCommand")
            {
                var batchLog = Business.LoadBatchLogByID(Convert.ToDecimal(e.CommandArgument));
                var batch = Business.LoadBatchMasterById(batchLog.BATCH_ID);
                if (batch != null)
                {
                    string strRequestUri = string.Format("{0}/{1}", batch.MAIN_URL, batchLog.RERUN_PATH);
                    Business.CallGetHttpClient(strRequestUri);
                    LoadBatchMasterData();

                    string[] strIndex = ((GridView)sender).ClientID.Split('_');
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), string.Format("ExpandGrid('grdBatchMaster_imgLog_{0}');", strIndex[2]), true);
                }
            }
        }
        protected void GrdBatchLogDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ((GridView)sender).PageSize = AppConfig.GridMaxRowPerPage;
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataSource = Session[((GridView)sender).ClientID] as DataTable;
            ((GridView)sender).DataBind();
            string[] strIndex = ((GridView)sender).ClientID.Split('_');
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), string.Format("ExpandGrid('grdBatchMaster_grdBatchLog_{0}_imgLogDetail_{1}');", strIndex[2], strIndex[4]), true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), string.Format("ExpandGrid('grdBatchMaster_imgLog_{0}');", strIndex[2]), true);            
        }
        protected void GrdBatchLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ((GridView)sender).PageSize = AppConfig.GridMaxRowPerPage;
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataSource = Session[((GridView)sender).ClientID] as DataTable;
            ((GridView)sender).DataBind();
            string[] strIndex = ((GridView)sender).ClientID.Split('_');
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), string.Format("ExpandGrid('grdBatchMaster_imgLog_{0}');", strIndex[2]), true);
        }
        #endregion

        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadBatchMasterData();
        }

        private void LoadBatchMasterData()
        {
            grdBatchMaster.DataSource = Business.LoadAllBatchMaster();
            grdBatchMaster.DataBind();
        }




    }
}