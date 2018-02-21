using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerNoteActivityTypeRepository : BaseLookupRepository<WorkerNoteActivityType>, IWorkerNoteActivityTypeRepository
    {
        public WorkerNoteActivityTypeRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

    public interface IWorkerNoteActivityTypeRepository : IBaseLookupRepository<WorkerNoteActivityType>
    {
    }
}
