//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using eCMS.DataLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class ProgramRepository : BaseLookupRepository<Program>, IProgramRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public ProgramRepository(RepositoryContext context)
            : base(context)
        {
        }





        /// <summary>
        /// Add or Update program to database
        /// </summary>
        /// <param name="program">data to save</param>
        public void InsertOrUpdate(Program program)
        {
            program.LastUpdateDate = DateTime.Now;
            if (program.ID == default(int))
            {
                //set the date when this record was created
                program.CreateDate = program.LastUpdateDate;
                //add a new record to database
                context.Program.Add(program);
            }
            else
            {
                //update an existing record to database
                context.Entry(program).State = System.Data.Entity.EntityState.Modified;
            }
        }
        public IQueryable<Program> FindAllByWorkerID(int workerID)
        {
           
                return context.WorkerInRole.Join(context.Program, left => left.ProgramID, right => right.ID, (left, right) => new { left, right }).
                    Where(item => item.left.WorkerID == workerID).GroupBy(item => item.right).Select(item => item.Key);
           
        }
        public List<DropDownViewModel> NewFindAllByWorkerID(int workerID)
        {
            List<DropDownViewModel> program = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT PRG.ID,PRG.Name ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR "); 
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID "); 
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append("INNER JOIN Program AS PRG ON PR.ProgramID = PRG.ID ");
            sqlQuery.Append("WHERE WIR.WorkerID = " + workerID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            sqlQuery.Append("AND PRG.IsActive = 1 ");
            sqlQuery.Append("GROUP BY PRG.ID,PRG.Name ");
            sqlQuery.Append("ORDER BY PRG.Name ");

            program = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();

            return program;
        }
    }

    /// <summary>
    /// interface of Program containing necessary database operations
    /// </summary>
    public interface IProgramRepository : IBaseLookupRepository<Program>
    {
        void InsertOrUpdate(Program program);
        IQueryable<Program> FindAllByWorkerID(int workerID);

        List<DropDownViewModel> NewFindAllByWorkerID(int workerID);
    }
}
