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

        public List<InitialAssessmentVM> GetAllIndicators()
        {
            string sqlQuery = "";

            sqlQuery = "SELECT IT.ID AS IndicatorTypeID, IT.Name AS IndicatorName," +
                        "AI.Description1 AS AssesmentIndicatorDescription1, AI.Description2 AS AssesmentIndicatorDescription2," +
                        "AI.Description3 AS AssesmentIndicatorDescription3, AI.Description4 AS AssesmentIndicatorDescription4," +
                        "AI.Description5 AS AssesmentIndicatorDescription6, AI.Description6 AS AssesmentIndicatorDescription6 FROM IndicatorType IT " +
                        "LEFT JOIN AssesmentIndicators AI ON IT.ID = AI.IndicatorTYpeID";

            List<InitialAssessmentVM> dsResult = context.Database.SqlQuery<InitialAssessmentVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }
    }

    public interface ICaseInitialAssessmentRepository : IBaseRepository<CaseInitialAssessment>
    {
        List<InitialAssessmentVM> GetAllIndicators();        
    }

}
