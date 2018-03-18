using Cas.Biz;
using Cas.Common;
using Cas.Dal.BatchData;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace Cas.LogServce
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CARLogService : ICARLogService
    {
        #region Member
        CreateActivityLogBiz _biz = null;
        private readonly ILog _logger;
        #endregion

        #region Properties
        CreateActivityLogBiz business
        {
            get { return _biz = _biz ?? new CreateActivityLogBiz(); }
        }
        #endregion

        #region Constructor
        public CARLogService()
        {
            try
            {
                _logger = LogManager.GetLogger(typeof(CARLogService));
            }
            catch (Exception ex)
            {
                _logger.Error("E:--Exception occur:--\n", ex);
            }
        }
        #endregion

        #region BulkCreateActivityLog
        public string BulkCreateActivityLog(string systemCode)
        {
            try
            {
                string strResult = string.Empty;
                _logger.InfoFormat("I:--BulkCreateActivityLog({0})--Start--", systemCode);
                if(business.BulkCreateActivityLog(systemCode))
                {
                    strResult = "Success";
                }
                else
                {
                    strResult = "Fail";
                }
                _logger.InfoFormat("O:--BulkCreateActivityLog({0})--End--", systemCode);
                return strResult;
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("E:--BulkCreateActivityLog({0})--End--", ex.Message);
                _logger.Error("E:--BulkCreateActivityLog--", ex);
                return "Fail";
            }
        }
        public void BulkCreateActivityLogReRun(string systemCode, string fileName)
        {
            try
            {
                _logger.Info("I:--BulkCreateActivityLogReRun--Start--");
                business.BulkCreateActivityLog(systemCode, fileName);
                _logger.Info("O:--BulkCreateActivityLogReRun--End--");
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("E:--BulkCreateActivityLogReRun({0})--End--", ex.Message);
                _logger.Error("E:--BulkCreateActivityLogReRun--", ex);
            }
        }
        public void BulkCreateActivityRerunAll()
        {
            try
            {
                _logger.Info("I:--BulkCreateActivityRerunAll--Start--");
                business.BulkCreateActivityLogRerunAll();
                _logger.Info("O:--BulkCreateActivityRerunAll--End--");
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("E:--BulkCreateActivityRerunAll({0})--End--", ex.Message);
                _logger.Error("E:--BulkCreateActivityRerunAll--", ex);
            }
        }
        #endregion

        #region HttpCreateActivityLog
        //สำหรับ ระบบอื่นๆ เรียกใช้ตอนส่ง Request มาบอกว่าจะสร้าง ActivityLog
        public BatchResponseData CreateActivityLog(string system, string serviceName, string dataDate, string path, bool getResult)
        {
            BatchResponseData response = null;
            try
            {
                _logger.Info("I:--HTTPCreateActivityLog--Start--");
                _logger.InfoFormat("I:--HTTPCreateActivityLog--detail--system={0},serviceName={1},dataDate={2},path={3},getResult={4}--", system, serviceName, dataDate, path, getResult);
                HTTPCreateActivityLogRequest request = new HTTPCreateActivityLogRequest
                {
                    SystemCode = system,
                    ServiceName = serviceName,
                    DataDate = dataDate,
                    Path = path,
                    GetResult = getResult
                };
                _logger.Debug(request.SerializeObject());
                response = business.ValidateServiceRequest(request);
                if (response != null && !string.IsNullOrEmpty(response.ResponseCode))
                {
                    goto ReturnResult;
                }

                response = response ?? new BatchResponseData();
                response.ResponseCode = BatchResponse.Code000;
                response.ResponseMessage = BatchResponse.Message000;

                Task task = new Task(() => CreateActivityLog(request));
                task.Start();
                _logger.Info("O:--HTTPCreateActivityLog--End--");
            }
            catch (Exception ex)
            {
                response = response ?? new BatchResponseData();
                response.ResponseCode = BatchResponse.Code100;
                response.ResponseMessage = string.Format("{0} : {1}", BatchResponse.Message100, ex.Message);
                _logger.Error("E:--HTTPCreateActivityLog--", ex);
                _logger.InfoFormat("E:--HTTPCreateActivityLog({0})--End--", ex.Message);
            }
            ReturnResult:            
            _logger.Debug(response.SerializeObject());
            return response;
        }
        //สำหรับ ระบบอื่นๆ เรียกใช้เพื่อเอาผลของการส่ง Request มาสร้าง ActivityLog
        public BatchCreateActivityLogResponse CreateActivityLogResult(string system, string serviceName, string dataDate, string refNo)
        {            
            BatchCreateActivityLogResponse response = new BatchCreateActivityLogResponse();
            try
            {
                _logger.Info("I:--HTTPCreateActivityLogResult--Start--");
                _logger.InfoFormat("I:--HTTPCreateActivityLogResult--detail--system={0},serviceName={1},dataDate={2},refNo={3}--", system, serviceName, dataDate, refNo);
                response = business.CreateActivityLogResult(system, serviceName, dataDate,refNo);
                _logger.Info("O:--HTTPCreateActivityLogResult--End--");
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("E:--HTTPCreateActivityLogResult({0})--End--", ex.Message);
                _logger.Error("E:--HTTPCreateActivityLogResult--", ex);
                BatchCreateActivityLogBody body = new BatchCreateActivityLogBody();
                BatchResponseData status = new BatchResponseData();
                status.ResponseCode = BatchResponse.Code100;
                status.ResponseMessage = string.Format("{0}-{1}", BatchResponse.Message100, ex.Message);
                body.ResponseStatus = status;
                response.Body = new List<BatchCreateActivityLogBody>();
                response.Body.Add(body);
            }            
            _logger.Debug(response.SerializeObject());
            return response;
        }
        //สำหรับให้หน้าจอ Batch Mornitoring เรียกใช้เพื่อดึงข้อมูลมาสร้าง ActivityLog อีกรอบ
        public void HttpCreateActivityLogRerun(string system, string serviceName, string dataDate, string path, bool getResult)
        {
            CreateActivityLog(system, serviceName, dataDate, path, getResult);
        }
        #endregion

        #region Private Method
        //Method สำหรับสร้าง ActivityLog เป็น Method ที่ทำงานต่อหลังจากส่ง result ไปหา ระบบต้นทางแล้ว เรียกทำงานด้วย Threading.Task
        private void CreateActivityLog(HTTPCreateActivityLogRequest request)
        {
            try
            {
                _logger.Info("I:--HTTPCreateActivityLog--Start Task--");
                string strRequestResult = GetActivityLogData(request);
                if(string.IsNullOrWhiteSpace(strRequestResult))
                {
                    _logger.Info("O:--HTTPCreateActivityLog--End without data--");
                }
                else
                {
                    string strReferenceNo = business.HTTPCreateActivityLog(request, strRequestResult);
                    if (request.GetResult)
                    {
                        CreateActivityLogNotifyResult(request, strReferenceNo);
                    }
                    _logger.Info("O:--HTTPCreateActivityLog--End--");
                }

            }
            catch (Exception ex)
            {
                _logger.Error("E:--HTTPCreateActivityLog--Task--", ex);
            }
        }
        //Method สำหรับ Post ข้อมูลไประบบปลายทาง และรับข้อมูลมา เพื่อส่งไปสร้าง ActivityLog
        private string GetActivityLogData(HTTPCreateActivityLogRequest request)
        {
            _logger.InfoFormat("I:--HTTPGetActivityLogData--:--Request Data--:Uri/{0}", request.Path);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage resp = client.GetAsync(request.Path).Result;
                if (resp.IsSuccessStatusCode)
                {
                    // Parse the response body. 
                    resp.Content.ReadAsStringAsync().Wait();
                    string jsonString = resp.Content.ReadAsStringAsync().Result;
                    _logger.Debug(jsonString);
                    return jsonString;
                }
            }
            return string.Empty;
        }
        //Method สำหรับ แจ้งเตือนให้ระบบปลายทางทราบว่า สร้าง ActivityLog เสร็จแล้วมาดึง result ไปได้
        private void CreateActivityLogNotifyResult(HTTPCreateActivityLogRequest request, string referenceNo)
        {
            try
            {
                _logger.Info("I:--HTTPCreateActivityLogNotifyResult--Start--");
                var system = business.LoadBatchSystemMapping(request.SystemCode, Batch.HttpCreateActivityLog);
                var batch = business.LoadBatchMasterByCode(Batch.HttpCreateActivityLog);
                _logger.InfoFormat("I:--HTTPCreateActivityLogNotifyResult--:----:Uri/{0}", system.RESPONSE_URL);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    dynamic jsonObject = new JObject();
                    jsonObject.system = request.SystemCode;
                    jsonObject.serviceName = request.ServiceName;
                    jsonObject.dataDate = request.DataDate;
                    jsonObject.refNo = referenceNo;
                    jsonObject.path = string.Format("{0}/CreateActivityLogResult",batch.MAIN_URL);

                    var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = client.PostAsync(system.RESPONSE_URL, content).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        resp.Content.ReadAsStringAsync().Wait();
                        string jsonString = resp.Content.ReadAsStringAsync().Result;
                        _logger.Debug("--HTTPCreateActivityLogNotifyResult--:Result Request Data--:jsonString/" + jsonString);
                    }
                }

                _logger.Info("O:--HTTPCreateActivityLogNotifyResult--End--");
            }
            catch (Exception ex)
            {
                _logger.Error("E:--HTTPCreateActivityLogNotifyResult--", ex);
            }
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
                    if (_biz != null) { _biz.Dispose(); _biz = null; }
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
