using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using eCMS.DataLogic.Models;

namespace eCMS.BusinessLogic.Repositories
{
    public class SmartGoalRepository : BaseLookupRepository<SmartGoal>, ISmartGoalRepository
    {
        public SmartGoalRepository(RepositoryContext context)
            : base(context)
        {
        }

        public List<CaseSmartGoalAssignment> FindAllByCategoryID(int QualityOfLifeCategoryID)
        {
            return context.SmartGoal.Where(item => item.QualityOfLifeCategoryID == QualityOfLifeCategoryID && item.IsActive == true).OrderBy(item => item.Name).ToList().AsEnumerable().Select(item => new CaseSmartGoalAssignment() { SmartGoalID=item.ID, SmartGoalName=item.Name }).ToList();
        }
    }

    public interface ISmartGoalRepository : IBaseLookupRepository<SmartGoal>
    {
        List<CaseSmartGoalAssignment> FindAllByCategoryID(int QualityOfLifeCategoryID);
    }
}
