using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class HighestLevelOfEducationRepository : BaseLookupRepository<HighestLevelOfEducation>, IHighestLevelOfEducationRepository
    {
        public HighestLevelOfEducationRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IHighestLevelOfEducationRepository : IBaseLookupRepository<HighestLevelOfEducation>
    {
    }
}
