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
using System.Collections.Generic;
using System.Linq;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseWorkerMemberAssignmentRepository : BaseRepository<CaseWorkerMemberAssignment>, ICaseWorkerMemberAssignmentRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseWorkerMemberAssignmentRepository(RepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Add or Update caseworkermemberassignment to database
        /// </summary>
        /// <param name="caseworkermemberassignment">data to save</param>
        public void InsertOrUpdate(CaseWorkerMemberAssignment caseworkermemberassignment)
        {
            caseworkermemberassignment.LastUpdateDate = DateTime.Now;
            if (caseworkermemberassignment.ID == default(int))
            {
                //set the date when this record was created
                caseworkermemberassignment.CreateDate = caseworkermemberassignment.LastUpdateDate;
                //set the id of the worker who has created this record
                caseworkermemberassignment.CreatedByWorkerID = caseworkermemberassignment.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseWorkerMemberAssignment.Add(caseworkermemberassignment);
            }
            else
            {
                //update an existing record to database
                context.Entry(caseworkermemberassignment).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<CaseWorkerMemberAssignment> FindAllByCaseWorkerID(int caseWorkerID)
        {
            return context.CaseWorkerMemberAssignment.Where(item => item.CaseWorkerID == caseWorkerID).ToList();
        }
    }

    /// <summary>
    /// interface of CaseWorkerMemberAssignment containing necessary database operations
    /// </summary>
    public interface ICaseWorkerMemberAssignmentRepository : IBaseRepository<CaseWorkerMemberAssignment>
    {
        void InsertOrUpdate(CaseWorkerMemberAssignment caseworkermemberassignment);
        List<CaseWorkerMemberAssignment> FindAllByCaseWorkerID(int caseWorkerID);
    }
}
