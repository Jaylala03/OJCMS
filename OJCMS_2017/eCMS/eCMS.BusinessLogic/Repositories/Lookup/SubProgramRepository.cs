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
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class SubProgramRepository : BaseLookupRepository<SubProgram>, ISubProgramRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public SubProgramRepository(RepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Add or Update program to database
        /// </summary>
        /// <param name="program">data to save</param>
        public void InsertOrUpdate(SubProgram program)
        {
            program.LastUpdateDate = DateTime.Now;
            if (program.ID == default(int))
            {
                //set the date when this record was created
                program.CreateDate = program.LastUpdateDate;
                //add a new record to database
                context.SubProgram.Add(program);
            }
            else
            {
                //update an existing record to database
                context.Entry(program).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<SelectListItem> FindAllForDropDownListByProgramID(int programID)
        {
            var data = context.SubProgram.Where(item => item.IsActive == true && item.ProgramID == programID).OrderBy(item => item.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
            return data;
        }

        public List<SubProgram> FindAllByProgramID(int programID)
        {
            var data = context.SubProgram.Where(item => item.IsActive == true && item.ProgramID == programID).OrderBy(item => item.Name).ToList();
            return data;
        }

        public List<SelectListItem> FindAllForDropDownListByProgramIDAndWorkerIDAndRegionID(int programID, int workerID, int regionID)
        {
            if (programID > 0 && regionID > 0 && workerID>0)
            {
                var data = context.WorkerSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.WorkerInRole.WorkerID == workerID && item.right.ProgramID == programID && item.left.WorkerInRole.RegionID == regionID)
                    .GroupBy(item => new { item.right.ID, item.right.Name })
                    .OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                return data;
            }
            else if (programID > 0 && workerID > 0)
            {
                var data = context.WorkerSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.WorkerInRole.WorkerID == workerID && item.right.ProgramID == programID)
                    .GroupBy(item => new { item.right.ID, item.right.Name })
                    .OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                return data;
            }
            else if (regionID > 0 && workerID > 0)
            {
                var data = context.WorkerSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.WorkerInRole.WorkerID == workerID && item.left.WorkerInRole.RegionID == regionID)
                    .GroupBy(item => new { item.right.ID, item.right.Name })
                    .OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                return data;
            }
            else if (programID > 0 && regionID > 0)
            {
                var data = context.WorkerSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.right.ProgramID == programID && item.left.WorkerInRole.RegionID == regionID)
                    .GroupBy(item => new { item.right.ID, item.right.Name })
                    .OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                //var data = context.WorkerInRole.Join(context.WorkerSubProgram, left => left.ID, right => right.WorkerInRoleID, (left, right) => new { left, right })
                //    .Join(context.SubProgram, secondleft => secondleft.right.SubProgramID, secondright => secondright.ID, (secondleft, secondright) => new { secondleft, secondright })
                //    .Where(item => item.secondright.ProgramID == programID && item.secondleft.left.RegionID == regionID)
                //    .GroupBy(item => new { item.secondright.ID, item.secondright.Name })
                //    .OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                return data;
            }
            else if (programID > 0)
            {
                var data = context.SubProgram.Where(item=>item.ProgramID==programID).GroupBy(item => new { item.ID, item.Name }).OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                return data;
            }
            else if (regionID > 0)
            {
                var data = context.WorkerSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.WorkerInRole.RegionID == regionID)
                    .GroupBy(item => new { item.right.ID, item.right.Name })
                    .OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                return data;
            }
            else
            {
                var data = context.SubProgram.GroupBy(item => new { item.ID, item.Name }).OrderBy(item => item.Key.Name).AsEnumerable().Select(item => new SelectListItem() { Text = item.Key.Name, Value = item.Key.ID.ToString() }).ToList();
                return data;
            }
        }

        public List<SelectListItem> AllByRegionAndProgram(int programID, int workerID, int regionID)
        {
            List<DropDownViewModel> subprogram = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT SPRG.ID,SPRG.Name ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR "); 
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID "); 
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append("INNER JOIN PermissionSubProgram PSPRG ON PR.ID = PSPRG.PermissionRegionID ");
            sqlQuery.Append("INNER JOIN SubProgram AS SPRG ON PSPRG.SubProgramID = SPRG.ID ");
            sqlQuery.Append("WHERE SPRG.IsActive = 1 ");
            if (workerID > 0)
            {
                sqlQuery.Append("AND WIR.WorkerID = " + workerID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            }
            if (programID > 0)
            {
                sqlQuery.Append("AND PR.ProgramID = " + programID + " ");
            }
            if (regionID > 0)
            {
                sqlQuery.Append("AND PR.RegionID = " + regionID + " ");
            }
            sqlQuery.Append("GROUP BY SPRG.ID,SPRG.Name ");
            sqlQuery.Append("ORDER BY SPRG.Name ");

            subprogram = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();

            return subprogram.AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

        public List<SelectListItem> AllByRegionIDsAndProgramIDs(int[] regionIDs,int[] programIDs)
        {
            List<DropDownViewModel> subprogram = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            string programids = String.Join(",", programIDs);
            string regionids = String.Join(",", regionIDs);

            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT SPRG.ID,SPRG.Name ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append("INNER JOIN PermissionSubProgram PSPRG ON PR.ID = PSPRG.PermissionRegionID ");
            sqlQuery.Append("INNER JOIN SubProgram AS SPRG ON PSPRG.SubProgramID = SPRG.ID ");
            sqlQuery.Append("WHERE SPRG.IsActive = 1 ");

            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorker.ID > 0)
            {
                sqlQuery.Append("AND WIR.WorkerID = " + CurrentLoggedInWorker.ID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            }
            if (!string.IsNullOrEmpty(programids))
            {
                sqlQuery.Append("AND PR.ProgramID IN (" + programids + ") ");
            }
            if (!string.IsNullOrEmpty(regionids))
            {
                sqlQuery.Append("AND PR.RegionID IN (" + regionids + ") ");
            }
            sqlQuery.Append("GROUP BY SPRG.ID,SPRG.Name ");
            sqlQuery.Append("ORDER BY SPRG.Name ");

            subprogram = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();

            return subprogram.AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }

        public List<SubProgram> FindAllByWordkerID(int workerID, int programID)
        {
            if (programID > 0 && workerID > 0)
            {
                var data = context.WorkerSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.WorkerInRole.WorkerID == workerID && item.right.ProgramID == programID)
                    .GroupBy(item => new { item.right })
                    .OrderBy(item => item.Key.right.Name).Select(item => item.Key.right).ToList();
                return data;
            }
            else if (workerID > 0)
            {
                var data = context.WorkerSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right })
                    .Where(item => item.left.WorkerInRole.WorkerID == workerID)
                    .GroupBy(item => new { item.right })
                    .OrderBy(item => item.Key.right.Name).Select(item => item.Key.right).ToList();
                return data;
            }
            return null;
        }
    }

    /// <summary>
    /// interface of SubProgram containing necessary database operations
    /// </summary>
    public interface ISubProgramRepository : IBaseLookupRepository<SubProgram>
    {
        void InsertOrUpdate(SubProgram program);
        List<SelectListItem> FindAllForDropDownListByProgramIDAndWorkerIDAndRegionID(int programID, int workerID, int regionID);
        List<SelectListItem> FindAllForDropDownListByProgramID(int programID);
        List<SubProgram> FindAllByProgramID(int programID);
        List<SubProgram> FindAllByWordkerID(int workerID, int programID);

        List<SelectListItem> AllByRegionAndProgram(int programID, int workerID, int regionID);

        List<SelectListItem> AllByRegionIDsAndProgramIDs(int[] regionIds, int[] programIds);
    }
}
