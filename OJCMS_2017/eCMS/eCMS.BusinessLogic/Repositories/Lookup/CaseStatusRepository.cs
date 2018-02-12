using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class CaseStatusRepository : BaseLookupRepository<CaseStatus>, ICaseStatusRepository
    {
        public CaseStatusRepository(RepositoryContext context)
            : base(context)
        {     }
            public IQueryable<CaseStatus> FindAllByWorkerID(int workerID)
        {
            return context.CaseWorker.Join(context.CaseStatus, left => left.WorkerID, right => right.ID, (left, right) => new { left, right }).
                Where(item => item.left.WorkerID == workerID).Select(item => item.right);
        }         
    }

    public interface ICaseStatusRepository : IBaseLookupRepository<CaseStatus>
    {
        IQueryable<CaseStatus> FindAllByWorkerID(int workerID) ;
    }
}
