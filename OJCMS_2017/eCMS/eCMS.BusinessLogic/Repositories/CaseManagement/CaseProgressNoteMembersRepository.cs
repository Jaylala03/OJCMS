//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
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
    public class CaseProgressNoteMembersRepository : BaseRepository<CaseProgressNoteMembers>, ICaseProgressNoteMembersRepository
    {
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
     
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseProgressNoteMembersRepository(RepositoryContext context
            , IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            ,IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(context)
        {
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
            
        }

      

        /// <summary>
        /// Add or Update caseprogressnote to database
        /// </summary>
        /// <param name="caseprogressnote">data to save</param>
        public void InsertOrUpdate(CaseProgressNoteMembers caseprogressnote)
        {
            bool isNew = false;
           
            if (caseprogressnote.ID == default(int))
            {
                isNew = true;
                //set the date when this record was created
               
                //add a new record to database
                context.CaseProgressNoteMembers.Add(caseprogressnote);
            }
            else
            {
                //update an existing record to database
                context.Entry(caseprogressnote).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
           
        }

    

      

        public override void Delete(int id)
        {
            var entity = Find(id);
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            string sqlQuery = @"
delete from [CaseProgressNoteMembers] where id=@id;";
            sqlQuery = sqlQuery.Replace("@id", id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery);
          
        }

        public DataSourceResult Search(DataSourceRequest dsRequest,  int? caseprogressNoteId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
           
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Delete, true);
            DataSourceResult dsResult = context.CaseProgressNoteMembers
                //.Join(context.CaseWorkerMemberAssignment, left => left.CaseMemberID, right => right.CaseMemberID, (left, right) => new { left, right })

                .Where(item => item.CaseProgressNoteID == caseprogressNoteId)              
                .ToDataSourceResult(dsRequest);
            return dsResult;
        }

        public List<CaseProgressNoteMembers> SearchMembers(int? caseprogressNoteId)
        {
           

            //bool hasEditPermission = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Edit, true);
            //bool hasDeletePermission = workerroleactionpermissionRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Delete, true);
            var dsResult = context.CaseProgressNoteMembers
                //.Join(context.CaseWorkerMemberAssignment, left => left.CaseMemberID, right => right.CaseMemberID, (left, right) => new { left, right })

                .Where(item => item.CaseProgressNoteID == caseprogressNoteId)
               .ToList();
            return dsResult;
        }
    }

    /// <summary>
    /// interface of CaseProgressNote containing necessary database operations
    /// </summary>
    public interface ICaseProgressNoteMembersRepository : IBaseRepository<CaseProgressNoteMembers>
    {
       
        void InsertOrUpdate(CaseProgressNoteMembers caseprogressnote);
        DataSourceResult Search(DataSourceRequest dsRequest, int? caseprogressNoteId);
        List<CaseProgressNoteMembers> SearchMembers( int? caseprogressNoteId);

    }
}
