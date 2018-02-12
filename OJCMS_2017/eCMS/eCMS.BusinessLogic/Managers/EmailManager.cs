using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using System;
using System.Web.Mvc;

namespace eCMS.BusinessLogic
{
    public class EmailManager
    {
        private IWorkerRepository workerRepository;
        public EmailManager(IWorkerRepository workerRepository)
        {
            this.workerRepository = workerRepository;
        }

        public BaseModel BuildAndSendEmail(EmailTemplate emailTemplate, Worker worker)
        {
            BaseModel statusModel = new BaseModel();
            try
            {
                emailTemplate = EmailMessageBuilder.BuildEmail(emailTemplate, worker);
                try
                {
                    string senderEmailAddress = SiteConfigurationReader.FromEmail;
                    string senderName = SiteConfigurationReader.FromEmailName;
                    string receiverEmailAddress = string.Empty;
                    string receiverName = string.Empty;
                    if (worker != null)
                    {
                        receiverEmailAddress = worker.EmailAddress;
                        receiverName = worker.FirstName + " " + worker.LastName;
                    }
                    EmailSender newEmailSender = new EmailSender(senderEmailAddress, senderName, receiverEmailAddress, receiverName, emailTemplate.EmailSubject, emailTemplate.EmailBody,String.Empty);
                    if (newEmailSender.Send())
                    {
                        statusModel.SuccessMessage = "Email has been delivered successfully";
                    }
                    else
                    {
                        statusModel.ErrorMessage = "Email was not delivered successfully";
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                }
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                statusModel.ErrorMessage = Constants.Messages.UnhandelledError;
                ExceptionManager.Manage(ex);
            }
            return statusModel;
        }
    }
}
