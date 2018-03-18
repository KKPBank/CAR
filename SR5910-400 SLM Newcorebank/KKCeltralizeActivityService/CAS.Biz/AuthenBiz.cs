using Cas.Common;
using Cas.Dal;
using Cas.Dal.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cas.Biz
{
    public class AuthenBiz
    {
        string _error = "";
        public string ErrorMessage {  get { return _error; } }
        public bool CheckAuth(LogServiceHeader header, out string errorCode)
        {
            errorCode = "";
            bool ret = true;
            try
            {
                if (String.IsNullOrEmpty(header.SystemCode)) { errorCode = "CAS-E-101"; throw new ArgumentNullException("Data required: System Code"); }
                using (KKCASModel kdc = new KKCASModel())
                {
                    var sys = kdc.CAS_SYSTEM.Where(s => s.SYSTEM_ID == header.SystemCode).FirstOrDefault();
                    if (sys == null) { errorCode = "CAS-E-202"; throw new KeyNotFoundException("No permission"); }
                    if (!String.IsNullOrEmpty(sys.SYSTEM_SECURITY_KEY))
                    {
                        string enc = "";
                        if (!String.IsNullOrEmpty(header.SecurityKey)) enc = DataUtil.GetHashString(header.SecurityKey);
                        if (!sys.SYSTEM_SECURITY_KEY.Equals(enc)) { errorCode = "CAS-E-201"; throw new ArgumentException("Invalid security key."); }
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                _error = ExceptionService.GetMessage(ex);
            }
            return ret;
        }        
        public bool CheckAuth(string systemID, string serviceName)
        {
            using (KKCASModel kdc = new KKCASModel())
            {
                return kdc.CAS_ROLE.Where(r => r.SYSTEM_ID == systemID && r.CAS_SERVICE == serviceName).Any();
            }
        }
        public List<string> GetScope(string sSystem, string servcie)
        {
            using (KKCASModel kdc = new KKCASModel()) 
                return GetScope(kdc, sSystem, servcie);
        }
        public List<string> GetScope(KKCASModel kdc, string sSystem, string service)
        {
                 var lst = kdc.CAS_ROLE.Where(r => r.SYSTEM_ID == sSystem && r.CAS_SERVICE == service).ToList();
                if (lst.Count == 0) return null;
                if (lst.Where(l => l.CAS_SCOPE == null || l.CAS_SCOPE == "").Any())
                    return new List<string>() { "" };
                else
                    return lst.Select(l => l.CAS_SCOPE).ToList();
         }
    }
}
