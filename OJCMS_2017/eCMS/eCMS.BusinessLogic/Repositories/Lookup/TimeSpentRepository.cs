using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Web.Mvc;
using System.Linq;
namespace eCMS.BusinessLogic.Repositories
{
    public class TimeSpentRepository : BaseLookupRepository<TimeSpent>, ITimeSpentRepository
    {
        public TimeSpentRepository(RepositoryContext context)
            : base(context)
        {
        }

        public override System.Collections.Generic.List<System.Web.Mvc.SelectListItem> AllActiveForDropDownList
        {
            get
            {
                return context.TimeSpent.AsQueryable().Where(item => item.IsActive == true).OrderBy(item => item.ID).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
        }
    }

    public interface ITimeSpentRepository : IBaseLookupRepository<TimeSpent>
    {
    }
}
