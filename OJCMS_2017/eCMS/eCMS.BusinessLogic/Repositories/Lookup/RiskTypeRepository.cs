using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class RiskTypeRepository : BaseLookupRepository<RiskType>, IRiskTypeRepository
    {
        public RiskTypeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IRiskTypeRepository : IBaseLookupRepository<RiskType>
    {
    }
}
