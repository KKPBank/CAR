using Cas.Common;
using Cas.Dal.BatchData;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Cas.LogServce
{
    [ServiceContract(Namespace = ServicesNamespace.CarLogService)]
    public interface ICARLogService : IDisposable
    {
        //=========================================================================================
        //=======================Batch=============================================================
        //=========================================================================================
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "BulkCreateActivityLog?systemCode={systemCode}")]
        string BulkCreateActivityLog(string systemCode);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "BulkCreateActivityLogRerun?systemCode={systemCode}&fileName={fileName}")]
        void BulkCreateActivityLogReRun(string systemCode, string fileName);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "BulkCreateActivityRerunAll")]
        void BulkCreateActivityRerunAll();
        //=========================================================================================
        //=======================HTTP==============================================================
        //=========================================================================================
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CreateActivityLog")]
        BatchResponseData CreateActivityLog(string system, string serviceName, string dataDate, string path, bool getResult);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CreateActivityLogResult")]
        BatchCreateActivityLogResponse CreateActivityLogResult(string system, string serviceName, string dataDate, string refNo);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "HttpCreateActivityLogRerun?system={system}&serviceName={serviceName}&dataDate={dataDate}&path={path}&getResult={getResult}")]
        void HttpCreateActivityLogRerun(string system, string serviceName, string dataDate, string path, bool getResult);
    }
}
