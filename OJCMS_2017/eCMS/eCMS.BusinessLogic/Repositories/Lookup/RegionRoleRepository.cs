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
using EasySoft.Helper;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class RegionRoleRepository : BaseRepository<RegionRole>, IRegionRoleRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public RegionRoleRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<RegionRole> FindAllByRegionID(int regionID)
        {
            return context.RegionRole.Where(item => item.RegionID == regionID);
        }

        public IQueryable<RegionRole> FindAllByWorkerRoleID(int workerroleID)
        {
            return context.RegionRole.Where(item => item.WorkerRoleID == workerroleID);
        }

        public IQueryable<RegionRole> FindAllByWorkerRoleIDs(string workerroleID)
        {
            var data = context.RegionRole.Where(item => workerroleID.Contains(item.WorkerRoleID.ToString()));
            return data;
        }

        public RegionRole FindByRegionIDAndWorkerRoleID(int regionID, int workerroleID)
        {
            return context.RegionRole.SingleOrDefault(item => item.RegionID == regionID && item.WorkerRoleID == workerroleID);
        }


        /// <summary>
        /// Add or Update regionrole to database
        /// </summary>
        /// <param name="regionrole">data to save</param>
        public void InsertOrUpdate(int workerRoleID, string regionIDs)
        {
            string selectedRegion = regionIDs.Replace("false", string.Empty);
            string[] arraySelectedRegion = selectedRegion.ToStringArray(',', true);
            List<RegionRole> assignment = FindAllByWorkerRoleID(workerRoleID).ToList();
            if (arraySelectedRegion != null && arraySelectedRegion.Length > 0)
            {
                foreach (string RegionID in arraySelectedRegion)
                {
                    if (assignment.Where(item => item.RegionID == RegionID.ToInteger(true)).Count() == 0)
                    {
                        RegionRole newRegionRole = new RegionRole()
                        {
                            RegionID = RegionID.ToInteger(true),
                            WorkerRoleID = workerRoleID,
                            LastUpdateDate = DateTime.Now
                        };
                        InsertOrUpdate(newRegionRole);
                        Save();
                    }
                }
            }

            foreach (RegionRole existingMember in assignment)
            {
                if (arraySelectedRegion == null || !arraySelectedRegion.Contains(existingMember.RegionID.ToString(true)))
                {
                    Delete(existingMember);
                    Save();
                }
            }
            
        }

        public void InsertOrUpdate(RegionRole regionrole)
        {
            regionrole.LastUpdateDate = DateTime.Now;
            if (regionrole.ID == default(int))
            {
                //set the date when this record was created
                regionrole.CreateDate = regionrole.LastUpdateDate;
                //add a new record to database
                context.RegionRole.Add(regionrole);
            }
            else
            {
                //update an existing record to database
                context.Entry(regionrole).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<RegionRoleModel> FindAllForList()
        {
            List<RegionRoleModel> data = new List<RegionRoleModel>();
            var roleList = context.RegionRole.GroupBy(item => new { item.WorkerRoleID,item.WorkerRole.Name }).Select(item => item.Key).ToList();
            foreach (var role in roleList)
            {
                RegionRoleModel newRegionRoleModel = new RegionRoleModel()
                {
                    WorkerRoleName = role.Name
                };
                var regionRoleList = FindAllByWorkerRoleID(role.WorkerRoleID).ToList();
                if (regionRoleList != null)
                {
                    foreach (RegionRole regionRole in regionRoleList)
                    {
                        newRegionRoleModel.RegionNames = newRegionRoleModel.RegionNames.Concate(',', regionRole.Region.Name);
                    }
                }
                data.Add(newRegionRoleModel);
            }
            return data;
        }

    }

    /// <summary>
    /// interface of RegionRole containing necessary database operations
    /// </summary>
    public interface IRegionRoleRepository : IBaseRepository<RegionRole>
    {
        IQueryable<RegionRole> FindAllByRegionID(int regionID);
        IQueryable<RegionRole> FindAllByWorkerRoleID(int workerroleID);
        IQueryable<RegionRole> FindAllByWorkerRoleIDs(string workerroleIDs);
        RegionRole FindByRegionIDAndWorkerRoleID(int regionID, int workerroleID);
        void InsertOrUpdate(int workerRoleID, string regionIDs);
        void InsertOrUpdate(RegionRole regionrole);
        List<RegionRoleModel> FindAllForList();
    }
}
