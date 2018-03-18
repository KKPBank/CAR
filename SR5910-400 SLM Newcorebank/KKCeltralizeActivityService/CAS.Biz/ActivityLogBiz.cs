using Cas.Dal;
using System;

namespace Cas.Biz
{
    public class ActivityLogBiz
    {
        public static bool InsertServiceActivityLog(CAS_SERVICE_ACTIVITYLOG act, out string error)
        {
            bool ret = true;
            error = "";
            try
            {
                using (KKCASModel kdc = new KKCASModel())
                {
                    act.REQUEST_DATETIME = DateTime.Now;
                    kdc.CAS_SERVICE_ACTIVITYLOG.Add(act);
                    kdc.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ret = false;
                error = ex.Message;
            }
            return ret;
        }
    }
}
