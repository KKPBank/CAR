using System;
using System.Configuration;
using System.Web.Configuration;
using log4net;

///<summary>
/// Class Name : WebConfig
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Common.Utilities
{
    public class WebConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebConfig));

        #region "AppSettings"

        private const string EmailServerString = "Email-Server";
        private const string EmailServerPortString = "Email-Server-Port";
        private const string MailEnable = "MailEnable";
        private const string MailAuthenMethod = "MailAuthenMethod";
        private const string MailAuthenUser = "MailAuthenUser";
        private const string MailAuthenPassword = "MailAuthenPassword";
        private const string MailSenderEmail = "MailSenderEmail";
        private const string MailSenderName = "MailSenderName";
        private const string FixDestinationMail = "FixDestinationMail";
        private const string SoftwareVersion = "SoftwareVersion";
        private const string ServiceRetryNo = "ServiceRetryNo";
        private const string ServiceRetryInterval = "ServiceRetryInterval";

        private const string TaskUsername = "TaskUsername";
        private const string TaskPassword = "TaskPassword";
        private const string TotalCountToProcess = "TotalCountToProcess";
        private const string TaskEmailToAddress = "EmailToAddress";
        private const string MailTemplatesPath = "MailTemplatesPath";

        private const string BatchPathImport = "BatchPathImport";
        private const string BatchPathExport = "BatchPathExport";
        private const string BatchPathSource = "BatchPathSource";
        private const string BatchMainUrl = "BatchMainUrl";

        private const string SkipSFTP = "SkipSFTP";
        private const string BatchFilePrefix = "Batch-File-Prefix";
        private const string BatchRequestSuffix = "Batch-Request-Suffix";
        private const string BatchResponseSuffix = "Batch-Response-Suffix";
        private const string BatchSshServer = "Batch-SSH-Server";
        private const string BatchSshPort = "Batch-SSH-Port";
        private const string BatchSshUsername = "Batch-SSH-Username";
        private const string BatchSshPassword = "Batch-SSH-Password";
        private const string BatchSshRemoteDir = "Batch-SSH-RemoteDir";
        private const string NumberOfObjectsPerPage = "NumberOfObjectsPerPage";
        private const string TestDataFile = "TestDataFile";

        private const string BatchCARInsertStatus_PathImport = "BatchCARInsertStatus_PathImport";
        private const string BatchCARInsertStatus_PathArchives = "BatchCARInsertStatus_PathArchives";
        private const string BatchCARInsertStatus_IntervalDay = "BatchCARInsertStatus_IntervalDay";
        private const string BatchCARInsertStatus_File_Prefix = "BatchCARInsertStatus_File_Prefix";
        private const string BatchCARInsertStatus_SSH_Server = "BatchCARInsertStatus_SSH_Server";
        private const string BatchCARInsertStatus_SSH_Port = "BatchCARInsertStatus_SSH_Port";
        private const string BatchCARInsertStatus_SSH_Username = "BatchCARInsertStatus_SSH_Username";
        private const string BatchCARInsertStatus_SSH_Password = "BatchCARInsertStatus_SSH_Password";
        private const string BatchCARInsertStatus_SSH_RemoteDir = "BatchCARInsertStatus_SSH_RemoteDir";
        private const string BatchCARInsertStatus_EmailDisplayName = "BatchCARInsertStatus_EmailDisplayName";
        private const string BatchCARInsertStatus_EmailFromAddress = "BatchCARInsertStatus_EmailFromAddress";
        private const string BatchCARInsertStatus_EmailToAddress = "BatchCARInsertStatus_EmailToAddress";
        private const string BatchCARInsertStatus_EmailFromPassword = "BatchCARInsertStatus_EmailFromPassword";
        private const string BatchCARInsertStatus_EmailHostIP = "BatchCARInsertStatus_EmailHostIP";
        private const string BatchCARInsertStatus_EmailPort = "BatchCARInsertStatus_EmailPort";

        #endregion

        #region "Retrieve data in Web.config"

        public static string GetConnectionString(string name)
        {
            try
            {
                ConfigurationManager.RefreshSection("connectionStrings");
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch (Exception e)
            {
                log.Error(string.Format("{0}, Failed to get connection string information.", name), e);
                return string.Empty;
            }
        }

        private static string GetAppSetting(string name)
        {
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                return ConfigurationManager.AppSettings[name];
            }
            catch (Exception e)
            {
                log.Error(string.Format("{0}, Failed to get application string information.", name), e);
                return string.Empty;
            }
        }

        #endregion

        #region "Save data to Web.config"

        public static bool UpdateAppSetting(string key, string value)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                var appSettingsSection = (AppSettingsSection)config.GetSection("appSettings");

                if (appSettingsSection != null)
                {
                    appSettingsSection.Settings[key].Value = value;
                    config.Save();
                    ConfigurationManager.RefreshSection("appSettings");
                }

                return true;
            }
            catch (Exception e)
            {
                log.Error(string.Format("{0}, Failed to modify application setting information.", key), e);
                return false;
            }
        }

        #endregion

        #region "Mail Service"

        public static string GetEmailServer()
        {
            return GetAppSetting(EmailServerString);
        }

        public static bool SetEmailServer(string serverName)
        {
            return UpdateAppSetting(EmailServerString, serverName);
        }

        public static int GetEmailServerPort()
        {
            string portNumber = GetAppSetting(EmailServerPortString);
            return portNumber.ToNullable<int>() ?? 25;
        }

        public static bool SetEmailServerPort(string port)
        {
            return UpdateAppSetting(EmailServerPortString, port);
        }

        public static bool GetMailEnable()
        {
            bool MailEnableValue = true;

            if (!String.IsNullOrEmpty(GetAppSetting(MailEnable)))
            {
                MailEnableValue = Boolean.Parse(GetAppSetting(MailEnable));
            }

            return MailEnableValue;
        }

        public static string GetMailAuthenMethod()
        {
            string MailAuthenMethodValue = "default";

            if (!String.IsNullOrEmpty(GetAppSetting(MailAuthenMethod)))
            {
                MailAuthenMethodValue = GetAppSetting(MailAuthenMethod);
            }

            return MailAuthenMethodValue;
        }

        public static string GetMailAuthenUser()
        {
            string MailAuthenUserValue = string.Empty;

            if (!String.IsNullOrEmpty(GetAppSetting(MailAuthenUser)))
            {
                MailAuthenUserValue = GetAppSetting(MailAuthenUser);
            }

            return MailAuthenUserValue;
        }

        public static string GetMailAuthenPassword()
        {
            string MailAuthenPasswordValue = string.Empty;

            if (!String.IsNullOrEmpty(GetAppSetting(MailAuthenPassword)))
            {
                MailAuthenPasswordValue = GetAppSetting(MailAuthenPassword);
            }

            return MailAuthenPasswordValue;
        }

        public static string GetFixDestinationMail()
        {
            string FixDestinationMailValue = "";

            if (!String.IsNullOrEmpty(GetAppSetting(FixDestinationMail)))
            {
                FixDestinationMailValue = GetAppSetting(FixDestinationMail);
            }

            return FixDestinationMailValue;
        }

        public static string GetSenderEmail()
        {
            string senderEmail = string.Empty;
            if (!String.IsNullOrEmpty(GetAppSetting(MailSenderEmail)))
            {
                senderEmail = GetAppSetting(MailSenderEmail);
            }
            return senderEmail;
        }

        public static string GetSenderName()
        {
            string senderName = string.Empty;
            if (!String.IsNullOrEmpty(GetAppSetting(MailSenderName)))
            {
                senderName = GetAppSetting(MailSenderName);
            }
            return senderName;
        }

        #endregion

        #region "Common Appsettings"

        public static string GetSoftwareVersion()
        {
            return GetAppSetting(SoftwareVersion);
        }

        public static int GetServiceRetryInterval()
        {
            return GetAppSetting(ServiceRetryInterval).ToNullable<int>() ?? 120;
        }

        public static int GetServiceRetryNo()
        {
            return GetAppSetting(ServiceRetryNo).ToNullable<int>() ?? 3;
        }

        public static int GetTotalCountToProcess()
        {
            return GetAppSetting(TotalCountToProcess).ToNullable<int>() ?? 5;
        }

        #endregion

        #region "Batch"

        public static string GetTaskUsername()
        {
            return GetAppSetting(TaskUsername);
        }

        public static string GetTaskPassword()
        {
            return GetAppSetting(TaskPassword);
        }

        public static string GetTaskEmailToAddress()
        {
            return GetAppSetting(TaskEmailToAddress);
        }

        public static string GetMailTemplatesPath()
        {
            return GetAppSetting(MailTemplatesPath);
        }

        public static string GetBatchFilePrefix()
        {
            return GetAppSetting(BatchFilePrefix);
        }

        public static string GetBatchRequestSuffix()
        {
            return GetAppSetting(BatchRequestSuffix);
        }

        public static string GetBatchResponseSuffix()
        {
            return GetAppSetting(BatchResponseSuffix);
        }

        public static string GetBatchSshServer()
        {
            return GetAppSetting(BatchSshServer);
        }

        public static int GetBatchSshPort()
        {
            return GetAppSetting(BatchSshPort).ToNullable<int>() ?? 22;
        }

        public static string GetBatchSshUsername()
        {
            return GetAppSetting(BatchSshUsername);
        }

        public static string GetBatchSshPassword()
        {
            return GetAppSetting(BatchSshPassword);
        }

        public static string GetBatchSshRemoteDir()
        {
            return GetAppSetting(BatchSshRemoteDir);
        }
        
        public static bool GetSkipSFTP()
        {
            return GetAppSetting(SkipSFTP).ToNullable<bool>() ?? true;
        }

        public static string GetBatchPathImport()
        {
            return GetAppSetting(BatchPathImport);
        }

        public static string GetBatchPathExport()
        {
            return GetAppSetting(BatchPathExport);
        }

        public static string GetBatchPathSource()
        {
            return GetAppSetting(BatchPathSource);
        }

        public static int GetNumberOfObjectsPerPage()
        {
            return GetAppSetting(NumberOfObjectsPerPage).ToNullable<int>() ?? 1;
        }

        public static string GetTestDataFile()
        {
            return GetAppSetting(TestDataFile);
        }

        public static string GetBatchMainUrl()
        {
            return GetAppSetting(BatchMainUrl);
        }

        #endregion

        #region "BatchCARInsertStatus"
        public static string GetBatchCARInsertStatusPathImport()
        {
            string path = GetAppSetting(BatchCARInsertStatus_PathImport);
            if (System.IO.Directory.Exists(path) == false)
                System.IO.Directory.CreateDirectory(path);

            return path;
        }
        public static string GetBatchCARInsertStatusPathArchives()
        {
            string path = GetAppSetting(BatchCARInsertStatus_PathArchives);
            if (System.IO.Directory.Exists(path) == false)
                System.IO.Directory.CreateDirectory(path);

            return path;
        }
        public static int GetBatchCARInsertStatusIntervalDay()
        {
            return Convert.ToInt16(GetAppSetting(BatchCARInsertStatus_IntervalDay));
        }
        public static string GetBatchCARInsertStatusFilePrefix()
        {
            return GetAppSetting(BatchCARInsertStatus_File_Prefix);
        }
        public static string GetBatchCARInsertStatusSSHServer()
        {
            return GetAppSetting(BatchCARInsertStatus_SSH_Server);
        }
        public static int GetBatchCARInsertStatusSSHPort()
        {
            return Convert.ToInt16(GetAppSetting(BatchCARInsertStatus_SSH_Port));
        }
        public static string GetBatchCARInsertStatusSSHUsername()
        {
            return GetAppSetting(BatchCARInsertStatus_SSH_Username);
        }
        public static string GetBatchCARInsertStatusSSHPassword()
        {
            return GetAppSetting(BatchCARInsertStatus_SSH_Password);
        }
        public static string GetBatchCARInsertStatusSSHRemoteDir()
        {
            return GetAppSetting(BatchCARInsertStatus_SSH_RemoteDir);
        }
        public static string GetBatchCARInsertStatusEmailDisplayName()
        {
            return GetAppSetting(BatchCARInsertStatus_EmailDisplayName);
        }
        public static string GetBatchCARInsertStatusEmailFromAddress()
        {
            return GetAppSetting(BatchCARInsertStatus_EmailFromAddress);
        }
        public static string GetBatchCARInsertStatusEmailToAddress()
        {
            return GetAppSetting(BatchCARInsertStatus_EmailToAddress);
        }
        public static string GetBatchCARInsertStatusEmailFromPassword()
        {
            return GetAppSetting(BatchCARInsertStatus_EmailFromPassword);
        }
        public static string GetBatchCARInsertStatusEmailHostIP()
        {
            return GetAppSetting(BatchCARInsertStatus_EmailHostIP);
        }
        public static string GetBatchCARInsertStatusEmailPort()
        {
            return GetAppSetting(BatchCARInsertStatus_EmailPort);
        }

        #endregion
    }
}