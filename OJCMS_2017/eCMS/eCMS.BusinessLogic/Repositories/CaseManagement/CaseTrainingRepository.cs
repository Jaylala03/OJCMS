using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    public class CaseTrainingRepository : BaseRepository<TrainingModule>, ICaseTrainingRepository
    {
        public CaseTrainingRepository(RepositoryContext context)
            : base(context)
        {
        }



        public void InsertOrUpdate(TrainingModule trainingModule)
        {


            trainingModule.Created_date = DateTime.UtcNow;
            context.TrainingModule.Add(trainingModule);
            
            Save();

        }

        public DataSourceResult Search(DataSourceRequest dsRequest)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

          
            DataSourceResult dsResult = (from c in context.TrainingModule
                                         select c).OrderByDescending(item => item.Created_date).ToDataSourceResult(dsRequest);
            return dsResult;
        }

    }

    public interface ICaseTrainingRepository : IBaseRepository<TrainingModule>
    {
        void InsertOrUpdate(TrainingModule trainingModule);
        DataSourceResult Search(DataSourceRequest dsRequest);
       
    }
}
