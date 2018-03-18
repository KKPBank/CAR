using KKCAR.Common.Utilities;
using KKCAR.Data.DataAccess.Common;
using KKCAR.Entity;
using log4net;
using System;
using System.Linq;

///<summary>
/// Class Name : StatusDataAccess
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
    public class StatusDataAccess : BaseDataAccess, IStatusDataAccess
    {
        private readonly KKCARContextContainer _context;
        private LogMessageBuilder _logMsg = new LogMessageBuilder();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IStatusDataAccess));

        public StatusDataAccess(KKCARContextContainer context)
        {
            _context = context;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public CarStatusEntity GetCarID(CarStatusEntity status)
        {
            var refCarID = GetCarID(status.RefSystemId, status.RefSystemCode);
            var ownCarID = GetCarID(status.OwnerSystemId, status.OwnerSystemCode);
            string carID = !string.IsNullOrWhiteSpace(refCarID) ? refCarID : (!string.IsNullOrWhiteSpace(ownCarID) ? ownCarID : null);
            var cloned = status.CopyObject<CarStatusEntity>();
            cloned.CarID = carID;
            return cloned;
        }

        private string GetCarID(string sysId, string sysCode)
        {
            Logger.Info(_logMsg.Clear().SetPrefixMsg("Get CarID").Add("SystemID", sysId).Add("SystemCode", sysCode).ToInputLogString());

            var query = from sr in _context.CAR_STATUS_REFFERENCE.AsNoTracking()
                        from sm in _context.CAR_STATUS_MASTER.Where(x => sr.CAR_ID == x.CAR_ID).DefaultIfEmpty()
                        where sr.CAR_SYSTEM_ID == sysId && sr.CAR_SYSTEM_CODE == sysCode
                        select sm.CAR_ID;

            if (query.Any())
            {
                string carID = query.FirstOrDefault();
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Get CarID").Add("CarID", carID).ToSuccessLogString());
                return carID;
            }

            Logger.Info(_logMsg.Clear().SetPrefixMsg("Get CarID").Add("CarID", "-").ToSuccessLogString());
            return null;
        }

        public bool SaveCarStatus(CarStatusEntity status)
        {
            Logger.Info(_logMsg.Clear().SetPrefixMsg("Save CAR Status").Add("ReferenceNo", status.RefNo).Add("ChannelID", status.ChannelID)
                .Add("TransactionDateTime", status.TranDateTime.FormatDateTime(Constants.DateTimeFormat.DefaultFullDateTime))
                .Add("StatusDateTime", status.StatusDateTime.FormatDateTime(Constants.DateTimeFormat.DefaultFullDateTime))
                .Add("OwnerSystemId", status.OwnerSystemId).Add("OwnerSystemCode", status.OwnerSystemCode).Add("RefSystemId", status.RefSystemId)
                .Add("RefSystemCode", status.RefSystemCode).Add("CarID", status.CarID).ToInputLogString());

            try
            {
                using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;

                    try
                    {
                        var today = DateTime.Now;
                        bool isNewRecord = false;
                        CAR_STATUS_MASTER master = _context.CAR_STATUS_MASTER.FirstOrDefault(x => !string.IsNullOrEmpty(status.CarID) ? x.CAR_ID == status.CarID : false);
                        var statusDateTime = master == null ? new Nullable<DateTime>() : master.CAR_STATUS_DATETIME;
                        bool skipUpdate = statusDateTime == null ? false : (statusDateTime.Value > status.StatusDateTime);

                        Logger.Info(_logMsg.Clear().SetPrefixMsg("Is skip update status master and detail?").Add("SkipUpdate", skipUpdate).ToInputLogString());

                        if (!skipUpdate)
                        {
                            #region "Save Status Master"

                            if (master == null)
                            {
                                isNewRecord = true;
                                master = new CAR_STATUS_MASTER();
                                master.CAR_ID = this.GetNextSequenceValue(_context, Constants.SequenceName.CarID).ConvertToString();
                            }

                            master.CAR_TRAN_DATETIME = status.TranDateTime.Value;
                            master.CAR_STATUS_DATETIME = status.StatusDateTime.Value;
                            master.CAR_REFERNCE_NO = status.RefNo;
                            master.CAR_CHANNEL_ID = status.ChannelID;

                            if (!string.IsNullOrWhiteSpace(status.SubsID))
                            {
                                master.CAR_SUBSCRIPTION_ID = status.SubsID;
                            }
                            if (!string.IsNullOrWhiteSpace(status.SubsCusType))
                            {
                                master.CAR_SUBSCRIPTION_CUSTYPE = status.SubsCusType;
                            }
                            if (!string.IsNullOrWhiteSpace(status.SubsCardType))
                            { 
                                master.CAR_SUBSCRIPTION_CARDTYPE = status.SubsCardType;
                            }

                            master.CAR_PDM_PRODUCTGROUP_ID = status.PDMProdGrpID;
                            master.CAR_PDM_PRODUCTGROUP_DESC = status.PDMProdGrpDesc;
                            master.CAR_PDM_PRODUCTSUBGROUP_ID = status.PDMProdSubGrpID;
                            master.CAR_PDM_PRODUCTSUBGROUP_DESC = status.PDMProdSubGrpDesc;
                            master.CAR_PDM_PRODUCT_ID = status.PDMProdID;
                            master.CAR_PDM_PRODUCT_DESC = status.PDMProdDesc;
                            master.CAR_PDM_CAMPAIGN_ID = status.PDMCampaignID;
                            master.CAR_PDM_CAMPAIGN_DESC = status.PDMCampaignDesc;
                            master.CAR_CMT_PRODUCTGROUP_ID = status.CMTProdGrpID;
                            master.CAR_CMT_PRODUCTGROUP_DESC = status.CMTProdGrpDesc;
                            master.CAR_CMT_PRODUCT_ID = status.CMTProdID;
                            master.CAR_CMT_PRODUCT_DESC = status.CMTProdDesc;
                            master.CAR_CMT_CAMPAIGN_ID = status.CMTCampaignID;
                            master.CAR_CMT_CAMPAIGN_DESC = status.CMTCampaignDesc;
                            master.CAR_STATUS_CODE = status.StatusCode;
                            master.CAR_STATUS_DESC = status.StatusDesc;
                            master.CAR_SUBSTATUS_CODE = status.SubStatusCode;
                            master.CAR_SUBSTATUS_DESC = status.SubStatusDesc;
                            master.CAR_UPDATED_BRANCH = status.UpdatedBranch;
                            master.CAR_UPDATED_TEAM = status.UpdatedTeam;
                            master.CAR_UPDATED_ID = status.UpdatedID;
                            master.CAR_UPDATED_NAME = status.UpdatedName;
                            master.CAR_UPDATED_POSITION = status.UpdatedPosition;
                            master.CAR_REMARK = status.Remark;

                            if (isNewRecord)
                            {
                                _context.CAR_STATUS_MASTER.Add(master);
                            }
                            else
                            {
                                SetEntryStateModified(master);
                            }

                            isNewRecord = false;
                            this.Save();

                            #endregion

                            #region "Save Status Detail"

                            //CAR_STATUS_DETAIL detail = _context.CAR_STATUS_DETAIL.FirstOrDefault(x => x.CAR_ID == master.CAR_ID);

                            //if (detail == null)
                            //{
                            //    isNewRecord = true;
                            //    detail = new CAR_STATUS_DETAIL();
                            //    detail.CAR_ID = master.CAR_ID;
                            //}

                            //detail.CAR_CUSTOMER_LIST = status.JsonCusInfo;
                            //detail.CAR_OFFICER_LIST = status.JsonOfficerInfo;
                            //detail.CAR_APPLICATION_LIST = status.JsonAppInfo;
                            //detail.CAR_OTHER_LIST = status.JsonOtherInfo;

                            //if (isNewRecord)
                            //{
                            //    _context.CAR_STATUS_DETAIL.Add(detail);
                            //}
                            //else
                            //{
                            //    SetEntryStateModified(detail);
                            //}

                            //isNewRecord = false;
                            //this.Save();

                            #endregion                            
                        }

                        #region "Save Status Reference"

                        if (!string.IsNullOrWhiteSpace(status.RefSystemId) && !string.IsNullOrWhiteSpace(status.RefSystemCode)) {
                            bool isRefSysExists = _context.CAR_STATUS_REFFERENCE.Any(x => x.CAR_ID == master.CAR_ID
                                && x.CAR_SYSTEM_ID == status.RefSystemId && x.CAR_SYSTEM_CODE == status.RefSystemCode);

                            if (!isRefSysExists)
                            {
                                CAR_STATUS_REFFERENCE refSystem = new CAR_STATUS_REFFERENCE();
                                refSystem.CAR_ID = master.CAR_ID;
                                refSystem.CAR_SYSTEM_ID = status.RefSystemId;
                                refSystem.CAR_SYSTEM_CODE = status.RefSystemCode;
                                _context.CAR_STATUS_REFFERENCE.Add(refSystem);
                                this.Save();
                            }
                        }

                        bool isOwnSysExists = _context.CAR_STATUS_REFFERENCE.Any(x => x.CAR_ID == master.CAR_ID
                            && x.CAR_SYSTEM_ID == status.OwnerSystemId && x.CAR_SYSTEM_CODE == status.OwnerSystemCode);

                        if (!isOwnSysExists)
                        {
                            CAR_STATUS_REFFERENCE ownerSystem = new CAR_STATUS_REFFERENCE();
                            ownerSystem.CAR_ID = master.CAR_ID;
                            ownerSystem.CAR_SYSTEM_ID = status.OwnerSystemId;
                            ownerSystem.CAR_SYSTEM_CODE = status.OwnerSystemCode;
                            _context.CAR_STATUS_REFFERENCE.Add(ownerSystem);
                            this.Save();
                        }

                        #endregion

                        #region "Save Status History"

                        CAR_STATUS_HISTORY history = new CAR_STATUS_HISTORY();
                        history.CAR_ID = master.CAR_ID;
                        history.CAR_SEQ_ID = this.GetNextSequenceValue(_context, Constants.SequenceName.CarSeqID);
                        history.CAR_OWNERSYSTEM_ID = status.OwnerSystemId;
                        history.CAR_OWNERSYSTEM_CODE = status.OwnerSystemCode;
                        history.CAR_REFERENCESYSTEM_ID = status.RefSystemId;
                        history.CAR_REFERENCESYSTEM_CODE = status.RefSystemCode;
                        history.CAR_STATUS_CODE = status.StatusCode;
                        history.CAR_STATUS_DESC = status.StatusDesc;
                        history.CAR_SUBSTATUS_CODE = status.SubStatusCode;
                        history.CAR_SUBSTATUS_DESC = status.SubStatusDesc;
                        history.CAR_UPDATED_BRANCH = status.UpdatedBranch;
                        history.CAR_UPDATED_TEAM = status.UpdatedTeam;
                        history.CAR_UPDATED_ID = status.UpdatedID;
                        history.CAR_UPDATED_NAME = status.UpdatedName;
                        history.CAR_UPDATED_POSITION = status.UpdatedPosition;
                        history.CAR_REMARK = status.Remark;
                        history.CAR_STATUS_DATETIME = status.StatusDateTime;
                        history.CAR_CREATE_DATETIME = today;
                        _context.CAR_STATUS_HISTORY.Add(history);
                        this.Save();

                        #endregion

                        transaction.Commit();
                        Logger.Info(_logMsg.Clear().SetPrefixMsg("Save CAR Status").ToSuccessLogString());
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Logger.Info(_logMsg.Clear().SetPrefixMsg("Save CAR Status").Add("Error Message", ex.Message).ToFailLogString());
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
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Save CAR Status").Add("Error Message", ex.Message).ToFailLogString());
                Logger.Error("Exception occur:\n", ex);
            }

            return false;
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
