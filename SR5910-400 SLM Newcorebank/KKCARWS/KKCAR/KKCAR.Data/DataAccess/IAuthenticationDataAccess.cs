using KKCAR.Entity;
using System.Collections.Generic;

///<summary>
/// Class Name : IAuthenticationDataAccess
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Data.DataAccess
{
    public interface IAuthenticationDataAccess
    {
        bool IsSystemExists(string sysCode);
        ICollection<string> GetActiveSystemCodeList();
        bool CheckAuth(string sysCode, string secKey);
        SystemInfoEntity GetSystemInfoByCode(string sysCode);
        string GetResponseUrlBySysCode(string sysCode, string batchCode);
    }
}
