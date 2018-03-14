using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Linq;
using EasySoft.Helper;
namespace eCMS.BusinessLogic.Repositories
{
    public class IndicatorTypeRepository : BaseLookupRepository<Gender>, IIndicatorTypeRepository
    {
        public IndicatorTypeRepository(RepositoryContext context)
            :base(context)
        {
        }

        public IndicatorType Find(string name)
        {
            if (name.IsNotNullOrEmpty())
            {
                name = name.ToLower();
                return context.IndicatorType.FirstOrDefault(item => item.Name.ToLower().Contains(name));
            }
            return null;
        }

        public IQueryable<IndicatorType> GetAll()
        {
            return context.IndicatorType.Where(a => a.IsActive);
        }
    }

    public interface IIndicatorTypeRepository : IBaseLookupRepository<Gender>
    {
        IndicatorType Find(string name);
        IQueryable<IndicatorType> GetAll();
    }
}
