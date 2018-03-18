using KKCAR.Common.Utilities;
using KKCAR.Service.Messages.Batch;
using KKCAR.Service.Messages.Common;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

///<summary>
/// Class Name : IKKCARStatusRestService
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
    [ServiceContract(Namespace = Constants.ServicesNamespace.StatusRestService)]
    public interface IKKCARStatusRestService : IDisposable
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "BatchCARInsertStatus")]
        ResponseStatus InsertStatus_Post(string system, string serviceName, string dataDate, bool getResult, string path);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "BatchCARInsertStatus?system={system}&serviceName={serviceName}&dataDate={dataDate}&getResult={getResult}&path={path}")]
        ResponseStatus InsertStatus_Get(string system, string serviceName, string dataDate, bool getResult, string path);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendResult")]
        BulkInsertStatusResponse SendResult_Post(string system, string serviceName, string dataDate, string refNo);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SendResult?system={system}&serviceName={serviceName}&dataDate={dataDate}&refNo={refNo}")]
        BulkInsertStatusResponse SendResult_Get(string system, string serviceName, string dataDate, string refNo);
    }
}
