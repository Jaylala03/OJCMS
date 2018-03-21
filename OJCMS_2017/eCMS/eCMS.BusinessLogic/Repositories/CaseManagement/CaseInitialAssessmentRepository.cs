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
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EasySoft.Helper;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using Kendo.Mvc;
using eCMS.Shared;
using Kendo.Mvc.Extensions;
using System.Text;
using eCMS.DataLogic.ViewModels;
using eCMS.DataLogic.Models.Lookup;

namespace eCMS.BusinessLogic.Repositories
{
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseInitialAssessmentRepository : BaseRepository<CaseInitialAssessment>, ICaseInitialAssessmentRepository
    {

        public CaseInitialAssessmentRepository(RepositoryContext context)
            : base(context)
        {
        }

        public List<InitialAssessmentIndicatorsVM> GetAllIndicators()
        {
            string sqlQuery = "";

            sqlQuery = "SELECT IT.ID AS IndicatorTypeID, IT.Name AS IndicatorName," +
                        "ISNULL(AI.Description1,'') AS AssesmentIndicatorDescription1, ISNULL(AI.Description2,'') AS AssesmentIndicatorDescription2," +
                        "ISNULL(AI.Description3,'') AS AssesmentIndicatorDescription3, ISNULL(AI.Description4,'') AS AssesmentIndicatorDescription4," +
                        "ISNULL(AI.Description5,'') AS AssesmentIndicatorDescription5, ISNULL(AI.Description6,'') AS AssesmentIndicatorDescription6 FROM IndicatorType IT " +
                        "LEFT JOIN AssesmentIndicators AI ON IT.ID = AI.IndicatorTYpeID ORDER BY IT.ID";

            List<InitialAssessmentIndicatorsVM> dsResult = context.Database.SqlQuery<InitialAssessmentIndicatorsVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }

        public List<CaseInitialAssessmentVM> GetCaseAssessment(int CaseID)
        {
            string sqlQuery = "";

            sqlQuery = "SELECT CM.ID AS CaseMemberID, (CM.FirstName + ' ' + CM.LastName) AS CaseMemberName," +
            "IT.ID AS IndicatorTypeID,ISNULL(CIA.AssessmentValue,0) AS AssessmentValue " +
            "FROM CaseMember AS CM " +
            "CROSS JOIN IndicatorType AS IT " +
            "LEFT JOIN CaseInitialAssessment AS CIA ON CM.ID = CIA.CaseMemberID AND IT.ID = CIA.IndicatorTypeID " +
            "WHERE CM.CaseID = " + CaseID.ToString();

            List<CaseInitialAssessmentVM> dsResult = context.Database.SqlQuery<CaseInitialAssessmentVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }

        public void AddAssessment(int CaseID,List<CaseInitialAssessmentVM> asslist)
        {
            if (asslist.Count > 0)
            {
                int casememberid = asslist[0].CaseMemberID;
                context.CaseInitialAssessment.RemoveRange(context.CaseInitialAssessment.Where(c => c.CaseMemberID == casememberid));

                foreach (CaseInitialAssessmentVM assobj in asslist)
                {
                    CaseInitialAssessment coninfo = new CaseInitialAssessment()
                    {
                        CaseID = CaseID,
                        CaseMemberID = assobj.CaseMemberID,
                        IndicatorTypeID = assobj.IndicatorTypeID,
                        AssessmentValue = assobj.AssessmentValue,
                        LastUpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        LastUpdatedByWorkerID = CurrentLoggedInWorker.ID,
                        CreatedByWorkerID = CurrentLoggedInWorker.ID,
                        IsArchived = false
                    };
                    InsertOrUpdate(coninfo);
                    Save();
                }

                var varcase = context.Case.Where(c => c.ID == CaseID).SingleOrDefault();
                varcase.CaseAssessmentReviewed = true;
                Save();
            }

        }

        public bool CaseAssessmentReviewed(int CaseID)
        {
            return context.Case.Where(c => c.ID == CaseID).Select(c => c.CaseAssessmentReviewed).SingleOrDefault();
        }
    }

    public interface ICaseInitialAssessmentRepository : IBaseRepository<CaseInitialAssessment>
    {
        List<InitialAssessmentIndicatorsVM> GetAllIndicators();
        List<CaseInitialAssessmentVM> GetCaseAssessment(int CaseID);
        void AddAssessment(int CaseID, List<CaseInitialAssessmentVM> asslist);
        bool CaseAssessmentReviewed(int CaseID);
    }

}
