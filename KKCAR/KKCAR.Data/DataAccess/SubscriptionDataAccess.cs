using log4net;
using System.Linq;

///<summary>
/// Class Name : SubscriptionDataAccess
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
    public class SubscriptionDataAccess : ISubscriptionDataAccess
    {
        private readonly KKCARContextContainer _context;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SubscriptionDataAccess));

        public SubscriptionDataAccess(KKCARContextContainer context)
        {
            _context = context;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public bool IsSubsCusTypeExists(decimal custType)
        {
            return _context.CAS_SUBSCRIPTION_TYPE.Any(x => x.SUBSCRIPTION_TYPE_ID == custType);
        }

        public bool IsSubsCardTypeExists(string cardType)
        {
            return _context.CAS_SUBSCRIPTION_CARDTYPE.Any(x => x.SUBSCRIPTION_CARDTYPE_CODE == cardType);
        }
    }
}
