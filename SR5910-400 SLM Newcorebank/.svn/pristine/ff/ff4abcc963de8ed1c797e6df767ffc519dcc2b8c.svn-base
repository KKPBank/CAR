using KKCAR.Batch.Utilities;
using KKCAR.Common.Utilities;
using log4net;
using System;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json.Linq;

///<summary>
/// Class Name : StatusProcess
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Batch.Processes
{
    public class StatusProcess
    {
        private static TaskMailSender _mailSender;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StatusProcess));

        public static void InsertStatusAsync()
        {
            try
            {
                Logger.Info("I:--START--");
                string batchUrl = string.Format(CultureInfo.InvariantCulture, "{0}/KKCARBatchStatusService.svc/CARInsertStatus", WebConfig.GetBatchMainUrl());

                dynamic jsonObject = new JObject();
                jsonObject.username = WebConfig.GetTaskUsername();
                jsonObject.password = WebConfig.GetTaskPassword();
                jsonObject.skipSftp = false;

                TaskHelpers.HttpPostAysnc(batchUrl, jsonObject);
                System.Threading.Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Logger.InfoFormat("O:--FAILED--:Exception/{0}", ex.Message);
                Logger.Error("Exception occur:\n", ex);

                // Send mail to system administrator
                _mailSender = TaskMailSender.GetTaskMailSender();
                _mailSender.NotifyBatchInsertStatusFailed(WebConfig.GetTaskEmailToAddress(), DateTime.Now, new AggregateException(ex));
                Thread.Sleep(5000);
            }
        }
    }
}
