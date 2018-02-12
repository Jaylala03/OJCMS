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
using eCMS.ExceptionLoging;
using System.Data.SqlClient;
using System.Transactions;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class PermissionRegionRepository : BaseRepository<PermissionRegion>, IPermissionRegionRepository
    {
        protected IPermissionSubProgramRepository permissionsubprogramRepository;
        protected IPermissionJamatkhanaRepository permissionjamatkhanaRepository;
        protected IRegionRepository regionRepository;
        protected IJamatkhanaRepository jamatkhanaRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public PermissionRegionRepository(RepositoryContext context,
            IPermissionSubProgramRepository permissionsubprogramRepository,
            IPermissionJamatkhanaRepository permissionjamatkhanaRepository,
            IRegionRepository regionRepository
            , IJamatkhanaRepository jamatkhanaRepository)
            : base(context)
        {
            this.permissionsubprogramRepository = permissionsubprogramRepository;
            this.permissionjamatkhanaRepository = permissionjamatkhanaRepository;
            this.regionRepository = regionRepository;
            this.jamatkhanaRepository = jamatkhanaRepository;
        }

        public IQueryable<PermissionRegion> AllIncluding(int permissionId, params Expression<Func<PermissionRegion, object>>[] includeProperties)
        {
            IQueryable<PermissionRegion> query = context.PermissionRegion;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.PermissionID == permissionId).GroupBy(m => new { m.ProgramID, m.RegionID, m.PermissionID }).Select(m => m.FirstOrDefault());
        }

        public List<PermissionRegion> FindAllByPermissionID(int permissionID)
        {
            return context.PermissionRegion.Where(item => item.PermissionID == permissionID).GroupBy(m => new { m.ProgramID, m.RegionID, m.PermissionID }).Select(m => m.FirstOrDefault()).ToList();
        }

        /// <summary>
        /// Add or Update permissionregion to database
        /// </summary>
        /// <param name="permissionregion">data to save</param>
        public void InsertOrUpdate(PermissionRegion permissionregion)
        {
            if (permissionregion.RegionID == -1)
            {
                Add_AllRegionProgramSubprogramJamatkhana_ForPermission(permissionregion);
            }
            else
            {
                var existingPermissionRegion = context.PermissionRegion.SingleOrDefault(item => item.PermissionID == permissionregion.PermissionID && item.ProgramID == permissionregion.ProgramID && item.RegionID == permissionregion.RegionID);
                if (existingPermissionRegion != null && existingPermissionRegion.ID != permissionregion.ID)
                {
                    permissionregion.ID = existingPermissionRegion.ID;
                    permissionregion.CreateDate = existingPermissionRegion.CreateDate;
                    permissionregion.CreatedByWorkerID = existingPermissionRegion.CreatedByWorkerID;
                    Remove(existingPermissionRegion);
                }
                permissionregion.LastUpdateDate = DateTime.Now;
                if (permissionregion.ID == default(int))
                {
                    //set the date when this record was created
                    permissionregion.CreateDate = permissionregion.LastUpdateDate;
                    //set the id of the worker who has created this record
                    permissionregion.CreatedByWorkerID = permissionregion.LastUpdatedByWorkerID;
                    //add a new record to database
                    context.PermissionRegion.Add(permissionregion);
                }
                else
                {
                    //update an existing record to database
                    context.Entry(permissionregion).State = System.Data.Entity.EntityState.Modified;
                }
                Save();
                permissionsubprogramRepository.InsertOrUpdate(permissionregion.ID, permissionregion.SubProgramIDs);
                if (permissionregion.JamatkhanaIDs != null)
                    permissionjamatkhanaRepository.InsertOrUpdate(permissionregion.ID, permissionregion.JamatkhanaIDs.Select(x => Int32.Parse(x)).ToList());
            }
        }

        public void Add_AllRegionProgramSubprogramJamatkhana_ForPermission(PermissionRegion permissionregion)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DeleteByPermissionId(permissionregion.PermissionID);
                permissionregion.LastUpdateDate = DateTime.Now;
                List<SelectListItem> listvm = regionRepository.AllActiveForDropDownList;

                List<int> allselectedJK = null;
                List<int> selectedregionJK = null;
                if (permissionregion.JamatkhanaIDs != null)
                {
                    allselectedJK = permissionregion.JamatkhanaIDs.Select(x => Int32.Parse(x)).ToList();
                }
                foreach (SelectListItem item in listvm)
                {
                    PermissionRegion newPR = new PermissionRegion();
                    newPR.PermissionID = permissionregion.PermissionID;
                    newPR.ProgramID = permissionregion.ProgramID;
                    newPR.RegionID = Convert.ToInt32(item.Value);
                    if (allselectedJK != null)
                    {
                        List<int> regionJK = jamatkhanaRepository.FindAllJamatkhanaIDsByRegionID(newPR.RegionID);
                        selectedregionJK = regionJK.Intersect(allselectedJK).ToList();
                    }
                    //set the date when this record was created
                    newPR.CreateDate = permissionregion.LastUpdateDate;
                    newPR.LastUpdateDate = permissionregion.LastUpdateDate;
                    //set the id of the worker who has created this record
                    newPR.CreatedByWorkerID = permissionregion.LastUpdatedByWorkerID;
                    newPR.LastUpdatedByWorkerID = permissionregion.LastUpdatedByWorkerID;
                    newPR.IsArchived = false;
                    //add a new record to database
                    context.PermissionRegion.Add(newPR);
                    Save();
                    permissionsubprogramRepository.InsertOrUpdate(newPR.ID, permissionregion.SubProgramIDs);
                    if (selectedregionJK != null)
                    {
                        permissionjamatkhanaRepository.InsertOrUpdate(newPR.ID, selectedregionJK);
                    }
                }
                scope.Complete();
            }
        }
        public override PermissionRegion Find(int id)
        {
            return context.PermissionRegion.SingleOrDefault(item => item.ID == id);
        }

        public override void Delete(int id)
        {
            string sqlQuery = "DELETE FROM PermissionSubProgram WHERE PermissionRegionID = " + id.ToString() +
                ";DELETE FROM PermissionJamatkhana WHERE PermissionRegionID=" + id.ToString() +
                ";DELETE FROM PermissionRegion WHERE ID=" + id.ToString() + ";";
            context.Database.ExecuteSqlCommand(sqlQuery);
        }

        public void DeleteByPermissionId(int PermissionId)
        {
            string sqlQuery = "DELETE FROM PermissionSubProgram WHERE PermissionRegionID IN (SELECT ID FROM PermissionRegion WHERE PermissionId = " + PermissionId.ToString() + ") " +
                ";DELETE FROM PermissionJamatkhana WHERE PermissionRegionID IN (SELECT ID FROM PermissionRegion WHERE PermissionId = " + PermissionId.ToString() + ") " +
                ";DELETE FROM PermissionRegion WHERE PermissionId = " + PermissionId.ToString() + ";";
            context.Database.ExecuteSqlCommand(sqlQuery);
        }
    }

    /// <summary>
    /// interface of PermissionRegion containing necessary database operations
    /// </summary>
    public interface IPermissionRegionRepository : IBaseRepository<PermissionRegion>
    {
        IQueryable<PermissionRegion> AllIncluding(int permissionId, params Expression<Func<PermissionRegion, object>>[] includeProperties);
        List<PermissionRegion> FindAllByPermissionID(int permissionID);
        void InsertOrUpdate(PermissionRegion permissionregion);
        void DeleteByPermissionId(int PermissionId);
    }
}
