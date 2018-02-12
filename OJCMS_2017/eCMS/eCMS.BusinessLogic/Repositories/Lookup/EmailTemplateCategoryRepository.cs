
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class EmailTemplateCategoryRepository : BaseLookupRepository<EmailTemplateCategory>, IEmailTemplateCategoryRepository
    {
        public EmailTemplateCategoryRepository(RepositoryContext context)
            : base(context)
        {
        }

        public void InsertOrUpdate(EmailTemplateCategory emailtemplatecategory)
        {
            emailtemplatecategory.LastUpdateDate = DateTime.Now;
            if (emailtemplatecategory.ID == default(int))
            {
                emailtemplatecategory.CreateDate = emailtemplatecategory.LastUpdateDate;
                // New entity
                context.EmailTemplateCategory.Add(emailtemplatecategory);
            }
            else
            {
                // Existing entity
                context.Entry(emailtemplatecategory).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public EmailTemplateCategory FindByName(string name)
        {
            return context.EmailTemplateCategory.FirstOrDefault(item=>item.Name==name);
        }

    }

    public interface IEmailTemplateCategoryRepository : IBaseLookupRepository<EmailTemplateCategory>
    {
        void InsertOrUpdate(EmailTemplateCategory emailtemplatecategory);
        EmailTemplateCategory FindByName(string name);
    }
}
