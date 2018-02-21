//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.DataLogic.ViewModels;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;




namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseHouseholdIncomeRepository : BaseRepository<CaseHouseholdIncome>, ICaseHouseholdIncomeRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseHouseholdIncomeRepository(RepositoryContext context)
            : base(context)
        {
           
        }

        /// <summary>
        /// Add or Update case to database
        /// </summary>
        /// <param name="case">data to save</param>
        public void InsertOrUpdate(CaseHouseholdIncome varCase)
        {
            varCase.LastUpdateDate = DateTime.Now;
            if (varCase.ID == default(int))
            {
                //set the date when this record was created
                varCase.CreateDate = varCase.LastUpdateDate;
                //set the id of the worker who has created this record
                varCase.CreatedByWorkerID = varCase.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseHouseholdIncome.Add(varCase);
            }
            else
            {
                //update an existing record to database
                context.Entry(varCase).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public DataSourceResult Search(CaseHouseholdIncome searchParameters, DataSourceRequest paramDSRequest)
        {
            return null;
        }
    }

    /// <summary>
    /// interface of Case containing necessary database operations
    /// </summary>
    public interface ICaseHouseholdIncomeRepository : IBaseRepository<CaseHouseholdIncome>
    {
        void InsertOrUpdate(CaseHouseholdIncome varCase);
        DataSourceResult Search(CaseHouseholdIncome searchParameters, DataSourceRequest paramDSRequest);
    }
}
