using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ImmigrationCitizenshipStatusRepository : BaseLookupRepository<ImmigrationCitizenshipStatus>, IImmigrationCitizenshipStatusRepository
    {
        public ImmigrationCitizenshipStatusRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IImmigrationCitizenshipStatusRepository : IBaseLookupRepository<ImmigrationCitizenshipStatus>
    {
    }
}
