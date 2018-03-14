//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EasySoft.Helper;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using Kendo.Mvc;
using eCMS.Shared;
using Kendo.Mvc.Extensions;
using System.Text;
using eCMS.DataLogic.ViewModels;


namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class AssesmentIndicatorsRepository : BaseRepository<AssesmentIndicators>, IAssesmentIndicatorsRepository
    {

        public AssesmentIndicatorsRepository(RepositoryContext context)
            : base(context)
        {
        }

        public void InsertOrUpdate(AssesmentIndicators indicators)
        {
            indicators.LastUpdateDate = DateTime.Now;
            if (indicators.ID == default(int))
            {
                //set the date when this record was created
                indicators.CreatedByWorkerID = indicators.LastUpdatedByWorkerID;
                indicators.CreateDate = indicators.LastUpdateDate;
                //add a new record to database
                context.AssesmentIndicators.Add(indicators);
            }
            else
            {
                indicators.CreatedByWorkerID = indicators.LastUpdatedByWorkerID;
                indicators.CreateDate = indicators.LastUpdateDate;
                //update an existing record to database
                context.Entry(indicators).State = System.Data.Entity.EntityState.Modified;
            }
        }
    }

    public interface IAssesmentIndicatorsRepository : IBaseRepository<AssesmentIndicators>
    {
        void InsertOrUpdate(AssesmentIndicators indicators);
    }

}
