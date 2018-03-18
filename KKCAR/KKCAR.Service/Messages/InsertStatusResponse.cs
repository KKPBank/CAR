using KKCAR.Service.Messages.Common;
using System.ServiceModel;

///<summary>
/// Class Name : CreateStatusResponse
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Service.Messages
{
    [MessageContract]
    public class InsertStatusResponse
    {
        [MessageHeader]
        public Header Header { get; set; }

        [MessageBodyMember]
        public ResponseStatus ResponseStatus { get; set; }   

        public InsertStatusResponse()
        {
            this.ResponseStatus = new ResponseStatus();
        }
    }
}
