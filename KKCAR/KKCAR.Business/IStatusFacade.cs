using KKCAR.Service.Messages;
using KKCAR.Service.Messages.Batch;
using System;
using System.Collections.Generic;

///<summary>
/// Class Name : IStatusFacade
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
    public interface IStatusFacade : IDisposable
    {
        InsertStatusResponse InsertStatus(InsertStatusRequest request, bool skipValidate);
        TaskInsertStatusResult BulkInsertStatus(string filePath, string dataDate, string sysCode);
        TaskInsertStatusResult BatchInsertStatus(BatchInsertStatusRequest batchRequest, BulkInsertStatusRequest bulkRequest);
    }
}
