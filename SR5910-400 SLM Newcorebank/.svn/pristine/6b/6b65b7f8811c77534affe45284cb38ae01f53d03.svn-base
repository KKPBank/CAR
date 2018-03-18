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
                Logger.Info(_logMsg.Clear().Add("File Path", WebConfig.GetBatchCARInsertStatusPathImport()).ToInputLogString());

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

        public string GetMappingSysCbsCar(string sysCbs)
        {
            string ret = "";
            try
            {
                var query = from f in _context.CAR_SYS_CBS_FILE_MAPPING
                            where f.SYS_CBS == sysCbs
                            select f;

                if (query.Any() == true)
                {
                    ret = query.Take(1).FirstOrDefault().SYS_CAR;
                }
                else {
                    ret = sysCbs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }


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
