using KKCAR.Business;
using KKCAR.Common.Resources;
using KKCAR.Common.Utilities;
using KKCAR.Entity;
using KKCAR.Service.Messages;
using KKCAR.Service.Messages.Batch;
using KKCAR.Service.Messages.Common;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

///<summary>
/// Class Name : KKCARStatusRestService
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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class KKCARStatusRestService : IKKCARStatusRestService
    {
        private long _elapsedTime;
        private readonly ILog _logger;
        private KKCARMailSender _mailSender;
        private IStatusFacade _statusFacade;
        private IAuditLogFacade _auditLogFacade;
        private IAuthenticationFacade _authFacade;
        private System.Diagnostics.Stopwatch _stopwatch;
        private LogMessageBuilder _logMsg = new LogMessageBuilder();

        #region "Constructor"

        public KKCARStatusRestService()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                // Set logfile name and application name variables
                GlobalContext.Properties["ApplicationCode"] = "KKCARWS";
                GlobalContext.Properties["ServerName"] = Environment.MachineName;
                _logger = LogManager.GetLogger(typeof(KKCARStatusRestService));
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occur:\n", ex);
            }
        }

        #endregion

        #region "Received Notify"
        public ResponseStatus InsertStatus_Post(string system, string serviceName, string dataDate, bool getResult, string path)
        {
            BatchInsertStatusRequest request = new BatchInsertStatusRequest
            {
                SystemCode = system,
                ServiceName = serviceName,
                DataDate = dataDate,
                GetResult = getResult,
                Path = path
            };

            bool valid;
            _authFacade = new AuthenticationFacade();
            var response = _authFacade.ValidateServiceRequest(system, serviceName, out valid);

            if (!valid)
            {
                goto Outer;
            }

            if (dataDate.ParseDateTime("yyyyMMdd") == null)
            {
                response.ResponseCode = Constants.StatusResponse.Failed;
                response.ResponseMessage = string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidDateFormat, "Data Date");
            }
            else
            {
                response.ResponseCode = Constants.StatusResponse.Success;
                response.ResponseMessage = "Received Notify";
            }

            Task task = new Task(() => InsertStatus(request));
            task.Start();

        Outer:
            return response;
        }
        public ResponseStatus InsertStatus_Get(string system, string serviceName, string dataDate, bool getResult, string path)
        {
            BatchInsertStatusRequest request = new BatchInsertStatusRequest
            {
                SystemCode = system,
                ServiceName = serviceName,
                DataDate = dataDate,
                GetResult = getResult,
                Path = path
            };

            bool valid;
            _authFacade = new AuthenticationFacade();
            var response = _authFacade.ValidateServiceRequest(system, serviceName, out valid);

            if (!valid)
            {
                goto Outer;
            }

            if (dataDate.ParseDateTime("yyyyMMdd") == null)
            {
                response.ResponseCode = Constants.StatusResponse.Failed;
                response.ResponseMessage = string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidDateFormat, "Data Date");
            }
            else
            {
                response.ResponseCode = Constants.StatusResponse.Success;
                response.ResponseMessage = "Received Notify";
            }

            Task task = new Task(() => InsertStatus(request));
            task.Start();

        Outer:
            return response;
        }
        #endregion

        #region "Send Result"

        public BulkInsertStatusResponse SendResult_Get(string system, string serviceName, string dataDate, string refNo)
        {
            return this.SendResult(system, serviceName, dataDate, refNo);
        }

        public BulkInsertStatusResponse SendResult_Post(string system, string serviceName, string dataDate, string refNo)
        {
            return this.SendResult(system, serviceName, dataDate, refNo);
        }

        #endregion

        #region "Functions"

        private TaskInsertStatusResponse InsertStatus(BatchInsertStatusRequest batchRequest)
        {
            ThreadContext.Properties["EventClass"] = ApplicationHelpers.GetCurrentMethod(1);
            ThreadContext.Properties["RemoteAddress"] = ApplicationHelpers.GetClientIP();

            if (!string.IsNullOrWhiteSpace(batchRequest.SystemCode))
            {
                ThreadContext.Properties["UserID"] = batchRequest.SystemCode.ToUpperInvariant();
            }

            bool skipBatchLog = false;
            string refNo = string.Empty;
            DateTime schedDateTime = DateTime.Now;
            TaskInsertStatusResponse taskResponse = new TaskInsertStatusResponse(schedDateTime);

            try
            {
                _logger.Debug("I:-- Start Cron Job --:--Batch Insert Status--");

                var logDetail = string.Empty;
                var logErrCode = Constants.ErrorCode.KKCAR_SUCCESS;
                _stopwatch = System.Diagnostics.Stopwatch.StartNew();

                BulkInsertStatusRequest bulkRequest = RequestData(batchRequest.SystemCode, batchRequest.DataDate, batchRequest.Path);
                refNo = bulkRequest.Header.ReferenceNo;

                _statusFacade = new StatusFacade();
                var taskResult = _statusFacade.BatchInsertStatus(batchRequest, bulkRequest);
                taskResponse.TaskResults.Add(taskResult);

                if (taskResult.NumOfError > 0)
                {
                    logErrCode = Constants.ErrorCode.KKCAR_ERR103;
                    logDetail = "Invalid Data";
                }

                taskResponse.ElapsedTime = _elapsedTime;
                taskResponse.ResponseStatus = new ResponseStatus
                {
                    ResponseCode = logErrCode,
                    ResponseMessage = logDetail
                };

                skipBatchLog = true;
                _logger.InfoFormat("O:--SUCCESS--:--Batch Insert Status--");
                return taskResponse;
            }
            catch (CustomException exc)
            {
                var statusCode = exc.Data.Keys.Cast<string>().Single();
                var statusMessage = exc.Data[statusCode].ToString();

                taskResponse.ElapsedTime = _elapsedTime;
                taskResponse.ResponseStatus = new ResponseStatus
                {
                    ResponseCode = statusCode,
                    ResponseMessage = statusMessage
                };

                _logger.InfoFormat("O:--FAILED--:--Call Batch Insert Status--:Error Message/{0}", statusMessage);
                _logger.Error("Exception occur:\n", exc);
                return taskResponse;
            }
            catch (Exception exc)
            {
                taskResponse.ElapsedTime = _elapsedTime;
                taskResponse.ResponseStatus = new ResponseStatus
                {
                    ResponseCode = Constants.ErrorCode.KKCAR_ERR101,
                    ResponseMessage = "Internal Error"
                };

                _logger.InfoFormat("O:--FAILED--:--Call Batch Insert Status--:Error Message/{0}", exc.Message);
                _logger.Error("Exception occur:\n", exc);
                return taskResponse;
            }
            finally
            {
                #region "Save Batch Log"

                if (!skipBatchLog)
                {
                    var auditLog = new AuditLogEntity();
                    auditLog.BatchDate = batchRequest.DataDate;
                    auditLog.StartDateTime = schedDateTime;
                    auditLog.EndDateTime = DateTime.Now;
                    auditLog.LogDetail = taskResponse.ResponseStatus.ResponseMessage;
                    auditLog.RerunPath = GetNotifyUrl(batchRequest);
                    auditLog.SystemCode = batchRequest.SystemCode;
                    auditLog.ServiceName = batchRequest.ServiceName;

                    AppLog.AuditLog(auditLog);
                }

                #endregion

                if (batchRequest.GetResult && taskResponse.TaskResults.Any())
                {
                    _auditLogFacade = new AuditLogFacade();
                    var batchCmdPath = _auditLogFacade.GetBatchCmdPathByCode(Constants.BatchCode.BatchInsertStatus);
                    NotifyClient(batchRequest.SystemCode, batchRequest.ServiceName, batchRequest.DataDate, refNo, string.Format("{0}/SendResult", batchCmdPath));
                }

                _logger.DebugFormat("-- XMLResponse --\n{0}", taskResponse.SerializeObject());
                _stopwatch.Stop();
                _elapsedTime = _stopwatch.ElapsedMilliseconds;
                _logger.DebugFormat("O:--Finish--:ElapsedMilliseconds/{0}", _elapsedTime);
            }
        }

        private BulkInsertStatusResponse SendResult(string systemCode, string serviceName, string dataDate, string refNo)
        {
            try
            {
                _logger.InfoFormat("I:--START--:--Send Result--:systemCode/{0}:serviceName/{1}:dataDate/{2}:refNo/{3}", systemCode, serviceName, dataDate, refNo);

                _auditLogFacade = new AuditLogFacade();
                var result = _auditLogFacade.GetBatchInsertResult(systemCode, serviceName, dataDate, refNo);

                _logger.Info("O:--SUCCESS--:--Send Result--");
                return result;
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("O:--FAILED--:Send Result--:Error Message/{0}", ex.Message);
                _logger.Error("Exception occur:\n", ex);
            }

            return null;
        }

        private BulkInsertStatusRequest RequestData(string systemCode, string dataDate, string uri)
        {
            try
            {
                _logger.InfoFormat("I:--START--:--Request Data--:Uri/{0}:systemCode/{1}:dataDate/{2}", uri, systemCode, dataDate, uri);

                #region "Comment out"

                //using (var client = new System.Net.Http.HttpClient())
                //{
                //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //    dynamic jsonObject = new JObject();
                //    jsonObject.systemCode = systemCode;
                //    jsonObject.dataDate = dataDate;

                //    var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                //    System.Net.Http.HttpResponseMessage resp = client.PostAsync(uri, content).Result;

                //    if (resp.IsSuccessStatusCode)
                //    {
                //        // Parse the response body. 
                //        resp.Content.ReadAsStringAsync().Wait();
                //        string jsonString = resp.Content.ReadAsStringAsync().Result;
                //        _logger.Debug("O:--SUCCESS--:Request Data--:jsonString/" + jsonString);
                //        return JsonConvert.DeserializeObject<BulkInsertStatusRequest>(jsonString);
                //    }
                //}

                #endregion

                using (var client = new HttpClient())
                {
                    HttpResponseMessage resp = client.GetAsync(uri).Result;
                    string jsonString = resp.Content.ReadAsStringAsync().Result;
                    _logger.Debug("O:--SUCCESS--:Request Data--:jsonString/" + jsonString);
                    return JsonConvert.DeserializeObject<BulkInsertStatusRequest>(jsonString);
                }
            }
            catch (JsonSerializationException)
            {
                var cex = new CustomException("Invalid JSON format");
                cex.Data.Add(Constants.ErrorCode.KKCAR_ERR402, "Invalid JSON format");
                _logger.Debug("O:--FAILED--:Request Data--:jsonString/" + cex.Message);
                throw cex;
            }
            catch (System.Net.WebException)
            {
                var cex = new CustomException("Unable to connect to the remote server");
                cex.Data.Add(Constants.ErrorCode.KKCAR_ERR100, "Unable to connect to the remote server");
                _logger.Debug("O:--FAILED--:Request Data--:jsonString/" + cex.Message);
                throw cex;
            }
        }

        private bool NotifyClient(string systemCode, string serviceName, string dataDate, string refNo, string path)
        {
            _authFacade = new AuthenticationFacade();
            var uri = _authFacade.GetResponseUrlBySysCode(systemCode);
            _logger.InfoFormat("O:--Get Response URL--:Url/{0}", uri);

            if (!string.IsNullOrWhiteSpace(uri))
            {
                try
                {
                    _logger.InfoFormat("I:--START--:--Request Data--:Uri/{0}:Path/{1}:systemCode/{2}:serviceName/{3}:dataDate/{4}:refNo/{5}", 
                        uri, path, systemCode, serviceName, dataDate, refNo);

                    using (var client = new System.Net.Http.HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        dynamic jsonObject = new JObject();
                        jsonObject.system = systemCode;
                        jsonObject.serviceName = serviceName;
                        jsonObject.dataDate = dataDate;
                        jsonObject.refNo = refNo;
                        jsonObject.path = path;

                        var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                        System.Net.Http.HttpResponseMessage resp = client.PostAsync(uri, content).Result;

                        if (resp.IsSuccessStatusCode)
                        {
                            // Parse the response body. 
                            resp.Content.ReadAsStringAsync().Wait();
                            string jsonString = resp.Content.ReadAsStringAsync().Result;
                            _logger.Debug("O:--SUCCESS--:Request Data--:jsonString/" + jsonString);

                            return jsonString.Contains(Constants.StatusResponse.Success);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.InfoFormat("O:--FAILED--:Request Data--:Error Message/{0}", ex.Message);
                    _logger.Error("Exception occur:\n", ex);
                }
            }
            else
            {
                _logger.Info("O:--Get Response URL--:Url/NULL");
            }

            return false;
        }

        private void BatchSendMail(TaskInsertStatusResponse result)
        {
            try
            {
                _mailSender = KKCARMailSender.GetKKCARMailSender();
                var isError = result.TaskResults.Any(x => x.NumOfError > 0);

                if (isError)
                {
                    _mailSender.NotifyBatchInsertStatusWarned(WebConfig.GetTaskEmailToAddress(), result);
                }
                if (!(isError || Constants.ErrorCode.KKCAR_SUCCESS.Equals(result.ResponseStatus.ResponseCode)))
                {
                    _mailSender.NotifyBatchInsertStatusFailed(WebConfig.GetTaskEmailToAddress(), result.SchedDateTime, result.ResponseStatus.ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occur:\n", ex);
            }
        }

        private static string GetNotifyUrl(BatchInsertStatusRequest request)
        {
            return ApplicationHelpers.GetNotifyUrl(request.SystemCode, request.ServiceName, request.DataDate, request.GetResult, request.Path);
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
                    if (_statusFacade != null){ _statusFacade.Dispose(); }
                    if(_auditLogFacade != null){ _auditLogFacade.Dispose();}
                    if (_authFacade != null) { _authFacade.Dispose(); }
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
