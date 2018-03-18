using KKCAR.Batch.Utilities;
using KKCAR.Common.Utilities;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

///<summary>
/// Class Name : FileProcess
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
    public class FileProcess
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FileProcess));

        public static void InsertStatusByExcel()
        {
            try
            {
                Logger.Info("I:--START--");
                string batchUrl = string.Format(CultureInfo.InvariantCulture, "{0}/KKCARBatchStatusService.svc/CARInsertStatusByExcel", WebConfig.GetBatchMainUrl());

                dynamic jsonObject = new JObject();
                jsonObject.username = WebConfig.GetTaskUsername();
                jsonObject.password = WebConfig.GetTaskPassword();

                TaskHelpers.HttpPostAysnc(batchUrl, jsonObject);
                System.Threading.Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Logger.InfoFormat("O:--FAILED--:Exception/{0}", ex.Message);
                Logger.Error("Exception occur:\n", ex);
            }
        }

        public static void BatchInsertStatusByExcel()
        {
            try
            {
                Logger.Info("I:--START--");
                string batchUrl = string.Format(CultureInfo.InvariantCulture, "{0}/KKCARBatchStatusService.svc/BatchCARInsertStatusByExcel", WebConfig.GetBatchMainUrl());

                dynamic jsonObject = new JObject();
                jsonObject.username = WebConfig.GetTaskUsername();
                jsonObject.password = WebConfig.GetTaskPassword();

                TaskHelpers.HttpPostAysnc(batchUrl, jsonObject);
                System.Threading.Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Logger.InfoFormat("O:--FAILED--:Exception/{0}", ex.Message);
                Logger.Error("Exception occur:\n", ex);
            }
        }
    }
}
