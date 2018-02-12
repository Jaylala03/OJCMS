using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Linq;
using System.Web.Mvc;
namespace eCMS.BusinessLogic.Repositories
{
    public class MemberStatusRepository : BaseLookupRepository<MemberStatus>, IMemberStatusRepository
    {
        public MemberStatusRepository(RepositoryContext context)
            : base(context)
        {
        }

        public System.Collections.Generic.List<System.Web.Mvc.SelectListItem> AllActiveOpenForDropDownList
        {
            get
            {
                return context.MemberStatus.AsQueryable().Where(item => item.IsActive == true && item.ID!=14).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
        }

        public System.Collections.Generic.List<System.Web.Mvc.SelectListItem> AllActiveCompletedForDropDownList
        {
            get
            {
                return context.MemberStatus.AsQueryable().Where(item => item.IsActive == true && item.Name.Contains("Closed")).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
        }
    }

    public interface IMemberStatusRepository : IBaseLookupRepository<MemberStatus>
    {
        System.Collections.Generic.List<System.Web.Mvc.SelectListItem> AllActiveCompletedForDropDownList { get; }
        System.Collections.Generic.List<System.Web.Mvc.SelectListItem> AllActiveOpenForDropDownList { get; }
    }
}
