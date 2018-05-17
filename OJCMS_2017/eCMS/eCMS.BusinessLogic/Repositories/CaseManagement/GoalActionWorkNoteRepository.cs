//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.ViewModels;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class GoalActionWorkNoteRepository : BaseRepository<GoalActionWorkNote>, IGoalActionWorkNoteRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public GoalActionWorkNoteRepository(RepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Add or Update GoalActionWorkNote to database
        /// </summary>
        /// <param name="GoalActionWorkNote">data to save</param>
        public void InsertOrUpdate(GoalActionWorkNote GoalActionWorkNote)
        {

            GoalActionWorkNote.LastUpdateDate = DateTime.Now;
            if (GoalActionWorkNote.ID == default(int))
            {
                //set the date when this record was created
                GoalActionWorkNote.CreateDate = GoalActionWorkNote.LastUpdateDate;
                
                //set the id of the worker who has created this record
                GoalActionWorkNote.CreatedByWorkerID = GoalActionWorkNote.LastUpdatedByWorkerID;
                //GoalActionWorkNote.CreatedByWorkerID = GoalActionWorkNote.CreatedByWorkerID;
                //add a new record to database
                context.GoalActionWorkNote.Add(GoalActionWorkNote);
            }
            else
            {
                //update an existing record to database
                context.Entry(GoalActionWorkNote).State = System.Data.Entity.EntityState.Modified;
            }
            Save();

        }

        public DataSourceResult Search(DataSourceRequest dsRequest, int GoalID, int ActionID)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

            string sqlQuery = "";

            sqlQuery = "SELECT CWN.ID,CWN.CreateDate AS DateLogged, CWN.Note AS Notes, " +
            "CM.Name AS ContactMethod, CWN.NoteDate ,  " +
            "CAST(CWN.TimeSpentHours AS varchar) + ' hrs ' + (CASE WHEN CWN.TimeSpentMinutes > 0 THEN(CAST(CWN.TimeSpentMinutes AS varchar) + 'mins') else '' END) AS TimeSpent,  " +
            "ISNULL(CG.GoalDetail,CA.ActionDetail) AS Detail," +
            "GS.Name AS Status, W.FirstName + ' ' + W.LastName AS LoggedBy  " +
            "FROM GoalActionWorkNote AS CWN  " +
            "INNER JOIN ContactMethod AS CM ON CM.ID = CWN.ContactMethodID  " +
            "INNER JOIN Worker AS W ON W.ID = CWN.CreatedByWorkerID  " +
            "LEFT JOIN GoalStatus AS GS ON CWN.StatusID = GS.ID " +
            "LEFT JOIN CaseGoalNew AS CG ON CWN.CaseGoalID = CWN.ID  " +
            "LEFT JOIN CaseActionNew AS CA ON CWN.CaseActionID = CA.ID  ";
            if(GoalID > 0)
                sqlQuery += "WHERE CWN.CaseGoalID = " + GoalID;
            if(ActionID > 0)
                sqlQuery += "WHERE CWN.CaseActionID = " + ActionID;

            DataSourceResult dsResult = context.Database.SqlQuery<GoalActionWorkNoteVM>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
            return dsResult;

        }
    }

    /// <summary>
    /// interface of GoalActionWorkNote containing necessary database operations
    /// </summary>
    public interface IGoalActionWorkNoteRepository : IBaseRepository<GoalActionWorkNote>
    {
        void InsertOrUpdate(GoalActionWorkNote GoalActionWorkNote);
        DataSourceResult Search(DataSourceRequest dsRequest,int GoalID, int ActionID);
    }
}
