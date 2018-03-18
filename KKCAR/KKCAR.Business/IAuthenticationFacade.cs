using KKCAR.Service.Messages.Common;
using System;

///<summary>
/// Class Name : IAuthenticationFacade
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
    public interface IAuthenticationFacade : IDisposable
    {
        string GetResponseUrlBySysCode(string systemCode);
        ResponseStatus VerifyServiceRequest(Header header, out bool valid);
        ResponseStatus ValidateServiceRequest(string systemCode, string serviceName, out bool valid);
    }
}
