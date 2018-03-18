using System;
using System.DirectoryServices.AccountManagement;

namespace Cas.Common
{
    public class LdapAuthentication
    {
        #region Member
        private string domain = string.Empty;
        #endregion

        public LdapAuthentication()
        {
            domain = AppConfig.LdapDomain;
        }
        public bool checkAuthenticated(string username, string password)
        {
            PrincipalContext ctx = null;
            UserPrincipal user = null;
            PrincipalSearcher search = null;
            try
            {
                string domainAndUsername = string.Format(@"{0}\{1}", domain, username);
                ctx = new PrincipalContext(ContextType.Domain, domain, domainAndUsername, password);
                user = new UserPrincipal(ctx);
                user.SamAccountName = username;
                search = new PrincipalSearcher(user);
                UserPrincipal result = (UserPrincipal)search.FindOne();


                if (result != null)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Ldap Error", ex);
            }
            finally
            {
                ctx = null;
                user = null;
                search = null;
            }
        }
    }
}
