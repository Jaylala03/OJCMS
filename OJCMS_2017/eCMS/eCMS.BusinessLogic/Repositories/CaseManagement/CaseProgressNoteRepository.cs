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
    public class CaseProgressNoteRepository : BaseRepository<CaseProgressNote>, ICaseProgressNoteRepository
    {
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        private readonly ICaseWorkerRepository caseworkerRepository;
        private readonly IWorkerNotificationRepository workernotificationRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseProgressNoteRepository(RepositoryContext context,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseWorkerRepository caseworkerRepository,
            IWorkerNotificationRepository workernotificationRepository)
            : base(context)
        {
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
            this.caseworkerRepository = caseworkerRepository;
            this.workernotificationRepository = workernotificationRepository;
        }

        public IQueryable<CaseProgressNote> AllIncluding(int caseId, params Expression<Func<CaseProgressNote, object>>[] includeProperties)
        {
            IQueryable<CaseProgressNote> query = context.CaseProgressNote;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseID == caseId);
        }

        /// <summary>
        /// Add or Update caseprogressnote to database
        /// </summary>
        /// <param name="caseprogressnote">data to save</param>
        public void InsertOrUpdate(CaseProgressNote caseprogressnote)
        {
            bool isNew = false;
            caseprogressnote.LastUpdateDate = DateTime.Now;
            if (caseprogressnote.ID == default(int))
            {
                isNew = true;
                //set the date when this record was created
                caseprogressnote.CreateDate = caseprogressnote.LastUpdateDate;
                //set the id of the worker who has created this record
                caseprogressnote.CreatedByWorkerID = caseprogressnote.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseProgressNote.Add(caseprogressnote);
            }
            else
            {
                //update an existing record to database
                context.Entry(caseprogressnote).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            if (caseprogressnote.ID > 0)
            {
                CaseWorker primaryWorker = caseworkerRepository.FindPrimary(caseprogressnote.CaseID);
                if (primaryWorker != null)
                {
                    string caseLink = "/CaseManagement/CaseProgressNote/Edit?noteID=" + caseprogressnote.ID + "&CaseID=" + caseprogressnote.CaseID + "&CaseMemberID=" + caseprogressnote.CaseMemberID;
                    WorkerNotification workerNotification = new WorkerNotification()
                    {
                        IsRead = false,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = caseprogressnote.LastUpdatedByWorkerID,
                        ReferenceLink = caseLink,
                        WorkerID = primaryWorker.WorkerID
                    };
                    if (isNew)
                    {
                        workerNotification.Notification = "A new progress note has been added to a case. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the note detail.";
                    }
                    else
                    {
                        workerNotification.Notification = "A progress note has been updated. Please <a href='" + caseLink + "' target='_blank'>click here</a> to see the note detail.";
                    }
                    workernotificationRepository.InsertOrUpdate(workerNotification);
                    workernotificationRepository.Save();
                }
            }
        }

        public CaseProgressNote FindInitialNoteByCaseID(int caseID)
        {
            return context.CaseMember.Join(context.CaseProgressNote, left => left.ID, right => right.CaseMemberID, (left, right) => new { left, right }).
                Where(item => item.left.CaseID == caseID && item.right.ActivityTypeID == 1).Select(item => item.right).FirstOrDefault();
        }

        public DataSourceResult MyNotification(DataSourceRequest dsRequest)
        {
            DataSourceResult result = new DataSourceResult();
            result = context.CaseProgressNote.Join(context.CaseMember, left => left.CaseMemberID, right => right.ID, (left, right) => new { left, right })
                .Join(context.CaseWorker, secondleft => secondleft.right.CaseID, secondright => secondright.CaseID, (secondleft, secondright) => new { secondleft, secondright })
                .Where(item => item.secondright.WorkerID == CurrentLoggedInWorker.ID)
                .OrderByDescending(item => item.secondleft.left.NoteDate)
            .Select(item => new
            {
                CaseProgramName = item.secondright.Case.Program.Name,
                CaseDisplayID = item.secondright.Case.DisplayID,
                CaseMemberName = item.secondleft.right.FirstName + " " + item.secondleft.right.LastName,
                ActivityTypeName = item.secondleft.left.ActivityType.Name,
                item.secondleft.left.NoteDate,
                item.secondleft.left.ID,
                CaseID = item.secondright.Case.ID,
                CreatedByWorkerName = item.secondleft.left.CreatedByWorker.FirstName + " " + item.secondleft.left.CreatedByWorker.LastName
            }).ToDataSourceResult(dsRequest);
            return result;
        }

        public override void Delete(int id)
        {
            var entity = Find(id);
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            string sqlQuery = @"delete from [CaseAction] where caseprogressnoteid=@id;
delete from [CaseProgressNote] where id=@id;";
            sqlQuery = sqlQuery.Replace("@id", id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery);

        }

        public DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, List<int> workerRoleIDs)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            //if (caseMemberId.HasValue && caseMemberId > 0)
            //{
            //    FilterDescriptor filterDescriptor = new FilterDescriptor("CaseMemberID", FilterOperator.IsEqualTo, caseMemberId.Value);
            //    dsRequest.Filters.Add(filterDescriptor);
            //}
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Read, true);
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Delete, true);
            //DataSourceResult dsResult = context.CaseProgressNote
            //    .Join(context.CaseProgressNoteMembers, left => left.ID, right => right.CaseProgressNoteID, (left, right) => new { left,right})
            //    .Join(context.CaseMember,secondleft=>secondleft.right.CaseMemberID,secondright=>secondright.ID,(secondleft,secondright)=>new {secondleft,secondright})
            //    //.Join(context.CaseWorkerMemberAssignment, left => left.CaseMemberID, right => right.CaseMemberID, (left, right) => new { left, right })
            //    .Where(item => item.secondleft.left.CaseMember.CaseID == caseId || item.secondright.CaseID==caseId)
            //    .Where(item => context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(item.secondleft.left.CaseMemberID) || workerRoleIDs.Contains("1") || context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(item.secondleft.right.CaseMemberID))
            //    .OrderByDescending(item => item.secondleft.left.CreateDate).ToList()
            //    .Select(
            //    caseprogressnote => new
            //    {
            //        caseprogressnote.secondleft.left.ID,
            //        CaseID = (caseprogressnote.secondleft.left.CaseMember.CaseID>0?caseprogressnote.secondleft.left.CaseMember.CaseID:caseprogressnote.secondright.CaseID),
            //        //caseprogressnote.CaseMemberID,
            //        caseprogressnote.secondleft.left.NoteDate,
            //        caseprogressnote.secondleft.left.Note,
            //        CaseMemberName = (caseprogressnote.secondleft.left.CaseMember != null ? caseprogressnote.secondleft.left.CaseMember.FirstName + " " + caseprogressnote.secondleft.left.CaseMember.LastName : (caseprogressnote.secondright != null ? caseprogressnote.secondright.FirstName + " " + caseprogressnote.secondright.LastName :"")),
            //        ActivityTypeName = caseprogressnote.secondleft.left.ActivityType != null ? caseprogressnote.secondleft.left.ActivityType.Name : "",
            //        HasPermissionToEdit = hasEditPermission ? "" : "display:none;",
            //        HasPermissionToDelete = hasDeletePermission ? "" : "display:none;"
            //    }
            //    ).ToDataSourceResult(dsRequest);

            //DataSourceResult dsResult = (from s in context.CaseProgressNote
            //                             join m1 in context.CaseMember
            //                             on s.CaseMemberID equals m1.ID
            //                             into tempmember
            //                             from mem in tempmember.DefaultIfEmpty()  
            //                             join cm in context.CaseProgressNoteMembers
            //                             on s.ID equals cm.CaseProgressNoteID into temp
            //                             from t in temp.DefaultIfEmpty()                                                              

            //                             where ( t.CaseMember.CaseID==caseId || mem.CaseID==caseId)
            //                             && (context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(s.CaseMemberID)
            //                             || workerRoleIDs.Contains("1")
            //                             || context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(t.CaseMemberID)
            //                             )
            //                             select new { 
            //                             s.ID,
            //                             s.NoteDate,
            //                             s.Note,
            //                             CaseMemberName = (t.CaseMember != null ? t.CaseMember.FirstName + " " + t.CaseMember.LastName : (mem != null ? mem.FirstName + " " + mem.LastName : "")),
            //                             HasPermissionToEdit = hasEditPermission ? "" : "display:none;",
            //                             HasPermissionToDelete = hasDeletePermission ? "" : "display:none;"
            //                             }).ToDataSourceResult(dsRequest);


            string sqlQuery = "";
            sqlQuery += "DECLARE @WorkerIds nvarchar(max) ; ";
            sqlQuery += " SET @WorkerIds='" + workerRoleIDs + "' ;";
            sqlQuery += " SELECT " + caseId + " AS CaseID,cpn.ID,(SELECT STUFF((SELECT ', ' + (cm1.FirstName+' '+cm1.LastName) from CaseProgressNote cpn1 left join CaseProgressNoteMembers cpm1 on cpn1.ID=cpm1.CaseProgressNoteID left join CaseMember cm1 on cm1.ID=cpm1.CaseMemberID or cm1.ID=cpn1.CaseMemberID where cm1.CaseID=" + caseId + " and cpn1.ID=cpn.ID ";
            sqlQuery += " ";
            sqlQuery += "" + (caseMemberId.HasValue && caseMemberId > 0 ? " and cm1.ID=" + caseMemberId + "" : "") + "";
            sqlQuery += " FOR XML PATH('')), 1, 2, '')) as CaseMemberName";
            sqlQuery += ",cpn.NoteDate,at.Name as ActivityTypeName,cpn.Note,'" + (hasReadPermission ? "" : "display:none;") + "' AS HasPermissionToRead,'" + (hasEditPermission ? "" : "display:none;") + "' AS HasPermissionToEdit,'" + (hasDeletePermission ? "" : "display:none;") + "' AS HasPermissionToDelete  from CaseProgressNote cpn";
            sqlQuery += " join ActivityType at on  at.ID=cpn.ActivityTypeID  left join CaseProgressNoteMembers cpm on cpn.ID=cpm.CaseProgressNoteID ";
            sqlQuery += " left join CaseMember cm on cm.ID=cpm.CaseMemberID or cm.ID=cpn.CaseMemberID where cm.CaseID=" + caseId + "";
            sqlQuery += "";
            sqlQuery += "" + (caseMemberId.HasValue && caseMemberId > 0 ? " and cm.ID=" + caseMemberId + "" : "") + "";
            sqlQuery += " group by cpn.ID,NoteDate,at.Name,Note";

            DataSourceResult dsResult = context.Database.SqlQuery<CaseProgressNoteModel>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
            return dsResult;
        }

        public string GetprogressNoteByCaseID(int CaseID)
        {
            string note = "";
            //string sqlQuery = @"Select top 1 Note from CaseProgressNote
            //                    where ID in(Select CaseProgressNoteID from CaseProgressNoteMembers
            //                    where CaseMemberID in (Select ID from CaseMember where CaseID=@id))
            //                    or CaseMemberID in (Select ID from CaseMember where CaseID=@id)
            //                    order by ID";

            //string sqlQuery = @"Select top 1 Note from CaseProgressNote CP
            //                    LEFT JOIN CaseProgressNoteMembers CM ON CM.CaseProgressNoteID = CP.ID
            //                    LEFT JOIN CaseMember CAM ON CAM.ID = CP.CaseMemberID OR CAM.ID = CP.CaseMemberID
            //                    WHERE CAM.CaseID=@id OR CP.CaseMemberID =@id order by CP.ID";

            string sqlQuery = @"SELECT dbo.GetLatNotesByCaseID(@id) AS Note";
            sqlQuery = sqlQuery.Replace("@id", CaseID.ToString());
            var response = context.Database.SqlQuery<CaseProgressNoteModel>(sqlQuery.ToString()).ToList();
            if (response != null && response.Count > 0)
            {
                note = response[0].Note;
            }
            return note;
        }
    }

    /// <summary>
    /// interface of CaseProgressNote containing necessary database operations
    /// </summary>
    public interface ICaseProgressNoteRepository : IBaseRepository<CaseProgressNote>
    {
        IQueryable<CaseProgressNote> AllIncluding(int caseId, params Expression<Func<CaseProgressNote, object>>[] includeProperties);
        void InsertOrUpdate(CaseProgressNote caseprogressnote);
        CaseProgressNote FindInitialNoteByCaseID(int caseID);
        DataSourceResult MyNotification(DataSourceRequest dsRequest);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, List<int> workerRoleIDs);
        string GetprogressNoteByCaseID(int CaseID);
    }
}
