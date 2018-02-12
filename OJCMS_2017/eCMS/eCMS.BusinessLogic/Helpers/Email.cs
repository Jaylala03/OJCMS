using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.BusinessLogic.Helpers
{
    public class ExcEMail
    {
        #region Properties
        private static string userName = ConfigurationManager.AppSettings["userName"].ToString();
        private static string password = ConfigurationManager.AppSettings["password"].ToString();
        private static string mailFrom = ConfigurationManager.AppSettings["mailFrom"].ToString();
        //private static string mailFromInfo = ConfigurationManager.AppSettings["mailFromInfo"].ToString();
        //private static string mailFromNonReply = ConfigurationManager.AppSettings["mailFromNonReply"].ToString();
        //private static string mailFromSupport = ConfigurationManager.AppSettings["mailFromSupport"].ToString();
        private static string mailTo = ConfigurationManager.AppSettings["mailTo"].ToString();
        private static string bccAddress = ConfigurationManager.AppSettings["bccAddress"].ToString();
        private static string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
        private static string testMode = ConfigurationManager.AppSettings["testMode"].ToString();
        private static string commaDelimCCs = "";
        #endregion



        #region Method : SendEmail by Mohd
        public static void SendEmail(string subject, string message, bool isBodyHtml)
        {
            MailMessage msg = new MailMessage(mailFrom, mailTo, subject, message);
            msg.IsBodyHtml = isBodyHtml;
            SetBCCAddress(msg);
            SetUserCredentialAndProcessMail(msg, mailTo);
        }
        #endregion

        #region Method : SendEmail by Mohd
        public static bool SendEmail(string toMail, string subject, string message, bool isBodyHtml)
        {
            bool isSuccess = false;
            MailMessage msg = new MailMessage(mailFrom, toMail, subject, message);

            msg.IsBodyHtml = isBodyHtml;
            SetBCCAddress(msg);
            isSuccess = SetUserCredentialAndProcessMail(msg, toMail);
            return isSuccess;
        }
        #endregion

        #region Method : SendEmail by Mohd
        public static bool SendEmail(string toMail, string subject, string message, bool isBodyHtml, string mailType)
        {
            bool isSuccess = false;
            MailMessage msg = new MailMessage(mailFrom, toMail, subject, message);

            msg.IsBodyHtml = isBodyHtml;
            SetBCCAddress(msg);
            isSuccess = SetUserCredentialAndProcessMail(msg, toMail, mailType);
            return isSuccess;
        }
        #endregion

        #region Method : SendEmail by Mohd
        public static void SendEmail(string toMail, string subject, string message, bool isBodyHtml, DataTable lstAttachment)
        {
            MailMessage msg = new MailMessage(mailFrom, toMail, subject, message);
            msg.IsBodyHtml = isBodyHtml;
            if (lstAttachment != null)
            {
                if (lstAttachment.Rows.Count > 0)
                {
                    foreach (DataRow item in lstAttachment.Rows)
                    {
                        Attachment attach = new Attachment(item["FileFullPath"].ToString());
                        msg.Attachments.Add(attach);
                    }
                }
            }
            SetBCCAddress(msg);
            SetUserCredentialAndProcessMail(msg, toMail);

        }
        #endregion

        #region Method : SendEmail by Mohd
        public static void SendEmail(string[] mailTo, string subject, string message, bool isBodyHtml)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(mailFrom);

            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = isBodyHtml;

            SetBCCAddress(msg);
            foreach (string strMailTo in mailTo)
            {
                SetUserCredentialAndProcessMail(msg, strMailTo);
            }

        }
        #endregion

        #region Method SetBCCAddress
        private static void SetBCCAddress(MailMessage msg)
        {
            if (commaDelimCCs != "")
                msg.CC.Add(commaDelimCCs);
            if (bccAddress.Trim() != "")
            {
                string[] bccAddresses = bccAddress.Split(',');
                if (bccAddresses.Length > 0)
                {
                    for (int i = 0; i <= bccAddresses.Length - 1; i++)
                    {
                        msg.Bcc.Add(bccAddresses[i]);
                    }
                }
            }
        }
        #endregion

        #region Method SetUserCredentialAndProcessMail
        private static bool SetUserCredentialAndProcessMail(MailMessage msg, string mailTo)
        {
            bool isSuccess = false;
            NetworkCredential cred = new NetworkCredential(userName, password);
            SmtpClient mailClient = testMode == "1" ? new SmtpClient(smtpServer, 587) : new SmtpClient(smtpServer, 26);
            mailClient.EnableSsl = testMode == "1" ? true : false;
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = cred;
            try
            {
                if (testMode == "1")
                {
                    using (MailMessage mailMessage = new MailMessage(new MailAddress(mailFrom, "OneJamat Case Management System"), new MailAddress(mailTo)))
                    {
                        mailMessage.Subject = msg.Subject;
                        mailMessage.Body = msg.Body;
                        mailMessage.IsBodyHtml = msg.IsBodyHtml;

                        if (msg.Attachments != null && msg.Attachments.Count > 0)
                        {
                            AttachmentCollection attachment = msg.Attachments;
                            foreach (Attachment item in attachment)
                            {
                                mailMessage.Attachments.Add(item);
                            }
                        }
                        mailClient.Send(mailMessage);
                    }
                }
                else
                {
                    mailClient.Send(msg);
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return isSuccess;
        }
        #endregion

        #region Method SetUserCredentialAndProcessMail
        private static bool SetUserCredentialAndProcessMail(MailMessage msg, string mailTo, string MailType)
        {
            bool isSuccess = false;

            NetworkCredential cred = new NetworkCredential(userName, password);
            SmtpClient mailClient = testMode == "1" ? new SmtpClient(smtpServer, 587) : new SmtpClient(smtpServer, 26);
            mailClient.EnableSsl = testMode == "1" ? true : false;
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = cred;
            try
            {

                string SetMailFrom = "vijay";
                //if (MailType.ToLower() == "N".ToLower())
                //{
                //    SetMailFrom = mailFromNonReply;
                //}
                //else if (MailType.ToLower() == "I".ToLower())
                //{
                //    SetMailFrom = mailFromInfo;
                //}
                //else if (MailType.ToLower() == "S".ToLower())
                //{
                //    SetMailFrom = mailFromSupport;
                //}
                if (testMode == "1")
                {

                    MailMessage mailMessage = new MailMessage(SetMailFrom, mailTo, msg.Subject, msg.Body);
                    mailMessage.IsBodyHtml = msg.IsBodyHtml;
                    if (msg.Attachments != null && msg.Attachments.Count > 0)
                    {
                        AttachmentCollection attachment = msg.Attachments;
                        foreach (Attachment item in attachment)
                        {
                            mailMessage.Attachments.Add(item);
                        }
                    }
                    mailClient.Send(mailMessage);
                }
                else
                {
                    msg.From = new MailAddress(SetMailFrom);
                    mailClient.Send(msg);
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return isSuccess;
        }
        #endregion

    }
}
