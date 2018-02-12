//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.ExceptionLoging;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EasySoft.Helper;
using Kendo.Mvc.Extensions;
using eCMS.Shared;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseMemberProfileRepository : BaseRepository<CaseMemberProfile>, ICaseMemberProfileRepository
    {
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseMemberProfileRepository(RepositoryContext context,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository
            ,IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(context)
        {
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
        }

        public IQueryable<CaseMemberProfile> AllIncluding(int caseId, params Expression<Func<CaseMemberProfile, object>>[] includeProperties)
        {
            IQueryable<CaseMemberProfile> query = context.CaseMemberProfile;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseMember.CaseID == caseId);
        }

        public CaseMemberProfile FindByCaseMemberID(int casememberID)
        {
            return context.CaseMemberProfile.SingleOrDefault(item => item.CaseMemberID == casememberID);
        }

        /// <summary>
        /// Add or Update casememberprofile to database
        /// </summary>
        /// <param name="casememberprofile">data to save</param>
        public void InsertOrUpdate(CaseMemberProfile casememberprofile)
        {
            if (casememberprofile.ProfileTypeID == 1 && casememberprofile.ID == default(int))
            {
                int count = context.CaseMemberProfile.Where(item => item.CaseMemberID == casememberprofile.CaseMemberID && item.ProfileTypeID == casememberprofile.ProfileTypeID).Count();
                if (count > 0)
                {
                    throw new CustomException(CustomExceptionType.CommonDuplicacy, "You can't add multiple initial profile for the selected family or family member");
                }
            }
            if (casememberprofile.ProfileTypeID == 3 && casememberprofile.ID == default(int))
            {
                int count = context.CaseMemberProfile.Where(item => item.CaseMemberID == casememberprofile.CaseMemberID && item.ProfileTypeID == casememberprofile.ProfileTypeID).Count();
                if (count > 0)
                {
                    throw new CustomException(CustomExceptionType.CommonDuplicacy, "You can't add multiple discharge profile for the selected family or family member");
                }
            }
            if (casememberprofile.ProfileTypeID != 1 && casememberprofile.ID == default(int))
            {
                //Initial profile count
                int count = context.CaseMemberProfile.Where(item => item.CaseMemberID == casememberprofile.CaseMemberID && item.ProfileTypeID == 1).Count();
                if (count == 0)
                {
                    throw new CustomException(CustomExceptionType.CommonDuplicacy, "You should add initial profile first for the selected family or family member");
                }
            }
            if (casememberprofile.ProfileDate.IsValidDate())
            {
                //Check if there is any other profile on the same profile date
                CaseMemberProfile existingCaseMemberProfile = context.CaseMemberProfile.FirstOrDefault(item => item.CaseMemberID == casememberprofile.CaseMemberID && item.ProfileDate.Day == casememberprofile.ProfileDate.Day && item.ProfileDate.Month == casememberprofile.ProfileDate.Month && item.ProfileDate.Year == casememberprofile.ProfileDate.Year);
                if (existingCaseMemberProfile!=null && existingCaseMemberProfile.ID != casememberprofile.ID)
                {
                    throw new CustomException(CustomExceptionType.CommonDuplicacy, "A profile already exist with the same profile date for the selected family or family member");
                }
                else if (existingCaseMemberProfile != null)
                {
                    Remove(existingCaseMemberProfile);
                }
            }
            casememberprofile.LastUpdateDate = DateTime.Now;
            if (casememberprofile.ID == default(int))
            {
                //set the date when this record was created
                casememberprofile.CreateDate = casememberprofile.LastUpdateDate;
                //set the id of the worker who has created this record
                casememberprofile.CreatedByWorkerID = casememberprofile.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseMemberProfile.Add(casememberprofile);
            }
            else
            {
                //update an existing record to database
                context.Entry(casememberprofile).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();
            }
            if (caseMemberId.HasValue && caseMemberId > 0)
            {
                FilterDescriptor filterDescriptor = new FilterDescriptor("CaseMemberID", FilterOperator.IsEqualTo, caseMemberId.Value);
                dsRequest.Filters.Add(filterDescriptor);
            }
            bool hasReadPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Read, true);
            bool hasEditPermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Edit, true);
            bool hasDeletePermission = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.CaseManagement, Constants.Controllers.CaseMemberProfile, Constants.Actions.Delete, true);
            bool IsUserAdminWorker = CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1;
            DataSourceResult dsResult = context.CaseMemberProfile
                //.Join(context.CaseWorkerMemberAssignment, left => left.CaseMemberID, right => right.CaseMemberID, (left, right) => new { left, right })
                //.Where(item => item.left.CaseMember.CaseID == caseId && item.right.CaseWorker.WorkerID == workerId)
                .Where(item => item.CaseMember.CaseID == caseId)
                //<JL:Comment:06/18/2017>
                //.Where(item => context.CaseWorkerMemberAssignment.Where(worker => worker.CaseWorker.WorkerID == workerId).Select(member => member.CaseMemberID).Contains(item.CaseMemberID) || IsUserAdminWorker)
                .OrderByDescending(item => item.ProfileDate).ToList()
                .AsEnumerable()
                .Select(
                item => new CaseMemberProfile()
                {
                    ID=item.ID,
                    CreatedByWorkerName = item.CreatedByWorker.FirstName + " " + item.CreatedByWorker.LastName,
                    ProfileDate=item.ProfileDate,
                    ProfileTypeName = item.ProfileType!=null ? item.ProfileType.Name : "",
                    HighestLevelOfEducationName = item.HighestLevelOfEducation!=null ? item.HighestLevelOfEducation.Name : "",
                    AnnualIncomeName = item.AnnualIncome!=null ? item.AnnualIncome.Name : "",
                    HousingQualityName = item.HousingQuality!=null ? item.HousingQuality.Name : "",
                    HousingQualityNote=item.HousingQualityNote,
                    HealthCondition=item.HealthCondition,
                    Occupation=item.Occupation,
                    CaseID = item.CaseMember.CaseID,
                    CaseMemberID=item.CaseMemberID,
                    ProfileTypeID=item.ProfileTypeID,
                    HasPermissionToRead = IsUserAdminWorker || hasReadPermission ? "" : "display:none;",
                    //HasPermissionToEdit = IsUserAdminWorker || (item.ProfileTypeID != 2 && hasEditPermission) ? "" : "display:none;",
                    HasPermissionToEdit = IsUserAdminWorker ||  hasEditPermission ? "" : "display:none;",
                    HasPermissionToDelete = IsUserAdminWorker || hasDeletePermission ? "" : "display:none;"
                }
                ).ToDataSourceResult(dsRequest);
            return dsResult;
        }
    }

    /// <summary>
    /// interface of CaseMemberProfile containing necessary database operations
    /// </summary>
    public interface ICaseMemberProfileRepository : IBaseRepository<CaseMemberProfile>
    {
        IQueryable<CaseMemberProfile> AllIncluding(int caseId, params Expression<Func<CaseMemberProfile, object>>[] includeProperties);
        CaseMemberProfile FindByCaseMemberID(int casememberID);
        void InsertOrUpdate(CaseMemberProfile casememberprofile);
        DataSourceResult Search(DataSourceRequest dsRequest, int caseId, int workerId, int? caseMemberId);
    }
}
