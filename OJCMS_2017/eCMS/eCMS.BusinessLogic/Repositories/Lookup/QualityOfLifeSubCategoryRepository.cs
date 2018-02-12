using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class QualityOfLifeSubCategoryRepository : BaseLookupRepository<QualityOfLifeSubCategory>, IQualityOfLifeSubCategoryRepository
    {
        public QualityOfLifeSubCategoryRepository(RepositoryContext context)
            : base(context)
        {
        }

        public List<QualityOfLifeSubCategory> FindAllByQualityOfLifeCategoryID(int qualityOfLifeCategoryID)
        {
            return context.QualityOfLifeSubCategory.Where(item => item.QualityOfLifeCategoryID == qualityOfLifeCategoryID).ToList();
        }
    }

    public interface IQualityOfLifeSubCategoryRepository : IBaseLookupRepository<QualityOfLifeSubCategory>
    {
        List<QualityOfLifeSubCategory> FindAllByQualityOfLifeCategoryID(int qualityOfLifeCategoryID);
    }
}
