using KKCAR.Entity;
using KKCAR.Service.Messages.Batch;
using System;

///<summary>
/// Class Name : IAuditLogFacade
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
    public interface IAuditLogFacade : IDisposable
    {
        void AddLog(AuditLogEntity auditLog);
        void AddTraceLog(TraceLogEntity traceLog);
        string GetBatchCmdPathByCode(string code);
        void AddLog(string systemCode, DateTime startDateTime, DateTime endDateTime, LogStatus logStatus);
        BulkInsertStatusResponse GetBatchInsertResult(string systemCode, string serviceName, string dataDate, string refNo);
    }
}
