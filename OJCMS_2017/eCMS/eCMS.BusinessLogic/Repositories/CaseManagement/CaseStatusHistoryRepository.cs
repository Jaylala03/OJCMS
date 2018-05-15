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
    public class CaseStatusHistoryRepository : BaseRepository<CaseStatusHistory>, ICaseStatusHistoryRepository
    {
        public CaseStatusHistoryRepository(RepositoryContext context)
            : base(context)
        {
        }

        public void InsertOrUpdate(CaseStatusHistory casestatushistory)
        {
            casestatushistory.LastUpdateDate = DateTime.Now;
            if (casestatushistory.ID == default(int))
            {
                //set the date when this record was created
                casestatushistory.CreateDate = casestatushistory.LastUpdateDate;
                //set the id of the worker who has created this record
                casestatushistory.CreatedByWorkerID = casestatushistory.LastUpdatedByWorkerID;
                //add a new record to database
                context.CaseStatusHistory.Add(casestatushistory);
            }
            else
            {
                ////update an existing record to database
                ////set the date when this record was created
                //casestatushistory.CreateDate = casestatushistory.LastUpdateDate;

                ////set the id of the worker who has created this record
                //casestatushistory.CreatedByWorkerID = casestatushistory.LastUpdatedByWorkerID;
                ////CaseWorkerNote.CreatedByWorkerID = CaseWorkerNote.CreatedByWorkerID;
                ////add a new record to database
                //context.Entry(casestatushistory).State = System.Data.Entity.EntityState.Modified;
            }
            Save();
        }

        public string CaseStatusByCaseID(int CaseID)
        {
            string CaseStatus = context.Case.Where(c => c.ID == CaseID).Select(c => c.CaseStatus.Name).FirstOrDefault();

            return CaseStatus;
        }

        public int CaseStatusIDByCaseID(int CaseID)
        {
            int CaseStatusID = context.Case.Where(c => c.ID == CaseID).Select(c => c.CaseStatus.ID).FirstOrDefault();

            return CaseStatusID;
        }

        public List<CaseStatusHistoryVM> AllCaseStatusByCaseID(int CaseID)
        {
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT CSH.ID AS ID, CSH.CaseID AS CaseID, CAST(CSH.CreateDate as date) AS CreatedDate, CS.[Description] AS CaseStatus, ");
            sqlQuery.Append("CSH.ReasonID AS ReasonID, CSH.StatusID AS StatusID, ");
            sqlQuery.Append("RSD.[Description] AS Reason, CSH.Justification AS Justification ");
            sqlQuery.Append("FROM CaseStatusHistory CSH ");
            sqlQuery.Append("INNER JOIN ReasonsForDischarge RSD ON CSH.ReasonID = RSD.ID ");
            sqlQuery.Append("INNER JOIN CaseStatus CS ON CSH.StatusID = CS.ID ");
            sqlQuery.Append("WHERE CSH.CaseID = " + CaseID + " ORDER BY CSH.CreateDate DESC ");
            
            List<CaseStatusHistoryVM> dsResult = context.Database.SqlQuery<CaseStatusHistoryVM>(sqlQuery.ToString()).ToList();
            return dsResult;
        }
    }

    public interface ICaseStatusHistoryRepository : IBaseRepository<CaseStatusHistory>
    {
        void InsertOrUpdate(CaseStatusHistory casestatushistory);

        string CaseStatusByCaseID(int CaseID);

        int CaseStatusIDByCaseID(int CaseID);

        List<CaseStatusHistoryVM> AllCaseStatusByCaseID(int CaseID);
    }
}
