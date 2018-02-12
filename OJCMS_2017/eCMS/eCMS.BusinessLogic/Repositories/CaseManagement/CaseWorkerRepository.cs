//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using eCMS.DataLogic.Models;
using eCMS.BusinessLogic.Repositories.Context;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using eCMS.Shared;
using System.Text;
using eCMS.DataLogic.ViewModels;
using eCMS.BusinessLogic.Helpers;
using EasySoft.Helper;

using eCMS.DataLogic.Models.Lookup;

using eCMS.ExceptionLoging;

using Kendo.Mvc.Extensions;

using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseWorkerRepository : BaseRepository<CaseWorker>, ICaseWorkerRepository
    {
        private readonly IWorkerNotificationRepository workernotificationRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseWorkerRepository(RepositoryContext context, IWorkerNotificationRepository workernotificationRepository)
            : base(context)
        {
            this.workernotificationRepository = workernotificationRepository;
        }

        public IQueryable<CaseWorker> AllIncluding(int caseId, params Expression<Func<CaseWorker, object>>[] includeProperties)
        {
            IQueryable<CaseWorker> query = context.CaseWorker;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseID == caseId);
        }

        public IQueryable<CaseWorker> FindAllByCaseID(int caseID)
        {
            return context.CaseWorker.Where(item => item.CaseID == caseID);
        }

        public IQueryable<CaseWorker> FindAllByWorkerID(int workerID)
        {
            return context.CaseWorker.Where(item => item.WorkerID == workerID);
        }

        /// <summary>
        /// Add or Update caseworker to database
        /// </summary>
        /// <param name="caseworker">data to save</param>
        public void InsertOrUpdate(CaseWorker caseworker)
        {
            //when a member is set as primary, release others from primary is there is any
            if (caseworker.IsPrimary && caseworker.CaseID > 0)
            {
                string sqlQuery = "UPDATE CaseWorker SET IsPrimary=0 WHERE CaseID=" + caseworker.CaseID;
                context.Database.ExecuteSqlCommand(sqlQuery);
            }
            //set a member as primary if there is no primary member has been set
            if (!caseworker.IsPrimary)
            {
                int count = context.CaseWorker.Where(item => item.CaseID == caseworker.CaseID && item.IsPrimary == true).Count();
                if (count == 0)
                {
                    caseworker.IsPrimary = true;
                }
            }
            caseworker.LastUpdateDate = DateTime.Now;
            if (caseworker.ID == default(int))
            {
                //set the date when this record was created
                caseworker.CreateDate = caseworker.LastUpdateDate;
                //set the id of the worker who has created this record
                caseworker.CreatedByWorkerID = caseworker.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseWorker.Add(caseworker);
            }
            else
            {
                //update an existing record to database
                var NewCaseworker = context.CaseWorker.Where(a => a.ID == caseworker.ID).FirstOrDefault();

                NewCaseworker.CaseID = caseworker.CaseID;
                NewCaseworker.WorkerID = caseworker.WorkerID;
                NewCaseworker.IsActive = caseworker.IsActive;
                NewCaseworker.AllowNotification = caseworker.AllowNotification;
                NewCaseworker.IsPrimary = caseworker.IsPrimary;
                NewCaseworker.IsArchived = caseworker.IsArchived;
                NewCaseworker.LastUpdateDate = DateTime.Now;
                //context.Entry(caseworker).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (caseworker.IsPrimary)
            {
                string caseLink = "/CaseManagement/CaseMemberProfile/Index?caseId="+caseworker.CaseID;
                WorkerNotification workerNotification = new WorkerNotification()
                {
                    IsRead = false,
                    LastUpdateDate = DateTime.Now,
                    LastUpdatedByWorkerID = caseworker.LastUpdatedByWorkerID,
                    Notification = "You are assigned to a new case. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the case detail.",
                    ReferenceLink = caseLink,
                    WorkerID = caseworker.WorkerID
                };
                workernotificationRepository.InsertOrUpdate(workerNotification);
                workernotificationRepository.Save();
            }
        }

        public override void Delete(int id)
        {
            
            var entity = Find(id);
            if (entity.IsPrimary && entity.CaseID > 0)
            {
                string sqlQuery1 = "UPDATE CaseWorker SET IsPrimary=1 WHERE CaseID=" + entity.CaseID + " AND ID=(Select Max(ID) from [CaseWorker]  WHERE CaseID=" + entity.CaseID + ")";
                context.Database.ExecuteSqlCommand(sqlQuery1);
            }
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            string sqlQuery = @"delete from [CaseWorkerMemberAssignment] where CaseWorkerID=@CaseWorkerID;
                                delete from [CaseAction] where CaseWorkerID=@CaseWorkerID;
                                --delete from [CaseAssessmentLivingCondition] where CaseAssessmentID in (select ID from [CaseAssessment] where DocumentedByID=@CaseWorkerID);
                                DELETE CALC FROM [CaseAssessmentLivingCondition] CALC INNER JOIN [CaseAssessment] CA ON CA.ID = CALC.CaseAssessmentID WHERE CA.DocumentedByID=@CaseWorkerID;
                                delete from [CaseAssessment] where DocumentedByID=@CaseWorkerID;
                                delete from [CaseWorker] where id=@CaseWorkerID;";
            sqlQuery = sqlQuery.Replace("@CaseWorkerID", id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery);

        }


        public DataSourceResult Search(CaseWorker searchParameters, DataSourceRequest paramDSRequest)
        {
            DataSourceRequest dsRequest = paramDSRequest;
            if (dsRequest == null)
            {
                dsRequest = new DataSourceRequest();
            }
            if (dsRequest.Filters == null || (dsRequest.Filters != null && dsRequest.Filters.Count == 0))
            {
                if (dsRequest.Filters == null)
                {
                    dsRequest.Filters = new List<IFilterDescriptor>();
                }
            }
            if (dsRequest.Sorts == null || (dsRequest.Sorts != null && dsRequest.Sorts.Count == 0))
            {
                if (dsRequest.Sorts == null)
                {
                    dsRequest.Sorts = new List<SortDescriptor>();
                }
                SortDescriptor defaultSortExpression = new SortDescriptor("WorkerName", System.ComponentModel.ListSortDirection.Descending);
                dsRequest.Sorts.Add(defaultSortExpression);
            }
            if (dsRequest.PageSize == 0)
            {
                dsRequest.PageSize = Constants.CommonConstants.DefaultPageSize;
            }

            StringBuilder sqlQuery = new StringBuilder(@"select CW.*,WR.Name as RoleName,(W.FirstName+' '+W.Lastname) as WorkerName,(SELECT Stuff(
            (
            SELECT ', ' + R.Name
            FROM Region R
            WHERE R.ID in (SELECT PR.RegionID 
                        FROM WorkerInRoleNew AS WIR 
                        INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID 
                        INNER JOIN Permission AS P ON WRP.PermissionID = P.ID 
                        INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID 
                        WHERE WIR.WorkerID = W.ID
                        GROUP BY PR.RegionID  )
            FOR XML PATH('')
            ), 1, 2, '')) AS RegionName,
        (SELECT Stuff(
            (
            SELECT ', ' + (CM.FirstName+' '+CM.LastName)
            FROM CaseMember CM
            where CM.ID in (Select CaseMemberID from CaseWorkerMemberAssignment CWM WHERE CWM.CaseWorkerID= CW.ID )
            FOR XML PATH('')
            ), 1, 2, '')) AS AssignedMembers
            from CaseWorker CW
            join Worker W on CW.WorkerID=W.ID
            join WorkerInRoleNew WIR on W.ID=WIR.WorkerID
            join WorkerRole WR on WR.ID=WIR.WorkerRoleID    
            WHERE CW.CaseID=" + searchParameters.CaseID + "");
            sqlQuery=sqlQuery.Append(@"
            group by CW.ID,CW.CaseID,CW.WorkerId,CW.IsActive,
            CW.AllowNotification,CW.IsPrimary,CW.CreatedByWorkerID,
            CW.LastUpdatedByWorkerID,CW.IsArchived,CW.CreateDate,
            CW.LastUpdateDate,WR.Name,W.ID,W.FirstName,W.Lastname"
           );

            DataSourceResult dataSourceResult = context.Database.SqlQuery<WorkerSearchViewModel>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);

            DataSourceRequest dsRequestTotalCountQuery = new DataSourceRequest();
            dsRequestTotalCountQuery.Filters = dsRequest.Filters;
            dataSourceResult.Total = context.Database.SqlQuery<WorkerSearchViewModel>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequestTotalCountQuery).Data.AsQueryable().Count();
            return dataSourceResult;
        }



        public CaseWorker FindPrimary(int caseID)
        {
            return context.CaseWorker.FirstOrDefault(item=>item.CaseID==caseID && item.IsPrimary==true);
        }
    }

    /// <summary>
    /// interface of CaseWorker containing necessary database operations
    /// </summary>
    public interface ICaseWorkerRepository : IBaseRepository<CaseWorker>
    {
        IQueryable<CaseWorker> AllIncluding(int caseId, params Expression<Func<CaseWorker, object>>[] includeProperties);
        IQueryable<CaseWorker> FindAllByCaseID(int caseID);
        CaseWorker FindPrimary(int caseID);
        IQueryable<CaseWorker> FindAllByWorkerID(int workerID);
        void InsertOrUpdate(CaseWorker caseworker);
        DataSourceResult Search(CaseWorker searchParameters, DataSourceRequest paramDSRequest);
    }
}
