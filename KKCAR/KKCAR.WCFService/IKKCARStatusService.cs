using KKCAR.Common.Utilities;
using KKCAR.Service.Messages;
using System;
using System.ServiceModel;

///<summary>
/// Class Name : IKKCARStatusService
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.WCFService
{
    [ServiceContract(Namespace = Constants.ServicesNamespace.StatusService)]
    public interface IKKCARStatusService : IDisposable
    {
        [OperationContract]
        InsertStatusResponse CARInsertStatus(InsertStatusRequest request);
    }
}
