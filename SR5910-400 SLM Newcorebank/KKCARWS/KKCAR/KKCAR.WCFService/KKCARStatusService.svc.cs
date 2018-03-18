using KKCAR.Business;
using KKCAR.Common.Utilities;
using KKCAR.Entity;
using KKCAR.Service.Messages;
using log4net;
using System;
using System.ServiceModel;

///<summary>
/// Class Name : KKCARStatusService
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.WCFService
{
    public class KKCARStatusService : IKKCARStatusService
    {
        private readonly ILog _logger;
        private IStatusFacade _statusFacade;
        private IAuditLogFacade _auditLogFacade;

        #region "Constructor"

        public KKCARStatusService()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                // Set logfile name and application name variables
                GlobalContext.Properties["ApplicationCode"] = "KKCARWS";
                GlobalContext.Properties["ServerName"] = Environment.MachineName;
                _logger = LogManager.GetLogger(typeof(KKCARStatusService));
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occur:\n", ex);
            }
        }
        #endregion

        public InsertStatusResponse CARInsertStatus(InsertStatusRequest request)
        {
            var requestDateTime = DateTime.Now;
            InsertStatusResponse response = null;
            string ipAddress = ApplicationHelpers.GetClientIP();
            var requestUrl = OperationContext.Current.RequestContext.RequestMessage.Headers.To.OriginalString;

            ThreadContext.Properties["EventClass"] = ApplicationHelpers.GetCurrentMethod(1);
            ThreadContext.Properties["RemoteAddress"] = ipAddress;

            if (request != null && request.Header != null)
            {
                ThreadContext.Properties["UserID"] = request.Header.SystemCode;
            }

            try
            {
                _statusFacade = new StatusFacade();
                response = _statusFacade.InsertStatus(request, false, false);
                return response;
            }
            finally
            {
                SaveTraceLog(requestDateTime, ipAddress, requestUrl, request, response);
            }
        }

        #region "Functions"

        private void SaveTraceLog(DateTime requestDateTime, string ipAddress, string requestUrl, InsertStatusRequest request, InsertStatusResponse response)
        {
            TraceLogEntity traceLog = new TraceLogEntity();
            traceLog.SystemCode = request != null ? request.Header.SystemCode : string.Empty;
            traceLog.ServiceName = Constants.ServiceName.InsertStatus;
            traceLog.IpAddress = ipAddress;
            traceLog.ReferenceCode = request != null ? request.Header.ReferenceNo : string.Empty;
            traceLog.RequestDateTime = requestDateTime;
            traceLog.RequestUrl = requestUrl;
            traceLog.ResponseCode = response != null ? response.ResponseStatus.ResponseCode : string.Empty;
            traceLog.ResponseMessage = response != null ?  response.ResponseStatus.ResponseMessage : string.Empty;
            traceLog.XmlRequest = request != null ? request.SerializeObject() : string.Empty;
            traceLog.XmlResponse = request != null ? response.SerializeObject() : string.Empty;

            _auditLogFacade = new AuditLogFacade();
            _auditLogFacade.AddTraceLog(traceLog);
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
                    if (_statusFacade != null)
                    {
                        _statusFacade.Dispose();
                    }
                    if (_auditLogFacade != null)
                    {
                        _auditLogFacade.Dispose();
                    }
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
