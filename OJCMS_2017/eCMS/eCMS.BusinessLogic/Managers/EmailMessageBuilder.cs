using EasySoft.Helper;
using eCMS.DataLogic.Models;
using eCMS.ExceptionLoging;
using System.Web.Mvc;

namespace eCMS.BusinessLogic
{
    public static class EmailMessageBuilder
    {
        public static EmailTemplate BuildEmail(EmailTemplate emailTemplate, Worker worker)
        {
            if (emailTemplate != null)
            {
                if (emailTemplate.EmailBody.Contains("[Worker.") && worker != null)
                {
                    emailTemplate.EmailBody = emailTemplate.EmailBody.Replace("[Worker.FullName]", worker.FirstName+" "+worker.LastName);
                    emailTemplate.EmailBody = emailTemplate.EmailBody.Replace("[Worker.FirstName]", worker.FirstName);
                    emailTemplate.EmailBody = emailTemplate.EmailBody.Replace("[Worker.LastName]", worker.LastName);
                    emailTemplate.EmailBody = emailTemplate.EmailBody.Replace("[Worker.LoginName]", worker.LoginName);
                    emailTemplate.EmailBody = emailTemplate.EmailBody.Replace("[Worker.Password]", CryptographyHelper.Decrypt(worker.Password));
                }
            }
            else
            {
                throw new CustomException(CustomExceptionType.CommonCriticalDataNotFound, "Email template not found");
            }
            return emailTemplate;
        }
    }
}
