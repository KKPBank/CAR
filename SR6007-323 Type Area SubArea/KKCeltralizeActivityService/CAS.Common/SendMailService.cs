using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Cas.Common
{
    public static class SendMailService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SendMailService));        
        public static void SendMail(string MailSubject, string MailBody, string AttachmentText)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            MailAddress fromAddress = new MailAddress(AppConfig.EmailFromAddress, AppConfig.EmailDisplayName.Trim() ?? "");
            try
            {
                client.Host = AppConfig.EmailHostIP;
                client.Port = AppConfig.EmailPort;
                client.EnableSsl = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(AppConfig.EmailFromAddress, AppConfig.EmailFromPassword);
                client.Timeout = 300000;

                foreach (string mailto in AppConfig.EmailToAddress.Split(';'))
                {
                    mail.To.Add(new MailAddress(mailto.Trim()));
                }

                mail.From = fromAddress;
                mail.Subject = MailSubject;
                mail.IsBodyHtml = true;
                mail.Body = MailBody;

                if (!string.IsNullOrEmpty(AttachmentText))
                {
                    writer.Write(AttachmentText);
                    writer.Flush();
                    stream.Position = 0;     // read from the start of what was written
                    mail.Attachments.Add(new Attachment(stream, "AttachmentFile.txt", "text/plain"));
                }                
            }
            catch (Exception ex)
            {
                _logger.Error("--E:--SendMail--End--", ex);
            }
            finally
            {
                client.Send(mail);
                mail = null;
                client = null;
                fromAddress = null;
                writer.Close();
                writer = null;
                stream = null;
            }
        }
    }
}
