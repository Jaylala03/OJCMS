using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class ActivityTypeRepository : BaseLookupRepository<ActivityType>, IActivityTypeRepository
    {
        public ActivityTypeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IActivityTypeRepository : IBaseLookupRepository<ActivityType>
    {
    }
}
