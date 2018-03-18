using KKCAR.Common.Utilities;
using KKCAR.Data.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

///<summary>
/// Class Name : CommonFacade
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Business
{
    public class CommonFacade : ICommonFacade
    {
        private readonly KKCARContextContainer _context;
        private ISubscriptionDataAccess _subsDataAccess;
        private IAuthenticationDataAccess _authDataAccess;

        public CommonFacade()
        {
            _context = new KKCARContextContainer();
        }

        public bool IsSubsCusTypeExists(string subsCustType)
        {
            //decimal? subsCustTypeId = subsCustType.ToNullable<decimal>();

            if (!string.IsNullOrEmpty(subsCustType))
            {
                _subsDataAccess = new SubscriptionDataAccess(_context);
                return _subsDataAccess.IsSubsCusTypeExists(subsCustType);
            }

            return true;
        }

        public bool IsSystemExists(string sysCode)
        {
            _authDataAccess = new AuthenticationDataAccess(_context);
            return _authDataAccess.IsSystemExists(sysCode);
        }

        public bool IsSubsCardTypeExists(string subsCardType)
        {
            _subsDataAccess = new SubscriptionDataAccess(_context);
            return _subsDataAccess.IsSubsCardTypeExists(subsCardType);
        }

        public ICollection<string> GetActiveSystemCodeList()
        {
            _authDataAccess = new AuthenticationDataAccess(_context);
            return _authDataAccess.GetActiveSystemCodeList();
        }

        public static DateTime ConvertStringToDate(string str) {
            //Format str = yyyyMMdd
            DateTime? ret = null;

            if (str.Length == 8) {
                int yy = Convert.ToInt16(str.Substring(0, 4));
                int mm = Convert.ToInt16(str.Substring(4, 2));
                int dd = Convert.ToInt16(str.Substring(6, 2));
                ret = new DateTime(yy, mm, dd);
            }

            return ret.Value;
        }

        public static DateTime ConvertStringToStatusDateTime(string str)
        {
            //Format yyyy-MM-dd-HH.mm.ss.ff  Ex. 2017-12-04-08.53.32.50
            DateTime? ret = null;
            if (str.Length == 22)
            {
                string[] tmpDate = str.Split('-');
                if (tmpDate.Length == 4)
                {
                    int yy = Convert.ToInt16(tmpDate[0]);
                    int mm = Convert.ToInt16(tmpDate[1]);
                    int dd = Convert.ToInt16(tmpDate[2]);

                    string[] tmpTime = tmpDate[3].Split('.');
                    if (tmpTime.Length == 4)
                    {
                        int hh = Convert.ToInt16(tmpTime[0]);
                        int mi = Convert.ToInt16(tmpTime[1]);
                        int ss = Convert.ToInt16(tmpTime[2]);
                        int ff = Convert.ToInt16(tmpTime[3]);

                        ret = new DateTime(yy, mm, dd, hh, mi, ss, ff);
                    }
                    else
                    {
                        ret = new DateTime(yy, mm, dd);
                    }
                }
            }

            return ret.Value;
        }

        #region "IDisposable"

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (_context != null) { _context.Dispose(); }
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
