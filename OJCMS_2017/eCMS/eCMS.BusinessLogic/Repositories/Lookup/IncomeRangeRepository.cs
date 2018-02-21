using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class IncomeRangeRepository : BaseLookupRepository<IncomeRange>, IIncomeRangeRepository
    {
        public IncomeRangeRepository(RepositoryContext context)
            : base(context)
        {
            
        }
        public IQueryable<IncomeRange> GetAll()
        {
            return context.IncomeRange.Where(a => a.IsActive);
        }
    }

    public interface IIncomeRangeRepository : IBaseLookupRepository<IncomeRange>
    {
        IQueryable<IncomeRange> GetAll();

    }
}
