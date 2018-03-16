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
using eCMS.BusinessLogic.Helpers;
using System.Web.Mvc;
using EasySoft.Helper;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseMemberRepository : BaseRepository<CaseMember>, ICaseMemberRepository
    {
        private readonly ICaseWorkerMemberAssignmentRepository caseworkermemberassignmentRepository;
        private readonly ICaseWorkerRepository caseworkerRepository;

        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseMemberRepository(RepositoryContext context, 
            ICaseWorkerMemberAssignmentRepository caseworkermemberassignmentRepository,
            ICaseWorkerRepository caseworkerRepository)
            : base(context)
        {
            this.caseworkermemberassignmentRepository=caseworkermemberassignmentRepository;
            this.caseworkerRepository = caseworkerRepository;
        }

        public IQueryable<CaseMember> AllIncluding(int caseId, params Expression<Func<CaseMember, object>>[] includeProperties)
        {
            IQueryable<CaseMember> query = context.CaseMember;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseID == caseId);
        }


        public List<SelectListItem> FindAllByCaseIDForDropDownList(int caseID)
        {
            return context.CaseMember.Where(item => item.CaseID == caseID).AsEnumerable().Select(item => new SelectListItem { Value = item.ID.ToString(), Text = item.FirstName + " " + item.LastName }).ToList();
        }

        public List<SelectListItem> FindAllByCaseIDAndWorkerIDForDropDownList(int caseID,int workerID)
        {
            return context.CaseMember.Join(context.CaseWorkerMemberAssignment, left => left.ID, right => right.CaseMemberID, (left, right) => new { left,right }).Where(item => item.left.CaseID == caseID && item.right.CaseWorker.WorkerID==workerID).AsEnumerable().Select(item => new SelectListItem { Value = item.left.ID.ToString(), Text = item.left.FirstName + " " + item.left.LastName }).ToList();
        }

        public List<SelectListItem> FindAllByCaseIDAndWorkerIDForDropDownListForTermination(int caseID, int workerID)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            var memberList = context.CaseMember.Join(context.CaseWorkerMemberAssignment, left => left.ID, right => right.CaseMemberID, (left, right) => new { left, right }).Where(item => item.left.CaseID == caseID && item.right.CaseWorker.WorkerID == workerID).AsEnumerable().Select(item => new SelectListItem { Value = item.left.ID.ToString(), Text = item.left.FirstName + " " + item.left.LastName }).ToList();
            if (memberList != null)
            {
                foreach (SelectListItem member in memberList)
                {
                    int caseMemberID = member.Value.ToInteger(true);
                    if (context.CaseAssessment.Where(item => item.CaseMemberID == caseMemberID && item.AssessmentTypeID == 3).Count() == 0)
                    {
                        selectListItems.Add(member);
                    }
                }
            }
            return selectListItems;
        }

        /// <summary>
        /// Add or Update casemember to database
        /// </summary>
        /// <param name="casemember">data to save</param>
        public void InsertOrUpdate(CaseMember casemember)
        {
            //set a member as primary if there is no primary member has been set
            if (!casemember.IsPrimary)
            {
                int count = context.CaseMember.Where(item => item.CaseID == casemember.CaseID && item.IsPrimary==true).Count();
                if (count == 0)
                {
                    casemember.IsPrimary = true;
                }
            }
            string caseDisplayID = string.Empty;
            //when a member is set as primary, release others from primary is there is any
            if (casemember.IsPrimary && casemember.CaseID>0)
            {
                string sqlQuery = "UPDATE CaseMember SET IsPrimary=0 WHERE CaseID=" + casemember.CaseID;
                context.Database.ExecuteSqlCommand(sqlQuery);
                Case varCase = context.Case.SingleOrDefault(item => item.ID == casemember.CaseID);
                if (varCase != null)
                {
                    string regionCode = varCase.Region.Code;
                    if (regionCode.IsNullOrEmpty())
                    {
                        if (varCase.Region.Name.IsNotNullOrEmpty())
                        {
                            regionCode = varCase.Region.Name.Substring(0, 2).ToUpper();
                        }
                    }
                    if (varCase.DisplayID.IsNotNullOrEmpty() && varCase.DisplayID.Contains("NA"))
                    {
                        varCase.DisplayID = MiscUtility.GetCasePersonalizedId(regionCode, casemember.FirstName, casemember.LastName, casemember.CaseID, varCase.DisplayID);
                    }
                    context.Entry(varCase).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    caseDisplayID = varCase.DisplayID;
                }
            }
            if (casemember.DisplayID.IsNullOrEmpty())
            {
                if (caseDisplayID.IsNullOrEmpty())
                {
                    Case varCase = context.Case.SingleOrDefault(item => item.ID == casemember.CaseID);
                    if (varCase != null)
                    {
                        caseDisplayID = varCase.DisplayID;
                    }
                    if (caseDisplayID.IsNullOrEmpty())
                    {
                        CaseMember originalCaller = GetOriginalCaller(casemember.CaseID);
                        if (originalCaller == null)
                        {
                            originalCaller = context.CaseMember.OrderBy(item => item.ID).FirstOrDefault(item=>item.CaseID==casemember.CaseID);
                            if (originalCaller != null)
                            {
                                originalCaller.IsPrimary = true;
                                context.Entry(originalCaller).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        string regionCode = varCase.Region.Code;
                        if (regionCode.IsNullOrEmpty())
                        {
                            if (varCase.Region.Name.IsNotNullOrEmpty())
                            {
                                regionCode = varCase.Region.Name.Substring(0, 2).ToUpper();
                            }
                        }
                        if (varCase.DisplayID.IsNotNullOrEmpty() && varCase.DisplayID.Contains("NA"))
                        {
                            varCase.DisplayID = MiscUtility.GetCasePersonalizedId(regionCode, casemember.FirstName, casemember.LastName, casemember.CaseID, varCase.DisplayID);
                        }
                        context.Entry(varCase).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        caseDisplayID = varCase.DisplayID;
                    }
                }
            }
            int sequence = 1;

            //Update case display id
            List<CaseMember> allCaseMember = context.CaseMember.Where(item => item.CaseID == casemember.CaseID && item.ID!=casemember.ID).OrderBy(item => item.ID).ToList();
            if (allCaseMember != null)
            {
                foreach (CaseMember existingCaseMember in allCaseMember)
                {
                    casemember.Sequence = sequence;
                    casemember.DisplayID = caseDisplayID + "-" + casemember.Sequence.ToString().PadLeft(3, '0');
                    sequence++;
                }
                Save();
            }
            casemember.Sequence = sequence;
            casemember.DisplayID = caseDisplayID + "-" + casemember.Sequence.ToString().PadLeft(3, '0');
            casemember.LastUpdateDate = DateTime.Now;
            if (casemember.ID == default(int))
            {
                //set the date when this record was created
                casemember.CreateDate = casemember.LastUpdateDate;
                //set the id of the worker who has created this record
                casemember.CreatedByWorkerID = casemember.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseMember.Add(casemember);
                Save();
                CaseWorker primaryWorker = caseworkerRepository.FindPrimary(casemember.CaseID);
                if (primaryWorker != null)
                {
                    CaseWorkerMemberAssignment newCaseWorkerMemberAssignment = new CaseWorkerMemberAssignment()
                    {
                        CaseMemberID = casemember.ID,
                        CaseWorkerID = primaryWorker.ID,
                        CreateDate = DateTime.Now,
                        CreatedByWorkerID = casemember.CreatedByWorkerID,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedByWorkerID = casemember.CreatedByWorkerID
                    };
                    caseworkermemberassignmentRepository.InsertOrUpdate(newCaseWorkerMemberAssignment);
                    caseworkermemberassignmentRepository.Save();
                }
            }
            else
            {
                //update an existing record to database
                context.Entry(casemember).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void UpdateAndSave(CaseMember casemember)
        {
            casemember.LastUpdateDate = DateTime.Now;
            casemember.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            context.Entry(casemember).State = System.Data.Entity.EntityState.Modified;
            Save();
        }

        public override void Delete(int id)
        {
            var entity = Find(id);
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            string sqlQuery = @"delete from [CaseWorkerMemberAssignment] where casememberid=@casememberid;
                                --delete from [CaseAction] where caseprogressnoteid in (select id from [CaseProgressNote] where casememberid=@casememberid);
                                DELETE CA FROM [CaseAction] AS CA INNER JOIN [CaseProgressNote] CPN ON CPN.ID = CA.caseprogressnoteid WHERE CPN.casememberid=@casememberid;
                                delete from [CaseProgressNote] where casememberid=@casememberid;
                                delete from [CaseMemberContact] where casememberid=@casememberid;
                                delete from [casemember] where id=@casememberid;";
            sqlQuery = sqlQuery.Replace("@casememberid", id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery);

        }

        public CaseMember GetOriginalCaller(int caseID)
        {
            return context.CaseMember.FirstOrDefault(item => item.IsPrimary == true && item.CaseID==caseID);
        }
    }

    /// <summary>
    /// interface of CaseMember containing necessary database operations
    /// </summary>
    public interface ICaseMemberRepository : IBaseRepository<CaseMember>
    {
        IQueryable<CaseMember> AllIncluding(int caseId, params Expression<Func<CaseMember, object>>[] includeProperties);
        List<SelectListItem> FindAllByCaseIDForDropDownList(int caseID);
        List<SelectListItem> FindAllByCaseIDAndWorkerIDForDropDownList(int caseID, int workerID);
        List<SelectListItem> FindAllByCaseIDAndWorkerIDForDropDownListForTermination(int caseID, int workerID);
        CaseMember GetOriginalCaller(int caseID);
        void InsertOrUpdate(CaseMember casemember);
        void UpdateAndSave(CaseMember casemember);
    }
}
