using System;
using KKCAR.Entity;
using log4net;

///<summary>
/// Class Name : AppLog
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
    public class AppLog
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AppLog));

        public static void AuditLog(AuditLogEntity auditLog)
        {
            try
            {
                IAuditLogFacade auditLogFacade = new AuditLogFacade();
                auditLogFacade.AddLog(auditLog);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }
        }

        public static void AuditLog(string batchCode, DateTime startDateTime, DateTime endDateTime, LogStatus logStatus)
        {
            try
            {
                IAuditLogFacade auditLogFacade = new AuditLogFacade();
                auditLogFacade.AddLog(batchCode, startDateTime, endDateTime, logStatus);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }
        }
    }
}
