using KKCAR.Common.Mail;
using KKCAR.Common.Resources;
using KKCAR.Common.Utilities;
using KKCAR.Service.Messages.Batch;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Xml.Linq;

///<summary>
/// Class Name : KKCARMailSender
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Business
{
    public class KKCARMailSender : MailSender
    {
        private static XDocument _mailSubjectDoc = null;
        private const string TmpFolder = "~/Templates/Mail/";
        private static readonly KKCARMailSender EmailSender = new KKCARMailSender();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(KKCARMailSender));

        // Mail Templates
        private const string TmpNotifyBatchInsertStatusWarned = "NotifyBatchInsertStatusWarned.html";
        private const string TmpNotifyBatchInsertStatusFailed = "NotifyBatchInsertStatusFailed.html";

        public static KKCARMailSender GetKKCARMailSender()
        {
            return EmailSender;
        }

        public bool SendMail(string senderEmail, string receiverEmails, string ccEmails, string subject, string message,
            List<byte[]> attachmentStreams, List<string> attachmentFilenames)
        {
            return base.SendMail(senderEmail, receiverEmails, ccEmails, subject, message, attachmentStreams, attachmentFilenames);
        }

        public bool NotifyBatchInsertStatusFailed(string strReceivers, DateTime scheduledDate, AggregateException exception)
        {
            string senderEmail = WebConfig.GetSenderEmail();
            string senderName = WebConfig.GetSenderName();
            EmailAddress sender = new EmailAddress(senderEmail, senderName);
            List<EmailAddress> receivers = GetEmailAddresses(strReceivers);
            string schedDateTime = scheduledDate.FormatDateTime(Constants.DateTimeFormat.DefaultFullDateTime);
            string errorMsg = exception.ToErrorMessage().ToLineBreak();

            Hashtable hData = new Hashtable();
            hData.Add("SCHEDULE_DATETIME", schedDateTime);
            hData.Add("SCHEDULE_DATE", scheduledDate.FormatDateTime(Constants.DateTimeFormat.ShortDate));
            hData.Add("SCHEDULE_TIME", scheduledDate.FormatDateTime(Constants.DateTimeFormat.FullTime));
            hData.Add("ERROR_MESSAGE", errorMsg);

            string subject = GetMailSubject(Constants.MailSubject.NotifyBatchInsertStatusFailed).NamedFormat(new { timestamp = schedDateTime });
            string templateFilePath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", GetTemplatesDirectory(), TmpNotifyBatchInsertStatusFailed);

            if (!File.Exists(templateFilePath))
            {
                Logger.ErrorFormat("Mail-template ({0}) not found, notify's email can not send", TmpNotifyBatchInsertStatusFailed);
                return false;
            }

            string message = GenText(templateFilePath, hData);
            //Logger.InfoFormat("I:--Message Body--:Detail/{0}", message);

            return base.SendMail(sender, receivers, null, subject, message, null, null);
        }

        public bool NotifyBatchInsertStatusFailed(string strReceivers, DateTime scheduledDate, string errorMsg)
        {
            string senderEmail = WebConfig.GetSenderEmail();
            string senderName = WebConfig.GetSenderName();
            EmailAddress sender = new EmailAddress(senderEmail, senderName);
            List<EmailAddress> receivers = GetEmailAddresses(strReceivers);
            string schedDateTime = scheduledDate.FormatDateTime(Constants.DateTimeFormat.DefaultFullDateTime);

            Hashtable hData = new Hashtable();
            hData.Add("SCHEDULE_DATETIME", schedDateTime);
            hData.Add("SCHEDULE_DATE", scheduledDate.FormatDateTime(Constants.DateTimeFormat.ShortDate));
            hData.Add("SCHEDULE_TIME", scheduledDate.FormatDateTime(Constants.DateTimeFormat.FullTime));
            hData.Add("ERROR_MESSAGE", errorMsg);

            string subject = GetMailSubject(Constants.MailSubject.NotifyBatchInsertStatusFailed).NamedFormat(new { timestamp = schedDateTime });
            string templateFilePath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", GetTemplatesDirectory(), TmpNotifyBatchInsertStatusFailed);

            if (!File.Exists(templateFilePath))
            {
                Logger.ErrorFormat("Mail-template ({0}) not found, notify's email can not send", TmpNotifyBatchInsertStatusFailed);
                return false;
            }

            string message = GenText(templateFilePath, hData);
            //Logger.InfoFormat("I:--Message Body--:Detail/{0}", message);

            return base.SendMail(sender, receivers, null, subject, message, null, null);
        }

        public bool NotifyBatchInsertStatusWarned(string strReceivers, TaskInsertStatusResponse taskResponse)
        {
            string senderEmail = WebConfig.GetSenderEmail();
            string senderName = WebConfig.GetSenderName();
            EmailAddress sender = new EmailAddress(senderEmail, senderName);
            List<EmailAddress> receivers = GetEmailAddresses(strReceivers);
            string schedDateTime = taskResponse.SchedDateTime.FormatDateTime(Constants.DateTimeFormat.DefaultFullDateTime);

            Hashtable hData = new Hashtable();
            hData.Add("SCHEDULE_DATETIME", taskResponse.SchedDateTime.FormatDateTime(Constants.DateTimeFormat.DefaultFullDateTime));
            hData.Add("SCHEDULE_DATE", taskResponse.SchedDateTime.FormatDateTime(Constants.DateTimeFormat.ShortDate));
            hData.Add("SCHEDULE_TIME", taskResponse.SchedDateTime.FormatDateTime(Constants.DateTimeFormat.FullTime));
            hData.Add("ELAPSED_TIME", string.Format(CultureInfo.InvariantCulture, "{0} (ms)", taskResponse.ElapsedTime));
            hData.Add("TASK_RESULT_TABLE", ConvertBatchFileListToHtml(taskResponse));


            string subject = GetMailSubject(Constants.MailSubject.NotifyBatchInsertStatusFailed).NamedFormat(new { timestamp = schedDateTime });
            string templateFilePath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", GetTemplatesDirectory(), TmpNotifyBatchInsertStatusWarned);

            if (!File.Exists(templateFilePath))
            {
                Logger.ErrorFormat("Mail-template ({0}) not found, notify's email can not send", TmpNotifyBatchInsertStatusWarned);
                return false;
            }

            string message = GenText(templateFilePath, hData);
            //Logger.InfoFormat("I:--Message Body--:Detail/{0}", message);

            return base.SendMail(sender, receivers, null, subject, message, null, null);
        }

        #region "Functions"

        private static string GetMailSubject(string templateSubject)
        {
            try
            {
                if (_mailSubjectDoc == null)
                {
                    string template = string.Format(CultureInfo.InvariantCulture, "{0}/MailSubjectTemplate.xml", GetTemplatesDirectory());
                    if (!File.Exists(template))
                    {
                        throw new ArgumentException("Mail subject template file not found.");
                    }

                    _mailSubjectDoc = XDocument.Load(template);
                }

                return (from fn in _mailSubjectDoc.Descendants("template")
                        where fn.Attribute("name").Value.ToUpper(CultureInfo.InvariantCulture) == templateSubject.ToUpper(CultureInfo.InvariantCulture)
                        select fn.Element("subject").Value).FirstOrDefault<string>();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                throw;
            }
        }

        private static List<EmailAddress> GetEmailAddresses(string strReceivers)
        {
            List<EmailAddress> receivers = null;
            IList<object> results = StringHelpers.ConvertStringToList(strReceivers, ';');

            if (results != null)
            {
                receivers = (from str in results
                             select new EmailAddress
                             {
                                 Address = (string)str,
                                 Name = (string)str
                             }).ToList();
            }

            return receivers;
        }

        private static string ConvertBatchFileListToHtml(TaskInsertStatusResponse taskResponse)
        {
            var taskResults = taskResponse.TaskResults;
            StringBuilder sb = new StringBuilder();
            const string tmplCol = "<TD {0}>{1}</TD>";

            if (taskResults != null && taskResults.Count > 0)
            {
                for (int i = 0; i <= taskResults.Count - 1; i++)
                {
                    var taskResult = taskResults[i];
                    if (!string.IsNullOrWhiteSpace(taskResult.FileName))
                    {
                        sb.Append("<TR>\n");
                        sb.AppendFormat(tmplCol, "style='width:30%!important;'", taskResult.FileName);
                        sb.AppendFormat(tmplCol, "style='width:10%!important;'", (taskResult.TotalRecords == 0 ? "-" : taskResult.TotalRecords.FormatNumber()));
                        sb.AppendFormat(tmplCol, "style='width:10%!important;'", (taskResult.NumOfComplete == 0 ? "-" : taskResult.NumOfComplete.FormatNumber()));
                        sb.AppendFormat(tmplCol, "style='width:10%!important;'", (taskResult.NumOfError == 0 ? "-" : taskResult.NumOfError.FormatNumber()));
                        //sb.AppendFormat(tmplCol, "style='width:40%!important;'", (!string.IsNullOrWhiteSpace(taskResult.ErrorMessage) ? taskResult.ErrorMessage : "-"));
                        sb.Append("</TR>\n");
                    }
                }
            }
            else
            {
                sb.Append("<TR>\n");
                sb.AppendFormat("<TD colspan='5' class='text-center'>{0}</TD>", Resource.Msg_NoRecords);
                sb.Append("</TR>\n");
            }

            return sb.ToString();
        }

        private static string GetTemplatesDirectory()
        {
            return HostingEnvironment.MapPath(TmpFolder);
        }

        #endregion
    }
}
