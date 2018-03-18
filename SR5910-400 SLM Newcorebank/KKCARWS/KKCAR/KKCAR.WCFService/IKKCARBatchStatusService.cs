﻿using KKCAR.Common.Utilities;
using KKCAR.Service.Messages.Batch;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

///<summary>
/// Class Name : IKKCARBatchStatusService
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
    [ServiceContract(Namespace = Constants.ServicesNamespace.BatchStatusService)]
    public interface IKKCARBatchStatusService : IDisposable
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CARInsertStatus")]
        TaskInsertStatusResponse InsertStatus_Post(string system, string fileName);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CARInsertStatus?system={system}&fileName={fileName}")]
        TaskInsertStatusResponse InsertStatus_Get(string system, string fileName);

        //[OperationContract]
        //TaskInsertStatusResponse BatchInsertStatusByText();

    }
}
