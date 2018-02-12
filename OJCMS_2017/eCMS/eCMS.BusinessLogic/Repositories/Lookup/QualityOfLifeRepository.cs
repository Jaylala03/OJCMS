using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class QualityOfLifeRepository : BaseLookupRepository<QualityOfLife>, IQualityOfLifeRepository
    {
        public QualityOfLifeRepository(RepositoryContext context)
            : base(context)
        {
        }

        public List<QualityOfLife> FindAllByQualityOfLifeSubCategoryID(int qualityOfLifeSubCategoryID)
        {
            return context.QualityOfLife.Where(item => item.QualityOfLifeSubCategoryID == qualityOfLifeSubCategoryID).ToList();
        }
    }

    public interface IQualityOfLifeRepository : IBaseLookupRepository<QualityOfLife>
    {
        List<QualityOfLife> FindAllByQualityOfLifeSubCategoryID(int qualityOfLifeSubCategoryID);
    }
}
