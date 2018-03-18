using KKCAR.Batch.Processes;
using log4net;
using System;
using System.Globalization;

///<summary>
/// Class Name : Program
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Batch
{
    class Program
    {
        private static ILog _log;

        static void Main(string[] args)
        {
            try
            {
                // Set logfile name and application name variables
                log4net.GlobalContext.Properties["ApplicationCode"] = "KKCAR_BATCH";
                log4net.GlobalContext.Properties["ServerName"] = System.Environment.MachineName;
                log4net.ThreadContext.Properties["UserID"] = GetCurrentUser();
                _log = LogManager.GetLogger(typeof(Program));
            }
            catch (Exception ex)
            {
                _log.Error("Exception occur:\n", ex);
            }

            foreach (string arg in args)
            {
                var commandLine = arg.Substring(0, 3);
                switch (commandLine.ToUpper(CultureInfo.InvariantCulture))
                {
                    case "/IS":
                        _log.Info("I:--START--:--InsertStatus--");
                        StatusProcess.InsertStatusAsync();
                        _log.Info("O:--SUCCESS--:--InsertStatus--");
                        break;
                    case "/X1":
                        _log.Info("I:--START--:--InsertStatus by Excel--");
                        FileProcess.InsertStatusByExcel();
                        _log.Info("O:--SUCCESS--:--InsertStatus by Excel--");
                        break;
                    case "/X2":
                        _log.Info("I:--START--:--Batch InsertStatus by Excel--");
                        FileProcess.BatchInsertStatusByExcel();
                        _log.Info("O:--SUCCESS--:--Batch InsertStatus by Excel--");
                        break;
                    default:
                        // do other stuff...
                        break;
                }
            }
        }

        private static string GetCurrentUser()
        {
            try
            {
                string domainName = Environment.UserDomainName.ToUpperInvariant();
                string accountName = Environment.UserName.ToUpperInvariant();
                return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", domainName, accountName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
