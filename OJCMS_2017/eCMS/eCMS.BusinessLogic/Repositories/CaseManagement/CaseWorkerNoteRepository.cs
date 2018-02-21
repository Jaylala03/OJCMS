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
    public class CaseWorkerNoteRepository : BaseRepository<CaseWorkerNote>, ICaseWorkerNoteRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseWorkerNoteRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<CaseWorkerNote> AllIncluding(int caseId, params Expression<Func<CaseWorkerNote, object>>[] includeProperties)
        {
            IQueryable<CaseWorkerNote> query = context.CaseWorkerNote;
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
        /// Add or Update CaseWorkerNote to database
        /// </summary>
        /// <param name="CaseWorkerNote">data to save</param>
        public void InsertOrUpdate(CaseWorkerNote CaseWorkerNote)
        {
            CaseWorkerNote.LastUpdateDate = DateTime.Now;
            if (CaseWorkerNote.ID == default(int))
            {
                //set the date when this record was created
                CaseWorkerNote.CreateDate = CaseWorkerNote.LastUpdateDate;
                //set the id of the worker who has created this record
                CaseWorkerNote.CreatedByWorkerID = CaseWorkerNote.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseWorkerNote.Add(CaseWorkerNote);
            }
            else
            {
                //update an existing record to database
                context.Entry(CaseWorkerNote).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
            
        }

        public override void Delete(int id)
        {
            var entity = Find(id);
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            string sqlQuery = @"delete from [CaseAction] where CaseWorkerNoteid=@id;
delete from [CaseWorkerNote] where id=@id;";
            sqlQuery = sqlQuery.Replace("@id", id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery);

        }

        public DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, List<int> workerRoleIDs)
        {
            //if (dsRequest.Filters == null)
            //{
            //    dsRequest.Filters = new List<IFilterDescriptor>();
            //}
            //bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorkerNote, Constants.Actions.Read, true);
            //bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorkerNote, Constants.Actions.Edit, true);
            //bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseWorkerNote, Constants.Actions.Delete, true);

            //string sqlQuery = "";
            //sqlQuery += "DECLARE @WorkerIds nvarchar(max) ; ";
            //sqlQuery += " SET @WorkerIds='" + workerRoleIDs + "' ;";
            //sqlQuery += " SELECT " + caseId + " AS CaseID,cpn.ID,(SELECT STUFF((SELECT ', ' + (cm1.FirstName+' '+cm1.LastName) from CaseWorkerNote cpn1 left join CaseWorkerNoteMembers cpm1 on cpn1.ID=cpm1.CaseWorkerNoteID left join CaseMember cm1 on cm1.ID=cpm1.CaseMemberID or cm1.ID=cpn1.CaseMemberID where cm1.CaseID=" + caseId + " and cpn1.ID=cpn.ID ";
            //sqlQuery += " ";
            //sqlQuery += "" + (caseMemberId.HasValue && caseMemberId > 0 ? " and cm1.ID=" + caseMemberId + "" : "") + "";
            //sqlQuery += " FOR XML PATH('')), 1, 2, '')) as CaseMemberName";
            //sqlQuery += ",cpn.NoteDate,at.Name as ActivityTypeName,cpn.Note,'" + (hasReadPermission ? "" : "display:none;") + "' AS HasPermissionToRead,'" + (hasEditPermission ? "" : "display:none;") + "' AS HasPermissionToEdit,'" + (hasDeletePermission ? "" : "display:none;") + "' AS HasPermissionToDelete  from CaseWorkerNote cpn";
            //sqlQuery += " join ActivityType at on  at.ID=cpn.ActivityTypeID  left join CaseWorkerNoteMembers cpm on cpn.ID=cpm.CaseWorkerNoteID ";
            //sqlQuery += " left join CaseMember cm on cm.ID=cpm.CaseMemberID or cm.ID=cpn.CaseMemberID where cm.CaseID=" + caseId + "";
            //sqlQuery += "";
            //sqlQuery += "" + (caseMemberId.HasValue && caseMemberId > 0 ? " and cm.ID=" + caseMemberId + "" : "") + "";
            //sqlQuery += " group by cpn.ID,NoteDate,at.Name,Note";

            //DataSourceResult dsResult = context.Database.SqlQuery<CaseWorkerNoteModel>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
            //return dsResult;

            return null;
        }

        public string GetworkerNoteByCaseID(int CaseID)
        {
            string note = "";

            //string sqlQuery = @"SELECT dbo.GetLatNotesByCaseID(@id) AS Note";
            //sqlQuery = sqlQuery.Replace("@id", CaseID.ToString());
            //var response = context.Database.SqlQuery<CaseWorkerNoteModel>(sqlQuery.ToString()).ToList();
            //if (response != null && response.Count > 0)
            //{
            //    note = response[0].Note;
            //}
            return note;
        }
    }

    /// <summary>
    /// interface of CaseWorkerNote containing necessary database operations
    /// </summary>
    public interface ICaseWorkerNoteRepository : IBaseRepository<CaseWorkerNote>
    {
        void InsertOrUpdate(CaseWorkerNote CaseWorkerNote);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId, List<int> workerRoleIDs);
        string GetworkerNoteByCaseID(int CaseID);
    }
}
