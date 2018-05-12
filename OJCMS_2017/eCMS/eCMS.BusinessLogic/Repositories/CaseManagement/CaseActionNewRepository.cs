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
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseActionNewRepository : BaseRepository<CaseActionNew>, ICaseActionNewRepository
    {
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        private readonly IWorkerToDoRepository workertodoRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseActionNewRepository(RepositoryContext context,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            IWorkerToDoRepository workertodoRepository)
            : base(context)
        {
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
            this.workertodoRepository = workertodoRepository;
        }

        /// <summary>
        /// Add or Update CaseActionNew to database
        /// </summary>
        /// <param name="CaseActionNew">data to save</param>
        public void InsertOrUpdate(CaseActionNew CaseActionNew)
        {
            bool isNew = false;
            if (CaseActionNew.CaseMemberID > 0)
            {
                CaseActionNew.ServiceProviderID = null;
                CaseActionNew.WorkerID = null;
            }
            if (CaseActionNew.ServiceProviderID == 0)
            {
                CaseActionNew.ServiceProviderID = null;
            }
            if (CaseActionNew.WorkerID == 0)
            {
                CaseActionNew.WorkerID = null;
            }
            CaseActionNew.LastUpdateDate = DateTime.Now;
            if (CaseActionNew.ID == default(int))
            {
                isNew = true;
                //set the date when this record was created
                CaseActionNew.CreateDate = CaseActionNew.LastUpdateDate;
                //set the id of the worker who has created this record
                CaseActionNew.CreatedByWorkerID = CaseActionNew.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseActionNew.Add(CaseActionNew);
            }
            else
            {
                //update an existing record to database
                context.Entry(CaseActionNew).State = System.Data.Entity.EntityState.Modified;
            }
            Save();

            if (CaseActionNew.WorkerID.HasValue && CaseActionNew.WorkerID.Value > 0)
            {

                string caseLink = "/CaseManagement/CaseActionNew/Index?CaseID=" + CaseActionNew.CaseID + "&CaseMemberID=" + CaseActionNew.CaseMemberID;
                WorkerToDo workerToDo = new WorkerToDo()
                {
                    LastUpdateDate = DateTime.Now,
                    LastUpdatedByWorkerID = CaseActionNew.LastUpdatedByWorkerID,
                    ReferenceLink = caseLink,
                    WorkerID = CaseActionNew.WorkerID.Value,
                    IsCompleted = false,
                };
                if (isNew)
                {
                    workerToDo.Subject = "A new action has been assigned to you. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the detail.";
                }
                else
                {
                    workerToDo.Subject = "An action assigned to you has been updated. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the detail.";
                }
                workertodoRepository.InsertOrUpdate(workerToDo);
                workertodoRepository.Save();
            }
        }


        public DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int CaseGoalId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAction, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAction, Constants.Actions.Delete, true);
            StringBuilder sqlQuery = new StringBuilder(@"SELECT [CA].[ID]
                                                        ,[CG].[GoalStatusID]
                                                        ,[CG].[PriorityTypeID]
                                                        ,[CA].[WorkerID]
                                                        ,[CA].[CaseMemberID]
                                                        ,[CA].[ServiceProviderID]
                                                        ,GAR.Name AS AssigneeRole
	                                                    ,CASE 
                                                        WHEN [CA].[CaseMemberID] IS NOT NULL THEN CM.FirstName + ' ' + CM.LastName
                                                        WHEN [CA].[ServiceProviderID] IS NOT NULL THEN SP.Name 
		                                                WHEN [CA].[WorkerID] IS NOT NULL THEN [W].[FirstName] + ' ' + [W].[LastName]
                                                        ELSE CA.AssigneeOther 
		                                                END [AssignedTo]
                                                        ,[ActionDetail]
                                                        ,[CA].[CreatedByWorkerID]
                                                        ,[CA].[LastUpdatedByWorkerID]
                                                        ,[CA].[IsArchived]
                                                        ,[CA].[CreateDate]
                                                        ,[CA].[LastUpdateDate]
                                                        ," + caseId + @" [CaseID]
                                                        ,'" + hasDeletePermission.ToDisplayStyle() + @"' [HasPermissionToDelete]
                                                        ,'" + hasEditPermission.ToDisplayStyle() + @"' [HasPermissionToEdit]");
            sqlQuery.Append(@" FROM [dbo].[CaseActionNew] [CA] 
                            INNER JOIN CaseGoal AS CG ON CA.CaseGoalID = CG.ID
                            INNER JOIN GoalAssigneeRole AS GAR ON CA.GoalAssigneeRoleID = GAR.ID
                            LEFT JOIN [dbo].[Worker] [W] ON [CW].[WorkerID]= [W].[ID]
                            LEFT JOIN CaseMember CM ON CM.ID = [CA].[CaseMemberID]
                            LEFT JOIN ServiceProvider SP ON SP.ID = [CA].[ServiceProviderID]
                            WHERE [CA].IsArchived = 0 AND CA.CaseGoalID = " + CaseGoalId);
            sqlQuery.Append(" ORDER BY [CA].[CreateDate] DESC");
            DataSourceResult dsResult = context.Database.SqlQuery<CaseActionNew>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
            return dsResult;
        }


       

        public void Update(CaseActionNew CaseActionNew)
        {
            CaseActionNew.LastUpdateDate = DateTime.Now;
            if (CaseActionNew.ID == default(int))
            {
                //set the date when this record was created
                CaseActionNew.CreateDate = CaseActionNew.LastUpdateDate;
                //set the id of the worker who has created this record
                CaseActionNew.CreatedByWorkerID = CaseActionNew.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseActionNew.Add(CaseActionNew);
            }
            else
            {
                //update an existing record to database
                context.Entry(CaseActionNew).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
        }

        public List<CaseGoalWorkNoteGridVM> CaseGoalWorkNote(int CaseGoalID)
        {
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT CGN.ID AS CaseGoalID, GAWN.ContactMethodID AS ContactMethodID,");
            sqlQuery.Append("CAST(GAWN.CreateDate as date) AS CreateDate, GAWN.Note AS Note,");
            sqlQuery.Append("CM.Description AS ContactMethod, GAWN.NoteDate AS NoteDate,");
            sqlQuery.Append("CAST(GAWN.TimeSpentHours as varchar) + ':' + CAST(GAWN.TimeSpentMinutes as varchar) AS TimeSpent,");
            sqlQuery.Append("CGN.GoalDetail AS GoalDetail FROM GoalActionWorkNote GAWN ");
            sqlQuery.Append("INNER JOIN ContactMethod CM ON GAWN.ContactMethodID = CM.ID ");
            sqlQuery.Append("INNER JOIN CaseGoalNew CGN ON GAWN.CaseGoalID = CGN.ID ");
            sqlQuery.Append("WHERE GAWN.ID = " + CaseGoalID + "");            

            List<CaseGoalWorkNoteGridVM> dsResult = context.Database.SqlQuery<CaseGoalWorkNoteGridVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }
    }

    /// <summary>
    /// interface of CaseActionNew containing necessary database operations
    /// </summary>
    public interface ICaseActionNewRepository : IBaseRepository<CaseActionNew>
    {
        void InsertOrUpdate(CaseActionNew CaseActionNew);
        void Update(CaseActionNew CaseActionNew);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int CaseGoalId);
        List<CaseGoalWorkNoteGridVM> CaseGoalWorkNote(int CaseGoalID);
    }
}
