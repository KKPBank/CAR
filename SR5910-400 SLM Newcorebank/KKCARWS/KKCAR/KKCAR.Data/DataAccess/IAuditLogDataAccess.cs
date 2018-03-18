using KKCAR.Entity;
using System;

///<summary>
/// Class Name : IAuditLogDataAccess
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Data.DataAccess
{
    public interface IAuditLogDataAccess
    {
        void AddLog(AuditLogEntity auditlog);
        void AddTraceLog(TraceLogEntity traceLog);
        string GetBatchCmdPathByCode(string code);
        void AddLog(string batchCode, DateTime startDateTime, DateTime endDateTime, LogStatus logStatus);
        AuditLogEntity GetBatchLog(string batchCode, string systemCode, string serviceName, string dateDate, string refNo);
    }
}
