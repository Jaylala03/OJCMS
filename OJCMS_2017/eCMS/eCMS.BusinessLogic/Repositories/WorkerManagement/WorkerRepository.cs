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
    public class WorkerRepository : BaseRepository<Worker>, IWorkerRepository
    {
        private readonly IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository;
        public WorkerRepository(RepositoryContext context
            ,IWorkerRoleActionPermissionNewRepository workerroleactionpermissionnewRepository)
            : base(context)
        {
            this.workerroleactionpermissionnewRepository = workerroleactionpermissionnewRepository;
        }

        public Worker Find(string workerName, string password)
        {
            return context.Worker.SingleOrDefault(u => u.LoginName == workerName && u.Password == password);
        }

        public Worker Find(string workerName)
        {
            return context.Worker.SingleOrDefault(u => u.LoginName == workerName);
        }

        public Worker FindByEmailAddress(string emailAddress)
        {
            return context.Worker.SingleOrDefault(u => u.EmailAddress == emailAddress);
        }

        public IEnumerable<SelectListItem> AllActiveForDropDownList
        {
            get
            {
                return context.Worker.AsEnumerable().Select(item => new SelectListItem { Text = item.FirstName + " " + item.LastName, Value = item.ID.ToString() });
            }
        }

        public Worker InsertOrUpdate(Worker worker, int actionByWorkerID)
        {
            try
            {
                worker.EmailAddress = worker.EmailAddress.Trim();
                Worker existingWorker = context.Worker.SingleOrDefault(u => u.EmailAddress == worker.EmailAddress && u.IsActive);
                Worker existingLoginWorker = context.Worker.SingleOrDefault(u => u.LoginName == worker.LoginName);
                if (existingLoginWorker != null && existingLoginWorker.ID != worker.ID)
                {
                    throw new CustomException(CustomExceptionType.CommonDuplicacy, "Login name already registered");
                }
                if (existingWorker != null && existingWorker.ID != worker.ID)
                {
                    throw new CustomException(CustomExceptionType.CommonDuplicacy, "Email address already registered");
                }
                else if (existingWorker != null)
                {
                    Remove(existingWorker);
                }
                if (worker.Password.ToLower() != worker.ConfirmPassword.ToLower())
                {
                    throw new CustomException(CustomExceptionType.CommonInvalidData, "Password does not match");
                }
                if (worker.Password.IsNullOrEmpty())
                {
                    worker.Password = "password";
                    worker.ConfirmPassword = worker.Password;
                }
                if (!worker.EmailAddress.Contains("@"))
                {
                    worker.EmailAddress = worker.EmailAddress.RemoveWhiteSpaces().ToLower() + "@organizedchaostech.com";
                }
                if (worker.ID == 0)
                {
                    worker.CreatedByworkerID = actionByWorkerID;
                }
                
                if (existingLoginWorker != null)
                    context.Entry(existingLoginWorker).State = System.Data.Entity.EntityState.Detached;
                
                InsertOrUpdate(worker);
                Save();
                return worker;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException validationException)
            {
                StringBuilder logBuilder = new StringBuilder();
                foreach (var error in validationException.EntityValidationErrors)
                {
                    var entry = error.Entry;
                    foreach (var err in error.ValidationErrors)
                    {
                        logBuilder.AppendLine(err.PropertyName + " Error Message: " + err.ErrorMessage);
                        worker.ErrorMessage = err.ErrorMessage;
                    }
                    if (worker.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            catch (CustomException ex)
            {
                worker.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                worker.ErrorMessage = Constants.Messages.UnhandelledError;
                ExceptionManager.Manage(ex);
            }
            return worker;
        }

        public BaseModel ForgotPassword(string emailAddress)
        {
            BaseModel statusModel = new BaseModel();
            try
            {
                Worker worker = FindByEmailAddress(emailAddress);
                if (worker != null)
                {
                    EmailTemplateRepository emailTemplateRepository = new EmailTemplateRepository(context);
                    EmailTemplate emailTemplate = emailTemplateRepository.FindByEmailTemplateCategoryID(1);
                    emailTemplateRepository.Remove(emailTemplate);
                    if (emailTemplate != null)
                    {
                        EmailManager emailManager = new EmailManager(this);
                        statusModel = emailManager.BuildAndSendEmail(emailTemplate, worker);
                    }
                    else
                    {
                        throw new CustomException(CustomExceptionType.CommonCriticalDataNotFound, "Email template not found");
                    }
                }
                else
                {
                    throw new CustomException(CustomExceptionType.CommonCriticalDataNotFound, "We don't find any record using the email address");
                }
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch
            {
                statusModel.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            return statusModel;
        }

        public BaseModel ChangePassword(int id, string currentPassword, string newPasword, string newConfirmPassword)
        {
            BaseModel statusModel = new BaseModel();
            if (currentPassword.IsNullOrEmpty() || newPasword.IsNullOrEmpty() || newConfirmPassword.IsNullOrEmpty())
            {
                statusModel.ErrorMessage = "Please input value in all required fields";
                return statusModel;
            }

            if (currentPassword==newPasword)
            {
                statusModel.ErrorMessage = "Your new password can't be the same as current password";
                return statusModel;
            }

            if (newPasword != newConfirmPassword)
            {
                statusModel.ErrorMessage = "Password does not match";
                return statusModel;
            }

            Worker worker = Find(id);
            if (worker != null)
            {
                string passwordInSystem =CryptographyHelper.Decrypt(worker.Password);
                if (currentPassword != passwordInSystem)
                {
                    statusModel.ErrorMessage = "Your current password is wrong";
                    return statusModel;
                }
                worker.Password = CryptographyHelper.Encrypt(newPasword);
                worker.ConfirmPassword = worker.Password;
                worker.LastPasswordChangeDate = DateTime.Now;
                InsertOrUpdate(worker);
                Save();
                statusModel.SuccessMessage = "Your password has been changed successfully";
            }
            return statusModel;
        }

        public List<int> setProgramRole(int workerID)
        {  
            var role = context.WorkerInRoleNew.Join(context.WorkerRolePermission, left => left.WorkerRoleID, right => right.ID, (left, right) => new { left, right }).
                Where(item => item.left.WorkerID == workerID).Select(item => item.right.ID).ToList();
               
            return role.ToList();
        }
        public CaseWorker FindByPrimaryAndCaseID(int caseID)
        {
            return context.CaseWorker.Where(item => item.CaseID == caseID && item.IsPrimary==true).FirstOrDefault();
        }

        public  IEnumerable<SelectListItem> FindAllByCaseID(int caseID)
        {
            return context.CaseWorker.Join(context.Worker, left => left.WorkerID, right => right.ID, (left, right) => new { left, right }).Where(item => item.left.CaseID == caseID).AsEnumerable().Select(item => new SelectListItem { Text = item.right.FirstName+" "+item.right.LastName, Value = item.left.ID.ToString() }).ToList();
        }

        public IEnumerable<SelectListItem> FindAllByCaseMemberID(int caseMemberID)
        {
            return context.CaseWorkerMemberAssignment.Join(context.CaseWorker, firstLeft => firstLeft.CaseWorkerID, firstRight => firstRight.ID, (firstLeft, firstRight) => new { firstLeft,firstRight })
                .Join(context.Worker, secondLeft => secondLeft.firstRight.WorkerID, secondRight => secondRight.ID, (secondLeft, secondRight) => new { secondLeft, secondRight })
                .Where(item => item.secondLeft.firstLeft.CaseMemberID == caseMemberID).AsEnumerable().Select(item => new SelectListItem { Text = item.secondRight.FirstName + " " + item.secondRight.LastName, Value = item.secondLeft.firstRight.ID.ToString() }).ToList();
        }

        public IEnumerable<SelectListItem> FindAllForAssignmentByCaseID(int caseID)
        {
            return context.CaseWorker.Join(context.Worker, left => left.WorkerID, right => right.ID, (left, right) => new { left, right }).Where(item => item.left.CaseID == caseID).AsEnumerable().Select(item => new SelectListItem { Text = item.right.FirstName + "" + item.right.LastName, Value = item.right.ID.ToString() }).ToList(); ;
        }

        public IEnumerable<SelectListItem> FindAllByRoleID(int roleID)
        {
            return context.Worker.Join(context.WorkerInRoleNew, left => left.ID, right => right.WorkerID, (left, right) => new { left, right }).Where(item => item.right.WorkerRoleID == roleID).GroupBy(item => new { item.left.ID, item.left.FirstName, item.left.LastName }).AsEnumerable().Select(item => new SelectListItem { Text = item.Key.FirstName + " " + item.Key.LastName, Value = item.Key.ID.ToString() }).ToList();
        }

        //public string FindWorkerNameByCaseWorkerID(int caseWorkerID) 
        //{
        //    return context.CaseWorker.Join(context.Worker, left => left.WorkerID, right => right.ID, (left, right) => new { left, right }).Where(item => item.left.ID == caseWorkerID).Select(item =>item.right.FirstName +" "+item.right.LastName).FirstOrDefault();
        
        //}

//        public DataSourceResult Search(WorkerSearchViewModel searchParameters, DataSourceRequest paramDSRequest)
//        {
//            bool IsCurrentUserNotAdminRole = (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1);
//            string hasEditPermission = (workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.WorkerManagement, Constants.Controllers.Worker, Constants.Actions.Edit, true)).ToDisplayStyle();
//            string hasDeletePermission = (workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.WorkerManagement, Constants.Controllers.Worker, Constants.Actions.Delete, true)).ToDisplayStyle();

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
//                SortDescriptor defaultSortExpression = new SortDescriptor("LastUpdateDate", System.ComponentModel.ListSortDirection.Descending);

//                dsRequest.Sorts.Add(defaultSortExpression);
//            }
//            if (dsRequest.PageSize == 0)
//            {
//                dsRequest.PageSize = Constants.CommonConstants.DefaultPageSize;
//            }
//            StringBuilder sqlQuery = new StringBuilder(@"SELECT DISTINCT 
//                            W.ID,W.EmailAddress,W.IsActive, [W].[FirstName] + ' ' + [W].[LastName] [Name],
//                            '" + hasDeletePermission + @"' [HasPermissionToDelete],
//                            '" + hasEditPermission + @"' [HasPermissionToEdit]
//                            FROM Worker AS [W] 
//                            LEFT JOIN WorkerInRoleNew WIR on W.ID = WIR.WorkerID
//                            LEFT JOIN WorkerRolePermissionNew WRP on WIR.WorkerRoleID = WRP.WorkerRoleID
//                            LEFT JOIN Permission AS P ON WRP.PermissionID = P.ID
//                            LEFT JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID
//                            LEFT JOIN PermissionSubProgram AS PSP ON PR.ID = PSP.PermissionRegionID
//                            LEFT JOIN PermissionJamatkhana AS PJK ON PR.ID = PJK.PermissionRegionID
//                            WHERE [W].[ID] > 0 ");
//            if (IsCurrentUserNotAdminRole)
//            {
//                sqlQuery.Append(" AND [W].[ID] <> " + CurrentLoggedInWorker.ID + " ");
//            }
//            if (searchParameters.RegionID > 0)
//            {
//                searchParameters.RegionID = searchParameters.RegionID;
//                sqlQuery.Append(" AND [PR].[RegionID] =" + searchParameters.RegionID + "");
//            }
//            //else if (IsCurrentLoggedInWorkerRegionalAdministrator) //<JL:Comment:06/11/2017>
//            else if (IsCurrentUserNotAdminRole) //<JL:Add:06/11/2017>
//            {
//                StringBuilder regionquery = new StringBuilder();
//                regionquery.Append("SELECT DISTINCT PR.RegionID ");
//                regionquery.Append("FROM Worker AS [W] ");
//                regionquery.Append("LEFT JOIN WorkerInRoleNew WIR on W.ID = WIR.WorkerID ");
//                regionquery.Append("LEFT JOIN WorkerRolePermissionNew WRP on WIR.WorkerRoleID = WRP.WorkerRoleID ");
//                regionquery.Append("LEFT JOIN Permission AS P ON WRP.PermissionID = P.ID ");
//                regionquery.Append("LEFT JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
//                //regionquery.Append("WHERE [W].ID = " + CurrentLoggedInWorker.ID + " AND WIR.WorkerRoleID = " + SiteConfigurationReader.RegionalAdministratorRoleID); //<JL:Comment:06/11/2017>
//                regionquery.Append("WHERE [W].ID = " + CurrentLoggedInWorker.ID); //<JL:Add:06/11/2017>
//                searchParameters.RegionID = searchParameters.RegionID;
//                //sqlQuery.Append(" AND [PR].[RegionID] = (SELECT RegionID FROM WorkerInRoleNew WIR WHERE WorkerRoleID = " + SiteConfigurationReader.RegionalAdministratorRoleID + " AND WorkerID = " + CurrentLoggedInWorker.ID + ")");
//                sqlQuery.Append(" AND [PR].[RegionID] IN (" + regionquery.ToString() + ")");
//                sqlQuery.Append(" AND [W].[ID] !=1");
//            }
//            if (searchParameters.FirstName.IsNotNullOrEmpty())
//            {
//                searchParameters.FirstName = searchParameters.FirstName.Trim();
//                sqlQuery.Append(" AND [W].[FirstName] LIKE '%" + searchParameters.FirstName + "%'");
//            }

//            if (searchParameters.LastName.IsNotNullOrEmpty())
//            {
//                searchParameters.LastName = searchParameters.LastName.Trim();
//                sqlQuery.Append(" AND [W].[LastName] LIKE '%" + searchParameters.LastName + "%'");
//            }

//            if (searchParameters.RoleID > 0)
//            {
//                sqlQuery.Append(" AND [W].[ID] IN (SELECT [WorkerID] FROM WorkerInRoleNew WHERE WorkerRoleID = " + searchParameters.RoleID + ")");
//            }
//            if (searchParameters.ProgramID > 0)
//            {
//                sqlQuery.Append(" AND [PR].[ProgramID] =" + searchParameters.ProgramID + "");
//            }
//            else if (IsCurrentUserNotAdminRole) //<JL:Add:06/11/2017>
//            {
//                StringBuilder programquery = new StringBuilder();
//                programquery.Append("SELECT DISTINCT PR.ProgramID ");
//                programquery.Append("FROM Worker AS [W] ");
//                programquery.Append("INNER JOIN WorkerInRoleNew WIR on W.ID = WIR.WorkerID ");
//                programquery.Append("INNER JOIN WorkerRolePermissionNew WRP on WIR.WorkerRoleID = WRP.WorkerRoleID ");
//                programquery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
//                programquery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
//                programquery.Append("WHERE [W].ID = " + CurrentLoggedInWorker.ID);
//                searchParameters.ProgramID = searchParameters.ProgramID;

//                sqlQuery.Append(" AND [PR].[ProgramID] IN (" + programquery.ToString() + ")");
//                sqlQuery.Append(" AND [W].[ID] !=1");
//            }
//            if (IsCurrentUserNotAdminRole) //<JL:Add:06/11/2017>
//            {
//                StringBuilder subprogramquery = new StringBuilder();
//                subprogramquery.Append("SELECT DISTINCT PSP.SubProgramID ");
//                subprogramquery.Append("FROM Worker AS [W] ");
//                subprogramquery.Append("INNER JOIN WorkerInRoleNew WIR on W.ID = WIR.WorkerID ");
//                subprogramquery.Append("INNER JOIN WorkerRolePermissionNew WRP on WIR.WorkerRoleID = WRP.WorkerRoleID ");
//                subprogramquery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
//                subprogramquery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
//                subprogramquery.Append("INNER JOIN PermissionSubProgram AS PSP ON PR.ID = PSP.PermissionRegionID ");
//                subprogramquery.Append("WHERE [W].ID = " + CurrentLoggedInWorker.ID);

//                sqlQuery.Append(" AND [PSP].[SubProgramID] IN (" + subprogramquery.ToString() + ")");
//                sqlQuery.Append(" AND [W].[ID] !=1");

//                StringBuilder jkquery = new StringBuilder();
//                jkquery.Append("SELECT DISTINCT PJK.JamatkhanaID ");
//                jkquery.Append("FROM Worker AS [W] ");
//                jkquery.Append("INNER JOIN WorkerInRoleNew WIR on W.ID = WIR.WorkerID ");
//                jkquery.Append("INNER JOIN WorkerRolePermissionNew WRP on WIR.WorkerRoleID = WRP.WorkerRoleID ");
//                jkquery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
//                jkquery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");
//                jkquery.Append("LEFT JOIN PermissionJamatkhana AS PJK ON PR.ID = PJK.PermissionRegionID ");
//                jkquery.Append("WHERE [W].ID = " + CurrentLoggedInWorker.ID);

//                sqlQuery.Append(" AND [PJK].[JamatkhanaID] IN (" + jkquery.ToString() + ")");
//            }

//            DataSourceResult dataSourceResult = context.Database.SqlQuery<WorkerListViewModel>(sqlQuery.ToString()).AsEnumerable().GroupBy(m=>m.ID).Select(m=>m.First()).ToDataSourceResult(dsRequest);

//            DataSourceRequest dsRequestTotalCountQuery = new DataSourceRequest();
//            dsRequestTotalCountQuery.Filters = dsRequest.Filters;
//            dataSourceResult.Total = context.Database.SqlQuery<WorkerListViewModel>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequestTotalCountQuery).Data.AsQueryable().Count();
//            return dataSourceResult;
//        }

        public DataSourceResult Search(WorkerSearchViewModel searchParameters, DataSourceRequest paramDSRequest)
        {
            bool IsCurrentUserNotAdminRole = (CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1);
            string hasEditPermission = (workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.WorkerManagement, Constants.Controllers.Worker, Constants.Actions.Edit, true)).ToDisplayStyle();
            string hasDeletePermission = (workerroleactionpermissionnewRepository.HasPermission(CurrentLoggedInWorkerRoleIDs, Constants.Areas.WorkerManagement, Constants.Controllers.Worker, Constants.Actions.Delete, true)).ToDisplayStyle();

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
                SortDescriptor defaultSortExpression = new SortDescriptor("LastUpdateDate", System.ComponentModel.ListSortDirection.Descending);

                dsRequest.Sorts.Add(defaultSortExpression);
            }
            if (dsRequest.PageSize == 0)
            {
                dsRequest.PageSize = Constants.CommonConstants.DefaultPageSize;
            }
            StringBuilder sqlQuery;

            sqlQuery = new StringBuilder(@"SELECT DISTINCT 
                            W.ID,W.EmailAddress,W.IsActive, [W].[FirstName] + ' ' + [W].[LastName] [Name],
                            '" + hasDeletePermission + @"' [HasPermissionToDelete],
                            '" + hasEditPermission + @"' [HasPermissionToEdit]
                            FROM Worker AS [W] ");

            if (IsCurrentUserNotAdminRole)
            {
                sqlQuery.Append(" INNER JOIN WorkerInRoleNew WIR on W.ID = WIR.WorkerID");
                sqlQuery.Append(" INNER JOIN WorkerRolePermissionNew WRP on WIR.WorkerRoleID = WRP.WorkerRoleID");

                if (searchParameters.RegionID > 0 || searchParameters.ProgramID > 0)
                {
                    sqlQuery.Append(" INNER JOIN PermissionRegion AS PR ON WRP.PermissionID = PR.PermissionID");
                    if (searchParameters.RegionID > 0)
                    {
                        sqlQuery.Append(" AND [PR].[RegionID] =" + searchParameters.RegionID + "");
                    }
                    if (searchParameters.ProgramID > 0)
                    {
                        sqlQuery.Append(" AND [PR].[ProgramID] =" + searchParameters.ProgramID + "");
                    }
                }

                sqlQuery.Append(" INNER JOIN (");
                sqlQuery.Append(" SELECT WRP.PermissionID");
                sqlQuery.Append(" FROM WorkerInRoleNew AS WIR ");
                sqlQuery.Append(" INNER JOIN WorkerRolePermissionNew WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID");
                sqlQuery.Append(" WHERE [WIR].WorkerID = " + CurrentLoggedInWorker.ID + " ");
                sqlQuery.Append(" ) AS CW ON [WRP].[PermissionID] = CW.PermissionID");
            }
            else
            {
                sqlQuery.Append(" LEFT JOIN WorkerInRoleNew WIR on W.ID = WIR.WorkerID");
                sqlQuery.Append(" LEFT JOIN WorkerRolePermissionNew WRP on WIR.WorkerRoleID = WRP.WorkerRoleID");

                if (searchParameters.RegionID > 0 || searchParameters.ProgramID > 0)
                {
                    sqlQuery.Append(" LEFT JOIN PermissionRegion AS PR ON WRP.PermissionID = PR.PermissionID");
                    if (searchParameters.RegionID > 0)
                    {
                        sqlQuery.Append(" AND [PR].[RegionID] =" + searchParameters.RegionID + "");
                    }
                    if (searchParameters.ProgramID > 0)
                    {
                        sqlQuery.Append(" AND [PR].[ProgramID] =" + searchParameters.ProgramID + "");
                    }
                }
            }

            sqlQuery.Append(" WHERE [W].[ID] > 0 ");

            if (IsCurrentUserNotAdminRole)
            {
                sqlQuery.Append(" AND [W].[ID] NOT IN (" + CurrentLoggedInWorker.ID + ",1) ");
            }

            if (searchParameters.FirstName.IsNotNullOrEmpty())
            {
                searchParameters.FirstName = searchParameters.FirstName.Trim();
                sqlQuery.Append(" AND [W].[FirstName] LIKE '%" + searchParameters.FirstName + "%'");
            }

            if (searchParameters.LastName.IsNotNullOrEmpty())
            {
                searchParameters.LastName = searchParameters.LastName.Trim();
                sqlQuery.Append(" AND [W].[LastName] LIKE '%" + searchParameters.LastName + "%'");
            }

            if (searchParameters.RoleID > 0)
            {
                sqlQuery.Append(" AND [W].[ID] IN (SELECT [WorkerID] FROM WorkerInRoleNew WHERE WorkerRoleID = " + searchParameters.RoleID + ")");
            }

            DataSourceResult dataSourceResult = context.Database.SqlQuery<WorkerListViewModel>(sqlQuery.ToString()).AsEnumerable().GroupBy(m => m.ID).Select(m => m.First()).ToDataSourceResult(dsRequest);

            DataSourceRequest dsRequestTotalCountQuery = new DataSourceRequest();
            dsRequestTotalCountQuery.Filters = dsRequest.Filters;
            dataSourceResult.Total = context.Database.SqlQuery<WorkerListViewModel>(sqlQuery.ToString()).AsEnumerable().ToDataSourceResult(dsRequestTotalCountQuery).Data.AsQueryable().Count();
            return dataSourceResult;
        }

        public override void Delete(int id)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("DELETE FROM WorkerInRoleNew WHERE WorkerID=" + id); 
            sqlQuery.Append(";");
            sqlQuery.Append("DELETE FROM Worker WHERE ID=" + id);
            context.Database.ExecuteSqlCommand(sqlQuery.ToString());
        }

        public List<SelectListItem> FindAllPossible(int programID, int regionID, int subProgramID, int? RoleId = 0)
        {
            if (RoleId > 0)
            {
                return context.Worker.Join(context.WorkerInRole, left => left.ID, right => right.WorkerID, (left, right) => new { left, right })
                 .Join(context.WorkerSubProgram, secondLeft => secondLeft.right.ID, secondright => secondright.WorkerInRoleID, (secondleft, secondright) => new { secondleft, secondright })
                 .Where(item => item.secondleft.right.ProgramID == programID && item.secondleft.right.RegionID == regionID && item.secondright.SubProgramID == subProgramID && item.secondleft.right.WorkerRoleID==RoleId)
                 .GroupBy(item => new { item.secondleft.left.ID, item.secondleft.left.FirstName, item.secondleft.left.LastName }).AsEnumerable()
                 .Select(item => new SelectListItem { Text = item.Key.FirstName + " " + item.Key.LastName, Value = item.Key.ID.ToString() }).ToList();
                
            }
            else
            {
                return context.Worker.Join(context.WorkerInRole, left => left.ID, right => right.WorkerID, (left, right) => new { left, right })
                    .Join(context.WorkerSubProgram, secondLeft => secondLeft.right.ID, secondright => secondright.WorkerInRoleID, (secondleft, secondright) => new { secondleft, secondright })
                    .Where(item => item.secondleft.right.ProgramID == programID && item.secondleft.right.RegionID == regionID && item.secondright.SubProgramID == subProgramID)
                    .GroupBy(item => new { item.secondleft.left.ID, item.secondleft.left.FirstName, item.secondleft.left.LastName }).AsEnumerable()
                    .Select(item => new SelectListItem { Text = item.Key.FirstName + " " + item.Key.LastName, Value = item.Key.ID.ToString() }).ToList();
            }
        }
        public List<SelectListItem> NewFindAllPossible(int programID, int regionID, int subProgramID, int? RoleId = 0)
        {
            List<DropDownViewModel> worker = null;

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT W.ID,(W.FirstName + ' ' + W.LastName) AS Name ");
            sqlQuery.Append("FROM WORKER AS W ");
            sqlQuery.Append("INNER JOIN WorkerInRoleNew AS WIR ON W.ID = WIR.WorkerID ");

            if (RoleId.HasValue && RoleId.Value > 0)
                sqlQuery.Append(" AND WIR.WorkerRoleID = " + RoleId + " ");

            if (programID > 0 || regionID > 0 || subProgramID > 0)
            { 
                sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID "); 
                sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
                sqlQuery.Append("INNER JOIN PermissionRegion AS PR ON P.ID = PR.PermissionID ");

                if (programID > 0)
                    sqlQuery.Append(" AND PR.ProgramID = " + programID + " ");

                if (regionID > 0)
                    sqlQuery.Append(" AND PR.RegionID  = " + regionID + " ");

                if (subProgramID > 0)
                {
                    sqlQuery.Append("INNER JOIN PermissionSubProgram PSPRG ON PR.ID = PSPRG.PermissionRegionID ");
                    sqlQuery.Append(" AND PSPRG.SubProgramID = " + subProgramID + " ");
                }
            }

            sqlQuery.Append("GROUP BY W.ID,W.FirstName,W.LastName ");
            sqlQuery.Append("ORDER BY W.FirstName ");

            worker = context.Database.SqlQuery<DropDownViewModel>(sqlQuery.ToString()).ToList();
            return worker.AsEnumerable().Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();

        }
    }

    public interface IWorkerRepository : IBaseRepository<Worker>
    {
        Worker Find(string workerName, string password);
        Worker Find(string workerName);
        Worker FindByEmailAddress(string emailAddress);
        IEnumerable<SelectListItem> AllActiveForDropDownList { get; }
        Worker InsertOrUpdate(Worker worker, int actionByWorkerID);
        BaseModel ForgotPassword(string emailAddress);
        BaseModel ChangePassword(int id,string currentPassword,string newPasword,string newConfirmPassword);
        List<int> setProgramRole(int ID);
        IEnumerable<SelectListItem> FindAllByCaseID(int caseID);
        IEnumerable<SelectListItem> FindAllByCaseMemberID(int caseMemberID);
        IEnumerable<SelectListItem>  FindAllForAssignmentByCaseID(int caseID);
        DataSourceResult Search(WorkerSearchViewModel searchParameters, DataSourceRequest paramDSRequest);
        IEnumerable<SelectListItem> FindAllByRoleID(int roleID);
        List<SelectListItem> FindAllPossible(int programID, int regionID, int subProgramID,int? RoleId=0);
        List<SelectListItem> NewFindAllPossible(int programID, int regionID, int subProgramID, int? RoleId = 0);
        CaseWorker FindByPrimaryAndCaseID(int caseID);
    }
}
