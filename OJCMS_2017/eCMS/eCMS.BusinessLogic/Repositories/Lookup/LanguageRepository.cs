using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class LanguageRepository : BaseLookupRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface ILanguageRepository : IBaseLookupRepository<Language>
    {
    }
}
