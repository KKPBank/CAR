using KKCAR.Common.Resources;
using KKCAR.Common.Utilities;
using KKCAR.Data.DataAccess;
using KKCAR.Entity;
using KKCAR.Service.Messages.Common;
using log4net;
using System;
using System.Globalization;
using System.Text;

///<summary>
/// Class Name : AuthenticationFacade
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
    public class AuthenticationFacade : IAuthenticationFacade
    {
        private string _errorCode = string.Empty;
        private string _errorMessage = string.Empty;
        private IChannelDataAccess _channelDataAccess;
        private readonly KKCARContextContainer _context;
        private IAuthenticationDataAccess _authDataAccess;
        private LogMessageBuilder _logMsg = new LogMessageBuilder();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuthenticationFacade));

        public AuthenticationFacade()
        {
            _context = new KKCARContextContainer();
        }

        public ResponseStatus VerifyServiceRequest(Header header, out bool valid)
        {
            bool noError = true;
            StringBuilder sb = new StringBuilder("");
            var responseStatus = new ResponseStatus();

            #region "Validate Service Header"

            if (string.IsNullOrWhiteSpace(header.ReferenceNo))
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_RefNo));
            }
            if (!string.IsNullOrWhiteSpace(header.ReferenceNo) && header.ReferenceNo.NullSafeTrim().Length > Constants.MaxLength.RefNo)
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_StringLength, Resource.Lbl_RefNo, Constants.MaxLength.RefNo));
            }
            if(!string.IsNullOrWhiteSpace(header.ReferenceNo) && !ApplicationHelpers.ValidateRefNo(header.ReferenceNo))
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidData, Resource.Lbl_RefNo));
            }

            if (header.TransactionDateTime == null)
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_TransactionDate));
            }

            if (string.IsNullOrWhiteSpace(header.ServiceName))
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_ServiceName));
            }

            if (string.IsNullOrWhiteSpace(header.SystemCode))
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_SystemCode));
            }
            
            if (string.IsNullOrWhiteSpace(header.ChannelID))
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_ChannelID));
            }
            if(!string.IsNullOrWhiteSpace(header.ChannelID))
            {
                _channelDataAccess = new ChannelDataAccess(_context);
                bool isExists = _channelDataAccess.IsChannelExists(header.ChannelID);
                
                if(!isExists)
                {
                    noError = false;
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidData, Resource.Lbl_ChannelID));
                }
                if (header.ChannelID.Length > Constants.MaxLength.ChannelID)
                {
                    noError = false;
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_StringLength, Resource.Lbl_ChannelID, Constants.MaxLength.ChannelID));
                }
            }

            #endregion

            if (!noError)
            {
                responseStatus.ResponseCode = Constants.ErrorCode.KKCAR_ERR101;
                responseStatus.ResponseMessage = sb.ToString().TrimEnd(new char[] { '\r', '\n' });
                goto Outer;
            }

            if (!Constants.ServiceName.InsertStatus.ToUpperInvariant().Equals(header.ServiceName.ToUpperInvariant()))
            {
                noError = false;
                responseStatus.ResponseCode = Constants.ErrorCode.KKCAR_ERR203;
                responseStatus.ResponseMessage = "Invalid Service Name.";
                goto Outer;
            }

            if (!this.CheckAuth(header))
            {
                noError = false;
                responseStatus.ResponseCode = _errorCode;
                responseStatus.ResponseMessage = _errorMessage;
            }

        Outer:
            valid = noError;
            return responseStatus;
        }

        public ResponseStatus ValidateServiceRequest(string systemCode, string serviceName, out bool valid)
        {
            bool noError = true;
            StringBuilder sb = new StringBuilder("");
            var responseStatus = new ResponseStatus();

            #region "Validate Service"

            if (string.IsNullOrWhiteSpace(serviceName))
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_ServiceName));
            }

            if (string.IsNullOrWhiteSpace(systemCode))
            {
                noError = false;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_SystemCode));
            }

            #endregion

            if (!noError)
            {
                responseStatus.ResponseCode = Constants.ErrorCode.KKCAR_ERR101;
                responseStatus.ResponseMessage = sb.ToString();
                goto Outer;
            }

            if (!Constants.ServiceName.BatchInsertStatus.ToUpperInvariant().Equals(serviceName.ToUpperInvariant()))
            {
                noError = false;
                responseStatus.ResponseCode = Constants.ErrorCode.KKCAR_ERR203;
                responseStatus.ResponseMessage = "Invalid Service Name.";
                goto Outer;
            }

            if (!this.CheckAuth(systemCode, string.Empty))
            {
                noError = false;
                responseStatus.ResponseCode = _errorCode;
                responseStatus.ResponseMessage = _errorMessage;
            }

        Outer:
            valid = noError;
            return responseStatus;
        }

        public string GetResponseUrlBySysCode(string systemCode)
        {
            _authDataAccess = new AuthenticationDataAccess(_context);
            return _authDataAccess.GetResponseUrlBySysCode(systemCode, Constants.BatchCode.BatchInsertStatus);
        }

        #region "Functions"

        private bool CheckAuth(Header header)
        {
            bool noError = true;
            string sysCode = header.SystemCode;
            string secKey = header.SecurityKey;

            try
            {
                _authDataAccess = new AuthenticationDataAccess(_context);
                SystemInfoEntity sysInfo = _authDataAccess.GetSystemInfoByCode(sysCode);

                if (sysInfo == null)
                {
                    _errorCode = Constants.ErrorCode.KKCAR_ERR202;
                    throw new AuthenticationException("No permission.");
                }

                if (!string.IsNullOrWhiteSpace(sysInfo.SecKey) && !string.IsNullOrWhiteSpace(secKey))
                {
                    string encrypt = ApplicationHelpers.GetHashString(secKey);

                    if (!sysInfo.SecKey.Equals(encrypt))
                    {
                        _errorCode = Constants.ErrorCode.KKCAR_ERR201;
                        throw new AuthenticationException("Invalid security key.");
                    }
                }
            }
            catch (AuthenticationException aex)
            {
                noError = false;
                _errorMessage = aex.Message;
                Logger.Error("AuthenticationException occur:\n", aex);
            }
            catch (Exception ex)
            {
                noError = false;
                _errorCode = Constants.ErrorCode.KKCAR_ERR100;
                _errorMessage = ex.Message;
                Logger.Error("Exception occur:\n", ex);
            }

            return noError;
        }

        private bool CheckAuth(string sysCode, string secKey)
        {
            bool noError = true;

            try
            {
                _authDataAccess = new AuthenticationDataAccess(_context);
                SystemInfoEntity sysInfo = _authDataAccess.GetSystemInfoByCode(sysCode);

                if (sysInfo == null)
                {
                    _errorCode = Constants.ErrorCode.KKCAR_ERR202;
                    throw new AuthenticationException("No permission.");
                }

                if (!string.IsNullOrWhiteSpace(sysInfo.SecKey) && !string.IsNullOrWhiteSpace(secKey))
                {
                    string encrypt = ApplicationHelpers.GetHashString(secKey);

                    if (!sysInfo.SecKey.Equals(encrypt))
                    {
                        _errorCode = Constants.ErrorCode.KKCAR_ERR201;
                        throw new AuthenticationException("Invalid security key.");
                    }
                }
            }
            catch (AuthenticationException aex)
            {
                noError = false;
                _errorMessage = aex.Message;
                Logger.Error("AuthenticationException occur:\n", aex);
            }
            catch (Exception ex)
            {
                noError = false;
                _errorCode = Constants.ErrorCode.KKCAR_ERR100;
                _errorMessage = ex.Message;
                Logger.Error("Exception occur:\n", ex);
            }

            return noError;
        }

        #endregion

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
