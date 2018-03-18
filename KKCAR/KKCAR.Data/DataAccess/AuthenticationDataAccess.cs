using KKCAR.Entity;
using log4net;
using System.Collections.Generic;
using System.Linq;

///<summary>
/// Class Name : AuthenticationDataAccess
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
namespace KKCAR.Data.DataAccess
{
    public sealed class AuthenticationDataAccess : IAuthenticationDataAccess
    {
        private readonly KKCARContextContainer _context;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuthenticationDataAccess));

        public AuthenticationDataAccess(KKCARContextContainer context)
        {
            _context = context;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public bool CheckAuth(string systemCode, string secKey)
        {
            return _context.CAS_SYSTEM.Any(x => x.SYSTEM_ID == systemCode && x.SYSTEM_SECURITY_KEY == secKey);
        }

        public SystemInfoEntity GetSystemInfoByCode(string sysCode)
        {
            var query = from s in _context.CAS_SYSTEM
                        where sysCode.Equals(s.SYSTEM_ID)
                        select new SystemInfoEntity
                        {
                            Code = s.SYSTEM_ID,
                            Name = s.SYSTEM_NAME,
                            SecKey = s.SYSTEM_SECURITY_KEY
                        };

            return query.Any() ? query.FirstOrDefault() : null;
        }

        public string GetResponseUrlBySysCode(string sysCode, string batchCode)
        {
            var query = from bs in _context.CAR_BATCH_SYSTEM_MAPPING
                        join bt in _context.CAR_BATCH on bs.BATCH_ID equals bt.BATCH_ID
                        where bs.SYSTEM_ID == sysCode && bt.BATCH_CODE == batchCode
                        select bs.RESPONSE_URL;

            return query.Any() ? query.FirstOrDefault() : null;
        }

        public bool IsSystemExists(string sysCode)
        {
            return _context.CAS_SYSTEM.Any(x => x.SYSTEM_ID == sysCode);
        }    

        public ICollection<string> GetActiveSystemCodeList()
        {
            var query = _context.CAS_SYSTEM.Select(x => x.SYSTEM_ID);
            return query.Any() ? query.ToList() : null;
        }
    }
}
