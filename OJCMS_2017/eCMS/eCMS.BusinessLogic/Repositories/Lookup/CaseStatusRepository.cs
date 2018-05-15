using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
        public  List<SelectListItem> AllExceptCurrentDropDownList(int statusid)
        {
            return context.CaseStatus.Where(item => item.IsActive == true && item.ID != statusid).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

    }

    public interface ICaseStatusRepository : IBaseLookupRepository<CaseStatus>
    {
        IQueryable<CaseStatus> FindAllByWorkerID(int workerID) ;
        List<SelectListItem> AllExceptCurrentDropDownList(int statusid);
    }
}
