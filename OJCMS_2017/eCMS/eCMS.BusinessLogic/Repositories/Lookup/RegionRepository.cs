//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************


using eCMS.DataLogic.Models.Lookup;
using eCMS.DataLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using eCMS.BusinessLogic.Repositories;
using eCMS.BusinessLogic.Repositories.Context;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class RegionRepository : BaseLookupRepository<Region>, IRegionRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public RegionRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<Region> FindAllByCountryID(int countryID)
        {
            return context.Region.Where(item => item.CountryID == countryID);
        }

        /// <summary>
        /// Add or Update region to database
        /// </summary>
        /// <param name="region">data to save</param>
        public void InsertOrUpdate(Region region)
        {
            region.LastUpdateDate = DateTime.Now;
            if (region.ID == default(int))
            {
                //set the date when this record was created
                region.CreateDate = region.LastUpdateDate;
                //set the id of the worker who has created this record
                //add the record to database
                context.Region.Add(region);
            }
            else
            {
                //update the record to database
                context.Entry(region).State = System.Data.Entity.EntityState.Modified;
            }
        }
        public IQueryable<Region> FindAllByWorkerID(int workerID, int programID)
        {
            if (workerID > 0 && programID > 0)
            {

                return context.WorkerInRole.Join(context.Region, left => left.RegionID, right => right.ID, (left, right) => new { left, right }).
              Where(item => item.left.WorkerID == workerID && item.left.ProgramID == programID).GroupBy(item => item.right).Select(item => item.Key);

            }
            else if (workerID > 0)
            {
                return context.WorkerInRole.Join(context.Region, left => left.RegionID, right => right.ID, (left, right) => new { left, right }).
                    Where(item => item.left.WorkerID == workerID).GroupBy(item => item.right).Select(item => item.Key);
            }
            return null;
        }

        public List<DropDownViewModel> NewFindAllByWorkerID(int workerID, int programID)
        {
            List<DropDownViewModel> Region = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT RG.ID,RG.Name ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append("INNER JOIN Region AS RG ON PR.RegionID = RG.ID ");
            sqlQuery.Append("WHERE WIR.WorkerID = " + workerID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            sqlQuery.Append("AND RG.IsActive = 1 ");
            if (programID > 0)
            {
                sqlQuery.Append("AND PR.ProgramID = " + programID + " ");
            }
            sqlQuery.Append("GROUP BY RG.ID,RG.Name ");
            sqlQuery.Append("ORDER BY RG.Name ");

            Region = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();

            return Region;
        }

        public List<SelectListItem> FindAllByPrograms(int workerID, int[] programIDs)
        {
            List<DropDownViewModel> Region = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            string programids = String.Join(",", programIDs);

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT RG.ID,RG.Name ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
            sqlQuery.Append("INNER JOIN Region AS RG ON PR.RegionID = RG.ID ");
            sqlQuery.Append("WHERE RG.IsActive = 1 ");
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorker.ID > 0)
            {
                sqlQuery.Append("AND WIR.WorkerID = " + CurrentLoggedInWorker.ID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            }
            if (!string.IsNullOrEmpty(programids))
            {
                sqlQuery.Append("AND PR.ProgramID IN (" + programids + ") ");
            }
            sqlQuery.Append("GROUP BY RG.ID,RG.Name ");
            sqlQuery.Append("ORDER BY RG.Name ");

            Region = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();

            return Region.AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
        }
    }


    /// <summary>
    /// interface of Region containing necessary database operations
    /// </summary>
    public interface IRegionRepository : IBaseLookupRepository<Region>
    {
        IQueryable<Region> FindAllByCountryID(int countryID);
        void InsertOrUpdate(Region region);
        IQueryable<Region> FindAllByWorkerID(int workerID, int programID);

        List<DropDownViewModel> NewFindAllByWorkerID(int workerID, int programID);
        List<SelectListItem> FindAllByPrograms(int workerID, int[] programIDs);

    }
}
