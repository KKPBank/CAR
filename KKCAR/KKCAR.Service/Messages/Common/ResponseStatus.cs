using System.ServiceModel;

///<summary>
/// Class Name : ResponseStatus
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Service.Messages.Common
{
    [MessageContract]
    public class ResponseStatus
    {
        [MessageBodyMember]
        public string ResponseCode { get; set; }

        [MessageBodyMember]
        public string ResponseMessage { get; set; }
    }
}
