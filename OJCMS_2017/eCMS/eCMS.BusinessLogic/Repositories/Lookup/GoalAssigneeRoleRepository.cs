using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class GoalAssigneeRoleRepository : BaseLookupRepository<GoalAssigneeRole>, IGoalAssigneeRoleRepository
    {
        public GoalAssigneeRoleRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IGoalAssigneeRoleRepository : IBaseLookupRepository<GoalAssigneeRole>
    {
    }
}
