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

        public CaseHouseholdIncomeVM GetInitialIncomeForCaseSummary(int CaseId)
        {
            StringBuilder sqlQuery = new StringBuilder(@"SELECT [C].NoOfMembers AS NoOfMembers, [C].NoOfChild AS NoOfChild, [C].NoOfSeniors AS NoOfSeniors, [C].NoOfPhysicallyDisabled AS NoOfPhysicallyDisabled, [C].CreateDate AS CreatedDate, [C].IsLICO AS IsLICO, [IR].Name AS IncomeRanges FROM CaseHouseholdIncome[C]");

            sqlQuery.Append(" INNER JOIN IncomeRange[IR] ON[C].IncomeRangeID = [IR].ID");
            sqlQuery.Append(" INNER JOIN[Case] AS[CS] ON[CS].ID = [C].CaseID");
            sqlQuery.Append(" WHERE [C].[IsArchived] = 0 AND [C].CaseId = " + CaseId + " AND [C].IsInitialIncome = 1 ");

            CaseHouseholdIncomeVM casesummary = context.Database.SqlQuery<CaseHouseholdIncomeVM>(sqlQuery.ToString()).AsEnumerable().FirstOrDefault();

            return casesummary;
        }
        public CaseHouseholdIncomeVM GetCurrentIncomeForCaseSummary(int CaseId)
        {
            StringBuilder sqlQuery = new StringBuilder(@"SELECT TOP 1 [C].NoOfMembers AS NoOfMembers, [C].NoOfChild AS NoOfChild, [C].NoOfSeniors AS NoOfSeniors, [C].NoOfPhysicallyDisabled AS NoOfPhysicallyDisabled, [C].CreateDate AS CreatedDate, [C].IsLICO AS IsLICO, [IR].Name AS IncomeRanges FROM CaseHouseholdIncome[C]");

            sqlQuery.Append(" INNER JOIN IncomeRange[IR] ON[C].IncomeRangeID = [IR].ID");
            sqlQuery.Append(" INNER JOIN[Case] AS[CS] ON[CS].ID = [C].CaseID");
            sqlQuery.Append(" WHERE [C].[IsArchived] = 0 AND [C].CaseId = " + CaseId + " AND [C].IsInitialIncome = 0 ");
            sqlQuery.Append("ORDER BY [C].CreateDate DESC");

            CaseHouseholdIncomeVM casesummary = context.Database.SqlQuery<CaseHouseholdIncomeVM>(sqlQuery.ToString()).AsEnumerable().FirstOrDefault();

            return casesummary;
        }
    }

    /// <summary>
    /// interface of Case containing necessary database operations
    /// </summary>
    public interface ICaseHouseholdIncomeRepository : IBaseRepository<CaseHouseholdIncome>
    {
        void InsertOrUpdate(CaseHouseholdIncome varCase);
        DataSourceResult Search(CaseHouseholdIncome searchParameters, DataSourceRequest paramDSRequest);
        CaseHouseholdIncomeVM GetInitialIncomeForCaseSummary(int CaseId);
        CaseHouseholdIncomeVM GetCurrentIncomeForCaseSummary(int CaseId);
    }
}
