using Cas.Common;
using Cas.Dal;
using System.Collections.Generic;
using System.Linq;

namespace Cas.Biz
{
    public class ComboSourceBiz
    {
        /// <summary>
        /// RETURN NAME and VALUE
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="DisplayField"></param>
        /// <param name="ValueField"></param>
        /// <param name="WhereString"></param>
        /// <param name="OrderString"></param>
        /// <param name="doDistinct"></param>
        /// <returns></returns>
        public List<ComboSourceData> GetComboSource(string TableName, string DisplayField, string ValueField, string WhereString, string OrderString, bool doDistinct)
        {
            string sqlz = " SELECT " + (doDistinct ? " DISTINCT " : "") + "TO_CHAR(" + DisplayField + ") as NAME, TO_CHAR(" + ValueField + ") as VALUE FROM " + AppConfig.DbSchema + "." + TableName + " ";
            if (WhereString.Trim() != "") sqlz += " WHERE " + WhereString + " ";
            if (OrderString.Trim() != "") sqlz += " ORDER BY " + OrderString + " ";

            using (KKCASModel kdc = new KKCASModel())
            {
                return kdc.Database.SqlQuery<ComboSourceData>(sqlz, new object[] { }).ToList();
            }
        }
    }

    public class ComboSourceData
    {
        public string VALUE { get; set; }
        public string NAME { get; set; }
    }
}
