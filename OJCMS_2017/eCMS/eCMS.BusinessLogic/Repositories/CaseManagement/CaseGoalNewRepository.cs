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
using System.Linq;
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
    }

    public interface ICaseGoalNewRepository : IBaseRepository<CaseGoalNew>
    {
        void InsertOrUpdate(CaseGoalNew casegoalnew);
    }
}
