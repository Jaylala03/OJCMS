using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class EthnicityRepository : BaseLookupRepository<Ethnicity>, IEthnicityRepository
    {
        public EthnicityRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IEthnicityRepository : IBaseLookupRepository<Ethnicity>
    {
    }
}
