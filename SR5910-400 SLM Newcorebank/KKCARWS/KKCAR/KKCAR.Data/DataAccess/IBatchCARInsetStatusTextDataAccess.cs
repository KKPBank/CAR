using System;
using System.Collections.Generic;
using KKCAR.Entity;

namespace KKCAR.Data.DataAccess
{
    public interface IBatchCARInsetStatusTextDataAccess
    {
        List<BatchCARInsertStatusTextFileEntity> GetFileHistoryList();
        void UpdateFileStatus(decimal? ExtSysStatusCbsFileID, string statusName, string errorMessage);
        string GetMappingSysCbsCar(string sysCbs);
        long GetNextSequenceValue(string SequenceName);
    }
}
