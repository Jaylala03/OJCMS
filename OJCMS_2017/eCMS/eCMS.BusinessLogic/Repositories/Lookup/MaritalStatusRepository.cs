using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class MaritalStatusRepository : BaseLookupRepository<MaritalStatus>, IMaritalStatusRepository
    {
        public MaritalStatusRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IMaritalStatusRepository : IBaseLookupRepository<MaritalStatus>
    {
    }
}
