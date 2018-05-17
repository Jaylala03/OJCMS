//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.DataLogic.ViewModels;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class CaseGoalNewRepository : BaseRepository<CaseGoalNew>, ICaseGoalNewRepository
    {
        public CaseGoalNewRepository(RepositoryContext context)
            : base(context)
        {
        }

        public void InsertOrUpdate(CaseGoalNew casegoalnew)
        {
            casegoalnew.LastUpdateDate = DateTime.Now;
            if (casegoalnew.ID == default(int))
            {
                //set the date when this record was created
                casegoalnew.CreateDate = casegoalnew.LastUpdateDate;
                //set the id of the worker who has created this record
                casegoalnew.CreatedByWorkerID = casegoalnew.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseGoalNew.Add(casegoalnew);
            }
            else
            {
                //update an existing record to database
                //var NewCaseworker = context.CaseWorker.Where(a => a.ID == caseworker.ID).FirstOrDefault();

                //NewCaseworker.CaseID = caseworker.CaseID;
                //NewCaseworker.WorkerID = caseworker.WorkerID;
                //NewCaseworker.IsActive = caseworker.IsActive;
                //NewCaseworker.AllowNotification = caseworker.AllowNotification;
                //NewCaseworker.IsPrimary = caseworker.IsPrimary;
                //NewCaseworker.IsArchived = caseworker.IsArchived;
                //NewCaseworker.LastUpdateDate = DateTime.Now;
                //context.Entry(caseworker).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
        }

        public IQueryable<CaseGoalNew> FindAllByCaseID(int caseID)
        {
            return context.CaseGoalNew.Where(item => item.CaseID == caseID);
        }

        public IQueryable<CaseGoalNew> AllIncluding(int caseId, params Expression<Func<CaseGoalNew, object>>[] includeProperties)
        {
            IQueryable<CaseGoalNew> query = context.CaseGoalNew;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.Where(item => item.CaseID == caseId);
        }

        public List<CaseGoalGridVM> CaseGoalNewByCaseID(int CaseID)
        {
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT CG.CaseID,CG.ID AS CaseGoalID,(CM.FirstName+' '+CM.LastName) AS FamilyMember,");
            sqlQuery.Append("GS.Name AS GoalStatus,CG.GoalDetail ,RT.Name AS Priority,CG.CreateDate,CG.LastUpdateDate,");
            sqlQuery.Append("((CASE WHEN Education = 1 THEN 'Education' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN IncomeLivelihood = 1 THEN ',IncomeLivelihood' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN Assets = 1 THEN ',Assets' ELSE '' END) +  ");
            sqlQuery.Append("(CASE WHEN Housing = 1 THEN ',Housing' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN SocialSupport = 1 THEN ',SocialSupport' ELSE '' END) +  ");
            sqlQuery.Append("(CASE WHEN Dignity = 1 THEN ',Dignity' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN Health = 1 THEN ',Health' ELSE '' END)) AS Indicators,");
            sqlQuery.Append("('Total:' + (SELECT CAST(ISNULL(COUNT(*),0) AS varchar) FROM CaseActionNew WHERE CaseGoalID = 1)) + ");
            sqlQuery.Append("ISNULL(((SELECT ', ' + (GS.Name + ':' + CAST(COUNT(*) AS VARCHAR)) ");
            sqlQuery.Append("FROM  CaseActionNew AS CAN ");
            sqlQuery.Append("INNER JOIN CaseGoalNew AS CGN ON CAN.CaseGoalID = CGN.ID ");
            sqlQuery.Append("INNER JOIN GoalStatus AS GS ON CAN.ActionStatusID = GS.ID ");
            sqlQuery.Append("WHERE CGN.CaseID = " + CaseID + " GROUP BY GS.Name FOR XML PATH(''))),'') AS ActionsSummary ");
            sqlQuery.Append("FROM CaseGoalNew AS CG ");
            sqlQuery.Append("INNER JOIN CaseMember AS CM ON CG.CaseMemberID = CM.ID ");
            sqlQuery.Append("INNER JOIN GoalStatus AS GS ON CG.GoalStatusID = GS.ID ");
            sqlQuery.Append("INNER JOIN RiskType AS RT ON CG.ID = RT.ID ");
            sqlQuery.Append("WHERE CG.CaseID = " + CaseID + " ; ");

            List<CaseGoalGridVM> dsResult = context.Database.SqlQuery<CaseGoalGridVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }

        public List<CaseGoalServiceGridVM> CaseGoalHistory(int CaseID)
        {
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT CG.CaseID,CG.ID AS CaseGoalID,(CM.FirstName+' '+CM.LastName) AS AssignedTo,'Family Member' AS AsigneeRole,");
            sqlQuery.Append("GS.Name AS GoalStatus,CG.GoalDetail ,RT.Name AS Priority,CG.CreateDate,CG.LastUpdateDate,");
            sqlQuery.Append("((CASE WHEN Education = 1 THEN 'Education' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN IncomeLivelihood = 1 THEN ',IncomeLivelihood' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN Assets = 1 THEN ',Assets' ELSE '' END) +  ");
            sqlQuery.Append("(CASE WHEN Housing = 1 THEN ',Housing' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN SocialSupport = 1 THEN ',SocialSupport' ELSE '' END) +  ");
            sqlQuery.Append("(CASE WHEN Dignity = 1 THEN ',Dignity' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN Health = 1 THEN ',Health' ELSE '' END)) AS Indicators ");
            sqlQuery.Append("FROM CaseGoalNew AS CG ");
            sqlQuery.Append("INNER JOIN CaseMember AS CM ON CG.CaseMemberID = CM.ID ");
            sqlQuery.Append("INNER JOIN GoalStatus AS GS ON CG.GoalStatusID = GS.ID ");
            sqlQuery.Append("INNER JOIN RiskType AS RT ON CG.ID = RT.ID ");
            sqlQuery.Append("WHERE CG.CaseID = " + CaseID + " ; ");

            List<CaseGoalServiceGridVM> dsResult = context.Database.SqlQuery<CaseGoalServiceGridVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }


        public CaseGoalEditVM GetCaseGoal(int CaseGoalID)
        {
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT CG.ID,CG.GoalStatusID,CG.PriorityTypeID,CG.GoalDetail,(CM.FirstName+' '+CM.LastName) AS CaseMemberName,");
            sqlQuery.Append("((CASE WHEN Education = 1 THEN 'Education' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN IncomeLivelihood = 1 THEN ',IncomeLivelihood' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN Assets = 1 THEN ',Assets' ELSE '' END) +  ");
            sqlQuery.Append("(CASE WHEN Housing = 1 THEN ',Housing' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN SocialSupport = 1 THEN ',SocialSupport' ELSE '' END) +  ");
            sqlQuery.Append("(CASE WHEN Dignity = 1 THEN ',Dignity' ELSE '' END) + ");
            sqlQuery.Append("(CASE WHEN Health = 1 THEN ',Health' ELSE '' END)) AS Indicators ");
            sqlQuery.Append("FROM CaseGoalNew AS CG ");
            //sqlQuery.Append("INNER JOIN CaseMember AS CM ON CG.CaseMemberID = CM.ID ");
            //sqlQuery.Append("WHERE CG.ID = " + CaseGoalID + " ; ");
            sqlQuery.Append("INNER JOIN CaseMember AS CM ON CG.CaseID = CM.CaseID ");
            sqlQuery.Append("WHERE CG.ID = " + CaseGoalID + " ; ");

            List<CaseGoalEditVM> dsResult = context.Database.SqlQuery<CaseGoalEditVM>(sqlQuery.ToString()).ToList();
            return dsResult.FirstOrDefault() ;
        }
        public bool UpdateDetails(CaseGoalEditVM casegoal)
        {
            var goal = context.CaseGoalNew.Find(casegoal.ID);
            if (goal != null)
            {
                goal.PriorityTypeID = casegoal.PriorityTypeID;
                goal.GoalStatusID = casegoal.GoalStatusID;
                goal.LastUpdateDate = DateTime.Now;
                context.Entry(goal).State = System.Data.Entity.EntityState.Modified;
                Save();

                return true;
            }
            return false;
        }
    }

    public interface ICaseGoalNewRepository : IBaseRepository<CaseGoalNew>
    {
        void InsertOrUpdate(CaseGoalNew casegoalnew);
        IQueryable<CaseGoalNew> FindAllByCaseID(int caseID);
        IQueryable<CaseGoalNew> AllIncluding(int caseId, params Expression<Func<CaseGoalNew, object>>[] includeProperties);
        List<CaseGoalGridVM> CaseGoalNewByCaseID(int CaseID);
        List<CaseGoalServiceGridVM> CaseGoalHistory(int CaseID);

        CaseGoalEditVM GetCaseGoal(int CaseGoalID);
        bool UpdateDetails(CaseGoalEditVM casegoal);
    }
}
