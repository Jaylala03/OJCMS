using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class EmailTemplateRepository : BaseRepository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(RepositoryContext context)
            : base(context)
        {
        }

        public EmailTemplate FindByEmailTemplateCategoryID(int emailtemplatecategoryID)
        {
            return context.EmailTemplate.SingleOrDefault(item => item.EmailTemplateCategoryID == emailtemplatecategoryID);
        }

        public EmailTemplate FindAllByEmailTemplateCategoryName(string emailtemplatecategoryName)
        {
            //return context.EmailTemplate.Where(item => item.EmailTemplateCategoryName == emailtemplatecategoryName).FirstOrDefault();
            return context.EmailTemplate.FirstOrDefault(x => x.Name == emailtemplatecategoryName);
        }

        public void InsertOrUpdate(EmailTemplate emailtemplate)
        {
            emailtemplate.LastUpdateDate = DateTime.Now;
            if (emailtemplate.ID == default(int))
            {
                emailtemplate.CreateDate = emailtemplate.LastUpdateDate;
                emailtemplate.CreatedByWorkerID = emailtemplate.LastUpdatedByWorkerID;
                // New entity
                context.EmailTemplate.Add(emailtemplate);
            }
            else
            {
                // Existing entity
                context.Entry(emailtemplate).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<SelectListItem> FindAllByWorkerID(int WorkerID)
        {
            return context.EmailTemplate.Where(item => item.CreatedByWorkerID == WorkerID && item.EmailTemplateCategory.IsSystem==false).AsEnumerable().Select(item => new SelectListItem { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

    }

    public interface IEmailTemplateRepository : IBaseRepository<EmailTemplate>
    {
        EmailTemplate FindByEmailTemplateCategoryID(int emailtemplatecategoryID);
        EmailTemplate FindAllByEmailTemplateCategoryName(string emailtemplatecategoryName);
        List<SelectListItem> FindAllByWorkerID(int WorkerID);
        void InsertOrUpdate(EmailTemplate emailtemplate);
    }
}
