using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class IntakeMethodRepository : BaseLookupRepository<IntakeMethod>, IIntakeMethodRepository
    {
        public IntakeMethodRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IIntakeMethodRepository : IBaseLookupRepository<IntakeMethod>
    {
    }
}
