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
    public class CaseActionRepository : BaseRepository<CaseAction>, ICaseActionRepository
    {
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        private readonly ICaseWorkerRepository caseworkerRepository;
        private readonly IWorkerNotificationRepository workernotificationRepository;
        private readonly ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository;
        private readonly IWorkerToDoRepository workertodoRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseActionRepository(RepositoryContext context,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseWorkerRepository caseworkerRepository,
            IWorkerNotificationRepository workernotificationRepository,
            ICaseSmartGoalServiceProviderRepository casesmartgoalserviceproviderRepository,
            IWorkerToDoRepository workertodoRepository)
            : base(context)
        {
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.workernotificationRepository = workernotificationRepository;
            this.casesmartgoalserviceproviderRepository = casesmartgoalserviceproviderRepository;
            this.workertodoRepository = workertodoRepository;
        }

        public IQueryable<CaseAction> AllIncluding(int caseprogressnoteId, params Expression<Func<CaseAction, object>>[] includeProperties)
        {
            IQueryable<CaseAction> query = context.CaseAction;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseProgressNoteID == caseprogressnoteId);
        }

        /// <summary>
        /// Add or Update caseaction to database
        /// </summary>
        /// <param name="caseaction">data to save</param>
        public void InsertOrUpdate(CaseAction caseaction)
        {
            bool isNew = false;
            if (caseaction.CaseSmartGoalServiceProviderID > 0)
            {
                caseaction.CaseProgressNoteID = null;
                caseaction.CaseSmartGoalID = null;
            }
            if (caseaction.CaseProgressNoteID == 0)
            {
                caseaction.CaseProgressNoteID = null;
            }
            if (caseaction.CaseSmartGoalID == 0)
            {
                caseaction.CaseSmartGoalID = null;
            }
            caseaction.LastUpdateDate = DateTime.Now;
            if (caseaction.ID == default(int))
            {
                isNew = true;
                //set the date when this record was created
                caseaction.CreateDate = caseaction.LastUpdateDate;
                //set the id of the worker who has created this record
                caseaction.CreatedByWorkerID = caseaction.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseAction.Add(caseaction);
            }
            else
            {
                //update an existing record to database
                context.Entry(caseaction).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (caseaction.CaseProgressNoteID.HasValue && caseaction.CaseProgressNoteID.Value > 0)
            {
                CaseWorker primaryWorker = caseworkerRepository.FindPrimary(caseaction.CaseID);
                if (primaryWorker != null)
                {
                    string caseLink = "/CaseManagement/CaseProgressNote/Edit?noteID=" + caseaction.CaseProgressNoteID.Value + "&CaseID=" + caseaction.CaseID + "&CaseMemberID=" + caseaction.CaseMemberID;
                    WorkerNotification workerNotification = new WorkerNotification()
                    {
                        IsRead = false,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = caseaction.LastUpdatedByWorkerID,
                        ReferenceLink = caseLink,
                        WorkerID = primaryWorker.WorkerID
                    };
                    if (isNew)
                    {
                        workerNotification.Notification = "A new action has been added to a progress note. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the note detail.";
                    }
                    else
                    {
                        workerNotification.Notification = "A progress note action has been updated. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the note detail.";
                    }
                    workernotificationRepository.InsertOrUpdate(workerNotification);
                    workernotificationRepository.Save();
                }
            }
            else if (caseaction.CaseSmartGoalServiceProviderID.HasValue && caseaction.CaseSmartGoalServiceProviderID.Value > 0)
            {
                CaseSmartGoalServiceProvider casesmartgoalserviceprovider = casesmartgoalserviceproviderRepository.Find(caseaction.CaseSmartGoalServiceProviderID.Value);
                if (casesmartgoalserviceprovider != null && casesmartgoalserviceprovider.WorkerID.HasValue && casesmartgoalserviceprovider.WorkerID.Value>0)
                {
                    string caseLink = "/CaseManagement/CaseSmartGoalServiceProvider/Index?casesmartgoalId=" + casesmartgoalserviceprovider.CaseSmartGoalID + "&CaseID=" + caseaction.CaseID + "&CaseMemberID=" + caseaction.CaseMemberID;
                    WorkerNotification workerNotification = new WorkerNotification()
                    {
                        IsRead = false,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = casesmartgoalserviceprovider.LastUpdatedByWorkerID,
                        ReferenceLink = caseLink,
                        WorkerID = casesmartgoalserviceprovider.WorkerID.Value
                    };
                    if (isNew)
                    {
                        workerNotification.Notification = "A new action has been added to a service provider. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the service provider detail.";
                    }
                    else
                    {
                        workerNotification.Notification = "A service provider action has been updated. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the service provider detail.";
                    }
                    workernotificationRepository.InsertOrUpdate(workerNotification);
                    workernotificationRepository.Save();
                }
            }

            if (caseaction.CaseWorkerID.HasValue && caseaction.CaseWorkerID.Value>0)
            {
                int workerID = 0;
                if (caseaction.CaseSmartGoalServiceProviderID.HasValue && caseaction.CaseSmartGoalServiceProviderID.Value > 0)
                {
                    CaseSmartGoalServiceProvider casesmartgoalserviceprovider = casesmartgoalserviceproviderRepository.Find(caseaction.CaseSmartGoalServiceProviderID.Value);
                    if (casesmartgoalserviceprovider != null && casesmartgoalserviceprovider.WorkerID.HasValue)
                    {
                        workerID = casesmartgoalserviceprovider.WorkerID.Value;
                    }
                }
                if (workerID == 0)
                {
                    CaseWorker caseWorker = caseworkerRepository.Find(caseaction.CaseWorkerID.Value);
                    if (caseWorker != null)
                    {
                        workerID = caseWorker.WorkerID;
                    }
                }
                if (workerID > 0)
                {
                    string caseLink = "/CaseManagement/CaseAction/Index?CaseID=" + caseaction.CaseID + "&CaseMemberID=" + caseaction.CaseMemberID;
                    WorkerToDo workerToDo = new WorkerToDo()
                    {
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = caseaction.LastUpdatedByWorkerID,
                        ReferenceLink = caseLink,
                        WorkerID = workerID,
                        IsCompleted = false,
                    };
                    if (isNew)
                    {
                        workerToDo.Subject = "A new action has been assigned to you. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the detail.";
                    }
                    else if (caseaction.IsCompleted)
                    {
                        workerToDo.Subject = "An action assigned to you has been completed. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the detail.";
                    }
                    else
                    {
                        workerToDo.Subject = "An action assigned to you has been updated. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the detail.";
                    }
                    workertodoRepository.InsertOrUpdate(workerToDo);
                    workertodoRepository.Save();
                }
            }
        }

        public BaseModel CloseAction(string ids)
        {
            int totalSelected = 0;
            int totalUpdated = 0;
            int perrmissionCount = 0;
            BaseModel statusModel = new BaseModel();
            if (ids.IsNotNullOrEmpty())
            {
                string[] arrayIds = ids.ToStringArray(',');
                foreach (string id in arrayIds)
                {
                    totalSelected++;
                    CaseAction caseAction = Find(id.ToInteger(true));
                    if (caseAction.CreatedByWorkerID != CurrentLoggedInWorker.ID && (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) == -1 ))
                    {
                        perrmissionCount++;
                    }
                    else
                    {
                        if (caseAction != null && !caseAction.IsCompleted)
                        {
                            caseAction.IsCompleted = true;
                            caseAction.LastUpdateDate = DateTime.Now;
                            totalUpdated++;
                        }
                        else
                        {
                            Remove(caseAction);
                        }
                    }
                }
                Save();
            }
            if (totalSelected == 0)
            {
                statusModel.ErrorMessage = "Please select at least one action to close";
            }
            else if (totalSelected > 0 && totalUpdated > 0 && totalUpdated == totalSelected)
            {
                statusModel.SuccessMessage = "All the selected actions have been closed successfully";
            }
            else if (totalSelected > 0 && totalUpdated ==0)
            {
                statusModel.ErrorMessage = "All the selected actions are already closed";
            }
            else if (totalSelected > 0 && totalUpdated > 0 && totalUpdated != totalSelected)
            {
                statusModel.SuccessMessage = totalUpdated + " out of " + totalSelected + " selected actions have been closed successfully";
            }
            if (perrmissionCount > 0)
            {
                statusModel.ErrorMessage = "Some of the actions are not closed as you do not have permission for this action";
            }
            return statusModel;
        }

        public DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, bool isCompleted, int caseSmartGoalId, int caseProgressNoteId, int casesmartgoalserviceproviderid, bool includeServiceProviderAction, bool isProviderAction)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAction, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseAction, Constants.Actions.Delete, true);
            StringBuilder sqlQuery = new StringBuilder(@"SELECT [CA].[ID]
                                                        ,[CA].[CaseProgressNoteID]
	                                                    ,[CA].[CaseSmartGoalID]
                                                        ,[CA].[CaseSmartGoalServiceProviderID]
                                                        ,[CA].[CaseWorkerID]
                                                        ,[CA].[CaseMemberID]
	                                                    ,CASE 
		                                                --WHEN [CA].[CaseMemberID] IS NOT NULL THEN(SELECT CM.FirstName+' '+CM.LastName FROM CaseMember CM where CM.ID=[CA].[CaseMemberID])
                                                        WHEN [CA].[CaseMemberID] IS NOT NULL THEN CM.FirstName+' '+CM.LastName
		                                                ELSE [W].[FirstName]+' '+[W].[LastName]
		                                                END [CaseWorkerName]
                                                        ,[Action]
                                                        ,[ActionStartTime]
                                                        ,[ActionEndTime]
                                                        ,[IsCompleted]
                                                        ,[CA].[CreatedByWorkerID]
                                                        ,[CA].[LastUpdatedByWorkerID]
                                                        ,[CA].[IsArchived]
                                                        ,[CA].[CreateDate]
                                                        ,[CA].[LastUpdateDate]
                                                        ," + caseId + @" [CaseID]
                                                        ,'" + hasDeletePermission.ToDisplayStyle() + @"' [HasPermissionToDelete]
                                                        ,'" + hasEditPermission.ToDisplayStyle() + @"' [HasPermissionToEdit]");
                                                            if (isProviderAction)
                                                            {
                                                                sqlQuery.Append(", (SELECT TOP 1 SP.Name FROM CaseSmartGoalServiceProvider CSGSP LEFT JOIN ServiceProvider SP ON CSGSP.ServiceProviderID=SP.ID WHERE CSGSP.ID=[CA].[CaseSmartGoalServiceProviderID]) AS ServiceProviderName");
                                                            }
                                                            sqlQuery.Append(@" FROM [dbo].[CaseAction] [CA] 
                                                    LEFT JOIN [dbo].[CaseWorker] [CW] ON [CA].[CaseWorkerID]= [CW].[ID]
                                                    LEFT JOIN [dbo].[Worker] [W] ON [CW].[WorkerID]= [W].[ID]
                                                    LEFT JOIN CaseMember CM ON CM.ID=[CA].[CaseMemberID]
                                                    WHERE [CA].IsArchived=0");
            if (caseMemberId.HasValue && caseMemberId.Value > 0 && caseSmartGoalId == 0 && caseProgressNoteId == 0 && casesmartgoalserviceproviderid == 0)
            {
                sqlQuery.Append(" AND ( [CA].[CaseProgressNoteID] IN (SELECT CPN.ID FROM CaseProgressNote CPN WHERE CPN.CaseMemberID=" + caseMemberId.Value + ")");
                sqlQuery.Append(" OR ( [CA].[CaseProgressNoteID] IN (SELECT CPN.CaseProgressNoteID FROM CaseProgressNoteMembers CPN WHERE CPN.CaseMemberID=" + caseMemberId.Value + "))");
                sqlQuery.Append(" OR ( [CA].[CaseSmartGoalID] IN (SELECT CSG.ID FROM CaseSmartGoal CSG LEFT JOIN CaseGoal CG ON CG.ID=CSG.CaseGoalID WHERE CG.CaseMemberID=" + caseMemberId.Value + "))");
                sqlQuery.Append(" OR ( [CA].[CaseSmartGoalServiceProviderID] IN (SELECT CSGSP.ID FROM CaseSmartGoalServiceProvider CSGSP LEFT JOIN CaseSmartGoal CSG ON CSG.ID=CSGSP.CaseSmartGoalID LEFT JOIN CaseGoal CG ON CG.ID=CSG.CaseGoalID WHERE CG.CaseMemberID=" + caseMemberId.Value + "))");
                sqlQuery.Append(" ) ");
            }
            else if (caseId > 0 && caseSmartGoalId == 0 && caseProgressNoteId == 0 && casesmartgoalserviceproviderid == 0)
            {
                sqlQuery.Append(" AND ( [CA].[CaseProgressNoteID] IN (SELECT CPN.ID FROM CaseProgressNote CPN WHERE CPN.CaseMemberID IN (SELECT ID FROM CaseMember WHERE CaseID=" + caseId + "))");
                sqlQuery.Append(" OR ( [CA].[CaseProgressNoteID] IN (SELECT CPN.CaseProgressNoteID FROM CaseProgressNoteMembers CPN WHERE CPN.CaseMemberID IN (SELECT ID FROM CaseMember WHERE CaseID=" + caseId + ")))");
                sqlQuery.Append(" OR ( [CA].[CaseSmartGoalID] IN (SELECT CSG.ID FROM CaseSmartGoal CSG LEFT JOIN CaseGoal CG ON CG.ID=CSG.CaseGoalID WHERE CG.CaseMemberID IN (SELECT ID FROM CaseMember WHERE CaseID=" + caseId + ")))");
                sqlQuery.Append(" OR ( [CA].[CaseSmartGoalServiceProviderID] IN (SELECT CSGSP.ID FROM CaseSmartGoalServiceProvider CSGSP LEFT JOIN CaseSmartGoal CSG ON CSG.ID=CSGSP.CaseSmartGoalID LEFT JOIN CaseGoal CG ON CG.ID=CSG.CaseGoalID WHERE CG.CaseMemberID IN (SELECT ID FROM CaseMember WHERE CaseID=" + caseId + ")))");
                sqlQuery.Append(" ) ");
            }
            else if (workerId > 0 && caseSmartGoalId == 0 && caseProgressNoteId == 0 && casesmartgoalserviceproviderid == 0)
            {
                sqlQuery.Append(" AND (( [CA].[CaseProgressNoteID] IN (SELECT CPN.ID FROM CaseProgressNote CPN INNER JOIN CaseWorkerMemberAssignment CWMA ON CPN.CaseMemberID=CWMA.CaseMemberID LEFT JOIN CaseWorker CW ON CWMA.CaseWorkerID=CW.ID WHERE CW.WorkerID=" + workerId + "))");
                sqlQuery.Append(" OR ([CA].[CaseProgressNoteID] IN (SELECT CPN.CaseProgressNoteID FROM CaseProgressNoteMembers CPN INNER JOIN CaseWorkerMemberAssignment CWMA ON CPN.CaseMemberID=CWMA.CaseMemberID LEFT JOIN CaseWorker CW ON CWMA.CaseWorkerID=CW.ID WHERE CW.WorkerID=" + workerId + "))");
                sqlQuery.Append(" OR ([CA].[CaseSmartGoalID] IN (SELECT CSG.ID FROM CaseSmartGoal CSG LEFT JOIN CaseGoal CG ON CG.ID=CSG.CaseGoalID INNER JOIN CaseWorkerMemberAssignment CWMA ON CG.CaseMemberID=CWMA.CaseMemberID LEFT JOIN CaseWorker CW ON CWMA.CaseWorkerID=CW.ID WHERE CW.WorkerID=" + workerId + "))");
                sqlQuery.Append(" OR ([CA].[CaseSmartGoalServiceProviderID] IN (SELECT CSGSP.ID FROM CaseSmartGoalServiceProvider CSGSP LEFT JOIN CaseSmartGoal CSG ON CSG.ID=CSGSP.CaseSmartGoalID LEFT JOIN CaseGoal CG ON CG.ID=CSG.CaseGoalID INNER JOIN CaseWorkerMemberAssignment CWMA ON CG.CaseMemberID=CWMA.CaseMemberID LEFT JOIN CaseWorker CW ON CWMA.CaseWorkerID=CW.ID WHERE CW.WorkerID=" + workerId + "))");
                sqlQuery.Append(" ) ");
            }

            if (caseProgressNoteId > 0)
            {
                sqlQuery.Append(" AND [CA].[CaseProgressNoteID]=" + caseProgressNoteId);
            }
            else if (casesmartgoalserviceproviderid > 0)
            {
                sqlQuery.Append(" AND [CA].[CaseSmartGoalServiceProviderID]=" + casesmartgoalserviceproviderid);
            }
            else if (isProviderAction)
            {
                sqlQuery.Append(" AND [CA].[CaseSmartGoalServiceProviderID] IN (SELECT ID FROM CaseSmartGoalServiceProvider WHERE CaseSmartGoalID=" + caseSmartGoalId + " )");
            }
            else if (caseSmartGoalId > 0 && includeServiceProviderAction)
            {
                sqlQuery.Append(" AND ( [CA].[CaseSmartGoalID]=" + caseSmartGoalId);
                sqlQuery.Append(" OR [CA].[CaseSmartGoalServiceProviderID] IN (SELECT ID FROM CaseSmartGoalServiceProvider WHERE CaseSmartGoalID=" + caseSmartGoalId + " ))");
            }
            else if (caseSmartGoalId > 0)
            {
                sqlQuery.Append(" AND [CA].[CaseSmartGoalID]=" + caseSmartGoalId);
            }

            //if (caseProgressNoteId > 0)
            //{
            //    sqlQuery.Append(" AND [CA].[CaseProgressNoteID]=" + caseProgressNoteId);
            //}

            if (casesmartgoalserviceproviderid > 0)
            {
                //sqlQuery.Append(" AND [CA].[CaseSmartGoalServiceProviderID]=" + casesmartgoalserviceproviderid);
            }
            else
            {
                if (isCompleted)
                {
                    sqlQuery.Append(" AND [CA].[IsCompleted]=1 ");
                }
                else
                {
                    sqlQuery.Append(" AND [CA].[IsCompleted]=0 ");
                }
            }
            sqlQuery.Append(" ORDER BY [CA].[CreateDate] DESC");
            DataSourceResult dsResult = context.Database.SqlQuery<CaseActionListViewModel>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
            return dsResult;
        }

        public int FindPendingActionCount(int casesmartgoalId)
        {
            return context.CaseAction.Where(item => item.IsCompleted == false && item.ActionEndTime >= DateTime.Now && item.CaseSmartGoalID==casesmartgoalId).Count();
        }

        public void Update(CaseAction caseaction)
        {
            caseaction.LastUpdateDate = DateTime.Now;
            if (caseaction.ID == default(int))
            {
                //set the date when this record was created
                caseaction.CreateDate = caseaction.LastUpdateDate;
                //set the id of the worker who has created this record
                caseaction.CreatedByWorkerID = caseaction.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseAction.Add(caseaction);
            }
            else
            {
                //update an existing record to database
                context.Entry(caseaction).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
        }
    }

    /// <summary>
    /// interface of CaseAction containing necessary database operations
    /// </summary>
    public interface ICaseActionRepository : IBaseRepository<CaseAction>
    {
        IQueryable<CaseAction> AllIncluding(int caseprogressnoteId, params Expression<Func<CaseAction, object>>[] includeProperties);
        void InsertOrUpdate(CaseAction caseaction);
        void Update(CaseAction caseaction);
        BaseModel CloseAction(string ids);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, bool isCompleted, int caseSmartGoalId, int caseProgressNoteId, int casesmartgoalserviceproviderid, bool includeServiceProviderAction, bool isProviderAction);
        int FindPendingActionCount(int casesmartgoalId);
    }
}
