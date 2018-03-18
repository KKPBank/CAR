using KKCAR.Common.Utilities;
using KKCAR.Data.DataAccess.Common;
using KKCAR.Entity;
using log4net;
using System;
using System.Linq;

///<summary>
/// Class Name : AuditLogDataAccess
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Data.DataAccess
{
    public class AuditLogDataAccess : BaseDataAccess, IAuditLogDataAccess
    {
        private readonly KKCARContextContainer _context;
        private static readonly object syncLock = new object();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuditLogDataAccess));

        public AuditLogDataAccess(KKCARContextContainer context)
        {
            _context = context;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void AddLog(string batchCode, DateTime startDateTime, DateTime endDateTime, LogStatus logStatus)
        {
            try
            {
                Logger.Info("I:--START--:--Add Audit Log--");

                _context.Configuration.AutoDetectChangesEnabled = false;

                try
                {
                    var batch = _context.CAR_BATCH.FirstOrDefault(x => x.BATCH_CODE == batchCode);
                    batch.START_TIME = startDateTime;
                    batch.END_TIME = endDateTime;
                    batch.STATUS = logStatus.ToShortString();
                    SetEntryStateModified(batch);
                    Save();

                    Logger.Info("O:--SUCCESS--:--Add Audit Log--");
                }
                catch (Exception ex)
                {
                    Logger.InfoFormat("O:--FAILED--:--Add Audit Log--:Error Message/{0}", ex.Message);
                    Logger.Error("Exception occur:\n", ex);
                }
                finally
                {
                    _context.Configuration.AutoDetectChangesEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.InfoFormat("O:--FAILED--:--Add Audit Log--:Error Message/{0}", ex.Message);
                Logger.Error("Exception occur:\n", ex);
            }
        }

        public void AddLog(AuditLogEntity auditLog)
        {
            try
            {
                Logger.Info("I:--START--:--Add Audit Log--");

                using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;

                    try
                    {
                        decimal batchRound;
                        var batch = _context.CAR_BATCH.FirstOrDefault(x => x.BATCH_CODE == auditLog.BatchCode);

                        lock (syncLock)
                        {
                            // synchronize
                            CAR_BATCH_LOG existBatchLog = (from bl in _context.CAR_BATCH_LOG
                                                           join bt in _context.CAR_BATCH on bl.BATCH_ID equals bt.BATCH_ID
                                                           where bt.BATCH_ID == batch.BATCH_ID && bl.BATCH_DATE == auditLog.BatchDate
                                                           orderby bl.BATCH_ROUND descending
                                                           select bl).FirstOrDefault();

                            batchRound = existBatchLog != null ? (existBatchLog.BATCH_ROUND ?? 0) + 1 : 1;
                        }

                        CAR_BATCH_LOG batchLog = new CAR_BATCH_LOG();
                        batchLog.BATCH_ID = batch.BATCH_ID;
                        batchLog.BATCH_LOG_ID = this.GetNextSequenceValue(_context, Constants.SequenceName.CarBatchLogID);
                        batchLog.BATCH_DATE = auditLog.BatchDate;
                        batchLog.BATCH_ROUND = batchRound;
                        batchLog.FILE_NAME = auditLog.Filename;
                        batchLog.START_TIME = auditLog.StartDateTime.Value;
                        batchLog.END_TIME = auditLog.EndDateTime;
                        batchLog.STATUS = auditLog.StatusDisplay;
                        batchLog.ERROR_DETAIL = auditLog.LogDetail;
                        batchLog.RERUN_PATH = auditLog.RerunPath;
                        batchLog.TOTAL_HEADER = auditLog.NumOfTotal;
                        batchLog.TOTAL_DETAIL = auditLog.NumOfTotal;
                        batchLog.TOTAL_COMPLETE = auditLog.NumOfComplete;
                        batchLog.TOTAL_FAIL = auditLog.NumOfError;
                        batchLog.SYSTEM_CODE = auditLog.SystemCode;
                        batchLog.SERVICE_NAME = auditLog.ServiceName;
                        batchLog.REFERENCE_CODE = auditLog.ReferenceCode;
                        batchLog.TRANSACTION_DATE = auditLog.TransactionDate;
                        batchLog.STATUS = auditLog.StatusDisplay;
                        _context.CAR_BATCH_LOG.Add(batchLog);
                        Save();

                        if (auditLog.LogDetailList != null && auditLog.LogDetailList.Count > 0)
                        {
                            foreach (var auditLogDetail in auditLog.LogDetailList)
                            {
                                CAR_BATCH_LOG_DETAIL batchLogDetail = new CAR_BATCH_LOG_DETAIL
                                {
                                    BATCH_LOG_DETAIL_ID = this.GetNextSequenceValue(_context, Constants.SequenceName.CarBatchLogDetailID),
                                    BATCH_LOG_ID = batchLog.BATCH_LOG_ID,
                                    REFERENCE_NO = auditLogDetail.ReferenceNo,
                                    CHANNEL_ID = auditLogDetail.ChannelId,
                                    RESPONSE_CODE = auditLogDetail.LogCode,
                                    RESPONSE_MESSAGE = auditLogDetail.LogDetail
                                };

                                _context.CAR_BATCH_LOG_DETAIL.Add(batchLogDetail);
                            }

                            Save();
                        }

                        transaction.Commit();
                        Logger.Info("O:--SUCCESS--:--Add Audit Log--");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Logger.InfoFormat("O:--FAILED--:--Add Audit Log--:Error Message/{0}", ex.Message);
                        Logger.Error("Exception occur:\n", ex);
                    }
                    finally
                    {
                        _context.Configuration.AutoDetectChangesEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.InfoFormat("O:--FAILED--:--Add Audit Log--:Error Message/{0}", ex.Message);
                Logger.Error("Exception occur:\n", ex);
            }
        }

        public AuditLogEntity GetBatchLog(string batchCode, string systemCode, string serviceName, string dateDate, string refNo)
        {
            var batch = _context.CAR_BATCH.FirstOrDefault(x => x.BATCH_CODE == Constants.BatchCode.BatchInsertStatus);

            CAR_BATCH_LOG existBatchLog = (from bl in _context.CAR_BATCH_LOG
                                           join bt in _context.CAR_BATCH on bl.BATCH_ID equals bt.BATCH_ID
                                           where bt.BATCH_CODE == batchCode && bl.BATCH_DATE == dateDate
                                            && bl.SYSTEM_CODE == systemCode && bl.SERVICE_NAME == serviceName
                                            && bl.REFERENCE_CODE == refNo
                                           //orderby bl.BATCH_ROUND descending
                                           select bl).FirstOrDefault();

            var auditLog = new AuditLogEntity();

            if (existBatchLog != null)
            {
                auditLog.BatchDate = existBatchLog.BATCH_DATE;
                auditLog.BatchCode = batchCode;
                auditLog.SystemCode = existBatchLog.SYSTEM_CODE;
                auditLog.StartDateTime = existBatchLog.START_TIME;
                auditLog.EndDateTime = existBatchLog.END_TIME;
                auditLog.LogDetail = existBatchLog.ERROR_DETAIL;
                auditLog.NumOfTotal = existBatchLog.TOTAL_DETAIL;
                auditLog.NumOfComplete = existBatchLog.TOTAL_COMPLETE;
                auditLog.NumOfError = existBatchLog.TOTAL_FAIL;
                auditLog.RerunPath = existBatchLog.RERUN_PATH;
                auditLog.TransactionDate = existBatchLog.TRANSACTION_DATE.Value;
                auditLog.SystemCode = existBatchLog.SYSTEM_CODE;
                auditLog.ServiceName = existBatchLog.SERVICE_NAME;
                auditLog.ReferenceCode = existBatchLog.REFERENCE_CODE;

                var logDetailList = from ld in _context.CAR_BATCH_LOG_DETAIL
                                    where ld.BATCH_LOG_ID == existBatchLog.BATCH_LOG_ID
                                    select new LogDetailEntity
                                    {
                                        ReferenceNo = ld.REFERENCE_NO,
                                        ChannelId = ld.CHANNEL_ID,
                                        LogCode = ld.RESPONSE_CODE,
                                        LogDetail = ld.RESPONSE_MESSAGE
                                    };

                if (logDetailList.Any())
                {
                    auditLog.LogDetailList = logDetailList.ToList();
                }
            }

            return auditLog;
        }

        public string GetBatchCmdPathByCode(string code)
        {
            var query = from bt in _context.CAR_BATCH
                        where bt.BATCH_CODE == code
                        select bt.MAIN_URL;

            return query.Any() ? query.FirstOrDefault() : null;
        }

        public void AddTraceLog(TraceLogEntity traceLog)
        {
            Logger.Info("I:--START--:--Add Trace Log--");
            _context.Configuration.AutoDetectChangesEnabled = false;

            try
            {
                CAS_SERVICE_ACTIVITYLOG activityLog = new CAS_SERVICE_ACTIVITYLOG();
                activityLog.SYSTEM_CODE = traceLog.SystemCode;
                activityLog.SERVICE_NAME = traceLog.ServiceName;
                activityLog.REFERENCE_NO = traceLog.ReferenceCode;
                activityLog.RESPONSE_CODE = traceLog.ResponseCode;
                activityLog.RESPONSE_MESSAGE = traceLog.ResponseMessage;
                activityLog.REQUEST_IPADDRESS = traceLog.IpAddress;
                activityLog.REQUEST_DATETIME = traceLog.RequestDateTime;
                activityLog.REQUEST_URL = traceLog.RequestUrl;
                activityLog.XML_REQUEST = traceLog.XmlRequest;
                activityLog.XML_RESPONSE = traceLog.XmlResponse;

                _context.CAS_SERVICE_ACTIVITYLOG.Add(activityLog);
                Save();

                Logger.Info("O:--SUCCESS--:--Add Trace Log--");
            }
            catch (Exception ex)
            {
                Logger.InfoFormat("O:--FAILED--:--Add Trace Log--:Error Message/{0}", ex.Message);
                Logger.Error("Exception occur:\n", ex);
            }
            finally
            {
                _context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        #region "Persistence"

        private int Save()
        {
            return _context.SaveChanges();
        }

        private void SetEntryCurrentValues(object entityTo, object entityFrom)
        {
            _context.Entry(entityTo).CurrentValues.SetValues(entityFrom);
            // Set state to Modified
            _context.Entry(entityTo).State = System.Data.Entity.EntityState.Modified;
        }

        private void SetEntryStateModified(object entity)
        {
            if (_context.Configuration.AutoDetectChangesEnabled == false)
            {
                // Set state to Modified
                _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
        }

        #endregion
    }
}
