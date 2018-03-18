using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Cas.LogServce.Controls
{
    public partial class GridviewPageController : System.Web.UI.UserControl
    {
        GridView _gvMain = null;
        public event EventHandler PageChange;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void SetGridview(GridView gv)
        {
            _gvMain = gv;
        }

        public int SelectedPageIndex
        {
            get { return cmbPage.SelectedIndex; }
        }

        public Unit SetWidth
        {
            set { pnPageControl.Width = value; }
        }

        public bool SetVisible
        {
            set { pnPageControl.Visible = value; }
        }

        public void Update(object[] items, int page_index)
        {
            try
            {
                //ส่ง Default Page Size จาก Config File
                Update(items, page_index, 25);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(object[] items, int page_index, int page_size)
        {
            try
            {
                int page_index_new = page_index;
                int num_of_page = 0;
                int upper_bound = 0;
                int lower_bound = 0;
                ArrayList source = new ArrayList();

                page_index_new = page_index_new > -1 ? page_index_new : 0;

                if (items.Any())
                {
                    num_of_page = (int)Math.Ceiling(Convert.ToDouble(items.Count()) / Convert.ToDouble(page_size));

                    if (page_index_new >= num_of_page)
                        page_index_new = 0;

                    lower_bound = page_size * page_index_new;
                    upper_bound = (page_size * (page_index_new + 1)) - 1;

                    for (int i = lower_bound; i <= upper_bound; i++)
                    {
                        if ((i + 1) > items.Count())
                        {
                            upper_bound = i - 1;
                            break;
                        }
                        source.Add(items[i]);
                    }
                }

                UpdatePageControl(num_of_page, page_index_new, lower_bound, upper_bound, items.Count());
                BindGridview(source);
                pnPageControl.Visible = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMonitoring(object[] items, int page_index)
        {
            try
            {
                int page_size = int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridviewPageSizeMonitoring"]);
                int page_index_new = page_index;
                int num_of_page = 0;
                int upper_bound = 0;
                int lower_bound = 0;
                ArrayList source = new ArrayList();

                page_index_new = page_index_new > -1 ? page_index_new : 0;

                if (items.Any())
                {
                    num_of_page = (int)Math.Ceiling(Convert.ToDouble(items.Count()) / Convert.ToDouble(page_size));

                    if (page_index_new >= num_of_page)
                        page_index_new = 0;

                    lower_bound = page_size * page_index_new;
                    upper_bound = (page_size * (page_index_new + 1)) - 1;

                    for (int i = lower_bound; i <= upper_bound; i++)
                    {
                        if ((i + 1) > items.Count())
                        {
                            upper_bound = i - 1;
                            break;
                        }
                        source.Add(items[i]);
                    }
                }

                UpdatePageControl(num_of_page, page_index_new, lower_bound, upper_bound, items.Count());
                BindGridview(source);
                pnPageControl.Visible = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Unit GetGridviewWidth()
        {
            double width = 0;
            foreach (DataControlField df in _gvMain.Columns)
            {
                width += df.ItemStyle.Width.Value;
            }
            return Unit.Pixel(Convert.ToInt32(width));
        }
        private void BindGridview(ArrayList source)
        {
            if (_gvMain != null)
            {
                _gvMain.DataSource = source;
                _gvMain.DataBind();
            }
        }

        private void BindGridview(DataTable dtSource)
        {
            if (_gvMain != null)
            {
                _gvMain.DataSource = dtSource;
                _gvMain.DataBind();
            }
        }

        protected void cmbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void lnbFirst_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = 0;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void lnbNext_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = cmbPage.SelectedIndex + 1;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void lnbBack_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = cmbPage.SelectedIndex - 1;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected void lnbLast_Click(object sender, EventArgs e)
        {
            try
            {
                cmbPage.SelectedIndex = cmbPage.Items.Count - 1;
                if (PageChange != null)
                    PageChange(this, e);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdatePageControl(int num_of_page, int page_index, int lower_bound, int upper_bound, int item_count)
        {
            try
            {
                cmbPage.Items.Clear();

                if (item_count > 0)
                {
                    for (int i = 0; i < num_of_page; i++)
                    {
                        cmbPage.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }
                    cmbPage.SelectedIndex = cmbPage.Items.IndexOf(cmbPage.Items.FindByValue(page_index.ToString()));
                    lblTotalPage.Text = num_of_page.ToString("#,##0");

                    lnbFirst.Enabled = (cmbPage.SelectedIndex != 0);
                    lnbFirst.OnClientClick = lnbFirst.Enabled ? "showWait();" : "";

                    lnbBack.Enabled = (cmbPage.SelectedIndex != 0);
                    lnbBack.OnClientClick = lnbBack.Enabled ? "showWait();" : "";

                    lnbNext.Enabled = (cmbPage.SelectedIndex < cmbPage.Items.Count - 1);
                    lnbNext.OnClientClick = lnbNext.Enabled ? "showWait();" : "";

                    lnbLast.Enabled = (cmbPage.SelectedIndex < cmbPage.Items.Count - 1);
                    lnbLast.OnClientClick = lnbLast.Enabled ? "showWait();" : "";

                    lblSummary.Text = "รายการที่ " + (lower_bound + 1).ToString("#,##0") + " - " + (upper_bound + 1).ToString("#,##0") + " จาก <font class='hilightGreen'><b>" + item_count.ToString("#,##0") + "</b></font> รายการ";
                }
                else
                {
                    lblTotalPage.Text = "0";

                    lnbFirst.Enabled = false;
                    lnbFirst.OnClientClick = "";
                    lnbBack.Enabled = false;
                    lnbBack.OnClientClick = "";
                    lnbNext.Enabled = false;
                    lnbNext.OnClientClick = "";
                    lnbLast.Enabled = false;
                    lnbLast.OnClientClick = "";

                    lblSummary.Text = "รายการที่ 0 - 0 จาก <font class='hilightGreen'><b>0</b></font> รายการ";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GenerateRecordNumber(int columnIndexOfRecordNumber, int gvPageIndex)
        {
            GenerateRecordNumber(columnIndexOfRecordNumber, gvPageIndex, int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridviewPageSize"]));
        }
        public void GenerateRecordNumber(int columnIndexOfRecordNumber, int gvPageIndex, int gvPageSize)
        {
            if (_gvMain != null)
            {
                for (int i = 0; i < _gvMain.Rows.Count; i++)
                {
                    _gvMain.Rows[i].Cells[columnIndexOfRecordNumber].Text = ((gvPageIndex * gvPageSize) + (i + 1)).ToString();
                }
            }
        }
    }
}