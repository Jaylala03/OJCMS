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
using System.Web.Mvc;

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

            if (CaseActionNew.CaseMemberID > 0 && !CaseActionNew.OLDCaseMemberID.HasValue)
            {
                CaseActionNew.ServiceProviderID = null;
                CaseActionNew.WorkerID = null;
                CaseActionNew.AssigneeOther = null;
                CaseActionNew.SubjectMatterExpertOther = null;
            }
            if (CaseActionNew.ServiceProviderID > 0 && !CaseActionNew.OLDServiceProviderID.HasValue)
            {
                CaseActionNew.CaseMemberID = null;
                CaseActionNew.FamilyAgreeToAction = false;
                CaseActionNew.WorkerID = null;
                CaseActionNew.AssigneeOther = null;
                CaseActionNew.SubjectMatterExpertOther = null;
            }
            if (CaseActionNew.WorkerID > 0 && !CaseActionNew.OLDWorkerID.HasValue)
            {
                CaseActionNew.ServiceProviderID = null;
                CaseActionNew.CaseMemberID = null;
                CaseActionNew.FamilyAgreeToAction = false;
                CaseActionNew.AssigneeOther = null;
            }
            if (CaseActionNew.WorkerID > 0 || CaseActionNew.CaseMemberID > 0 || (CaseActionNew.ServiceProviderID > 0 && CaseActionNew.ServiceProviderID != 56))
            {
                CaseActionNew.AssigneeOther = null;
                CaseActionNew.SubjectMatterExpertOther = null;
            }
            if (!string.IsNullOrEmpty(CaseActionNew.SubjectMatterExpertOther))
            {
                CaseActionNew.AssigneeOther = null;
            }
            if (!string.IsNullOrEmpty(CaseActionNew.AssigneeOther))
            {
                CaseActionNew.SubjectMatterExpertOther = null;
            }
            if (!string.IsNullOrEmpty(CaseActionNew.AssigneeOther) && string.IsNullOrEmpty(CaseActionNew.OLDAssigneeOther))
            {
                CaseActionNew.ServiceProviderID = null;
                CaseActionNew.CaseMemberID = null;
                CaseActionNew.WorkerID = null;
            }
            if (!string.IsNullOrEmpty(CaseActionNew.SubjectMatterExpertOther) && string.IsNullOrEmpty(CaseActionNew.OLDSubjectMatterExpertOther))
            {
                CaseActionNew.ServiceProviderID = null;
                CaseActionNew.CaseMemberID = null;
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

            if (!string.IsNullOrEmpty(CaseActionNew.ServiceProviderOther))
            {
                CaseActionNew.AssigneeOther = CaseActionNew.ServiceProviderOther;
            }

            //if (!string.IsNullOrEmpty(CaseActionNew.SubjectMatterExpertOther))
            //{
            //    CaseActionNew.AssigneeOther = CaseActionNew.SubjectMatterExpertOther;
            //}

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
                                                        WHEN [CA].[ServiceProviderID] IS NOT NULL AND SP.Name <> 'Other' THEN SP.Name 
                                                        WHEN [CA].[ServiceProviderID] IS NOT NULL AND SP.Name = 'Other' THEN SP.Name + '-' + CA.AssigneeOther
		                                                WHEN [CA].[WorkerID] > 0 THEN [W].[FirstName] + ' ' + [W].[LastName]
                                                        WHEN [CA].[WorkerID] IS NULL AND ISNULL(CA.SubjectMatterExpertOther,'') <> '' THEN 'Subject Matter Expert - ' + CA.SubjectMatterExpertOther
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

            sqlQuery.Append("SELECT CGN.ID AS CaseGoalID, GAWN.ContactMethodID,");
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

        public List<CaseGoalActionGridVM> CaseGoalActionHistory(int CaseGoalID)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT CA.SortOrder, CA.CaseID,CA.CaseGoalID,CA.ID , ");
            sqlQuery.Append("CASE WHEN CA.CaseMemberID IS NOT NULL THEN (CM.FirstName+' '+CM.LastName)  ");
            sqlQuery.Append("WHEN [CA].[ServiceProviderID] IS NOT NULL AND SP.Name <> 'Other' THEN SP.Name ");
            sqlQuery.Append("WHEN [CA].[ServiceProviderID] IS NOT NULL AND SP.Name = 'Other' THEN SP.Name + '-' + CA.AssigneeOther ");
            sqlQuery.Append("WHEN [CA].[WorkerID] > 0 THEN [SME].[FirstName] + ' ' + [SME].[LastName] ");
            sqlQuery.Append("WHEN [CA].[WorkerID] IS NULL AND ISNULL(CA.SubjectMatterExpertOther,'') <> '' THEN 'Subject Matter Expert - ' + CA.SubjectMatterExpertOther ");
            sqlQuery.Append("ELSE ISNULL(AssigneeOther,'') END ");
            sqlQuery.Append("AS [AssignedTo],GAR.Name AS AssigneeRole, ");
            sqlQuery.Append("GS.Name AS ActionStatus,CA.ActionDetail,CA.CreateDate,CA.LastUpdateDate ");
            sqlQuery.Append("FROM CaseActionNew AS CA ");
            sqlQuery.Append("INNER JOIN GoalStatus AS GS ON CA.ActionStatusID = GS.ID ");
            sqlQuery.Append("INNER JOIN GoalAssigneeRole AS GAR ON CA.GoalAssigneeRoleID = GAR.ID ");
            sqlQuery.Append("LEFT JOIN CaseMember AS CM ON CA.CaseMemberID = CM.ID ");
            sqlQuery.Append("LEFT JOIN ServiceProvider AS SP ON CA.ServiceProviderID = SP.ID ");
            sqlQuery.Append("LEFT JOIN Worker AS SME ON CA.WorkerID = SME.ID ");
            sqlQuery.Append("WHERE CA.CaseGoalID = " + CaseGoalID + "; ");

            List<CaseGoalActionGridVM> dsResult = context.Database.SqlQuery<CaseGoalActionGridVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }
        public List<SelectListItem> GetAllActions(int CaseGoalID)
        {
            return context.CaseActionNew.Where(item => item.CaseGoalID == CaseGoalID).OrderBy(item => item.ID).AsEnumerable().Select(item => new SelectListItem() { Text = item.ActionDetail, Value = item.ID.ToString() }).ToList();
        }

        public int CaseGoalActionNewCountByCaseID(int CaseID)
        {
            var varCaseActionNew = context.CaseActionNew.Where(w => w.CaseID == CaseID);

            int varCaseActionNewCount = varCaseActionNew.Count();

            return varCaseActionNewCount;
        }

        public void UpdateMoveUpSortOrder(int CaseActionID, int SortOrder)
        {
            var actionNew = context.CaseActionNew.Find(CaseActionID);
            if (actionNew != null)
            {
                actionNew.SortOrder = SortOrder + 1;
                actionNew.LastUpdateDate = DateTime.Now;
                context.Entry(actionNew).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
        }

        public void UpdateMoveDownSortOrder(int CaseActionID, int SortOrder)
        {
            var actionNew = context.CaseActionNew.Find(CaseActionID);
            if (actionNew != null)
            {
                actionNew.SortOrder = SortOrder - 1;
                actionNew.LastUpdateDate = DateTime.Now;
                context.Entry(actionNew).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
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
        List<CaseGoalActionGridVM> CaseGoalActionHistory(int CaseGoalID);
        List<SelectListItem> GetAllActions(int CaseGoalID);
        int CaseGoalActionNewCountByCaseID(int CaseID);
        void UpdateMoveUpSortOrder(int CaseActionID, int SortOrder);
        void UpdateMoveDownSortOrder(int CaseActionID, int SortOrder);
    }
}
