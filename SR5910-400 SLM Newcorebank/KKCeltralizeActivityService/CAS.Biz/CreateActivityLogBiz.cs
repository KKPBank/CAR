using Cas.Common;
using Cas.Dal;
using Cas.Dal.BatchData;
using Cas.Dal.Data;
using log4net;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace Cas.Biz
{
    public class CreateActivityLogBiz: IDisposable
    {
        #region Member
        private readonly ILog _logger;
        private CreateActivityLogDataAccess _db = null;
        private AuthenBiz _auth = null;
        private BatchCreateActivityLogResponse _resp = null;
        private SftpClient _sftp = null;

        private string strErrorStep = string.Empty;
        private decimal? iTotalHeader = 0;
        private decimal? iTotalDetail = 0;
        private decimal? iTotalComplete = 0;
        private decimal? iTotalFail = 0;
        private string strFileName = string.Empty;
        private bool iException = false;
        private decimal dBatchID = 0;
        private decimal dBatchLogID = 0;
        private StringBuilder _stringbuilder = null;
        #endregion

        #region Constructor
        public CreateActivityLogBiz()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
                _logger = LogManager.GetLogger(typeof(CreateActivityLogBiz));
            }
            catch (Exception ex)
            {
                _logger.Error("E:--Biz-Exception occur:\n", ex);
            }
        }
        #endregion

        #region Properties
        private CreateActivityLogDataAccess database
        {
            get
            {
                _db = _db ?? new CreateActivityLogDataAccess();
                return _db;
            }
        }
        private AuthenBiz authenBusiness
        {
            get
            {
                _auth = _auth ?? new AuthenBiz();
                return _auth;
            }
        }
        private BatchCreateActivityLogResponse responseActLog
        {
            get
            {
                _resp = _resp ?? new BatchCreateActivityLogResponse();
                return _resp;
            }
        }
        private SftpClient sFTP
        {
            get
            {
                if(_sftp == null || !_sftp.IsConnected)
                {
                    _sftp = new SftpClient(AppConfig.CreateActivityLogSFTPAddress, AppConfig.CreateActivityLogSFTPUserName, AppConfig.CreateActivityLogSFTPPassword);
                }
                return _sftp;
            }
            set
            {
                _sftp = value;
            }
        }
        private StringBuilder strBuilder
        {
            get
            {
                return _stringbuilder = _stringbuilder ?? new StringBuilder();
            }
            set
            {
                _stringbuilder = value;
            }
        }
        #endregion

        #region For BulkCreateActivityLog --> SFTP
        public void BulkCreateActivityLogRerunAll()
        {
            string strSFTPFullPath = string.Empty;
            try
            {
                _logger.Info("I:--Biz-BulkCreateActivityLogRerunAll--Start--");
                foreach (var system in database.LoadAllSystemCode())
                {
                    if (!sFTP.IsConnected) sFTP.Connect();
                    strSFTPFullPath = string.Format("{0}/{1}/{2}", AppConfig.CreateActivityLogSFTPSourcePath, AppConfig.BulkCreateActivityLogRootPath, system.SYSTEM_ID);
                    if (sFTP.Exists(strSFTPFullPath))
                    {
                        if (sFTP.IsConnected) sFTP.Disconnect();
                        BulkCreateActivityLog(system.SYSTEM_ID, string.Empty);
                    }
                }                
                _logger.Info("O:--Biz-BulkCreateActivityLogRerunAll--End--");
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("E:--Biz-BulkCreateActivityLogRerunAll({0})--End--", ex.Message);
                _logger.Error("E:--Biz-BulkCreateActivityLogRerunAll--", ex);                
            }
            finally
            {
                if (sFTP.IsConnected) sFTP.Disconnect();
            }
        }
        public bool BulkCreateActivityLog(string systemCode, string fileName = "")
        {            
            try
            {
                _logger.InfoFormat("I:--Biz-BulkCreateActivityLog:{0}--Start--", systemCode);
                strErrorStep = "Set Batch Process to start";
                dBatchID = database.SetBatchStartProcess(Batch.BulkCreateActivityLog);
                strErrorStep = "Check existing System Code";
                if (database.CheckExistingSystemCode(systemCode))
                {
                    if(SFTPProcessing(systemCode, fileName))
                    {
                        database.SetBatchEndProcess(Batch.BulkCreateActivityLog, BatchStatus.Success);
                        _logger.InfoFormat("O:--Biz-BulkCreateActivityLog:{0}--End-Success--", systemCode);
                        return true;
                    }
                    else
                    {
                        _logger.InfoFormat("O:--Biz-BulkCreateActivityLog:{0}--End-Fail--", systemCode);
                        return false;
                    }
                }
                else
                {
                    throw new ArgumentException(BatchResponse.Code101 + " : Not found System Code in database.");
                }
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("E:--Biz-BulkCreateActivityLog:{0}({1})--End--", systemCode, ExceptionService.GetMessage(ex));
                _logger.Error("E:--Biz-BulkCreateActivityLog--", ex);
                iException = true;
                SetErrorDetail(ExceptionService.GetToString(ex));
                database.SetBatchEndProcess(Batch.BulkCreateActivityLog, BatchStatus.Fail);
                SendMailService.SendMail(string.Format(AppConfig.BulkCreateActivityLogEmailSubjectError, systemCode), CreateTemplateEmail(systemCode), string.Empty);
                return false;
            }
        }
        private bool SFTPProcessing(string systemCode, string fileName)
        {
            _logger.InfoFormat("I:--Biz-SFTPProcessing:{0}--Start--", systemCode);
            string strURLRerun = string.Empty;
            string strSFTPFullPath = string.Empty;
            string strLocalFullPath = string.Empty;
            object[] objFileList = null;
            try
            {                
                strErrorStep = "Connect to SFTP";
                if (!sFTP.IsConnected) sFTP.Connect();
                //สำหรับทำงานปกติ
                if (string.IsNullOrEmpty(fileName))
                {
                    strSFTPFullPath = string.Format("{0}/{1}/{2}", AppConfig.CreateActivityLogSFTPSourcePath, AppConfig.BulkCreateActivityLogRootPath, systemCode);
                    _logger.InfoFormat("I:--Biz-SFTPProcessing:{0}-{1}--", systemCode, strSFTPFullPath);
                    strErrorStep = "Get all files from sftp-" + strSFTPFullPath;
                    if(sFTP.Exists(strSFTPFullPath))
                    {
                        objFileList = sFTP.ListDirectory(strSFTPFullPath).ToArray().Where(x => x.Name != "." &&
                                                                                               x.Name != ".." &&
                                                                                               !x.IsDirectory &&
                                                                                               !x.Name.ToUpperInvariant().Contains("RESPONSE")).ToArray();
                    }
                    else
                    {
                        throw new ArgumentException(BatchResponse.Code300 + " : System folder not found");
                    }

                }
                //สำหรับ re-run โดยระบุชื่อ file
                else
                {
                    strSFTPFullPath = string.Format("{0}/{1}/{2}/{3}", AppConfig.CreateActivityLogSFTPSourcePath, AppConfig.BulkCreateActivityLogRootPath, systemCode, AppConfig.BulkCreateActivityLogRequestPath);
                    _logger.InfoFormat("I:--Biz-SFTPProcessing:{0}-{1}--", systemCode, strSFTPFullPath);
                    strErrorStep = "Get file from sftp-" + strSFTPFullPath;
                    if (sFTP.Exists(strSFTPFullPath))
                    {
                        objFileList = sFTP.ListDirectory(strSFTPFullPath).ToArray().Where(x => x.Name.ToUpperInvariant().Contains(fileName.ToUpperInvariant())).ToArray();
                    }
                    else
                    {
                        throw new ArgumentException(BatchResponse.Code300 + " : System folder not found");
                    }
                }
                if (sFTP.IsConnected) sFTP.Disconnect();
                //================================
                //================================
                //================================
                if (objFileList.Any())
                {
                    //create folder follow system code                                
                    strLocalFullPath = string.Format("{0}\\sFTPFile\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, systemCode);
                    if (!Directory.Exists(strLocalFullPath))
                    {
                        _logger.InfoFormat("I:--Biz-SFTPProcessing:{0}-CreateFolder:{1}--", systemCode, strLocalFullPath);
                        strErrorStep = "Create Local Folder-" + systemCode;
                        Directory.CreateDirectory(strLocalFullPath);
                    }
                    //================================
                    foreach(var fl in objFileList)
                    {
                        strFileName = ((Renci.SshNet.Sftp.SftpFile)fl).Name;
                        strURLRerun = string.Format("BulkCreateActivityLogRerun?systemCode={0}&fileName={1}", systemCode, strFileName);
                        dBatchLogID = database.SetBatchLogStartProcess(dBatchID, systemCode, SystemName.BulkCreateActivityLog, strFileName, strURLRerun);
                        _logger.InfoFormat("I:--Biz-SFTPProcessing:{0}--GetFile({1})--Start--", systemCode, strFileName);
                        //--------------------------------------
                        if (!sFTP.IsConnected) sFTP.Connect();
                        //copy file จาก sftp มาไว้ที่ local path
                        using (Stream fileStream = File.OpenWrite(string.Format("{0}\\{1}", strLocalFullPath, strFileName)))
                        {
                            sFTP.DownloadFile(string.Format("{0}/{1}", strSFTPFullPath, strFileName), fileStream);
                        }
                        if (sFTP.IsConnected) sFTP.Disconnect();
                        //ถ้ามี filename มาแสดงว่าเกิดจากการ re-run ไม่ต้องย้าย file แล้ว
                        if (string.IsNullOrEmpty(fileName))
                        {
                            if (!sFTP.IsConnected) sFTP.Connect();
                            //สร้าง folder Request กรณีที่ยังไม่มี    
                            if (!sFTP.Exists(string.Format("{0}/{1}", strSFTPFullPath, AppConfig.BulkCreateActivityLogRequestPath)))
                            {
                                sFTP.CreateDirectory(string.Format("{0}/{1}", strSFTPFullPath, AppConfig.BulkCreateActivityLogRequestPath));
                            }
                            //Copy souce file ไปไว้ที่ folder Request
                            using (Stream fileStream = File.OpenRead(string.Format("{0}\\{1}", strLocalFullPath, strFileName)))
                            {
                                sFTP.UploadFile(fileStream, string.Format("{0}/{1}/{2}", strSFTPFullPath, AppConfig.BulkCreateActivityLogRequestPath, strFileName));
                            }
                            //--------------------------------------
                            //Delete original file
                            sFTP.DeleteFile(string.Format("{0}/{1}", strSFTPFullPath, strFileName));
                            if (sFTP.IsConnected) sFTP.Disconnect();
                        }
                        //--------------------------------------
                        DataProcessing(systemCode, strFileName, string.Format("{0}\\{1}", strLocalFullPath, strFileName));
                        //--------------------------------------
                        var batchMapping = LoadBatchSystemMapping(systemCode, Batch.BulkCreateActivityLog);
                        if (batchMapping != null && batchMapping.IS_BATCH_RESULT)
                        {
                            //Copy Response file to sfpt
                            string strResultFileName = CreateOutputFile(strLocalFullPath, strFileName);
                            if (!sFTP.IsConnected) sFTP.Connect();
                            using (Stream fileStream = File.OpenRead(string.Format("{0}\\{1}", strLocalFullPath, strResultFileName)))
                            {
                                sFTP.UploadFile(fileStream, string.Format("{0}/{1}", strSFTPFullPath.Replace(string.Format("/{0}", AppConfig.BulkCreateActivityLogRequestPath), ""), strResultFileName));
                            }
                            if (sFTP.IsConnected) sFTP.Disconnect();
                        }
                        //--------------------------------------
                        _logger.InfoFormat("O:--Biz-SFTPProcessing:{0}--End--", systemCode);
                    }
                }
                else
                {
                    throw new ArgumentException(BatchResponse.Code300 + " : File not found");
                }
                return true;
            }
            catch(Exception ex)
            {
                if(sFTP.IsConnected) sFTP.Disconnect();
                _logger.InfoFormat("E:--Biz-SFTPProcessing:{0}({1})--End--", systemCode, ExceptionService.GetMessage(ex));
                _logger.Error("E:--Biz-SFTPProcessing--", ex);
                iException = true;
                SetErrorDetail(ExceptionService.GetToString(ex));
                dBatchLogID = database.SetBatchLogStartProcess(dBatchID, systemCode, SystemName.BulkCreateActivityLog, string.Empty, string.Empty);
                database.SetBatchLogEndProcess(dBatchID, dBatchLogID, systemCode, string.Empty, DateTime.Now, string.Empty, string.Format("{0}{1}{2}", strErrorStep, Environment.NewLine, strBuilder), 0, 0, 0, 0);
                SendMailService.SendMail(string.Format(AppConfig.BulkCreateActivityLogEmailSubjectError, systemCode), CreateTemplateEmail(systemCode), string.Empty);
                return false;
            }
        }
        private void DataProcessing(string systemCode, string fileName, string localFullFileName)
        {
            _logger.InfoFormat("I:--Biz-SFTP-DataProcessing:{0}-{1}--Start--", systemCode, fileName);
            strBuilder = null;
            iTotalHeader = 0;
            iTotalDetail = 0;
            iTotalComplete = 0;
            iTotalFail = 0;
            try
            {
                strErrorStep = "Read Data File-" + localFullFileName;
                string strAllData = File.ReadAllText(localFullFileName, Encoding.UTF8);
                strErrorStep = "Convert Data from JSON to object";
                JSONProcessing(strAllData, systemCode);
                _logger.InfoFormat("O:--Biz-SFTP-DataProcessing:{0}-{1}--End--", systemCode, fileName);
            }
            catch (Exception ex)
            {
                _logger.InfoFormat("E:--Biz-SFTP-DataProcessing:{0}-{1}-({2})--End--", systemCode, fileName, ExceptionService.GetMessage(ex));
                _logger.Error("E:--Biz-SFTP-DataProcessing--", ex);
                iException = true;
                SetErrorDetail(ExceptionService.GetToString(ex));
                database.SetBatchLogEndProcess(dBatchID, dBatchLogID, systemCode, string.Empty, DateTime.Now, string.Empty, ExceptionService.GetMessage(ex), iTotalHeader, iTotalDetail, iTotalComplete, iTotalFail);
                SendMailService.SendMail(string.Format(AppConfig.BulkCreateActivityLogEmailSubjectError, systemCode), CreateTemplateEmail(systemCode), string.Empty);
            }
        }
        #endregion

        #region For HttpCreateActivityLog --> POST
        public CAS_SYSTEM LoadSystemById(string systemCode)
        {
            return database.LoadSystemCodeById(systemCode);
        }
        public CAR_BATCH LoadBatchMasterByCode(string batchCode)
        {
            return database.LoadBatchMasterByCode(batchCode);
        }
        public CAR_BATCH LoadBatchMasterById(decimal batchId)
        {
            return database.LoadBatchMasterById(batchId);
        }
        public CAR_BATCH_LOG LoadBatchLogByID(decimal batchLogId)
        {
            return database.LoadBatchLogByID(batchLogId);
        }
        public CAR_BATCH_SYSTEM_MAPPING LoadBatchSystemMapping(string systemCode, string batchCode)
        {
            return database.LoadBatchSystemMapping(systemCode, batchCode);
        }
        public BatchResponseData ValidateServiceRequest(HTTPCreateActivityLogRequest request)
        {
            BatchResponseData response = new BatchResponseData();

            if (IsNullOrEmpty(request.SystemCode))
            {
                response.ResponseCode = BatchResponse.Code101;
                response.ResponseMessage = string.Format("{0} : {1}", BatchResponse.Message101, "System Code");
            }
            else if (IsNullOrEmpty(request.ServiceName))
            {
                response.ResponseCode = BatchResponse.Code101;
                response.ResponseMessage = string.Format("{0} : {1}", BatchResponse.Message101, "Service Name");
            }
            else if (IsNullOrEmpty(request.DataDate))
            {
                response.ResponseCode = BatchResponse.Code101;
                response.ResponseMessage = string.Format("{0} : {1}", BatchResponse.Message101, "Data Date");
            }
            else if (IsNullOrEmpty(request.Path))
            {
                response.ResponseCode = BatchResponse.Code101;
                response.ResponseMessage = string.Format("{0} : {1}", BatchResponse.Message101, "Path");
            }
            else if (!database.CheckExistingSystemCode(request.SystemCode.Trim()))
            {
                response.ResponseCode = BatchResponse.Code202;
                response.ResponseMessage = BatchResponse.Message202;
            }
            else if (request.ServiceName.Trim() != SystemName.HttpCreateActivityLog)
            {
                response.ResponseCode = BatchResponse.Code103;
                response.ResponseMessage = string.Format("{0} : {1}", BatchResponse.Message103, "Service Name");
            }
            else if (request.DataDate.Length != 8)
            {
                response.ResponseCode = BatchResponse.Code103;
                response.ResponseMessage = string.Format("{0} : {1}", BatchResponse.Message103, "Data Date(yyyyMMdd)");
            }
            return response;
        }
        public string HTTPCreateActivityLog(HTTPCreateActivityLogRequest request, string json)
        {
            _logger.Info("I:--Biz-HTTPCreateActivityLog--Start--");
            strFileName = string.Empty;
            strBuilder = null;
            iTotalHeader = 0;
            iTotalDetail = 0;
            iTotalComplete = 0;
            iTotalFail = 0;
            try
            {
                string strReRunURL = string.Format("HttpCreateActivityLogRerun?system={0}&serviceName={1}&dataDate={2}&path={3}&getResult={4}", request.SystemCode, request.ServiceName, request.DataDate, request.Path, request.GetResult);
                var batchMaster = database.LoadBatchMasterByCode(Batch.HttpCreateActivityLog);
                dBatchID = batchMaster.BATCH_ID;
                dBatchLogID = database.SetBatchLogStartProcess(dBatchID, request.SystemCode, SystemName.HttpCreateActivityLog, string.Empty, strReRunURL);
                //Return ReferenceNo ไปให้หน้าบ้านเพื่อตอบกลับ source system
                return JSONProcessing(json, request.SystemCode);
            }
            catch (Exception ex)
            {
                _logger.Error("E:--Biz-HTTPCreateActivityLog--", ex);
                iException = true;
                SetErrorDetail(ExceptionService.GetToString(ex));
                database.SetBatchLogEndProcess(dBatchID, dBatchLogID, request.SystemCode, string.Empty, DateTime.Now, string.Empty, ExceptionService.GetMessage(ex), iTotalHeader, iTotalDetail, iTotalComplete, iTotalFail);
                SendMailService.SendMail(string.Format(AppConfig.BulkCreateActivityLogEmailSubjectError, request.SystemCode), CreateTemplateEmail(request.SystemCode), string.Empty);
            }
            _logger.Info("O:--Biz-HTTPCreateActivityLog--End--");
            return string.Empty;
        }
        public BatchCreateActivityLogResponse CreateActivityLogResult(string systemCode, string serviceName, string dataDate, string refNo)
        {
            _logger.Info("I:--Biz-CreateActivityLogResult--Start--");
            var logHeader = database.CreateActivityLogResultHeader(Batch.HttpCreateActivityLog, systemCode, dataDate, refNo);
            BatchCreateActivityLogResponse log = new BatchCreateActivityLogResponse();
            if(logHeader != null)
            {
                var LogDetail = database.CreateActivityLogResultDetail(logHeader.BATCH_LOG_ID);
                log.Header = new BatchLogServiceHeaderResponse
                {
                    ReferenceCode = logHeader.REFERENCE_CODE,
                    FileName = string.Empty,
                    CreateDate = logHeader.TRANSACTION_DATE != null ? Convert.ToDateTime(logHeader.TRANSACTION_DATE).ToString(DateFormat.JsonDateFormat) : string.Empty,
                    CurrentSequence = 0,
                    TotalSequence = 0,
                    TotalRecord = logHeader.TOTAL_HEADER,
                    SystemCode = systemCode,
                    SecurityKey = string.Empty
                };
                log.Body = LogDetail;
            }
            else
            {
                _logger.Info("I:--Biz-CreateActivityLogResult--Header data not found--");
            }            
            _logger.Info("O:--Biz-CreateActivityLogResult--End--");
            _logger.Debug(log.SerializeObject());
            return log;
        }
        #endregion

        #region HttpClient
        //public void CallGetHttpClientAysnc(string requestUri)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        var result = client.GetAsync(requestUri).Result;
        //    }
        //}
        public void CallGetHttpClient(string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.GetAsync(requestUri).Result;
            }
        }
        #endregion

        #region For CAR BatchMonitoring
        public DataTable LoadAllBatchMaster()
        {
            return database.LoadAllBatchMaster();
        }
        public DataTable LoadBatchLog(decimal batchId, string batchDate)
        {
            return database.LoadBatchLog(batchId, batchDate);
        }
        public DataTable LoadBatchLogDetail(decimal batchLogId)
        {
            return database.LoadBatchLogDetail(batchLogId);
        }
        public void SetBatchStartProcess(string batchCode)
        {
            database.SetBatchStartProcess(batchCode);
        }
        public void SetBatchEndProcess(string batchCode, string batchStatus)
        {
            database.SetBatchEndProcess(batchCode, batchStatus);
        }
        #endregion

        #region Private method
        private string ValidateHeader(BatchLogServiceHeader header, string systemCode)
        {
            _logger.Info("I:--Biz-ValidateHeader--Start--");
            string strErrorDetail = string.Empty;
            string strAuthErrorCode;
            if (header != null)
            {
                if (string.IsNullOrEmpty(header.ReferenceCode)) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code101, BatchResponse.Message101, "ReferenceCode");
                else if (!string.IsNullOrEmpty(strFileName.Trim()) && string.IsNullOrEmpty(header.FileName)) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code101, BatchResponse.Message101, "FileName");
                else if (header.CreateDate == DateTime.MinValue) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code101, BatchResponse.Message101, "CreateDate"); 
                //else if (header.CurrentSequence <= 0) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code101, BatchResponse.Message101, "CurrentSequence");
                //else if (header.TotalSequence <= 0) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code101, BatchResponse.Message101, "TotalSequence");
                else if (header.TotalRecord > AppConfig.MaxRowPerRequest) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code401, BatchResponse.Message401, header.TotalRecord);
                else if (string.IsNullOrEmpty(header.SystemCode)) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code101, BatchResponse.Message101, "SystemCode");
                else if(header.FileName.Trim() != strFileName.Trim()) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code103, BatchResponse.Message103, "FileName mismatch"); 
                else if (header.SystemCode.Trim() != systemCode.Trim()) strErrorDetail = string.Format("{0} : {1}-{2}", BatchResponse.Code103, BatchResponse.Message103, "SystemCode mismatch");
                else
                {
                    LogServiceHeader logHeader = new LogServiceHeader();
                    logHeader.SystemCode = header.SystemCode;
                    logHeader.SecurityKey = header.SecurityKey;
                    
                    if (!authenBusiness.CheckAuth(logHeader, out strAuthErrorCode))
                    {
                        strErrorDetail = string.Format("{0}:{1}", strAuthErrorCode, authenBusiness.ErrorMessage);
                    }
                }
            }
            else
            {
                strErrorDetail = string.Format("{0}:{1}", BatchResponse.Code101, "Header required");
            }
            _logger.Info("O:--Biz-ValidateHeader--End--");
            return strErrorDetail;
        }        
        private string JSONProcessing(string jSon, string systemCode)
        {
            _logger.Info("I:--Biz-JSONProcessing--Start--");
            iException = false;
            string strMailHeader = string.Empty;
            string strErrorHeader = string.Empty;
            string strDetailError = string.Empty;
            BatchCreateActivityLogReqest req = null;
            var jsonSerialiser = new JavaScriptSerializer();
            try
            {
                req = jsonSerialiser.Deserialize<BatchCreateActivityLogReqest>(jSon);
                _logger.Debug(req.SerializeObject());
                BatchLogServiceHeaderResponse header = new BatchLogServiceHeaderResponse();
                header.ReferenceCode = req.Header.ReferenceCode;
                header.FileName = req.Header.FileName;
                header.CreateDate = req.Header.CreateDate != null ? req.Header.CreateDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : string.Empty;
                header.CurrentSequence = req.Header.CurrentSequence;
                header.TotalSequence = req.Header.TotalSequence;
                header.TotalRecord = req.Header.TotalRecord;
                header.SystemCode = req.Header.SystemCode;
                header.SecurityKey = string.Empty; //req.Header.SecurityKey;
                responseActLog.Header = header;
                responseActLog.Body = new List<BatchCreateActivityLogBody>();
            }
            catch(Exception ex)
            {
                _logger.Error("E:--Biz-JSONProcessing--", ex);
                CreateResponseMessage(string.Empty, string.Empty, BatchResponse.Code402, BatchResponse.Message402);
                throw new ArgumentException(string.Format("{0} : {1}-[{2}]", BatchResponse.Code402, BatchResponse.Message402, strFileName));
            }

            strErrorStep = "Validate Header";
            _logger.Info("I:--Biz-JSONProcessing--ValidateHeader--");
            strErrorHeader = ValidateHeader(req.Header, systemCode);
            if (string.IsNullOrEmpty(strErrorHeader))
            {
                iTotalHeader = req.Header.TotalRecord;
                iTotalDetail = req.Body == null ? 0 : req.Body.Count();
                if (iTotalHeader != iTotalDetail)
                {
                    CreateResponseMessage(string.Empty, string.Empty, BatchResponse.Code401, BatchResponse.Code401);
                    throw new ArgumentException(string.Format("{0} : {1}", BatchResponse.Code401, BatchResponse.Message401));
                }

                if(req.Body != null && req.Body.Any())
                {
                    //หา ReferenceNo และ SystemCode ทีมีอยู่แล้ว
                    strErrorStep = "Check Existring Reference No.";
                    var lstExisting = database.CheckExistActivityLog(req.Body.Select(x => x.ReferenceNo).ToList(), systemCode);
                    foreach (var l in req.Body)
                    {
                        if (lstExisting.Where(x => x == l.ReferenceNo).Any())
                        {
                            //Duplicate Data
                            strDetailError = string.Format("{0} : {1}-{2}", BatchResponse.Code403, BatchResponse.Message403, l.ReferenceNo);
                            SetErrorDetail(string.Format("{0}{1}", strDetailError, Environment.NewLine));
                            database.SaveBatchLogDetail(dBatchLogID, l.ReferenceNo, l.ChannelID, BatchResponse.Code403, BatchResponse.Message403);
                            CreateResponseMessage(l.ReferenceNo, l.ChannelID, BatchResponse.Code403, BatchResponse.Message403);
                            iTotalFail++;
                        }
                        else
                        {
                            try
                            {
                                //Complete
                                database.CreateActivityLog(l, systemCode);
                                strDetailError = string.Format("{0} : {1}-{2}", BatchResponse.Code000, BatchResponse.Message000, l.ReferenceNo);
                                SetErrorDetail(string.Format("{0}{1}", strDetailError, Environment.NewLine));
                                database.SaveBatchLogDetail(dBatchLogID, l.ReferenceNo, l.ChannelID, BatchResponse.Code000, BatchResponse.Message000);
                                CreateResponseMessage(l.ReferenceNo, l.ChannelID, BatchResponse.Code000, BatchResponse.Message000);
                                iTotalComplete++;
                            }
                            catch (Exception ed)
                            {
                                //Invalid Data
                                strDetailError = string.Format("{0} : {1}-{2}-{3}", BatchResponse.Code103, BatchResponse.Message103, l.ReferenceNo, ExceptionService.GetMessage(ed));
                                SetErrorDetail(string.Format("{0}{1}", strDetailError, Environment.NewLine));
                                database.SaveBatchLogDetail(dBatchLogID, l.ReferenceNo, l.ChannelID, BatchResponse.Code103, BatchResponse.Message103);
                                CreateResponseMessage(l.ReferenceNo, l.ChannelID, BatchResponse.Code103, BatchResponse.Message103);
                                iTotalFail++;
                            }
                        }
                    }
                }
                else
                {
                    iTotalComplete = 0;
                    iTotalDetail = 0;
                    iTotalFail = 0;
                    iTotalHeader = 0;
                }                

                strErrorStep = "Data Processing Complete";
                database.SetBatchLogEndProcess(dBatchID, dBatchLogID, systemCode, req.Header.ReferenceCode, req.Header.CreateDate, req.Header.CreateDate.Value.ToString("yyyyMMdd"), string.Empty, iTotalHeader, iTotalFail, iTotalComplete, iTotalFail);

                if (iTotalComplete >= 0 && iTotalFail == 0) strMailHeader = AppConfig.BulkCreateActivityLogEmailSubjectComplete;
                else if (iTotalComplete > 0 && iTotalFail > 0) strMailHeader = AppConfig.BulkCreateActivityLogEmailSubjectSemiComplete;
                else strMailHeader = AppConfig.BulkCreateActivityLogEmailSubjectError;
                //ส่งเมล์สรุปผลการทำงานทั้งหมด
                SendMailService.SendMail(string.Format(strMailHeader, systemCode), CreateTemplateEmail(systemCode), strBuilder.ToString());
                _logger.Info("O:--Biz-JSONProcessing--End--");
                return req.Header.ReferenceCode;
            }
            else
            {
                _logger.InfoFormat("O--Biz-JSONProcessing--ValidateHeader--Error:{0}", strErrorHeader);
                string[] strError = strErrorHeader.Split(':');
                CreateResponseMessage(string.Empty, string.Empty, strError[0].Trim(), strError[1].Trim());
                throw new ArgumentException(string.Format("{0} - [{1}]", strErrorHeader, strFileName));
            }
        }
        private string CreateTemplateEmail(string systemCode)
        {
            StringBuilder builder = new StringBuilder();
            string detail;
            using (StreamReader reader = new StreamReader(string.Format("{0}\\Template\\Mail\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, "BatchCreateActivityLogEmailTemplate.html")))
            {
                builder.Append(reader.ReadToEnd());
                if (!iException)
                {
                    if (iTotalComplete > 0 && iTotalFail == 0)
                    {
                        builder.Replace("{MAIL_SUBJECT}", string.Format(AppConfig.BulkCreateActivityLogEmailSubjectComplete, systemCode));
                    }
                    else if (iTotalComplete > 0 && iTotalFail > 0)
                    {
                        builder.Replace("{MAIL_SUBJECT}", string.Format(AppConfig.BulkCreateActivityLogEmailSubjectSemiComplete, systemCode));
                    }
                    else
                    {
                        builder.Replace("{MAIL_SUBJECT}", string.Format(AppConfig.BulkCreateActivityLogEmailSubjectError, systemCode));
                    }

                    detail = string.Format(@"<div style='text-align: left; font-family: tahoma; font-size: 13px; padding: 10px 50px;'>
                                                    <div>System Code : {0}</div>
                                                    <div>Created Date : {1}</div>
                                                    <div>File Name : {2}</div>
                                                    <div>Detail</div>
                                                    <div style='padding: 0px 20px;'>
                                                        <div>Total Header : {3}</div>
                                                        <div>Total Detail : {4}</div>
                                                        <div>Total Complete : {5}</div>
                                                        <div>Total Fail : {6}</div>
                                                    </div>
                                                </div>", systemCode
                                                        , DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                                                        , strFileName
                                                        , iTotalHeader
                                                        , iTotalDetail
                                                        , iTotalComplete
                                                        , iTotalFail
                                                        );
                }
                else
                {
                    builder.Replace("{MAIL_SUBJECT}", string.Format(AppConfig.BulkCreateActivityLogEmailSubjectError, string.IsNullOrEmpty(systemCode) ? "ทุกระบบ" : systemCode));

                    detail = string.Format(@"<div style='text-align: left; font-family: tahoma; font-size: 13px; padding: 10px 50px;'>
                                                <div>Created Date : {0}</div>
                                                <div>Step : {1}</div>
                                                <div>Detail : {2}</div>
                                            </div>",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                                                    , strErrorStep
                                                    , strBuilder);
                }
                builder.Replace("{DETAIL}", detail);
            }
            return builder.ToString();
        }
        private bool IsNullOrEmpty(object obj)
        {
            if(obj == null)
            {
                return true;
            }
            return string.IsNullOrWhiteSpace(obj.ToString().Trim());
        }
        private void CreateResponseMessage(string referenceNo, string channelId, string responseCode, string responseMessage)
        {
            BatchCreateActivityLogBody body = new BatchCreateActivityLogBody();
            BatchResponseData status = new BatchResponseData();
            status.ResponseCode = responseCode;
            status.ResponseMessage = responseMessage;
            body.ResponseStatus = status;
            body.ReferenceNo = referenceNo;
            body.ChannelId = channelId;
            responseActLog.Body.Add(body);
        }
        private string CreateOutputFile(string localPath, string fileName)
        {
            string[] strArrInputFileName = fileName.Split('.');
            string strExt = strArrInputFileName[strArrInputFileName.Length - 1];
            string strOutputFileName = string.Format("{0}_Response.{1}", fileName.Replace("."+strExt, ""), strExt);
            var jsonSerialiser = new JavaScriptSerializer();
            File.WriteAllText(string.Format("{0}\\{1}", localPath, strOutputFileName), jsonSerialiser.Serialize(responseActLog)); 
            return strOutputFileName;
        }
        private void SetErrorDetail(string ErrorDetail)
        {
            strBuilder.Append(ErrorDetail);
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
                    _db = null;
                    _auth = null;
                    _resp = null;
                    _stringbuilder = null;
                    if (_sftp.IsConnected)
                    {
                        _sftp.Disconnect();
                    }
                    _sftp.Dispose();
                    _sftp = null;
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
