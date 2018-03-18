///<summary>
/// Class Name : IChannelDataAccess
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
    public interface IChannelDataAccess
    {
        bool IsChannelExists(string channelId);
    }
}
