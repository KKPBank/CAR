using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using KKCAR.Service.Messages.Batch;
using KKCAR.Service.Messages.Common;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using log4net;
using KKCAR.Business;
using KKCAR.Common.Utilities;
using KKCAR.Entity;


namespace KKCAR.WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "KKCARBatchInsertStatusText" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select KKCARBatchInsertStatusText.svc or KKCARBatchInsertStatusText.svc.cs at the Solution Explorer and start debugging.
    public class KKCARBatchInsertStatusText : IKKCARBatchInsertStatusText
    {
        private readonly ILog _logger;
        private IFileFacade _fileFacade;
        private IBatchCARInstartStatusTextFacade _batchFacade;
        private LogMessageBuilder _logMsg = new LogMessageBuilder();

        #region "Properties"

        private long _elapsedTime;
        private long _seqCounter;
        private object sync = new Object();
        private System.Diagnostics.Stopwatch _stopwatch;
        private void IncrementSeqCounter()
        {
            Interlocked.Increment(ref _seqCounter);
        }
        #endregion

        #region "Constructor"

        public KKCARBatchInsertStatusText()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                // Set logfile name and application name variables
                GlobalContext.Properties["ApplicationCode"] = "KKCARWS";
                GlobalContext.Properties["ServerName"] = Environment.MachineName;
                _logger = LogManager.GetLogger(typeof(KKCARBatchStatusService));
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occur:\n", ex);
            }
        }
        #endregion

        public TaskInsertStatusResponse BatchInsertStatusByText()
        {
            ThreadContext.Properties["EventClass"] = ApplicationHelpers.GetCurrentMethod(1);
            ThreadContext.Properties["RemoteAddress"] = ApplicationHelpers.GetClientIP();

            var logDetail = string.Empty;
            var logErrCode = string.Empty;
            _stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _logger.Debug("I:-- Start Cron Job --:--Batch InsertStatus by Excel--");

            DateTime schedDateTime = DateTime.Now;
            TaskInsertStatusResponse taskResponse = new TaskInsertStatusResponse(schedDateTime);

            try
            {
                #region "Batch Setting"
                string dataDate = DateTime.Now.FormatDateTime("yyyyMMdd");
                //string batchFilePrefix = !string.IsNullOrWhiteSpace(fileName) ? fileName.ExtractNamePrefix() : WebConfig.GetBatchFilePrefix();

                #endregion

                //1. Read files from SFTP
                LoadFileListViaSFTP();

                //2. เก็บชื่อไฟล์ลง DB เพื่อเตรียมทำการ Process กรณีมีไฟล์อยู่แล้ว ก็ไม่ต้องทำซ้ำ
                _batchFacade = new BatchCARInstartStatusTextFacade();
                List<BatchCARInsertStatusTextFileEntity> fileList = _batchFacade.GetFileHistoryList();

                //3. เช็ค Format TextFile ทุก Record
                foreach (BatchCARInsertStatusTextFileEntity file in fileList)
                {
                    _logger.Info(_logMsg.Clear().Add("Validate Text Format", file.fileName).ToInputLogString());
                    ValidateTextfileEntity validTextFormat = _batchFacade.ValidateTextFileFormat(file.filePath);
                    if (validTextFormat.IsValid == false)
                    {
                        _logger.Error(_logMsg.Clear().Add("Invalid Textfile", file.fileName).Add("ErrorMessage", validTextFormat.ErrorMessage).ToInputLogString());
                        _batchFacade.UpdateFileStatus(file.carSysStatusCbsFileId, Constants.BatchCARProcessStatus.Fail, "Invalid Textfile " + file.fileName + Environment.NewLine + validTextFormat.ErrorMessage);
                        break;
                    }
                }

                //4. Process Text File ตาม Requirement
                if (fileList != null && fileList.Any())
                {
                    List<CARInsertStatusTextfileDataList> dataList = new List<CARInsertStatusTextfileDataList>();
                    Task.Factory.StartNew(() => Parallel.ForEach(fileList, new ParallelOptions { MaxDegreeOfParallelism = WebConfig.GetTotalCountToProcess() },
                            file =>
                            {
                                lock (sync)
                                {
                                    this.IncrementSeqCounter();
                                    BulkInsertStatusRequest reqBatch = _batchFacade.LoadBatchCARInsertStatusTextData(file.filePath);
                                    var reqList = _batchFacade.PopulateBatchCARInsertStatusTextRequest(reqBatch);

                                    var taskResult = _batchFacade.BatchCARInsertStatusText(reqBatch, reqList, dataDate);
                                    taskResponse.TaskResults.Add(taskResult);
                                    _logger.Info(_logMsg.Clear().SetPrefixMsg("Read Data From Text File").Add("FileName", file).Add("Sequence", _seqCounter).ToInputLogString());

                                    CARInsertStatusTextfileDataList data = new CARInsertStatusTextfileDataList();
                                    data.TextFileDataList = reqBatch;
                                    data.ExtSysStatusCbsFileId = file.carSysStatusCbsFileId.Value;
                                    dataList.Add(data);
                                }
                            })).Wait();


                    //5. นำข้อมูลใน Text File เก็บลงใน Archives
                    if (dataList.Count > 0)
                    {
                        _logger.Info(_logMsg.Clear().Add("InsertArchiveData", "TABLE : CAR_SYS_STATUS_CBS_FILE_DATA").ToInputLogString());
                        if (_batchFacade.ArchiveExtSysStatusCbsFileData(dataList) == true)
                        {
                            Task.Factory.StartNew(() => Parallel.ForEach(fileList, new ParallelOptions { MaxDegreeOfParallelism = WebConfig.GetTotalCountToProcess() },
                                file => {
                                    lock (sync)
                                    {
                                        _logger.Info(_logMsg.Clear().SetPrefixMsg("MoveBatchFileToArchive").Add("FileName", file.fileName).ToInputLogString());
                                        if (MoveBatchFileToArchive(file.filePath) == true)
                                        {
                                            _logger.Info(_logMsg.Clear().SetPrefixMsg("Update File Status Success").Add("FileName", file.fileName).ToInputLogString());
                                            _batchFacade.UpdateFileStatus(file.carSysStatusCbsFileId, Constants.BatchCARProcessStatus.Success, "");
                                        }
                                    }
                                })).Wait();
                        }

                    }
                }

            Outer:
                if (taskResponse.TaskResults.Count == 0)
                {
                    logErrCode = Constants.ErrorCode.KKCAR_SUCCESS;
                    logDetail = "No files are found in SFTP";
                }
                if (taskResponse.TaskResults.Any(x => x.NumOfError > 0))
                {
                    logErrCode = Constants.ErrorCode.KKCAR_ERR103;
                    logDetail = "Invalid Data";
                }

                _logger.DebugFormat("-- XMLResponse --\n{0}", taskResponse.SerializeObject());
                _stopwatch.Stop();
                _elapsedTime = _stopwatch.ElapsedMilliseconds;
                _logger.DebugFormat("O:--Finish--:ElapsedMilliseconds/{0}", _elapsedTime);

                taskResponse.ElapsedTime = _elapsedTime;
                taskResponse.ResponseStatus = new ResponseStatus
                {
                    ResponseCode = logErrCode,
                    ResponseMessage = logDetail
                };

                _logger.InfoFormat("O:--SUCCESS--:--Bulk Insert Status--");
                return taskResponse;
            }
            catch (Exception ex)
            {
                return taskResponse;
            }
        }

        #region BatchCARInsertStatusText
        private void LoadFileListViaSFTP()
        {
            string fiPrefix = WebConfig.GetBatchCARInsertStatusFilePrefix();   //BchCARInsertSts_
            string NameYesterday = fiPrefix + DateTime.Now.AddDays(0 - WebConfig.GetBatchCARInsertStatusIntervalDay()).ToString("yyyyMMdd");
            string NameTomorrow = fiPrefix + DateTime.Now.AddDays(WebConfig.GetBatchCARInsertStatusIntervalDay()).ToString("yyyyMMdd");


            _fileFacade = new FileFacade();
            string localPath = WebConfig.GetBatchCARInsertStatusPathImport();
            //if (Directory.Exists(localPath) == false)
            //    Directory.CreateDirectory(localPath);

            _fileFacade.DownloadBatchFilesViaFTP(localPath, fiPrefix, NameYesterday.Length);
        }

        private bool MoveBatchFileToArchive(string filePath)
        {
            bool ret = false;
            try
            {
                FileInfo fInfo = new FileInfo(filePath);
                File.Copy(filePath, WebConfig.GetBatchCARInsertStatusPathArchives() + "\\" + fInfo.Name, true);

                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
                ret = true;
            }
            catch (Exception ex)
            {
                //throw ex;
                _logger.Info(_logMsg.Clear().SetPrefixMsg("MoveBatchFileToArchive").Add("FilePath", filePath).ToInputLogString());
            }

            return ret;
        }
        #endregion
    }
}
