using KKCAR.Common.Resources;
using KKCAR.Common.Utilities;
using KKCAR.Data.DataAccess;
using KKCAR.Entity;
using KKCAR.Service.Messages;
using KKCAR.Service.Messages.Batch;
using KKCAR.Service.Messages.Common;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

///<summary>
/// Class Name : StatusFacade
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
    public class StatusFacade : IStatusFacade
    {
        const string Delimeter = "\n";
        private IFileFacade _fileFacade;
        private IBatchCARInstartStatusTextFacade _batchFacade;
        private ICommonFacade _commonFacade;
        private KKCARContextContainer _context;
        private IAuthenticationFacade _authFacade;
        private IStatusDataAccess _statusDataAccess;
        private LogMessageBuilder _logMsg = new LogMessageBuilder();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StatusFacade));

        public StatusFacade()
        {
        }

        public InsertStatusResponse InsertStatus(InsertStatusRequest request, bool skipValidate)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Logger.Info(_logMsg.Clear().SetPrefixMsg("Call KKCARStatusService.InsertStatus").ToInputLogString());
            Logger.Debug("I:--START--:--KKCARStatusService.InsertStatus--");

            bool valid = true;
            var header = request.Header;
            InsertStatusResponse response = new InsertStatusResponse();

            #region "Validate Header"

            if (header != null)
            {
                _authFacade = new AuthenticationFacade();

                response.Header = new Service.Messages.Common.Header
                {
                    ChannelID = header.ChannelID,
                    ReferenceNo = header.ReferenceNo,
                    SystemCode = header.SystemCode,
                    ServiceName = header.ServiceName,
                    TransactionDateTime = header.TransactionDateTime
                };

                if (!skipValidate)
                {
                    response.ResponseStatus = _authFacade.VerifyServiceRequest(header, out valid);
                }
            }

            #endregion

            request.Header.SecurityKey = null; // Override security key
            Logger.DebugFormat("-- XMLRequest --\n{0}", request.SerializeObject());

            #region "Call Validator"

            if (!valid)
            {
                goto Outer;
            }
            else
            {
                _commonFacade = new CommonFacade();
                ValidationContext vc = new ValidationContext(request, null, null);
                var validationResults = new List<ValidationResult>();
                valid = Validator.TryValidateObject(request, vc, validationResults, true);

                if (!string.IsNullOrWhiteSpace(request.SubscriptionID) && string.IsNullOrWhiteSpace(request.SubscriptionCardType))
                {
                    valid = false;
                    validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_SubsCardType)));
                }
                if (string.IsNullOrWhiteSpace(request.SubscriptionID) && !string.IsNullOrWhiteSpace(request.SubscriptionCardType))
                {
                    valid = false;
                    validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_SubsID)));
                }
                if (!string.IsNullOrWhiteSpace(request.SubscriptionID) && !string.IsNullOrWhiteSpace(request.SubscriptionCardType))
                {
                    if (!_commonFacade.IsSubsCardTypeExists(request.SubscriptionCardType))
                    {
                        valid = false;
                        validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidData, Resource.Lbl_SubsCardType)));
                    }
                }

                if (!_commonFacade.IsSubsCusTypeExists(request.SubscriptionCusType))
                {
                    valid = false;
                    validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidData, Resource.Lbl_SubsCusType)));
                }
                
                if (!string.IsNullOrWhiteSpace(request.OwnerSystemId) && !string.IsNullOrWhiteSpace(request.OwnerSystemCode))
                {
                    if (!_commonFacade.IsSystemExists(request.OwnerSystemCode))
                    {
                        valid = false;
                        validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidData, Resource.Lbl_OwnerSystemCode)));
                    }
                }

                if (string.IsNullOrWhiteSpace(request.RefSystemId) && !string.IsNullOrWhiteSpace(request.RefSystemCode))
                {
                    valid = false;
                    validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_RefSystemId)));
                }
                if (!string.IsNullOrWhiteSpace(request.RefSystemId) && string.IsNullOrWhiteSpace(request.RefSystemCode))
                {
                    valid = false;
                    validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_RequiredField, Resource.Lbl_RefSystemCode)));
                }
                if (!string.IsNullOrWhiteSpace(request.RefSystemId) && !string.IsNullOrWhiteSpace(request.RefSystemCode))
                {
                    if (!_commonFacade.IsSystemExists(request.RefSystemCode))
                    {
                        valid = false;
                        validationResults.Add(new ValidationResult(string.Format(CultureInfo.InvariantCulture, Resource.ValErr_InvalidData, Resource.Lbl_RefSystemCode)));
                    }
                }

                if (!valid)
                {
                    string msg = "Bad Request, the body is not valid:\n{0}";
                    response.ResponseStatus.ResponseCode = Constants.ErrorCode.KKCAR_ERR102;
                    response.ResponseStatus.ResponseMessage = string.Format(CultureInfo.InvariantCulture, msg,
                        validationResults.Select(x => x.ErrorMessage).Aggregate((i, j) => i + Delimeter + j)).TrimEnd(new char[] { '\r', '\n' });
                    goto Outer;
                }

                #region "Save Car Status"

                if (_context == null)
                {
                    _context = new KKCARContextContainer();
                }

                Logger.Debug("I:--START--:--StatusDataAccess.SaveCarStatus--");
                var sw1 = System.Diagnostics.Stopwatch.StartNew();

                _statusDataAccess = new StatusDataAccess(_context);
                CarStatusEntity status = _statusDataAccess.GetCarID(PopulateStatusObject(request));
                bool success = _statusDataAccess.SaveCarStatus(status);
                response.ResponseStatus.ResponseCode = success ? Constants.ErrorCode.KKCAR_SUCCESS : Constants.ErrorCode.KKCAR_ERR100;
                response.ResponseStatus.ResponseMessage = success ? "Data saved successfully." : "Failed to save data.";

                sw1.Stop();
                Logger.DebugFormat("O:--Finish--:ElapsedMilliseconds/{0}", sw1.ElapsedMilliseconds);

                #endregion
            }

        #endregion

        Outer:
            Logger.Info(_logMsg.Clear().SetPrefixMsg("Call KKCARStatusService.InsertStatus").Add("ResponseCode", response.ResponseStatus.ResponseCode)
                .Add("ResponseMessage", response.ResponseStatus.ResponseMessage).ToOutputLogString());
            Logger.DebugFormat("-- XMLResponse --\n{0}", response.SerializeObject());
            stopwatch.Stop();
            Logger.DebugFormat("O:--Finish--:ElapsedMilliseconds/{0}", stopwatch.ElapsedMilliseconds);

            return response;
        }

        public TaskInsertStatusResult BulkInsertStatus(string filePath, string dataDate, string sysCode)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Logger.Info(_logMsg.Clear().SetPrefixMsg("Call KKBatchCARStatusService.BulkInsertStatus").ToInputLogString());
            Logger.Debug("I:--START--:--KKCARStatusService.BulkInsertStatus--");

            string logDetail = string.Empty;
            DateTime startDateTime = DateTime.Now;
            BulkInsertStatusResponse resBatch = null;
            string fileName = Path.GetFileName(filePath);
            string logErrorCode = Constants.ErrorCode.KKCAR_SUCCESS;
            int totalRecords = 0, numOfError = 0, numOfComplete = 0;
            TaskInsertStatusResult taskResult = new TaskInsertStatusResult();
            string batchPathExport = ApplicationHelpers.GetBatchPathExport(sysCode);
            string batchPathSource = ApplicationHelpers.GetBatchPathSource(sysCode);
            string srcFileName = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", batchPathSource, fileName);

            try
            {
                _fileFacade = new FileFacade();
                BulkInsertStatusRequest reqBatch = _fileFacade.LoadJsonObject<BulkInsertStatusRequest>(filePath);
                var reqList = PopulateInsertStatusRequest(reqBatch);

                Logger.Info(_logMsg.Clear().SetPrefixMsg("Populate InsertStatusRequest")
                    .Add("List Size", (reqList == null || !reqList.Any()) ? 0 : reqList.Count()).ToOutputLogString());

                resBatch = new BulkInsertStatusResponse(reqBatch.Header);

                if (reqList.Any())
                {
                    totalRecords = reqList.Count();
                    resBatch.Body = new BatchResponseBody[totalRecords];

                    for (int i = 0; i < totalRecords; i++)
                    {
                        var req = reqList.ElementAt(i);
                        var res = this.InsertStatus(req, true);
                        resBatch.Body[i] = new BatchResponseBody
                        {
                            ChannelId = res.Header.ChannelID,
                            ReferenceNo = res.Header.ReferenceNo,
                            ResponseStatus = new BatchResponseStatus
                            {
                                ResponseCode = res.ResponseStatus.ResponseCode,
                                ResponseMessage = res.ResponseStatus.ResponseMessage
                            }
                        };
                    }
                }

                // Export Json Result
                _fileFacade = new FileFacade();

                var exFileName = string.Format(CultureInfo.InvariantCulture, "{0}_{1}.txt", fileName.ExtractNamePrefix(), WebConfig.GetBatchResponseSuffix());
                var jsonString = JsonConvert.SerializeObject(resBatch);

                Logger.Info(_logMsg.Clear().SetPrefixMsg("Manages File").Add("BatchPathExport", batchPathExport)
                    .Add("ExportFileName", exFileName).ToInputLogString());

                if (!_fileFacade.ExportBatchResponse(batchPathExport, exFileName, jsonString))
                {
                    logErrorCode = Constants.ErrorCode.KKCAR_ERR100;
                    logDetail = "Cannot export files";
                    Logger.Info(_logMsg.Clear().SetPrefixMsg("Export response file").Add("Error Message", logDetail).ToFailLogString());
                    goto Outer;
                }

                if (!StreamDataHelpers.TryToCopy(filePath, srcFileName))
                {
                    logErrorCode = Constants.ErrorCode.KKCAR_ERR100;
                    logDetail = "Cannot copy file to target path";
                    Logger.Info(_logMsg.Clear().SetPrefixMsg("Copy import file to source path").Add("Error Message", logDetail).ToFailLogString());
                    goto Outer;
                }

                if (!StreamDataHelpers.TryToDelete(filePath))
                {
                    logErrorCode = Constants.ErrorCode.KKCAR_ERR100;
                    logDetail = "Cannot delete file";
                    Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete import file").Add("Error Message", logDetail).ToFailLogString());
                    goto Outer;
                }

                numOfError = resBatch.Body.Count(x => !Constants.ErrorCode.KKCAR_SUCCESS.Equals(x.ResponseStatus.ResponseCode));
                numOfComplete = resBatch.Body.Count(x => Constants.ErrorCode.KKCAR_SUCCESS.Equals(x.ResponseStatus.ResponseCode));

                if (numOfError > 0)
                {
                    logDetail = "Invalid Data";
                    logErrorCode = Constants.ErrorCode.KKCAR_ERR103;
                }

            Outer:
                taskResult.ExportFileName = exFileName;
                taskResult.FileName = fileName;
                taskResult.NumOfError = numOfError;
                taskResult.NumOfComplete = numOfComplete;
                taskResult.TotalRecords = totalRecords;
                taskResult.ResponseStatus.ResponseCode = logErrorCode;
                taskResult.ResponseStatus.ResponseMessage = logDetail;
                return taskResult;
            }
            finally
            {
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Call KKCARStatusService.InsertStatus").Add("Results", taskResult.ToString()).ToOutputLogString());
                Logger.DebugFormat("-- XMLResponse --\n{0}", taskResult.SerializeObject());
                stopwatch.Stop();
                Logger.DebugFormat("O:--Finish--:ElapsedMilliseconds/{0}", stopwatch.ElapsedMilliseconds);

                #region "Save Batch Log"

                var auditLog = new AuditLogEntity();
                auditLog.BatchDate = dataDate;
                auditLog.StartDateTime = startDateTime;
                auditLog.EndDateTime = DateTime.Now;
                auditLog.LogDetail = logDetail;
                auditLog.NumOfTotal = taskResult.TotalRecords;
                auditLog.NumOfComplete = taskResult.NumOfComplete;
                auditLog.NumOfError = taskResult.NumOfError;
                auditLog.BatchCode = Constants.BatchCode.BulkInsertStatus;
                auditLog.RerunPath = GetBatchUrl(sysCode, fileName);
                auditLog.SystemCode = resBatch.Header.SystemCode;
                auditLog.ServiceName = Constants.ServiceName.BulkInsertStatus;
                auditLog.Status = taskResult.NumOfError == 0 ? LogStatus.Success : LogStatus.Fail;
                auditLog.ReferenceCode = resBatch.Header.ReferenceNo;
                auditLog.TransactionDate = resBatch.Header.CreateDate;

                if (resBatch != null)
                {
                    if (resBatch.Body != null && resBatch.Body.Length > 0)
                    {
                        var logDetailList = new List<LogDetailEntity>();

                        foreach (var result in resBatch.Body)
                        {
                            var auditLogDetail = new LogDetailEntity();
                            auditLogDetail.ReferenceNo = result.ReferenceNo;
                            auditLogDetail.ChannelId = result.ChannelId;
                            auditLogDetail.LogCode = result.ResponseStatus.ResponseCode;
                            auditLogDetail.LogDetail = result.ResponseStatus.ResponseMessage;
                            logDetailList.Add(auditLogDetail);
                        }

                        auditLog.LogDetailList = logDetailList;
                    }
                }

                AppLog.AuditLog(auditLog);

                #endregion
            }
        }

        public TaskInsertStatusResult BatchInsertStatus(BatchInsertStatusRequest batchRequest, BulkInsertStatusRequest bulkRequest)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Logger.Info(_logMsg.Clear().SetPrefixMsg("Call KKBatchCARStatusService.BatchInsertStatus").ToInputLogString());
            Logger.Debug("I:--START--:--KKCARStatusService.BatchInsertStatus--");

            string logDetail = string.Empty;
            DateTime startDateTime = DateTime.Now;
            BulkInsertStatusResponse resBatch = null;
            string logErrorCode = Constants.ErrorCode.KKCAR_SUCCESS;
            int totalRecords = 0, numOfError = 0, numOfComplete = 0;
            TaskInsertStatusResult taskResult = new TaskInsertStatusResult();

            try
            {
                _fileFacade = new FileFacade();
                var reqList = PopulateInsertStatusRequest(bulkRequest);
                resBatch = new BulkInsertStatusResponse(bulkRequest.Header);

                Logger.Info(_logMsg.Clear().SetPrefixMsg("Populate InsertStatusRequest")
                    .Add("List Size", (reqList == null || !reqList.Any()) ? 0 : reqList.Count()).ToOutputLogString());

                if (reqList.Any())
                {
                    totalRecords = reqList.Count();
                    resBatch.Body = new BatchResponseBody[totalRecords];

                    for (int i = 0; i < totalRecords; i++)
                    {
                        var req = reqList.ElementAt(i);
                        var res = this.InsertStatus(req, true);
                        resBatch.Body[i] = new BatchResponseBody
                        {
                            ChannelId = res.Header.ChannelID,
                            ReferenceNo = res.Header.ReferenceNo,
                            ResponseStatus = new BatchResponseStatus
                            {
                                ResponseCode = res.ResponseStatus.ResponseCode,
                                ResponseMessage = res.ResponseStatus.ResponseMessage
                            }
                        };
                    }
                }

                numOfError = resBatch.Body.Count(x => !Constants.ErrorCode.KKCAR_SUCCESS.Equals(x.ResponseStatus.ResponseCode));
                numOfComplete = resBatch.Body.Count(x => Constants.ErrorCode.KKCAR_SUCCESS.Equals(x.ResponseStatus.ResponseCode));

                if (numOfError > 0)
                {
                    logDetail = "Invalid Data";
                    logErrorCode = Constants.ErrorCode.KKCAR_ERR103;
                }

                taskResult.NumOfError = numOfError;
                taskResult.NumOfComplete = numOfComplete;
                taskResult.TotalRecords = totalRecords;
                taskResult.ResponseStatus.ResponseCode = logErrorCode;
                taskResult.ResponseStatus.ResponseMessage = logDetail;

                return taskResult;
            }
            finally
            {
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Call KKCARStatusService.InsertStatus").Add("Results", taskResult.ToString()).ToOutputLogString());

                Logger.DebugFormat("-- XMLResponse --\n{0}", taskResult.SerializeObject());

                stopwatch.Stop();
                Logger.DebugFormat("O:--Finish--:ElapsedMilliseconds/{0}", stopwatch.ElapsedMilliseconds);

                #region "Save Batch Log"

                var auditLog = new AuditLogEntity();
                auditLog.BatchDate = batchRequest.DataDate;
                auditLog.BatchCode = Constants.BatchCode.BatchInsertStatus;
                auditLog.StartDateTime = startDateTime;
                auditLog.EndDateTime = DateTime.Now;
                auditLog.LogDetail = logDetail;
                auditLog.NumOfTotal = taskResult.TotalRecords;
                auditLog.NumOfComplete = taskResult.NumOfComplete;
                auditLog.NumOfError = taskResult.NumOfError;
                auditLog.RerunPath = GetNotifyUrl(batchRequest);
                auditLog.SystemCode = batchRequest.SystemCode;
                auditLog.ServiceName = batchRequest.ServiceName;
                auditLog.Status = taskResult.NumOfError == 0 ? LogStatus.Success : LogStatus.Fail;
                auditLog.ReferenceCode = bulkRequest.Header.ReferenceNo;
                auditLog.TransactionDate = bulkRequest.Header.CreateDate;

                if (resBatch != null)
                {
                    if (resBatch.Body != null && resBatch.Body.Length > 0)
                    {
                        var logDetailList = new List<LogDetailEntity>();

                        foreach (var result in resBatch.Body)
                        {
                            var auditLogDetail = new LogDetailEntity();
                            auditLogDetail.ReferenceNo = result.ReferenceNo;
                            auditLogDetail.ChannelId = result.ChannelId;
                            auditLogDetail.LogCode = result.ResponseStatus.ResponseCode;
                            auditLogDetail.LogDetail = result.ResponseStatus.ResponseMessage;
                            logDetailList.Add(auditLogDetail);
                        }

                        auditLog.LogDetailList = logDetailList;
                    }
                }

                AppLog.AuditLog(auditLog);

                #endregion
            }
        }

        

        #region "Functions"

        private static string GetNotifyUrl(BatchInsertStatusRequest request)
        {
            return ApplicationHelpers.GetNotifyUrl(request.SystemCode, request.ServiceName, request.DataDate, request.GetResult, request.Path);
        }

        private static string GetBatchUrl(string sysCode, string fileName)
        {
            return ApplicationHelpers.GetBatchUrl(sysCode, fileName);
        }

        private static CarStatusEntity PopulateStatusObject(InsertStatusRequest request)
        {
            CarStatusEntity status = new CarStatusEntity();
            status.RefNo = request.Header.ReferenceNo.NullSafeTrim();
            status.TranDateTime = request.Header.TransactionDateTime;
            status.StatusDateTime = request.StatusDateTime;
            status.ChannelID = request.Header.ChannelID.NullSafeTrim();
            status.SubsID = request.SubscriptionID.NullSafeTrim();
            status.SubsCusType = request.SubscriptionCusType.NullSafeTrim();
            status.SubsCardType = request.SubscriptionCardType.NullSafeTrim();
            status.PDMProdGrpID = request.PDMProductGroupID.NullSafeTrim();
            status.PDMProdGrpDesc = request.PDMProductGroupDesc.NullSafeTrim();
            status.PDMProdSubGrpID = request.PDMProductSubGroupID.NullSafeTrim();
            status.PDMProdSubGrpDesc = request.PDMProductSubGroupDesc.NullSafeTrim();
            status.PDMProdID = request.PDMProductID.NullSafeTrim();
            status.PDMProdDesc = request.PDMProductDesc.NullSafeTrim();
            status.PDMCampaignID = request.PDMCampaignID.NullSafeTrim();
            status.PDMCampaignDesc = request.PDMCampaignDesc.NullSafeTrim();
            status.CMTProdGrpID = request.CMTProductGroupID.NullSafeTrim();
            status.CMTProdGrpDesc = request.CMTProductGroupDesc.NullSafeTrim();
            status.CMTProdID = request.CMTProductID.NullSafeTrim();
            status.CMTProdDesc = request.CMTProductDesc.NullSafeTrim();
            status.CMTCampaignID = request.CMTCampaignID.NullSafeTrim();
            status.CMTCampaignDesc = request.CMTCampaignDesc.NullSafeTrim();
            status.OwnerSystemId = request.OwnerSystemId.NullSafeTrim();
            status.OwnerSystemCode = request.OwnerSystemCode.NullSafeTrim();
            status.RefSystemId = request.RefSystemId.NullSafeTrim();
            status.RefSystemCode = request.RefSystemCode.NullSafeTrim();
            status.StatusCode = request.Status.NullSafeTrim();
            status.StatusDesc = request.StatusName.NullSafeTrim();
            status.SubStatusCode = request.SubStatus.NullSafeTrim();
            status.SubStatusDesc = request.SubStatusName.NullSafeTrim();
            status.UpdatedBranch = request.UpdatedBRANCH.NullSafeTrim();
            status.UpdatedTeam = request.UpdatedTeam.NullSafeTrim();
            status.UpdatedID = request.UpdatedID.NullSafeTrim();
            status.UpdatedName = request.UpdatedName.NullSafeTrim();
            status.UpdatedPosition = request.UpdatedPosition.NullSafeTrim();
            //status.StatusNextBranch = request.StatusNextBranch;
            //status.StatusNextTeam = request.StatusNextTeam;
            status.Remark = request.Remark;

            //var jss = new JavaScriptSerializer();
            //status.JsonCusInfo = jss.Serialize(request.CustomerInfoList);
            //status.JsonOfficerInfo = jss.Serialize(request.OfficerInfoList);
            //status.JsonAppInfo = jss.Serialize(request.ApplicationInfoList);
            //status.JsonOtherInfo = jss.Serialize(request.OtherInfoList);
            return status;
        }

        private static IEnumerable<InsertStatusRequest> PopulateInsertStatusRequest(BulkInsertStatusRequest request)
        {
            var today = DateTime.Now;
            var results = from rq in request.Body
                          select new InsertStatusRequest
                          {
                              Header = new Service.Messages.Common.Header
                              {
                                  ChannelID = rq.ChannelId,
                                  ReferenceNo = rq.ReferenceNo,
                                  SecurityKey = request.Header.SecurityKey,
                                  ServiceName = Constants.ServiceName.InsertStatus,
                                  SystemCode = request.Header.SystemCode,
                                  TransactionDateTime = today
                              },
                              PDMCampaignDesc = rq.PDMCampaignDesc,
                              PDMCampaignID = rq.PDMCampaignId,
                              OwnerSystemCode = rq.OwnerSystemCode,
                              OwnerSystemId = rq.OwnerSystemId,
                              PDMProductDesc = rq.PDMProductDesc,
                              PDMProductGroupDesc = rq.PDMProductGroupDesc,
                              PDMProductGroupID = rq.PDMProductGroupId,
                              PDMProductID = rq.PDMProductId,
                              PDMProductSubGroupDesc = rq.PDMProductSubGroupDesc,
                              PDMProductSubGroupID = rq.PDMProductSubGroupId,
                              CMTProductID = rq.CMTProductID,
                              CMTProductDesc = rq.CMTProductDesc,
                              CMTProductGroupID = rq.CMTProductGroupID,
                              CMTProductGroupDesc = rq.CMTProductGroupDesc,
                              CMTCampaignID = rq.CMTCampaignID,
                              CMTCampaignDesc = rq.CMTCampaignDesc,
                              RefSystemCode = rq.RefSystemCode,
                              RefSystemId = rq.RefSystemId,
                              Status = rq.Status,
                              StatusDateTime = rq.StatusDateTime,
                              StatusName = rq.StatusName,
                              SubscriptionCardType = rq.SubscriptionCardType,
                              SubscriptionCusType = rq.SubscriptionCusType,
                              SubscriptionID = rq.SubscriptionId,
                              SubStatus = rq.SubStatus,
                              SubStatusName = rq.SubStatusName,
                              UpdatedBRANCH = rq.UpdatedBranch,
                              UpdatedID = rq.UpdatedId,
                              UpdatedName = rq.UpdatedName,
                              UpdatedPosition = rq.UpdatedPosition,
                              UpdatedTeam = rq.UpdatedTeam,
                              Remark = rq.Remark
                          };

            return results;
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
                    if (_context != null) { _context.Dispose(); }
                    if (_authFacade != null) { _authFacade.Dispose(); }
                    if (_commonFacade != null) { _commonFacade.Dispose(); }
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
