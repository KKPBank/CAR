using KKCAR.Common.Utilities;
using KKCAR.Data.DataAccess;
using KKCAR.Entity;
using KKCAR.Service.Messages.Batch;
using KKCAR.Service.Messages.Common;
using System;
using System.Linq;

///<summary>
/// Class Name : AuditLogFacade
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
    public class AuditLogFacade : IAuditLogFacade
    {
        private KKCARContextContainer _context;
        private IAuditLogDataAccess _auditLogDataAccess;

        public AuditLogFacade()
        {
            _context = new KKCARContextContainer();
        }

        public void AddLog(AuditLogEntity auditLog)
        {
            _auditLogDataAccess = new AuditLogDataAccess(_context);
            _auditLogDataAccess.AddLog(auditLog);
        }

        public void AddLog(string batchCode, DateTime startDateTime, DateTime endDateTime, LogStatus logStatus)
        {
            _auditLogDataAccess = new AuditLogDataAccess(_context);
            _auditLogDataAccess.AddLog(batchCode, startDateTime, endDateTime, logStatus);
        }

        public BulkInsertStatusResponse GetBatchInsertResult(string systemCode, string serviceName, string dataDate, string refNo)
        {
            _auditLogDataAccess = new AuditLogDataAccess(_context);
            var auditLog = _auditLogDataAccess.GetBatchLog(Constants.BatchCode.BatchInsertStatus, systemCode, serviceName, dataDate, refNo);

            BatchResponseBody[] body = null;
            BatchHeader header = new BatchHeader();
            header.CreateDate = auditLog.TransactionDate;
            header.SystemCode = auditLog.SystemCode;
            header.ReferenceNo = auditLog.ReferenceCode;

            if (auditLog.LogDetailList != null && auditLog.LogDetailList.Count > 0)
            {
                body = (from x in auditLog.LogDetailList
                        select new BatchResponseBody
                        {
                            ChannelId = x.ChannelId,
                            ReferenceNo = x.ReferenceNo,
                            ResponseStatus = new BatchResponseStatus
                            {
                                ResponseCode = x.LogCode,
                                ResponseMessage = x.LogDetail
                            }
                        }).ToArray();
            }

            BulkInsertStatusResponse response = new BulkInsertStatusResponse();
            response.Header = header;
            response.Body = body;
            return response;
        }

        public string GetBatchCmdPathByCode(string code)
        {
            _auditLogDataAccess = new AuditLogDataAccess(_context);
            return _auditLogDataAccess.GetBatchCmdPathByCode(code);
        }

        public void AddTraceLog(TraceLogEntity traceLog)
        {
            _auditLogDataAccess = new AuditLogDataAccess(_context);
            _auditLogDataAccess.AddTraceLog(traceLog);
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
