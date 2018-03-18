using Cas.Biz;
using System.Web.UI.WebControls;

namespace Cas.LogServce
{
    public  class Appz
    {
        public static decimal SafeDecimal(string str)
        {
            decimal r; decimal.TryParse(str, out r);
            return r;
        }

        public static int SafeInt(string str)
        {
            int r; int.TryParse(str, out r);
            return r;
        }

        public static void BuildCombo(DropDownList comboBox, string TableName, string DisplayField, string ValueField, string WhereString, string OrderString, string BlankText, string BlankValue, bool doDistinct)
        {
            ComboSourceBiz cFlow = new ComboSourceBiz();
            comboBox.DataSource = cFlow.GetComboSource(TableName, DisplayField, ValueField, WhereString, OrderString, doDistinct);
            comboBox.DataTextField = "NAME";
            comboBox.DataValueField = "VALUE";
            comboBox.DataBind();
            if (BlankText != null && BlankValue != null)
            {
                comboBox.Items.Insert(0, new System.Web.UI.WebControls.ListItem(BlankText, BlankValue));
            }
        }

        public static void BuildDistinctCombo(DropDownList comboBox, string TableName, string DisplayField, string ValueField, string WhereString, string OrderString, string BlankText, string BlankValue)
        {
            ComboSourceBiz cFlow = new ComboSourceBiz();
            comboBox.DataSource = cFlow.GetComboSource(TableName, DisplayField, ValueField, WhereString, OrderString, true);
            comboBox.DataTextField = "NAME";
            comboBox.DataValueField = "VALUE";
            comboBox.DataBind();
            if (BlankText != null && BlankValue != null)
            {
                comboBox.Items.Insert(0, new System.Web.UI.WebControls.ListItem(BlankText, BlankValue));
            }
        }

        public static void SetComboIndexByValue(DropDownList comboBox, string value)
        {
            comboBox.SelectedIndex = comboBox.Items.IndexOf(comboBox.Items.FindByValue(value));
        }

    }
}
