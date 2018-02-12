using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class FinancialAssistanceCategoryRepository : BaseLookupRepository<FinancialAssistanceCategory>, IFinancialAssistanceCategoryRepository
    {
        public FinancialAssistanceCategoryRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IFinancialAssistanceCategoryRepository : IBaseLookupRepository<FinancialAssistanceCategory>
    {
    }
}
