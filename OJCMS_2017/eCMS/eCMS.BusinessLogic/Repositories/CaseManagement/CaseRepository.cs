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
    /// <summary>
    /// this class implements the methods to do necessary database operations
    /// </summary>
    public class CaseRepository : BaseRepository<Case>, ICaseRepository
    {
        private readonly IRegionRepository regionRepository;
        private readonly ICaseMemberRepository casememberRepository;
        private readonly ICaseMemberContactRepository casemembercontactRepository;
        private readonly IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository;
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        private readonly ICaseProgressNoteRepository caseprogressnoteRepository;
        private readonly IWorkerRoleRepository workerRoleRepository;
        /// <summary>
        /// Initialize repository context
        /// </summary>
        /// <param name="context">database connection</param>
        public CaseRepository(RepositoryContext context,
            IRegionRepository regionRepository,
            ICaseMemberRepository casememberRepository,
            ICaseMemberContactRepository casemembercontactRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository,
            ICaseProgressNoteRepository caseprogressnoteRepository,IWorkerRoleRepository workerRoleRepository)
            : base(context)
        {
            this.regionRepository = regionRepository;
            this.casememberRepository = casememberRepository;
            this.casemembercontactRepository = casemembercontactRepository;
            this.workerroleactionpermissionRepository = workerroleactionpermissionRepository;
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
            this.caseprogressnoteRepository = caseprogressnoteRepository;
            this.workerRoleRepository = workerRoleRepository;
        }

        public IQueryable<Case> FindAllByProgramID(int programID)
        {
            return context.Case.Where(item => item.ProgramID == programID);
        }

        public IQueryable<Case> FindAllByRegionID(int regionID)
        {
            return context.Case.Where(item => item.RegionID == regionID);
        }

        /// <summary>
        /// Add or Update case to database
        /// </summary>
        /// <param name="case">data to save</param>
        public void InsertOrUpdate(Case varCase)
        {
            if (varCase.DisplayID.IsNullOrEmpty() || (varCase.DisplayID.IsNotNullOrEmpty() && varCase.DisplayID.Contains("NA")))
            {
                string regionCode = string.Empty;
                Region region = regionRepository.Find(varCase.RegionID);
                if (region != null)
                {
                    regionCode = region.Code;
                    if (regionCode.IsNullOrEmpty())
                    {
                        if (region.Name.IsNotNullOrEmpty())
                        {
                            regionCode = region.Name.Substring(0, 2).ToUpper();
                        }
                    }
                }
                string primaryCaseMemberFirstName = string.Empty;
                string primaryCaseMemberLastName = string.Empty;
                if (varCase.ID > 0)
                {
                    CaseMember primaryCaseMember = context.CaseMember.FirstOrDefault(item => item.CaseID == varCase.ID && item.IsPrimary == true);
                    if (primaryCaseMember != null)
                    {
                        primaryCaseMemberFirstName = primaryCaseMember.FirstName;
                        primaryCaseMemberLastName = primaryCaseMember.LastName;
                    }
                }
                varCase.DisplayID = MiscUtility.GetCasePersonalizedId(regionCode, primaryCaseMemberFirstName, primaryCaseMemberLastName, varCase.ID, varCase.DisplayID);
            }
            varCase.LastUpdateDate = DateTime.Now;
            if (varCase.ID == default(int))
            {
                //set the date when this record was created
                varCase.CreateDate = varCase.LastUpdateDate;
                //set the id of the worker who has created this record
                varCase.CreatedByWorkerID = varCase.LastUpdatedByWorkerID;
                //add a new record to database
                context.Case.Add(varCase);
            }
            else
            {
                //update an existing record to database
                context.Entry(varCase).State = System.Data.Entity.EntityState.Modified;
            }
        }

        //        public DataSourceResult Search(Case searchParameters, DataSourceRequest paramDSRequest)
        //        {

        //            DataSourceRequest dsRequest = paramDSRequest;
        //            if (dsRequest == null)
        //            {
        //                dsRequest = new DataSourceRequest();
        //            }
        //            if (dsRequest.Filters == null || (dsRequest.Filters != null && dsRequest.Filters.Count == 0))
        //            {
        //                if (dsRequest.Filters == null)
        //                {
        //                    dsRequest.Filters = new List<IFilterDescriptor>();
        //                }
        //            }
        //            if (dsRequest.Sorts == null || (dsRequest.Sorts != null && dsRequest.Sorts.Count == 0))
        //            {
        //                if (dsRequest.Sorts == null)
        //                {
        //                    dsRequest.Sorts = new List<SortDescriptor>();
        //                }
        //                SortDescriptor defaultSortExpression = new SortDescriptor("EnrollDate", System.ComponentModel.ListSortDirection.Descending);
        //                dsRequest.Sorts.Add(defaultSortExpression);
        //            }
        //            if (dsRequest.PageSize == 0)
        //            {
        //                dsRequest.PageSize = Constants.CommonConstants.DefaultPageSize;
        //            }

        //            string strColumns = @"SELECT 
        //                                [C].[ID],
        //                                [C].[DisplayID],
        //                                [C].[ProgramID],
        //                                [C].[RegionID],
        //                                [C].[SubProgramID],
        //                                [C].[JamatkhanaID],
        //                                [C].[CaseStatusID],
        //                                ISNULL(R.Name,'') as RegionName,
        //                                ISNULL(P.Name,'') as [SubProgramName],
        //                                EnrollDate,
        //                                ISNULL(CS.Name,'') as StatusName,
        //                                ISNULL([JK].[Name],'N/A') as JamatKhanaName,
        //                                --dbo.GetLatNotesByCaseID([C].[ID]) CaseNote,
        //                                (SELECT TOP 1 W.FirstName+' '+W.LastName  FROM CaseWorker CW LEFT JOIN Worker [W] ON CW.WorkerID=W.ID WHERE CW.IsPrimary=1 AND CW.CaseID=[C].[ID]) [CaseWorker],
        //                                (CASE WHEN EXISTS(SELECT TOP 1 CM.FirstName+' '+CM.LastName  FROM CaseMember CM WHERE  CM.CaseID=[C].[ID] AND CM.RelationshipStatusID=4)  
        //                                THEN (SELECT TOP 1 CM.FirstName+' '+CM.LastName  FROM CaseMember CM WHERE  CM.CaseID=[C].[ID] AND CM.RelationshipStatusID=4) ELSE (SELECT TOP 1 CM.FirstName+' '+CM.LastName  FROM CaseMember CM WHERE  CM.CaseID=[C].[ID])  END) [CaseMember],
        //                                '" + searchParameters.HasPermissionToCreate.ToDisplayStyle() + @"' [HasPermissionToCreate],
        //                                '" + searchParameters.HasPermissionToDelete.ToDisplayStyle() + @"' [HasPermissionToDelete],
        //                                '" + searchParameters.HasPermissionToEdit.ToDisplayStyle() + @"' [HasPermissionToEdit],
        //                                CASE WHEN [C].[CaseStatusID] IN (7,8) THEN '" + searchParameters.HasPermissionToReadmit.ToDisplayStyle() + @"' ELSE '' END [HasPermissionToReadmit]";

        //            string strCount = @"SELECT COUNT([C].[ID]) ";

        //            StringBuilder sqlQuery = new StringBuilder(@"FROM [dbo].[Case] [C] 
        //                                                        LEFT JOIN [dbo].[Region] [R] ON [R].[ID]=[C].[RegionID]
        //                                                        LEFT JOIN [dbo].[SubProgram] [P] ON [P].[ID]=[C].[SubProgramID]
        //                                                        LEFT JOIN [dbo].[CaseStatus] [CS] ON [CS].[ID]=[C].[CaseStatusID]
        //                                                        LEFT JOIN [dbo].[Jamatkhana] [JK] ON [JK].[ID]=[C].[JamatkhanaID]
        //                                                        WHERE [C].[IsArchived]=0");
        //            if (searchParameters.JamatkhanaID > 0)
        //            {
        //                sqlQuery.Append(" AND [C].[JamatkhanaID]=" + searchParameters.JamatkhanaID);
        //            }
        //            else if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
        //            {
        //                StringBuilder sqljk = new StringBuilder();
        //                sqljk.Append(" SELECT PJK.JamatKhanaID ");
        //                sqljk.Append(" FROM WorkerInRoleNew AS WIR ");
        //                sqljk.Append(" INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
        //                sqljk.Append(" INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
        //                sqljk.Append(" INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
        //                sqljk.Append(" INNER JOIN PermissionJamatkhana PJK ON PR.ID = PJK.PermissionRegionID ");
        //                sqljk.Append(" WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID);
        //                sqljk.Append(" GROUP BY PJK.JamatKhanaID ");

        //                sqlQuery = sqlQuery.Append(" AND [C].[JamatkhanaID] IN (" + sqljk.ToString() + ")");
        //            }
        //            if (searchParameters.CaseNumber.IsNotNullOrEmpty())
        //            {
        //                searchParameters.CaseNumber = searchParameters.CaseNumber.Trim();
        //                sqlQuery.Append(" AND ([C].[CaseNumber]  LIKE '%" + searchParameters.CaseNumber + "%' OR [C].[DisplayID] LIKE '%" + searchParameters.CaseNumber + "%')");
        //            }
        //            if (searchParameters.FirstName.IsNotNullOrEmpty())
        //            {
        //                searchParameters.FirstName = searchParameters.FirstName.Trim();
        //                sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseMember [CM] WHERE [CM].[FirstName] LIKE '%" + searchParameters.FirstName + "%')");
        //            }
        //            if (searchParameters.LastName.IsNotNullOrEmpty())
        //            {
        //                searchParameters.LastName = searchParameters.LastName.Trim();
        //                sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseMember [CM] WHERE [CM].[LastName] LIKE '%" + searchParameters.LastName + "%')");
        //            }
        //            if (searchParameters.DisplayID.IsNotNullOrEmpty())
        //            {
        //                searchParameters.DisplayID = searchParameters.DisplayID.Trim();
        //                sqlQuery.Append(" AND ( [C].[DisplayID] LIKE '%" + searchParameters.DisplayID + "%')");
        //            }
        //            if (searchParameters.ProgramID != 0)
        //            {
        //                sqlQuery = sqlQuery.Append(" AND [C].[ProgramID] IN (" + searchParameters.ProgramID + ")");
        //            }
        //            else if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
        //            {
        //                StringBuilder sqlprogram = new StringBuilder();
        //                sqlprogram.Append(" SELECT PR.ProgramID ");
        //                sqlprogram.Append(" FROM WorkerInRoleNew AS WIR ");
        //                sqlprogram.Append(" INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
        //                sqlprogram.Append(" INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
        //                sqlprogram.Append(" INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
        //                sqlprogram.Append(" WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID);
        //                sqlprogram.Append(" GROUP BY PR.ProgramID ");

        //                //sqlQuery = sqlQuery.Append(" AND [C].[ProgramID] IN ( SELECT ProgramID FROM WorkerInRole WHERE WorkerID = " + CurrentLoggedInWorker.ID + ")");
        //                sqlQuery = sqlQuery.Append(" AND [C].[ProgramID] IN ( " + sqlprogram.ToString() + ")");
        //            }
        //            if (searchParameters.SubProgramID != 0)
        //            {
        //                sqlQuery = sqlQuery.Append(" AND [C].[SubProgramID] IN (" + searchParameters.SubProgramID + ")");
        //            }
        //            else if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
        //            {
        //                StringBuilder sqlsubprogram = new StringBuilder();
        //                sqlsubprogram.Append(" SELECT PSP.SubProgramID ");
        //                sqlsubprogram.Append(" FROM WorkerInRoleNew AS WIR ");
        //                sqlsubprogram.Append(" INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
        //                sqlsubprogram.Append(" INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
        //                sqlsubprogram.Append(" INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
        //                sqlsubprogram.Append(" INNER JOIN PermissionSubProgram AS PSP ON PR.ID = PSP.PermissionRegionID ");
        //                sqlsubprogram.Append(" WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID);
        //                sqlsubprogram.Append(" GROUP BY PSP.SubProgramID ");

        //                //sqlQuery = sqlQuery.Append(" AND [C].[SubProgramID] IN ( SELECT SubProgramID FROM WorkerSubProgram WHERE WorkerInRoleID IN (SELECT ID FROM WorkerInRole WHERE WorkerID=" + CurrentLoggedInWorker.ID + "))");
        //                sqlQuery = sqlQuery.Append(" AND [C].[SubProgramID] IN (" + sqlsubprogram.ToString() + ")");
        //            }
        //            if (searchParameters.RegionID != 0)
        //            {
        //                sqlQuery = sqlQuery.Append("AND [C].[RegionID] IN (" + searchParameters.RegionID + ")");
        //            }
        //            else if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
        //            {
        //                StringBuilder sqlregion = new StringBuilder();
        //                sqlregion.Append(" SELECT PR.RegionID ");
        //                sqlregion.Append(" FROM WorkerInRoleNew AS WIR ");
        //                sqlregion.Append(" INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
        //                sqlregion.Append(" INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
        //                sqlregion.Append(" INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
        //                sqlregion.Append(" WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID);
        //                sqlregion.Append(" GROUP BY PR.RegionID ");

        //                //sqlQuery = sqlQuery.Append(" AND [C].[RegionID] IN ( SELECT RegionID FROM WorkerInRole WHERE WorkerID = " + CurrentLoggedInWorker.ID + ")");
        //                sqlQuery = sqlQuery.Append(" AND [C].[RegionID] IN (" + sqlregion.ToString() + ")");
        //            }
        //            if (searchParameters.CaseStatusID != 0)
        //            {
        //                sqlQuery = sqlQuery.Append("AND [C].[CaseStatusID] IN (" + searchParameters.CaseStatusID + ")");
        //            }
        //            if (!string.IsNullOrEmpty(searchParameters.PhoneNumber))
        //            {
        //                searchParameters.PhoneNumber = searchParameters.PhoneNumber.Trim();
        //                sqlQuery.Append(" AND [C].[ID] IN ( SELECT CaseID FROM CaseMember [CM] LEFT JOIN [CaseMemberContact] [CMC] ON [CM].ID=CMC.CaseMemberID WHERE [CMC].Contact LIKE '%" + searchParameters.PhoneNumber + "%')");
        //            }
        //            if (searchParameters.CreatedByWorkerName.IsNotNullOrEmpty())
        //            {
        //                searchParameters.CreatedByWorkerName = searchParameters.CreatedByWorkerName.Trim();
        //                string[] names = searchParameters.CreatedByWorkerName.ToStringArray(' ', true);
        //                StringBuilder nameWhereClause = new StringBuilder();
        //                if (names != null)
        //                {
        //                    foreach (string name in names)
        //                    {
        //                        if (nameWhereClause.ToString().IsNotNullOrEmpty())
        //                        {
        //                            nameWhereClause.Append(" OR ");
        //                        }
        //                        nameWhereClause.Append(" [W].[FirstName] LIKE '%" + name + "%' OR  [W].[LastName] LIKE '%" + name + "%' ");
        //                    }
        //                }
        //                sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseWorker [CW] LEFT JOIN Worker [W] ON [CW].[WorkerID]=[W].[ID] WHERE " + nameWhereClause.ToString() + ")");
        //            }

        //            if (searchParameters.AssignedToWorkerID > 0 && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
        //            {
        //                //IF worker has all cases view permission then do not filter by assigned worker id.
        //                if (workerroleactionpermissionnewRepository.HasAllCasesPermission(CurrentLoggedInWorkerRoleIDs, searchParameters.AssignedToWorkerID) == 0)
        //                {
        //                    sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseWorker [W] WHERE [W].[WorkerID]=" + searchParameters.AssignedToWorkerID + " AND [W].[IsActive]=1)");
        //                }
        //            }

        //            DataSourceResult dataSourceResult = context.Database.SqlQuery<CaseListViewModel>(strColumns + sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
        //            foreach (CaseListViewModel caseItem in dataSourceResult.Data)
        //            {
        //                caseItem.CaseNote = caseprogressnoteRepository.GetprogressNoteByCaseID(caseItem.ID);
        //                caseItem.HasPermissionToReadmit = "display:none;";
        //                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
        //                {
        //                    //<JL:comment:06/05/2017>
        //                    //caseItem.HasPermissionToEdit = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Edit, true).ToDisplayStyle();
        //                    //caseItem.HasPermissionToDelete = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true).ToDisplayStyle();
        //                    //caseItem.HasPermissionToCaseProgressNoteCreate = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Create, true).ToDisplayStyle();
        //                    //caseItem.HasPermissionToCaseProgressNoteIndex = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Index, true).ToDisplayStyle();
        //                    //</JL:comment:06/05/2017>
        //                    //<JL:add:06/05/2017>
        //                    caseItem.HasPermissionToEdit = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Edit, true).ToDisplayStyle();
        //                    caseItem.HasPermissionToDelete = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true).ToDisplayStyle();
        //                    caseItem.HasPermissionToCaseProgressNoteCreate = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Create, true).ToDisplayStyle();
        //                    caseItem.HasPermissionToCaseProgressNoteIndex = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Index, true).ToDisplayStyle();
        //                    //<JL:add:06/05/2017>

        //                    if (caseItem.CaseStatusID == 7 || caseItem.CaseStatusID == 8)
        //                    {
        //                        caseItem.HasPermissionToReadmit = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Readmit, true).ToDisplayStyle();
        //                    }
        //                }
        //                else
        //                {
        //                    caseItem.HasPermissionToEdit = String.Empty;
        //                    caseItem.HasPermissionToDelete = String.Empty;
        //                    caseItem.HasPermissionToCaseProgressNoteCreate = String.Empty;
        //                    caseItem.HasPermissionToCaseProgressNoteIndex = String.Empty;
        //                    if (caseItem.CaseStatusID == 7 || caseItem.CaseStatusID == 8)
        //                    {
        //                        caseItem.HasPermissionToReadmit = string.Empty;
        //                    }
        //                }
        //                List<SelectListItem> caseMemberList = casememberRepository.FindAllByCaseIDForDropDownList(caseItem.ID);
        //                if (caseMemberList != null && caseMemberList.Count > 0)
        //                {
        //                    foreach (SelectListItem caseMember in caseMemberList)
        //                    {
        //                        caseItem.AllCaseMember = caseItem.AllCaseMember.Concate(Environment.NewLine, caseMember.Text);
        //                        caseItem.AllCaseMemberWithContact = caseItem.AllCaseMemberWithContact.Concate(Environment.NewLine, caseMember.Text);
        //                        int caseMemberID = caseMember.Value.ToInteger(true);
        //                        string contactNumnbers = string.Empty;
        //                        List<CaseMemberContact> caseMemberContacts = context.CaseMemberContact.Where(item => item.CaseMemberID == caseMemberID && item.ContactMediaID != 1 && item.Contact != null && item.Contact != "").ToList();
        //                        if (caseMemberContacts != null && caseMemberContacts.Count > 0)
        //                        {
        //                            foreach (CaseMemberContact caseMemberContact in caseMemberContacts)
        //                            {
        //                                contactNumnbers = contactNumnbers.Concate(',', caseMemberContact.Contact);
        //                            }
        //                        }
        //                        if (contactNumnbers.IsNotNullOrEmpty())
        //                        {
        //                            caseItem.AllCaseMemberWithContact = caseItem.AllCaseMemberWithContact + "(" + contactNumnbers + ")";
        //                        }
        //                    }
        //                }
        //            }
        //            DataSourceRequest dsRequestTotalCountQuery = new DataSourceRequest();
        //            dsRequestTotalCountQuery.Filters = dsRequest.Filters;

        //            dataSourceResult.Total = context.Database.SqlQuery<Int32>(strCount + sqlQuery.ToString()).FirstOrDefault();
        //            return dataSourceResult;
        //        }

        public DataSourceResult Search(Case searchParameters, DataSourceRequest paramDSRequest)
        {
            if (workerRoleRepository.IsWorkerRegionalAdmin() > 0)
            {
                return new DataSourceResult();
            }
            
            DataSourceRequest dsRequest = paramDSRequest;
            if (dsRequest == null)
            {
                dsRequest = new DataSourceRequest();
            }
            if (dsRequest.Filters == null || (dsRequest.Filters != null && dsRequest.Filters.Count == 0))
            {
                if (dsRequest.Filters == null)
                {
                    dsRequest.Filters = new List<IFilterDescriptor>();
                }
            }
            if (dsRequest.Sorts == null || (dsRequest.Sorts != null && dsRequest.Sorts.Count == 0))
            {
                if (dsRequest.Sorts == null)
                {
                    dsRequest.Sorts = new List<SortDescriptor>();
                }
                SortDescriptor defaultSortExpression = new SortDescriptor("EnrollDate", System.ComponentModel.ListSortDirection.Descending);
                dsRequest.Sorts.Add(defaultSortExpression);
            }
            if (dsRequest.PageSize == 0)
            {
                dsRequest.PageSize = Constants.CommonConstants.DefaultPageSize;
            }

            string strColumns = @"SELECT ";
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
            {
                strColumns = strColumns + " DISTINCT ";
            }
            strColumns = strColumns + @" [C].[ID],
                                [C].[DisplayID],
                                [C].[ProgramID],
                                [C].[RegionID],
                                [C].[SubProgramID],
                                [C].[JamatkhanaID],
                                [C].[CaseStatusID],
                                ISNULL(R.Name,'') as RegionName,
                                ISNULL(P.Name,'') as [SubProgramName],
                                EnrollDate,
                                ISNULL(CS.Name,'') as StatusName,
                                ISNULL([JK].[Name],'N/A') as JamatKhanaName,
                                (SELECT TOP 1 W.FirstName+' '+W.LastName  FROM CaseWorker CW LEFT JOIN Worker [W] ON CW.WorkerID=W.ID WHERE CW.IsPrimary=1 AND CW.CaseID=[C].[ID]) [CaseWorker],
                                (CASE WHEN EXISTS(SELECT TOP 1 CM.FirstName+' '+CM.LastName  FROM CaseMember CM WHERE  CM.CaseID=[C].[ID] AND CM.RelationshipStatusID=4)  
                                THEN (SELECT TOP 1 CM.FirstName+' '+CM.LastName  FROM CaseMember CM WHERE  CM.CaseID=[C].[ID] AND CM.RelationshipStatusID=4) ELSE (SELECT TOP 1 CM.FirstName+' '+CM.LastName  FROM CaseMember CM WHERE  CM.CaseID=[C].[ID])  END) [CaseMember],
                                '" + searchParameters.HasPermissionToRead.ToDisplayStyle() + @"' [HasPermissionToRead],                                
                                '" + searchParameters.HasPermissionToCreate.ToDisplayStyle() + @"' [HasPermissionToCreate],
                                '" + searchParameters.HasPermissionToDelete.ToDisplayStyle() + @"' [HasPermissionToDelete],
                                '" + searchParameters.HasPermissionToEdit.ToDisplayStyle() + @"' [HasPermissionToEdit],
                                CASE WHEN [C].[CaseStatusID] IN (7,8) THEN '" + searchParameters.HasPermissionToReadmit.ToDisplayStyle() + @"' ELSE '' END [HasPermissionToReadmit]";

            string strCount = @"SELECT COUNT(DISTINCT [C].[ID]) ";

            StringBuilder sqlQuery = new StringBuilder(@"FROM [dbo].[Case] [C] ");

            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
            {
                sqlQuery.Append(" INNER JOIN ( SELECT PR.RegionId,PR.ProgramID, PSP.SubProgramID,PJK.JamatkhanaID,P.IsAllCases");
                sqlQuery.Append(" FROM WorkerInRoleNew AS WIR");
                sqlQuery.Append(" INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID");
                sqlQuery.Append(" INNER JOIN Permission AS P ON WRP.PermissionID = P.ID");
                sqlQuery.Append(" INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID");
                sqlQuery.Append(" INNER JOIN PermissionSubProgram AS PSP ON PR.ID = PSP.PermissionRegionID");
                sqlQuery.Append(" INNER JOIN PermissionJamatkhana PJK ON PR.ID = PJK.PermissionRegionID");
                sqlQuery.Append(" WHERE WIR.WorkerID = " + CurrentLoggedInWorker.ID + " ");
                sqlQuery.Append(" GROUP BY PR.RegionId,PR.ProgramID, PSP.SubProgramID,PJK.JamatkhanaID,P.IsAllCases) AS RP ON");
                sqlQuery.Append(" [C].[RegionID] = [RP].[RegionID] AND [C].[ProgramID] = [RP].[ProgramID]");
                sqlQuery.Append(" AND [C].[SubProgramID] = [RP].[SubProgramID] AND [C].[JamatkhanaID] = [RP].[JamatkhanaID]");
                sqlQuery.Append(" LEFT JOIN CaseWorker AS CW ON C.ID = CW.CaseID AND CW.WorkerID = " + CurrentLoggedInWorker.ID + " ");
            }
            sqlQuery.Append(" LEFT JOIN [dbo].[Region] [R] ON [R].[ID]=[C].[RegionID]");
            sqlQuery.Append(" LEFT JOIN [dbo].[SubProgram] [P] ON [P].[ID]=[C].[SubProgramID]");
            sqlQuery.Append(" LEFT JOIN [dbo].[CaseStatus] [CS] ON [CS].[ID]=[C].[CaseStatusID]");
            sqlQuery.Append(" LEFT JOIN [dbo].[Jamatkhana] [JK] ON [JK].[ID]=[C].[JamatkhanaID]");

            sqlQuery.Append(" WHERE [C].[IsArchived] = 0 ");
            
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
            {
                sqlQuery.Append(" AND (IsAllCases = 1 OR (IsAllCases = 0 AND WorkerID IS NOT NULL)) ");
            }
            if (searchParameters.JamatkhanaID > 0)
            {
                sqlQuery.Append(" AND [C].[JamatkhanaID]=" + searchParameters.JamatkhanaID);
            }
            if (searchParameters.CaseNumber.IsNotNullOrEmpty())
            {
                searchParameters.CaseNumber = searchParameters.CaseNumber.Trim();
                sqlQuery.Append(" AND ([C].[CaseNumber]  LIKE '%" + searchParameters.CaseNumber + "%' OR [C].[DisplayID] LIKE '%" + searchParameters.CaseNumber + "%')");
            }
            if (searchParameters.FirstName.IsNotNullOrEmpty())
            {
                searchParameters.FirstName = searchParameters.FirstName.Trim();
                sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseMember [CM] WHERE [CM].[FirstName] LIKE '%" + searchParameters.FirstName + "%')");
            }
            if (searchParameters.LastName.IsNotNullOrEmpty())
            {
                searchParameters.LastName = searchParameters.LastName.Trim();
                sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseMember [CM] WHERE [CM].[LastName] LIKE '%" + searchParameters.LastName + "%')");
            }
            if (searchParameters.DisplayID.IsNotNullOrEmpty())
            {
                searchParameters.DisplayID = searchParameters.DisplayID.Trim();
                sqlQuery.Append(" AND ( [C].[DisplayID] LIKE '%" + searchParameters.DisplayID + "%')");
            }
            if (searchParameters.ProgramID != 0)
            {
                sqlQuery = sqlQuery.Append(" AND [C].[ProgramID] IN (" + searchParameters.ProgramID + ")");
            }
            if (searchParameters.SubProgramID != 0)
            {
                sqlQuery = sqlQuery.Append(" AND [C].[SubProgramID] IN (" + searchParameters.SubProgramID + ")");
            }
            if (searchParameters.RegionID != 0)
            {
                sqlQuery = sqlQuery.Append("AND [C].[RegionID] IN (" + searchParameters.RegionID + ")");
            }
            if (searchParameters.CaseStatusID != 0)
            {
                sqlQuery = sqlQuery.Append("AND [C].[CaseStatusID] IN (" + searchParameters.CaseStatusID + ")");
            }
            if (!string.IsNullOrEmpty(searchParameters.PhoneNumber))
            {
                searchParameters.PhoneNumber = searchParameters.PhoneNumber.Trim();
                sqlQuery.Append(" AND [C].[ID] IN ( SELECT CaseID FROM CaseMember [CM] LEFT JOIN [CaseMemberContact] [CMC] ON [CM].ID=CMC.CaseMemberID WHERE [CMC].Contact LIKE '%" + searchParameters.PhoneNumber + "%')");
            }
            if (searchParameters.CreatedByWorkerName.IsNotNullOrEmpty())
            {
                searchParameters.CreatedByWorkerName = searchParameters.CreatedByWorkerName.Trim();
                string[] names = searchParameters.CreatedByWorkerName.ToStringArray(' ', true);
                StringBuilder nameWhereClause = new StringBuilder();
                if (names != null)
                {
                    foreach (string name in names)
                    {
                        if (nameWhereClause.ToString().IsNotNullOrEmpty())
                        {
                            nameWhereClause.Append(" OR ");
                        }
                        nameWhereClause.Append(" [W].[FirstName] LIKE '%" + name + "%' OR  [W].[LastName] LIKE '%" + name + "%' ");
                    }
                }
                sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseWorker [CW] LEFT JOIN Worker [W] ON [CW].[WorkerID]=[W].[ID] WHERE " + nameWhereClause.ToString() + ")");
            }

            if (searchParameters.AssignedToWorkerID > 0 && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1 && CurrentLoggedInWorkerRoleIDs.IndexOf(8) == -1)
            {
                //IF worker has all cases view permission then do not filter by assigned worker id.
                if (workerroleactionpermissionnewRepository.HasAllCasesPermission(CurrentLoggedInWorkerRoleIDs, searchParameters.AssignedToWorkerID) == 0)
                {
                    sqlQuery.Append(" AND [C].[ID] IN (SELECT CaseID FROM CaseWorker [W] WHERE [W].[WorkerID]=" + searchParameters.AssignedToWorkerID + " AND [W].[IsActive]=1)");
                }
            }

            DataSourceResult dataSourceResult = context.Database.SqlQuery<CaseListViewModel>(strColumns + sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequest);
            
            foreach (CaseListViewModel caseItem in dataSourceResult.Data)
            {
                caseItem.CaseNote = caseprogressnoteRepository.GetprogressNoteByCaseID(caseItem.ID);
                caseItem.HasPermissionToReadmit = "display:none;";
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                {
                    //<JL:comment:06/05/2017>
                    //caseItem.HasPermissionToEdit = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Edit, true).ToDisplayStyle();
                    //caseItem.HasPermissionToDelete = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true).ToDisplayStyle();
                    //caseItem.HasPermissionToCaseProgressNoteCreate = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Create, true).ToDisplayStyle();
                    //caseItem.HasPermissionToCaseProgressNoteIndex = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Index, true).ToDisplayStyle();
                    //</JL:comment:06/05/2017>
                    //<JL:add:06/05/2017>
                    caseItem.HasPermissionToRead = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Read, true).ToDisplayStyle();
                    caseItem.HasPermissionToEdit = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Edit, true).ToDisplayStyle();
                    caseItem.HasPermissionToDelete = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Delete, true).ToDisplayStyle();
                    caseItem.HasPermissionToCaseProgressNoteCreate = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Create, true).ToDisplayStyle();
                    caseItem.HasPermissionToCaseProgressNoteIndex = workerroleactionpermissionnewRepository.HasPermission(caseItem.ID, CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.CaseProgressNote, Constants.Actions.Index, true).ToDisplayStyle();
                    //<JL:add:06/05/2017>

                    if (caseItem.CaseStatusID == 7 || caseItem.CaseStatusID == 8)
                    {
                        caseItem.HasPermissionToReadmit = workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, CurrentLoggedInWorker.ID, caseItem.ProgramID, caseItem.RegionID, caseItem.SubProgramID, caseItem.JamatkhanaID, Constants.Areas.CaseManagement, Constants.Controllers.Case, Constants.Actions.Readmit, true).ToDisplayStyle();
                    }
                }
                else
                {
                    caseItem.HasPermissionToRead = String.Empty;
                    caseItem.HasPermissionToEdit = String.Empty;
                    caseItem.HasPermissionToDelete = String.Empty;
                    caseItem.HasPermissionToCaseProgressNoteCreate = String.Empty;
                    caseItem.HasPermissionToCaseProgressNoteIndex = String.Empty;
                    if (caseItem.CaseStatusID == 7 || caseItem.CaseStatusID == 8)
                    {
                        caseItem.HasPermissionToReadmit = string.Empty;
                    }
                }
                List<SelectListItem> caseMemberList = casememberRepository.FindAllByCaseIDForDropDownList(caseItem.ID);
                if (caseMemberList != null && caseMemberList.Count > 0)
                {
                    foreach (SelectListItem caseMember in caseMemberList)
                    {
                        caseItem.AllCaseMember = caseItem.AllCaseMember.Concate(Environment.NewLine, caseMember.Text);
                        caseItem.AllCaseMemberWithContact = caseItem.AllCaseMemberWithContact.Concate(Environment.NewLine, caseMember.Text);
                        int caseMemberID = caseMember.Value.ToInteger(true);
                        string contactNumnbers = string.Empty;
                        List<CaseMemberContact> caseMemberContacts = context.CaseMemberContact.Where(item => item.CaseMemberID == caseMemberID && item.ContactMediaID != 1 && item.Contact != null && item.Contact != "").ToList();
                        if (caseMemberContacts != null && caseMemberContacts.Count > 0)
                        {
                            foreach (CaseMemberContact caseMemberContact in caseMemberContacts)
                            {
                                contactNumnbers = contactNumnbers.Concate(',', caseMemberContact.Contact);
                            }
                        }
                        if (contactNumnbers.IsNotNullOrEmpty())
                        {
                            caseItem.AllCaseMemberWithContact = caseItem.AllCaseMemberWithContact + "(" + contactNumnbers + ")";
                        }
                    }
                }
            }
            DataSourceRequest dsRequestTotalCountQuery = new DataSourceRequest();
            dsRequestTotalCountQuery.Filters = dsRequest.Filters;

            dataSourceResult.Total = context.Database.SqlQuery<Int32>(strCount + sqlQuery.ToString()).FirstOrDefault();
            return dataSourceResult;
        }

        public Case Readmit(Case varCase)
        {
            Case newCase = new Case()
            {
                Address = varCase.Address,
                CaseNumber = varCase.CaseNumber,
                CaseStatusID = 1,
                City = varCase.City,
                EnrollDate = varCase.EnrollDate,
                FirstName = varCase.FirstName,
                HearingSourceID = varCase.HearingSourceID,
                IntakeMethodID = varCase.IntakeMethodID,
                JamatkhanaID = varCase.JamatkhanaID,
                LastName = varCase.LastName,
                LastUpdateDate = DateTime.Now,
                LastUpdatedByWorkerID = varCase.LastUpdatedByWorkerID,
                PhoneNumber = varCase.PhoneNumber,
                PostalCode = varCase.PostalCode,
                ProgramID = varCase.ProgramID,
                ReferenceCaseID = varCase.ID,
                ReferralDate = varCase.ReferralDate,
                ReferralSourceComments = varCase.ReferralSourceComments,
                ReferralSourceID = varCase.ReferralSourceID,
                RegionID = varCase.RegionID,
                SubProgramID = varCase.SubProgramID
            };
            InsertOrUpdate(newCase);
            Save();
            if (newCase.ID > 0)
            {
                List<CaseMember> caseMemberList = casememberRepository.AllIncluding(varCase.ID).ToList();
                if (caseMemberList != null)
                {
                    foreach (CaseMember caseMember in caseMemberList)
                    {
                        CaseMember newCaseMember = new CaseMember()
                        {
                            CaseID = newCase.ID,
                            DateOfBirth = caseMember.DateOfBirth,
                            EnrollDate = caseMember.EnrollDate,
                            EthnicityID = caseMember.EthnicityID,
                            FirstName = caseMember.FirstName,
                            GenderID = caseMember.GenderID,
                            IsPrimary = caseMember.IsPrimary,
                            LanguageID = caseMember.LanguageID,
                            LastName = caseMember.LastName,
                            LastUpdateDate = newCase.LastUpdateDate,
                            LastUpdatedByWorkerID = newCase.LastUpdatedByWorkerID,
                            MaritalStatusID = caseMember.MaritalStatusID,
                            MemberStatusID = caseMember.MemberStatusID,
                            RelationshipStatusID = caseMember.RelationshipStatusID,
                        };
                        casememberRepository.InsertOrUpdate(newCaseMember);
                        casememberRepository.Save();
                        List<CaseMemberContact> caseMemberContactList = casemembercontactRepository.FindAllByCaseMemberID(caseMember.ID).ToList();
                        if (caseMemberContactList != null)
                        {
                            foreach (CaseMemberContact caseMemberContact in caseMemberContactList)
                            {
                                CaseMemberContact newCaseMemberContact = new CaseMemberContact()
                                {
                                    CaseMemberID = newCaseMember.ID,
                                    Comments = caseMemberContact.Comments,
                                    Contact = caseMemberContact.Contact,
                                    ContactMediaID = caseMemberContact.ContactMediaID,
                                    ContactMediaName = caseMemberContact.ContactMediaName,
                                    LastUpdateDate = newCase.LastUpdateDate,
                                    LastUpdatedByWorkerID = newCase.LastUpdatedByWorkerID
                                };
                                casemembercontactRepository.InsertOrUpdate(newCaseMemberContact);
                                casemembercontactRepository.Save();
                            }
                        }
                    }
                }
            }
            return newCase;
        }

        public override void Delete(int id)
        {
            var entity = Find(id);
            context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            string sqlQuery = @"delete from [CaseSupportCircle] where caseid=@caseid;
delete from [CaseWorker] where caseid=@caseid;
delete from [CaseWorkerMemberAssignment] where casememberid in (select id from [casemember] where caseid=@caseid);
delete from [CaseAction] where caseprogressnoteid in (select id from [CaseProgressNote] where casememberid in (select id from [casemember] where caseid=@caseid));
delete from [CaseProgressNote] where casememberid in (select id from [casemember] where caseid=@caseid);
delete from [CaseMemberContact] where casememberid in (select id from [casemember] where caseid=@caseid);
delete from [casemember] where caseid=@caseid;
delete from [case] where id=@caseid;";
            sqlQuery = sqlQuery.Replace("@caseid", id.ToString());
            context.Database.ExecuteSqlCommand(sqlQuery);

        }
    }

    /// <summary>
    /// interface of Case containing necessary database operations
    /// </summary>
    public interface ICaseRepository : IBaseRepository<Case>
    {
        IQueryable<Case> FindAllByProgramID(int programID);
        IQueryable<Case> FindAllByRegionID(int regionID);
        void InsertOrUpdate(Case varCase);
        Case Readmit(Case varCase);
        DataSourceResult Search(Case searchParameters, DataSourceRequest paramDSRequest);
    }
}
