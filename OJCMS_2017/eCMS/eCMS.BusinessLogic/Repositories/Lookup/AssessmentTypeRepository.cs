using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class AssessmentTypeRepository : BaseLookupRepository<AssessmentType>, IAssessmentTypeRepository
    {
        public AssessmentTypeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IAssessmentTypeRepository : IBaseLookupRepository<AssessmentType>
    {
    }
}
