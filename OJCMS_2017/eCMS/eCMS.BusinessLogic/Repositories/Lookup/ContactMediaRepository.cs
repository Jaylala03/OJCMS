using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ContactMediaRepository : BaseLookupRepository<ContactMedia>, IContactMediaRepository
    {
        public ContactMediaRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IContactMediaRepository : IBaseLookupRepository<ContactMedia>
    {
    }
}
