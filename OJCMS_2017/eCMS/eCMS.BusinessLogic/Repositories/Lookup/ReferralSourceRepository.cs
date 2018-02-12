using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ReferralSourceRepository : BaseLookupRepository<ReferralSource>, IReferralSourceRepository
    {
        public ReferralSourceRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IReferralSourceRepository : IBaseLookupRepository<ReferralSource>
    {
    }
}
