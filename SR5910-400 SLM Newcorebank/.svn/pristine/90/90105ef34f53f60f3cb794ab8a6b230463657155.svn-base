using System;
using System.Configuration;

[assembly: CLSCompliant(true)]
namespace Cas.Common
{
    public static class AppConfig
    {
        public static int GridMaxRowPerPage
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["GRID_MAX_ROW_PER_PAGE"] ?? "10"); }
        }
        public static int MaxRowPerRequest
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MAX_ROW_PER_REQUEST"] ?? "1000"); }
        }
        public static string DbSchema
        {
            get { return ConfigurationManager.AppSettings["DBSCHEMA"] ?? ""; }
        }

        #region AD
        public static string LdapDomain
        {
            get { return ConfigurationManager.AppSettings["LDAP_DOMAIN"] ?? ""; }
        }
        #endregion

        #region Email Config Common
        public static string EmailDisplayName
        {
            get { return ConfigurationManager.AppSettings["EmailDisplayName"] ?? ""; }
        }
        public static string EmailFromAddress
        {
            get { return ConfigurationManager.AppSettings["EmailFromAddress"] ?? ""; }
        }
        public static string EmailToAddress
        {
            get { return ConfigurationManager.AppSettings["EmailToAddress"] ?? ""; }
        }
        public static string EmailFromPassword
        {
            get { return ConfigurationManager.AppSettings["EmailFromPassword"] ?? ""; }
        }
        public static string EmailHostIP
        {
            get { return ConfigurationManager.AppSettings["EmailHostIP"] ?? ""; }
        }
        public static int EmailPort
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"] ?? "25"); }
        }
        #endregion

        #region BulkCreateActivityLogService
        public static string CreateActivityLogSFTPAddress
        {
            get { return ConfigurationManager.AppSettings["SFTPAddress"] ?? ""; }
        }
        public static string CreateActivityLogSFTPUserName
        {
            get { return ConfigurationManager.AppSettings["SFTPUserName"] ?? ""; }
        }
        public static string CreateActivityLogSFTPPassword
        {
            get { return ConfigurationManager.AppSettings["SFTPPassword"] ?? ""; }
        }
        public static string CreateActivityLogSFTPSourcePath
        {
            get { return ConfigurationManager.AppSettings["SFTPSourcePath"] ?? ""; }
        }
        public static int CreateActivityLogSFTPFolderAddDate
        {
            get
            {
                try{return Convert.ToInt32(ConfigurationManager.AppSettings["SFTPFolderAddDate"]);}
                catch{return 0;}            }
        }
        public static string CreateActivityLogSFTPFolderFormatDate
        {
            get { return ConfigurationManager.AppSettings["SFTPFolderFormatDate"] ?? "yyyyMMdd"; }
        }
        public static string BulkCreateActivityLogEmailSubjectComplete
        {
            get { return ConfigurationManager.AppSettings["BulkCreateActivityLogEmailSubjectComplete"] ?? ""; }
        }
        public static string BulkCreateActivityLogEmailSubjectError
        {
            get { return ConfigurationManager.AppSettings["BulkCreateActivityLogEmailSubjectError"] ?? ""; }
        }
        public static string BulkCreateActivityLogEmailSubjectSemiComplete
        {
            get { return ConfigurationManager.AppSettings["BulkCreateActivityLogEmailSubjectSemiComplete"] ?? ""; }
        }
        public static string BulkCreateActivityLogRootPath
        {
            get { return ConfigurationManager.AppSettings["BulkCreateActivityLogRootPath"] ?? ""; }
        }
        public static string BulkCreateActivityLogRequestPath
        {
            get { return ConfigurationManager.AppSettings["BulkCreateActivityLogRequestPath"] ?? ""; }
        }

        #endregion
    }
}
