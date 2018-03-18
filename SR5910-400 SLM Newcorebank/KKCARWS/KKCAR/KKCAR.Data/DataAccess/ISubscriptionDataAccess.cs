///<summary>
/// Class Name : ISubscriptionDataAccess
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
    public interface ISubscriptionDataAccess
    {
        bool IsSubsCusTypeExists(string custType);
        bool IsSubsCardTypeExists(string cardType);
    }
}
