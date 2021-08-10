using System.Configuration;
using System.Net.Mail;

namespace ApplicationCode
{
    public class MailHandler
    {
        private static SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["mailServerName"]);

        public static void SendMail(string senderAddress, string[] emailRecipients, string subject, string messageBody)
        {
            MailMessage Mail = new MailMessage();
            foreach (string recipientAddress in emailRecipients)
            {
                Mail.To.Add(recipientAddress);
            }
            Mail.From = new MailAddress(senderAddress);
            Mail.Subject = subject;
            Mail.Body = messageBody;
            Mail.IsBodyHtml = true;

            smtpClient.Send(Mail);
        }

        public static void SendMail(string senderAddress, string emailRecipient, string subject, string messageBody)
        {
            string[] emailRecipients = { emailRecipient };
            SendMail(senderAddress, emailRecipients, subject, messageBody);
        }
    }
}