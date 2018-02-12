using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class AnnualIncomeRepository : BaseLookupRepository<AnnualIncome>, IAnnualIncomeRepository
    {
        public AnnualIncomeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IAnnualIncomeRepository : IBaseLookupRepository<AnnualIncome>
    {
    }
}
