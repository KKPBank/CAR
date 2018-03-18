using Cas.Common;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cas.LogServce
{
    public partial class ActivityLogin : System.Web.UI.Page
    {
        #region Properties
        private string system
        {
            get { return ViewState["CAS.SYSTEM"] as string; }
            set { ViewState["CAS.SYSTEM"] = value; }
        }
        private string userid
        {
            get { return ViewState["CAS.USERID"] as string; }
            set { ViewState["CAS.USERID"] = value; }
        }
        private string sr
        {
            get { return ViewState["CAS.SR"] as string; }
            set { ViewState["CAS.SR"] = value; }
        }
        private string lead
        {
            get { return ViewState["CAS.LEAD"] as string; }
            set { ViewState["CAS.LEAD"] = value; }
        }
        private string ticket
        {
            get { return ViewState["CAS.TICKET"] as string; }
            set { ViewState["CAS.TICKET"] = value; }
        }
        private string contract
        {
            get { return ViewState["CAS.CONTRACT"] as string; }
            set { ViewState["CAS.CONTRACT"] = value; }
        }
        private string noncustomer
        {
            get { return ViewState["CAS.NONCUSTOMER"] as string; }
            set { ViewState["CAS.NONCUSTOMER"] = value; }
        }
        private string subscription
        {
            get { return ViewState["CAS.SUBSCRIPTION"] as string; }
            set { ViewState["CAS.SUBSCRIPTION"] = value; }
        }
        private string subscriptiontype
        {
            get { return ViewState["CAS.SUBSCRIPTIONTYPE"] as string; }
            set { ViewState["CAS.SUBSCRIPTIONTYPE"] = value; }
        }
        private string refapp
        {
            get { return ViewState["CAS.REFAPP"] as string; }
            set { ViewState["CAS.REFAPP"] = value; }
        }

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;

            if(!Page.IsPostBack)
            {
                userid = Request.Form["userid"];
                sr = Request.Form["sr"];
                lead = Request.Form["lead"];
                ticket = Request.Form["ticket"];
                contract = Request.Form["contract"];
                noncustomer = Request.Form["noncustomer"];
                subscription = Request.Form["subscription"];
                subscriptiontype = Request.Form["subscriptiontype"];
                system = Request.Form["system"];
                refapp = Request.Form["refapp"];
            }

            if (userid == null)
            {
                Response.Redirect("ActivityDetail.aspx");
            }
            else if (userid != null)
            {
                Login1.UserName = userid;
            }
        }
        protected void LoginAuthenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                //LdapAuthentication ldap = new LdapAuthentication();
                //if (ldap.checkAuthenticated(Login1.UserName, Login1.Password))
                //{
                    Session["Authen"] = true;
                    Session["UserId"] = userid;
                    Response.Clear();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append(@"<body onload='document.forms[""form""].submit()'>");
                    sb.AppendFormat("<form name='form' action='{0}' method='post'>", "ActivityDetail.aspx");
                    sb.AppendFormat("<input type='hidden' name='system' value='{0}'>", ConvertSession(system));
                    sb.AppendFormat("<input type='hidden' name='userid' value='{0}'>", ConvertSession(userid));
                    sb.AppendFormat("<input type='hidden' name='sr' value='{0}'>", ConvertSession(sr));
                    sb.AppendFormat("<input type='hidden' name='lead' value='{0}'>", ConvertSession(lead));
                    sb.AppendFormat("<input type='hidden' name='ticket' value='{0}'>", ConvertSession(ticket));
                    sb.AppendFormat("<input type='hidden' name='contract' value='{0}'>", ConvertSession(contract));
                    sb.AppendFormat("<input type='hidden' name='noncustomer' value='{0}'>", ConvertSession(noncustomer));
                    sb.AppendFormat("<input type='hidden' name='subscription' value='{0}'>", ConvertSession(subscription));
                    sb.AppendFormat("<input type='hidden' name='subscriptiontype' value='{0}'>", ConvertSession(subscriptiontype));
                    sb.AppendFormat("<input type='hidden' name='refapp' value='{0}'>", ConvertSession(refapp));
                    sb.Append("</form>");
                    sb.Append("</body>");
                    sb.Append("</html>");
                    Response.Write(sb.ToString());
                    Response.End();
                //}
                //else
                //{
                //    lblUnauthorize.Visible = true;
                //    lblUnauthorize.Text = "CAS-E-202 - Bad Password.";
                //}
            }
            catch (Exception ex)
            {
                lblUnauthorize.Visible = true;
                lblUnauthorize.Text = "CAS-E-202 - " + ex.Message;
            }
        }
        private string ConvertSession(object Session)
        {
            if (Session != null)
            {
                return Session.ToString();
            }
            return string.Empty;
        }
    }
}