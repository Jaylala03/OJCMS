using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.WebControls;
using EasySoft.Helper;
using eCMS.DataLogic.Models;
using eCMS.Shared;
using eCMS.ExceptionLoging;

namespace eCMS.BusinessLogic
{
    public class EmailSender : IDisposable
    {
        bool isDisposed = false;
        private string attachments = string.Empty;
        public EmailSender(string from, string to, string subject, string body)
        {
            isDisposed = false;
            From = new KeyValuePair<string,string>(from,string.Empty);
            To.Add(new KeyValuePair<string,string>(to,string.Empty));
            Subject = subject;
            Body = body;
        }

        public EmailSender(string from, string fromName, string to, string toName, string subject, string body, string attachments)
        {
            isDisposed = false;
            From = new KeyValuePair<string, string>(from, fromName);
            To.Add(new KeyValuePair<string, string>(to, toName));
            Attachments = new EmailAttachments();
            Subject = subject;
            Body = body;
            string []attachmentArray = attachments.ToStringArray(',');
            if (attachmentArray != null)
            {
                foreach (string attachment in attachmentArray)
                {
                    Attachments.Add(attachment);
                }
            }
        }

        public EmailSender(string from, string[] to, string subject, string body)
        {
            isDisposed = false;
            From = new KeyValuePair<string, string>(from, string.Empty);
            if (to != null)
            {
                foreach (string toItem in to)
                {
                    To.Add(new KeyValuePair<string, string>(toItem, string.Empty));
                }
            }
            Subject = subject;
            Body = body;
        }

