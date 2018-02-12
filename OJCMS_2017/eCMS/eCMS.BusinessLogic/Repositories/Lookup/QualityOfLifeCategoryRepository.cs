using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class QualityOfLifeCategoryRepository : BaseLookupRepository<QualityOfLifeCategory>, IQualityOfLifeCategoryRepository
    {
        public QualityOfLifeCategoryRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IQualityOfLifeCategoryRepository : IBaseLookupRepository<QualityOfLifeCategory>
    {
    }
}
