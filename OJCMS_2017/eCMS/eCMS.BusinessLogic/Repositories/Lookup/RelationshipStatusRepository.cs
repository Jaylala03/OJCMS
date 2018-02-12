using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class RelationshipStatusRepository : BaseLookupRepository<RelationshipStatus>, IRelationshipStatusRepository
    {
        public RelationshipStatusRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IRelationshipStatusRepository : IBaseLookupRepository<RelationshipStatus>
    {
    }
}
