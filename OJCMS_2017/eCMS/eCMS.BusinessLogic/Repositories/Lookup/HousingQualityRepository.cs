using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class HousingQualityRepository : BaseLookupRepository<HousingQuality>, IHousingQualityRepository
    {
        public HousingQualityRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IHousingQualityRepository : IBaseLookupRepository<HousingQuality>
    {
    }
}
