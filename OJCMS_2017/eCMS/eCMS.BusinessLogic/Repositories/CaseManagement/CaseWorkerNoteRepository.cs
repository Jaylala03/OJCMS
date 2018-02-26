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
            var varcase = (from Case in context.Case.Where(c => c.ID == CaseWorkerNote.CaseID)
                            select new 
                            {
                                CaseStatusID = Case.CaseStatusID,
                                ProgramID = Case.ProgramID
                            }).FirstOrDefault();

            CaseWorkerNote.CaseStatusID = varcase.CaseStatusID;
            CaseWorkerNote.ProgramID = varcase.ProgramID;
            //CaseWorkerNote.CaseStatusID = context.Case.Where(c => c.ID == CaseWorkerNote.CaseID).Select(c => c.CaseStatusID).SingleOrDefault();

            CaseWorkerNote.LastUpdateDate = DateTime.Now;
            if (CaseWorkerNote.ID == default(int))
            {
                //set the date when this record was created
                CaseWorkerNote.CreateDate = CaseWorkerNote.LastUpdateDate;
                
                //set the id of the worker who has created this record
                CaseWorkerNote.CreatedByWorkerID = CaseWorkerNote.LastUpdatedByWorkerID;
                //CaseWorkerNote.CreatedByWorkerID = CaseWorkerNote.CreatedByWorkerID;
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

        //        public override void Delete(int id)
        //        {
        //            var entity = Find(id);
        //            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
        //            string sqlQuery = @"delete from [CaseAction] where CaseWorkerNoteid=@id;
        //delete from [CaseWorkerNote] where id=@id;";
        //            sqlQuery = sqlQuery.Replace("@id", id.ToString());
        //            context.Database.ExecuteSqlCommand(sqlQuery);

        //        }


        public DataSourceResult Search(DataSourceRequest dsRequest, int CaseId, int ProgramID)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }

            string sqlQuery = "";

            sqlQuery = "SELECT CWN.ID,CWN.CreateDate AS DateLogged,  CWN.Note AS Notes, " +
            "CM.Name AS ContactMethod, CWN.NoteDate AS ContactDate,  " +
            "CAST(CWN.TimeSpentHours AS varchar) + ' hrs ' + (CASE WHEN CWN.TimeSpentMinutes > 0 THEN(CAST(CWN.TimeSpentMinutes AS varchar) + 'mins') else '' END) AS TimeSpent,  " +
            "CS.Name AS CaseStatusAsDate, W.FirstName + ' ' + W.LastName AS LoggedBy, WNAT.Name AS WorkNoteWasLogged  " +
            "FROM CaseWorkerNote CWN  " +
            "INNER JOIN ContactMethod CM ON CM.ID = CWN.ContactMethodID  " +
            "INNER JOIN Worker W ON W.ID = CWN.CreatedByWorkerID  " +
            "INNER JOIN WorkerNoteActivityType WNAT ON WNAT.ID = CWN.WorkerNoteActivityTypeID  " +
            "INNER JOIN[Case] C ON C.ID = CWN.CaseID  " +
            "INNER JOIN Program P ON P.ID = CWN.ProgramID " +
            "INNER JOIN CaseStatus CS ON CS.ID = CWN.CaseStatusID " +
            "WHERE CWN.CaseID = " + CaseId + " AND CWN.ProgramID = " + ProgramID ;

            DataSourceResult dsResult = context.Database.SqlQuery<CaseWorkerNoteVM>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
            return dsResult;

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
        DataSourceResult Search(DataSourceRequest dsRequest, int CaseId, int ProgramID);
        string GetworkerNoteByCaseID(int CaseID);
    }
}
