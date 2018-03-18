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
            decimal? subsCustTypeId = subsCustType.ToNullable<decimal>();

            if (subsCustTypeId != null)
            {
                _subsDataAccess = new SubscriptionDataAccess(_context);
                return _subsDataAccess.IsSubsCusTypeExists(subsCustTypeId.Value);
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
            DateTime? ret = null;

            if (str.Length == 10) {
                string[] tmp = str.Split('/');
                if (tmp.Length == 3) {
                    ret = new DateTime(Convert.ToInt16(tmp[2]), Convert.ToInt16(tmp[1]), Convert.ToInt16(tmp[0]));
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
