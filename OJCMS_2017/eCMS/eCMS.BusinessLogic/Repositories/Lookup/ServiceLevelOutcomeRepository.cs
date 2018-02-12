using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ServiceLevelOutcomeRepository : BaseLookupRepository<ServiceLevelOutcome>, IServiceLevelOutcomeRepository
    {
        public ServiceLevelOutcomeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IServiceLevelOutcomeRepository : IBaseLookupRepository<ServiceLevelOutcome>
    {
    }
}
