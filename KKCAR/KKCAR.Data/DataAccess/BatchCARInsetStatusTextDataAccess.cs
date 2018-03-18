using System;
using System.Collections.Generic;
using System.Linq;
using KKCAR.Common.Utilities;
using KKCAR.Data.DataAccess.Common;
using KKCAR.Entity;
using log4net;
using System.IO;

namespace KKCAR.Data.DataAccess
{
    public class BatchCARInsetStatusTextDataAccess : BaseDataAccess, IBatchCARInsetStatusTextDataAccess
    {
        private string _errorCode = string.Empty;
        private string _errorMessage = string.Empty;
        //private IChannelDataAccess _channelDataAccess;
        private readonly KKCARContextContainer _context;
        //private IAuthenticationDataAccess _authDataAccess;
        private LogMessageBuilder _logMsg = new LogMessageBuilder();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BatchCARInsetStatusTextDataAccess));

        public BatchCARInsetStatusTextDataAccess(KKCARContextContainer context)
        {
            _context = context;
        }

        public List<BatchCARInsertStatusTextFileEntity> GetFileHistoryList()
        {
            List<BatchCARInsertStatusTextFileEntity> fileList = new List<BatchCARInsertStatusTextFileEntity>();
            try
            {
                string[] files = Directory.GetFiles(WebConfig.GetBatchCARInsertStatusPathImport());
                foreach (string f in files)
                {
                    FileInfo fInfo = new FileInfo(f);
                    //ErrorStep = "Validate Text History " + fInfo.Name;
                    Logger.Info(_logMsg.Clear().Add("Validate Text History", fInfo.Name).ToInputLogString());

                    var file = checkFileHistory(fInfo.Name);
                    if (file != null)
                    {
                        file.CAR_FILENAME = fInfo.Name;
                        file.CAR_FILEPATH = f;
                        file.CAR_FILE_CREATE_DATE = fInfo.CreationTime;
                        file.CAR_FILE_PROCESS_TIME = DateTime.Now;
                        file.CAR_PROCESS_STATUS = Constants.BatchCARProcessStatus.InProcess;

                        if (file.CAR_SYS_STATUS_CBS_FILE_ID == 0)
                        {
                            file.CAR_SYS_STATUS_CBS_FILE_ID = this.GetNextSequenceValue(_context, Constants.SequenceName.CarSysStatusCbsFile);
                            _context.CAR_SYS_STATUS_CBS_FILE.Add(file);
                            Logger.Info(_logMsg.Clear().Add("Process New File", fInfo.Name).ToInputLogString());
                        }
                        _context.SaveChanges();
                        fileList.Add(
                            new BatchCARInsertStatusTextFileEntity
                            {
                                carSysStatusCbsFileId = file.CAR_SYS_STATUS_CBS_FILE_ID,
                                fileName = file.CAR_FILENAME,
                                filePath = file.CAR_FILEPATH,
                                fileCreateDate = file.CAR_FILE_CREATE_DATE,
                                fileProcessTime = file.CAR_FILE_PROCESS_TIME,
                                processStatus = file.CAR_PROCESS_STATUS,
                            }
                        );
                    }
                }



            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }

            return fileList;
        }

        public void UpdateFileStatus(decimal? ExtSysStatusCbsFileID, string statusName, string errorMessage)
        {
            try
            {
                var query = _context.CAR_SYS_STATUS_CBS_FILE.Where(f => f.CAR_SYS_STATUS_CBS_FILE_ID == ExtSysStatusCbsFileID).FirstOrDefault();
                if (query != null)
                {
                    query.CAR_PROCESS_STATUS = statusName;
                    query.CAR_PROCESS_ERROR_STEP = errorMessage;

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //foreach (CAR_SYS_STATUS_CBS_FILE file in fileList)
        //{
        //    //3. เช็ค Format TextFile ทุก Record
        //    ErrorStep = "Validate Text Format " + file.kkslm_filename;
        //    _logger.Info(_logMsg.Clear().Add("Validate Text Format", file.kkslm_filename).ToInputLogString());
        //    ValidateTextfilData validTextFormat = ValidateTextFileFormat(file.kkslm_filepath);
        //    if (validTextFormat.IsValid == false)
        //    {
        //        ErrorStep = "Invalid Textfile " + file.kkslm_filename;
        //        _logger.Error(_logMsg.Clear().Add("Invalid Textfile", file.kkslm_filename).Add("ErrorMessage", validTextFormat.ErrorMessage).ToInputLogString());
        //        UpdateFileStatus(file.kkslm_ext_sys_status_cbs_file_id, AppConstant.Fail, "Invalid Textfile " + file.kkslm_filename + Environment.NewLine + validTextFormat.ErrorMessage);
        //        break;
        //    }

        //    //4. Process Text File ตาม Requirement
        //    ErrorStep = "GetEBatchCARInsertStatusList " + file.kkslm_filename;
        //    _logger.Info(_logMsg.Clear().Add("GetEBatchCARInsertStatusList", "").Add("FileName", file.kkslm_filename).ToInputLogString());
        //    var lists = GetEBatchCARInsertStatusList(file.kkslm_filepath, file.kkslm_ext_sys_status_cbs_file_id);
        //    if (lists != null)
        //    {
        //        bool isSuccess = true;
        //        totalRecord = lists.Count;
        //        foreach (BatchCARInsertStatusData data in lists)
        //        {
        //            try
        //            {
        //                SLMDBEntities slmdb = new SLMDBEntities();
        //                ErrorStep = "Find TicketId = " + data.RefSystemId;
        //                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == data.RefSystemId && p.is_Deleted == 0).FirstOrDefault();
        //                if (lead == null)
        //                {
        //                    _logger.ErrorFormat("Ticket Id {0} not found in SLM", data.RefSystemId);
        //                    throw new Exception("Ticket Id " + data.RefSystemId + " not found in SLM");
        //                }

        //                ErrorStep = "CheckStatus TicketId = " + data.RefSystemId;
        //                _logger.Info(_logMsg.Clear().Add("Check Ticket Status", " Ticket Id:" + data.RefSystemId));
        //                CheckStatus(slmdb, lead, data, batchCode);

        //                ErrorStep = "CheckPhoneCallHistory TicketId = " + data.RefSystemId;
        //                _logger.Info(_logMsg.Clear().Add("Check Phone Call History", " Ticket Id:" + data.RefSystemId));
        //                CheckPhoneCallHistory(slmdb, lead, data);

        //                slmdb.SaveChanges();

        //                totalSuccess += 1;
        //                _logger.Info(_logMsg.Clear().Add("TicketId", data.RefSystemId + ": SUCCESS").ToInputLogString());
        //            }
        //            catch (Exception ex)
        //            {
        //                totalFail += 1;
        //                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //                string errorDetail = "TicketId=" + data.RefSystemId + ", Error=" + message;

        //                BizUtil.InsertLog(batchMonitorId, data.RefSystemId, "", errorDetail);

        //                _logger.Error(_logMsg.Clear().Add("TicketId", data.ReferenceNo + ": FAIL").ToInputLogString());
        //            }
        //        }

        //        ErrorStep = "Set end time";
        //        BizUtil.SetEndTime(batchCode, batchMonitorId, AppConstant.Success, totalRecord, totalSuccess, totalFail);


        //        //5. นำข้อมูลใน Text File เก็บลงใน Archives
        //        int dataRow = 0;
        //        ErrorStep = "InsertArchiveData";
        //        _logger.Info(_logMsg.Clear().Add("InsertArchiveData", "TABLE : kkslm_ext_sys_status_cbs_file_data").ToInputLogString());
        //        foreach (BatchCARInsertStatusData list in lists)
        //        {
        //            SLMDBEntities slmdb = new SLMDBEntities();
        //            kkslm_ext_sys_status_cbs_file_data data = new kkslm_ext_sys_status_cbs_file_data
        //            {
        //                kkslm_ext_sys_status_cbs_file_id = file.kkslm_ext_sys_status_cbs_file_id,
        //                kkslm_reference_code = list.HeaderData.ReferenceCode,
        //                kkslm_file_name = list.HeaderData.FileName,
        //                kkslm_create_date = list.HeaderData.CreateDate,
        //                kkslm_current_sequence = Convert.ToInt16(list.HeaderData.CurrentSequence),
        //                kkslm_total_sequence = Convert.ToInt16(list.HeaderData.TotalSequence),
        //                kkslm_total_record = Convert.ToInt16(list.HeaderData.TotalRecord),
        //                kkslm_system_code = list.HeaderData.SystemCode,
        //                kkslm_reference_no = list.ReferenceNo,
        //                kkslm_channel_id = list.ChannelID,
        //                kkslm_status_date_time = list.StatusDateTime,
        //                kkslm_subscription_id = list.SubscriptionID,
        //                kkslm_subscription_cus_type = list.SubscriptionCusType,
        //                kkslm_substription_card_type = list.SubscriptionCardType,
        //                kkslm_owner_system_id = list.OwnerSystemId,
        //                kkslm_owner_system_code = list.OwnerSystemCode,
        //                kkslm_ref_system_id = list.RefSystemId,
        //                kkslm_ref_system_code = list.RefSystemCode,
        //                kkslm_status = list.Status,
        //                kkslm_status_name = list.StatusName
        //            };
        //            dataRow += 1;

        //            slmdb.kkslm_ext_sys_status_cbs_file_data.AddObject(data);
        //            isSuccess = (slmdb.SaveChanges() > 0);
        //            if (isSuccess == false)
        //            {
        //                ErrorStep = "Error InsertArchiveData at row " + dataRow.ToString();
        //                break;
        //            }
        //        }

        //        if (isSuccess == true)
        //        {
        //            ErrorStep = "MoveBatchFileToArchive";
        //            _logger.Info(_logMsg.Clear().Add("MoveBatchFileToArchive", "Filename:" + file.kkslm_filename));
        //            if (MoveBatchFileToArchive(file.kkslm_filepath) == true)
        //            {
        //                ErrorStep = "UpdateSuccessStatus Filename:" + file.kkslm_filename;
        //                _logger.Info(_logMsg.Clear().Add("UpdateSuccessStatus", "Filename:" + file.kkslm_filename));
        //                UpdateFileStatus(file.kkslm_ext_sys_status_cbs_file_id, AppConstant.Success, "");
        //            }
        //        }
        //    }
        //}


        private CAR_SYS_STATUS_CBS_FILE checkFileHistory(string FileName)
        {
            CAR_SYS_STATUS_CBS_FILE ret = null;
            try
            {
                var query = from f in _context.CAR_SYS_STATUS_CBS_FILE
                            where f.CAR_FILENAME == FileName
                            select f;

                if (query.Any() == true)
                {
                    ret = query.Take(1).Where(x => x.CAR_PROCESS_STATUS == Constants.BatchCARProcessStatus.Fail).FirstOrDefault();
                }
                else
                {
                    ret = new CAR_SYS_STATUS_CBS_FILE();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public long GetNextSequenceValue(string SequenceName) {
            return GetNextSequenceValue(_context, SequenceName);
        }
    }
}
