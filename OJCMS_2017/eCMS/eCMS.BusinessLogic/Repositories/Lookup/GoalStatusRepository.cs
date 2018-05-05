using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class GoalStatusRepository : BaseLookupRepository<GoalStatus>, IGoalStatusRepository
    {
        public GoalStatusRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IGoalStatusRepository : IBaseLookupRepository<GoalStatus>
    {
    }
}
