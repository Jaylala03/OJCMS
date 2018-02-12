using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class CurrencyRepository : BaseLookupRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface ICurrencyRepository : IBaseLookupRepository<Currency>
    {
    }
}
