//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using EasySoft.Helper;
using eCMS.DataLogic.ViewModels;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class RegionSubProgramRepository : BaseRepository<RegionSubProgram>, IRegionSubProgramRepository
    {
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public RegionSubProgramRepository(RepositoryContext context)
            : base(context)
        {
        }

        public IQueryable<RegionSubProgram> FindAllByRegionID(int regionID)
        {
            return context.RegionSubProgram.Where(item => item.RegionID == regionID);
        }

        public IQueryable<RegionSubProgram> FindAllBySubProgramID(int subprogramID)
        {
            return context.RegionSubProgram.Where(item => item.SubProgramID == subprogramID);
        }

        public RegionSubProgram FindByRegionIDAndSubProgramID(int regionID, int subprogramID)
        {
            return context.RegionSubProgram.SingleOrDefault(item => item.RegionID == regionID && item.SubProgramID == subprogramID);
        }

        /// <summary>
        /// Add or Update regionrole to database
        /// </summary>
        /// <param name="regionrole">data to save</param>
        public void InsertOrUpdate(int subProgramID, string regionIDs)
        {
            string selectedRegion = regionIDs.Replace("false", string.Empty);
            string[] arraySelectedRegion = selectedRegion.ToStringArray(',', true);
            List<RegionSubProgram> assignment = FindAllBySubProgramID(subProgramID).ToList();
            if (arraySelectedRegion != null && arraySelectedRegion.Length > 0)
            {
                foreach (string RegionID in arraySelectedRegion)
                {
                    if (assignment.Where(item => item.RegionID == RegionID.ToInteger(true)).Count() == 0)
                    {
                        RegionSubProgram newRegionSubProgram = new RegionSubProgram()
                        {
                            RegionID = RegionID.ToInteger(true),
                            SubProgramID = subProgramID,
                            LastUpdateDate = DateTime.Now
                        };
                        InsertOrUpdate(newRegionSubProgram);
                        Save();
                    }
                }
            }

            foreach (RegionSubProgram existingMember in assignment)
            {
                if (arraySelectedRegion == null || !arraySelectedRegion.Contains(existingMember.RegionID.ToString(true)))
                {
                    Delete(existingMember);
                    Save();
                }
            }

        }

        /// <summary>
        /// Add or Update regionsubprogram to database
        /// </summary>
        /// <param name="regionsubprogram">data to save</param>
        public void InsertOrUpdate(RegionSubProgram regionsubprogram)
        {
            regionsubprogram.LastUpdateDate = DateTime.Now;
            if (regionsubprogram.ID == default(int))
            {
                //set the date when this record was created
                regionsubprogram.CreateDate = regionsubprogram.LastUpdateDate;
                //add a new record to database
                context.RegionSubProgram.Add(regionsubprogram);
            }
            else
            {
                //update an existing record to database
                context.Entry(regionsubprogram).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public List<RegionSubProgram> FindAllByRegionIDsAndProgramIDs(int[] regionIds, int[] programIds)
        {
            var query = context.RegionSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right }).Where(o => regionIds.Contains(o.left.RegionID) && programIds.Contains(o.right.ProgramID)).Select(item=>item.left).ToList();
            return query;
        }

        public List<RegionSubProgramModel> FindAllForList()
        {
            List<RegionSubProgramModel> data = new List<RegionSubProgramModel>();
            var roleList = context.RegionSubProgram.Join(context.SubProgram, left => left.SubProgramID, right => right.ID, (left, right) => new { left, right }).GroupBy(item => new { item.left.SubProgramID, SubProgramName = item.left.SubProgram.Name, item.right.ProgramID, ProgramName = item.right.Program.Name }).Select(item => item.Key).ToList();
            foreach (var role in roleList)
            {
                RegionSubProgramModel newRegionSubProgramModel = new RegionSubProgramModel()
                {
                    SubProgramName = role.SubProgramName,
                    ProgramName = role.ProgramName
                };
                var regionsubprogramList = FindAllBySubProgramID(role.SubProgramID).ToList();
                if (regionsubprogramList != null)
                {
                    foreach (RegionSubProgram regionsubprogram in regionsubprogramList)
                    {
                        newRegionSubProgramModel.RegionNames = newRegionSubProgramModel.RegionNames.Concate(',', regionsubprogram.Region.Name);
                    }
                }
                data.Add(newRegionSubProgramModel);
            }
            return data;
        }

    }

    /// <summary>
    /// interface of RegionSubProgram containing necessary database operations
    /// </summary>
    public interface IRegionSubProgramRepository : IBaseRepository<RegionSubProgram>
    {
        IQueryable<RegionSubProgram> FindAllByRegionID(int regionID);
        IQueryable<RegionSubProgram> FindAllBySubProgramID(int subprogramID);
        RegionSubProgram FindByRegionIDAndSubProgramID(int regionID, int subprogramID);
        void InsertOrUpdate(RegionSubProgram regionsubprogram);
        List<RegionSubProgram> FindAllByRegionIDsAndProgramIDs(int[] regionIds,int[] programIds);
        void InsertOrUpdate(int subProgramID, string regionIDs);
        List<RegionSubProgramModel> FindAllForList();
    }
}
