using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class HearingSourceRepository : BaseLookupRepository<HearingSource>, IHearingSourceRepository
    {
        public HearingSourceRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IHearingSourceRepository : IBaseLookupRepository<HearingSource>
    {
    }
}
