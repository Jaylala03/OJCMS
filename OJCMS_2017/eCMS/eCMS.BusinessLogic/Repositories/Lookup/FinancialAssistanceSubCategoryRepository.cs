using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class FinancialAssistanceSubCategoryRepository : BaseLookupRepository<FinancialAssistanceSubCategory>, IFinancialAssistanceSubCategoryRepository
    {
        public FinancialAssistanceSubCategoryRepository(RepositoryContext context)
            : base(context)
        {
        }

        public List<SelectListItem> FindAllForDropDownList(int categoryID, int? regionID)
        {
            if (regionID.HasValue && regionID.Value > 0)
            {
                return context.FinancialAssistanceSubCategory.Where(item => item.IsActive == true && item.FinancialAssistanceCategoryID == categoryID && item.RegionID == regionID.Value).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
            else
            {
                return context.FinancialAssistanceSubCategory.Where(item => item.IsActive == true && item.FinancialAssistanceCategoryID == categoryID).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            }
        }
    }

    public interface IFinancialAssistanceSubCategoryRepository : IBaseLookupRepository<FinancialAssistanceSubCategory>
    {
        List<SelectListItem> FindAllForDropDownList(int categoryID, int? regionID);
    }
}
