using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ContactMethodRepository : BaseLookupRepository<ContactMethod>, IContactMethodRepository
    {
        public ContactMethodRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IContactMethodRepository : IBaseLookupRepository<ContactMethod>
    {
    }
}
