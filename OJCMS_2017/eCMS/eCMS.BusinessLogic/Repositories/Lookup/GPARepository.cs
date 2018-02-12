using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class GPARepository : BaseLookupRepository<GPA>, IGPARepository
    {
        public GPARepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IGPARepository : IBaseLookupRepository<GPA>
    {
    }
}
