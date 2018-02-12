using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System.Linq;
using EasySoft.Helper;
namespace eCMS.BusinessLogic.Repositories
{
    public class GenderRepository : BaseLookupRepository<Gender>, IGenderRepository
    {
        public GenderRepository(RepositoryContext context)
            :base(context)
        {
        }

        public Gender Find(string name)
        {
            if (name.IsNotNullOrEmpty())
            {
                name = name.ToLower();
                return context.Gender.FirstOrDefault(item => item.Name.ToLower().Contains(name));
            }
            return null;
        }
    }

    public interface IGenderRepository : IBaseLookupRepository<Gender>
    {
        Gender Find(string name);
    }
}
