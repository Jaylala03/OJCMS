using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class SavingsRepository : BaseLookupRepository<Savings>, ISavingsRepository
    {
        public SavingsRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface ISavingsRepository : IBaseLookupRepository<Savings>
    {
    }
}