        ~EmailSender()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                lstTo.Clear();
                lstCc.Clear();
                lstBcc.Clear();
                lstAttachments.Clear();
                lstLinkedResources.Clear();
                Subject = string.Empty;
                Body = string.Empty;
            }
        }

        //EmailTo lstTo = new EmailTo();
        List<KeyValuePair<string, string>> lstTo = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> lstCc = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> lstBcc = new List<KeyValuePair<string, string>>();
        //EmailCc lstCc = new EmailCc();
        //EmailBcc lstBcc = new EmailBcc();
        EmailAttachments lstAttachments = new EmailAttachments();
        EmailLinkedResources lstLinkedResources = new EmailLinkedResources();
        private string errorMessage = String.Empty;

        public KeyValuePair<string, string> From
        {
            set;
            get;
        }

        public List<KeyValuePair<string, string>> To
        {
            set
            {
                lstTo = value;
            }
            get
            {
                return lstTo;
            }
        }

        public List<KeyValuePair<string, string>> Cc
        {
            set
            {
                lstCc = value;
            }
            get
            {
                return lstCc;
            }
        }

        public List<KeyValuePair<string, string>> Bcc
        {
            set
            {
                lstBcc = value;
            }
            get
            {
                return lstBcc;
            }
        }

        public EmailAttachments Attachments
        {
            set
            {
                lstAttachments = value;
            }
            get
            {
                return lstAttachments;
            }
        }

        public EmailLinkedResources LinkedResources
        {
            set
            {
                lstLinkedResources = value;
            }
            get
            {
                return lstLinkedResources;
            }
        }

        public String Subject
        {
            set;
            get;
        }

        public String Body
        {
            get;
            set;
        }

        public Int32 EmailId
        {
            get;
            set;
        }

        public bool Send()
        {
            if (SiteConfigurationReader.EnableEmailSystem)
            {
                if (From.Key.IsNotNullOrEmpty())
                {
                    if (To != null && To.Count > 0)
                    {
                        if (Subject != String.Empty)
                        {
                            if (Body != String.Empty)
                            {
                                MailMessage mailMessage = new MailMessage();

                                MailAddress mailAddress = null;
                                if (From.Value.IsNotNullOrEmpty())
                                {
                                    mailAddress = new MailAddress(From.Key, From.Value);
                                }
                                else
                                {
                                    mailAddress = new MailAddress(From.Key);
                                }
                                mailMessage.From = mailAddress;
                                //mailMessage.ReplyTo = addressFrom;

                                //Adding receipient email addresses
                                foreach (var addressTo in To)
                                {
                                    try
                                    {
                                        if (addressTo.Value.IsNotNullOrEmpty())
                                        {
                                            mailAddress = new MailAddress(addressTo.Key, addressTo.Value);
                                        }
                                        else
                                        {
                                            mailAddress = new MailAddress(addressTo.Key);
                                        }
                                        mailMessage.To.Add(mailAddress);
                                    }
                                    catch
                                    {
                                        errorMessage = errorMessage + "Invalid Email";
                                        continue;
                                    }
                                }
                                mailMessage.Subject = Subject;

                                //CC
                                if (Cc != null && Cc.Count > 0)
                                {
                                    foreach (var addressCc in Cc)
                                    {
                                        try
                                        {
                                            if (addressCc.Value.IsNotNullOrEmpty())
                                            {
                                                mailAddress = new MailAddress(addressCc.Key, addressCc.Value);
                                            }
                                            else
                                            {
                                                mailAddress = new MailAddress(addressCc.Key);
                                            }
                                            mailMessage.CC.Add(mailAddress);
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                }

                                //Bcc
                                if (Bcc != null && Bcc.Count > 0)
                                {
                                    foreach (var addressBcc in Bcc)
                                    {
                                        try
                                        {
                                            if (addressBcc.Value.IsNotNullOrEmpty())
                                            {
                                                mailAddress = new MailAddress(addressBcc.Key, addressBcc.Value);
                                            }
                                            else
                                            {
                                                mailAddress = new MailAddress(addressBcc.Key);
                                            }
                                            mailMessage.Bcc.Add(mailAddress);
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                }

                                //Add Mail Attachments
                                long totalAttachmentSize = 0;
                                if (Attachments != null && Attachments.Count>0)
                                {
                                    foreach (String attachmentPath in Attachments)
                                    {
                                        try
                                        {
                                            FileInfo fileInfo = new FileInfo(attachmentPath);
                                            if (fileInfo.Exists)
                                            {
                                                totalAttachmentSize += fileInfo.Length;
                                                Attachment newAttachment = new Attachment(attachmentPath);
                                                mailMessage.Attachments.Add(newAttachment);
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                }
                                //if (totalAttachmentSize > EmailSettings.AttachmentSize)
                                //{
                                //    throw new CustomException(CustomExceptionType.EmailInvalidAttachment, string.Empty);
                                //}
                                //Add Email Banner Image
                                if (LinkedResources != null && LinkedResources.Count>0)
                                {
                                    AlternateView avMailMessage = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);
                                    foreach (ListItem maillinkedResource in LinkedResources)
                                    {
                                        try
                                        {
                                            string resourcePath = maillinkedResource.Text;
                                            string contentID = maillinkedResource.Value;
                                            if (resourcePath != "" && contentID != "")
                                            {
                                                LinkedResource newLinkedResource = new LinkedResource(resourcePath);
                                                newLinkedResource.ContentId = contentID;
                                                newLinkedResource.ContentType.Name = resourcePath;
                                                avMailMessage.LinkedResources.Add(newLinkedResource);
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                    mailMessage.AlternateViews.Add(avMailMessage);
                                }

                                mailMessage.Body = Body;
                                mailMessage.IsBodyHtml = true;

                                SmtpClient mailSender = new SmtpClient();                                
                                if (mailMessage.To.Count > 0 && SiteConfigurationReader.EnableEmailSystem)
                                {
                                    try
                                    {
                                        mailSender.Send(mailMessage);
                                        return true;
                                    }
                                    catch (SmtpFailedRecipientsException ex)
                                    {
                                        throw new CustomException(CustomExceptionType.EmailInvalidRecipient, string.Empty, ex);
                                    }
                                    catch (SmtpFailedRecipientException ex)
                                    {
                                        throw new CustomException(CustomExceptionType.EmailInvalidRecipient, string.Empty, ex);
                                    }
                                    catch (SmtpException ex)
                                    {
                                        throw new CustomException(CustomExceptionType.EmailSMTPNotResponding, string.Empty, ex);
                                    }
                                    finally
                                    {
                                        mailSender = null;
                                        mailMessage.Dispose();
                                    }
                                }
                                else
                                {
                                    throw new CustomException(CustomExceptionType.EmailRecipientNotFound, string.Empty);
                                }
                            }
                            else
                            {
                                throw new CustomException(CustomExceptionType.EmailBodyNotFound, string.Empty);
                            }
                        }
                        else
                        {
                            throw new CustomException(CustomExceptionType.EmailSubjectNotFound, string.Empty);
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomExceptionType.EmailRecipientNotFound, string.Empty);
                    }
                }
                else
                {
                    throw new CustomException(CustomExceptionType.EmailSenderNotFound, string.Empty);
                }
            }
            else
            {
                throw new CustomException(CustomExceptionType.EmailSystemDisabled, string.Empty);
            }
        }

        public bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // replace with proper validation
            CustomException newCustomException = new CustomException(CustomExceptionType.EmailSMTPNotResponding, "Certificate Validation Error", sslPolicyErrors.ToString()+Environment.NewLine+certificate.ToString(),null);
            ExceptionManager.Manage(newCustomException);
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else
                return false;
        }


    }

    public class EmailLinkedResources : CollectionBase
    {
        public int Add(string contentID, string contentResourcePath)
        {
            if (contentID != String.Empty && contentResourcePath != String.Empty)
            {
                ListItem mailResource = new ListItem(contentResourcePath, contentID);
                return List.Add(mailResource);
            }
            else
            {
                return 0;
            }
        }
    }

    public class EmailAttachments : CollectionBase
    {
        public int Add(string AttachmentResourcePath)
        {
            if (AttachmentResourcePath.Trim() != String.Empty)
            {
                return List.Add(AttachmentResourcePath);
            }
            else
            {
                return 0;
            }
        }
    }

    public class EmailTo : CollectionBase
    {
        public int Add(string To)
        {
            if (To.Trim() != String.Empty)
            {
                return List.Add(To);
            }
            else
            {
                return 0;
            }
        }
    }

    public class EmailCc : CollectionBase
    {
        public int Add(string Cc)
        {
            if (Cc.Trim() != String.Empty)
            {
                return List.Add(Cc);
            }
            else
            {
                return 0;
            }
        }
    }

    public class EmailBcc : CollectionBase
    {
        public int Add(string Bcc)
        {
            if (Bcc.Trim() != String.Empty)
            {
                return List.Add(Bcc);
            }
            else
            {
                return 0;
            }
        }
    }
}