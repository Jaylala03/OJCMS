using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ReasonsForDischargeRepository : BaseLookupRepository<ReasonsForDischarge>, IReasonsForDischargeRepository
    {
        public ReasonsForDischargeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IReasonsForDischargeRepository : IBaseLookupRepository<ReasonsForDischarge>
    {
    }
}
