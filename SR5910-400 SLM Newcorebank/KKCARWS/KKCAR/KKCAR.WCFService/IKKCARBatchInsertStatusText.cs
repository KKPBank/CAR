using System.ServiceModel;
using KKCAR.Common.Utilities;
using KKCAR.Service.Messages.Batch;

namespace KKCAR.WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IKKCARBatchInsertStatusText" in both code and config file together.
    [ServiceContract]
    public interface IKKCARBatchInsertStatusText
    {
        [OperationContract]
        TaskInsertStatusResponse BatchInsertStatusByText();
    }
}
