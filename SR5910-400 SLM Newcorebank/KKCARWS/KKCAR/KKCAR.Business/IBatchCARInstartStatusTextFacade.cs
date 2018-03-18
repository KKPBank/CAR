using KKCAR.Service.Messages;
using KKCAR.Service.Messages.Batch;
using KKCAR.Service.Messages.Common;
using KKCAR.Data.DataAccess;
using System.Collections.Generic;
using KKCAR.Entity;

namespace KKCAR.Business
{
    public interface IBatchCARInstartStatusTextFacade
    {
        List<BatchCARInsertStatusTextFileEntity> GetFileHistoryList();
        BulkInsertStatusRequest LoadBatchCARInsertStatusTextData(string filePath, int totalRecord);
        ValidateTextfileEntity ValidateTextFileFormat(string filePath);
        void UpdateFileStatus(decimal? kkslmExtSysStatusCbsFileID, string statusName, string errorMessage);
        TaskInsertStatusResult BatchCARInsertStatusText(BulkInsertStatusRequest reqBatch, IEnumerable<InsertStatusRequest> reqList, string dataDate);
        IEnumerable<InsertStatusRequest> PopulateBatchCARInsertStatusTextRequest(BulkInsertStatusRequest req);
        bool ArchiveExtSysStatusCbsFileData(List<CARInsertStatusTextfileDataList> dataList);
    }
}
