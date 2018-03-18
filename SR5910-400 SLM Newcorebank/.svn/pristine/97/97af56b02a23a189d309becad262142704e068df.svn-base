using log4net;
using System.Linq;
///<summary>
/// Class Name : ChannelDataAccess
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
    public class ChannelDataAccess : IChannelDataAccess
    {
        private readonly KKCARContextContainer _context;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ChannelDataAccess));

        public ChannelDataAccess(KKCARContextContainer context)
        {
            _context = context;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public bool IsChannelExists(string channelId)
        {
            return _context.CAS_CHANNEL.Any(x => x.CHANNEL_ID == channelId);
        }
    }
}
