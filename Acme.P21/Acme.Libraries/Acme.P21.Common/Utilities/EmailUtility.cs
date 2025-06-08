using System.Collections.Generic;
using System.Net.Mail;
using Acme.P21.Common.Logging;

namespace Acme.P21.Common.Utilities
{
    internal class EmailUtility
    {
        #region Constructors

        internal EmailUtility(ILoggingService logger, string smtpServer)
        {
            Logger = logger;
            SmtpServer = smtpServer;
        }

        #endregion

        #region Private Properties

        private ILoggingService Logger
        {
            get;
        }

        private string SmtpServer
        {
            get;
        }

        #endregion

        #region Internal Methods

        internal bool SendMail(string toAddress, string fromAddress, List<string> ccAddress, string subject, string emailBody, Attachment attachment = null)
        {
            return SendMail(toAddress, fromAddress, ccAddress, subject, emailBody, null, null, null, attachment);
        }

        internal bool SendMail(string toAddress, string fromAddress, List<string> ccAddress, string subject, string emailBody, string toAddressDisplay = null, string fromAddressDisplay = null, string bccAddress = null, Attachment attachment = null)
        {
            try
            {
                var fromMailAddress = fromAddressDisplay.IsNotNullOrEmpty() ? new MailAddress(fromAddress, fromAddressDisplay) : new MailAddress(fromAddress);
                var toMailAddress = toAddressDisplay.IsNotNullOrEmpty() ? new MailAddress(toAddress, toAddressDisplay) : new MailAddress(toAddress);

                using (var emailMessage = new MailMessage(fromMailAddress, toMailAddress))
                {
                    emailMessage.Subject = subject;
                    emailMessage.Body = emailBody;
                    emailMessage.IsBodyHtml = true;
                    if (ccAddress.IsNotNullOrEmpty())
                        foreach (var address in ccAddress)
                            emailMessage.CC.Add(new MailAddress(address));

                    emailMessage.ReplyToList.Add(new MailAddress(fromAddress));
                    if (attachment.IsNotNull())
                        emailMessage.Attachments.Add(attachment);

                    var smtp = new SmtpClient { Host = SmtpServer };
                    smtp.Send(emailMessage);
                }
            }
            catch (SmtpFailedRecipientException exception)
            {
                Logger.Warn(exception, "EmailUtilityException To: {0}, From: {1}", toAddress, fromAddress);
                return false;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "EmailUtilityException To: {0}, From: {1}", toAddress, fromAddress);
                return false;
            }
            return true;
        }

        #endregion
    }
}