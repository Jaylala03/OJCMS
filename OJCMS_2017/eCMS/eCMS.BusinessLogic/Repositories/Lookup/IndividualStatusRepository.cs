using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class IndividualStatusRepository : BaseLookupRepository<IndividualStatus>, IIndividualStatusRepository
    {
        public IndividualStatusRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IIndividualStatusRepository : IBaseLookupRepository<IndividualStatus>
    {
    }
}
