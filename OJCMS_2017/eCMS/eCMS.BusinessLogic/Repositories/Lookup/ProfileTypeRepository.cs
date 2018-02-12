using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ProfileTypeRepository : BaseLookupRepository<ProfileType>, IProfileTypeRepository
    {
        public ProfileTypeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IProfileTypeRepository : IBaseLookupRepository<ProfileType>
    {
    }
}
