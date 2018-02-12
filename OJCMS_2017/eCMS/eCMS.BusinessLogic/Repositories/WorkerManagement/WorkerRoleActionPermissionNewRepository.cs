using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerRoleActionPermissionNewRepository : BaseRepository<WorkerRoleActionPermissionNew>, IWorkerRoleActionPermissionNewRepository
    {
        public WorkerRoleActionPermissionNewRepository(RepositoryContext context)
            : base(context)
        {
        }

        public bool HasPermission(List<int> workerRoleIds, string areaName, string controllerName, string actionName, bool useCache = false)
        {
            try
            {
                if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1)
                {
                    return true;
                }
                areaName = areaName.ToLower();
                controllerName = controllerName.ToLower();
                actionName = actionName.ToLower();
                WorkerRoleActionPermissionNew permission = null;

                if (useCache)
                {
                    List<WorkerRoleActionPermissionNew> permissionList = WebHelper.CurrentSession.Content.WorkerRoleActionPermissionListNew;
                    if (permissionList == null || (permissionList != null && permissionList.Count == 0))
                    {
                        permissionList = GetWorkerRoleActionPermissionNewList(workerRoleIds, CurrentLoggedInWorker.ID); //context.WorkerRoleActionPermission.ToList();
                        WebHelper.CurrentSession.Content.WorkerRoleActionPermissionListNew = permissionList;
                    }

                    if (areaName.IsNotNullOrEmpty() && controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    {
                        permission = permissionList.FirstOrDefault(item => item.AreaName.ToLower() == areaName && item.ControllerName.ToLower() == controllerName 
                            && item.ActionMethodName.ToLower() == actionName && CurrentLoggedInWorkerRoleIDs.IndexOf(item.WorkerRoleID) != -1 );
                    }
                    else if (controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    {
                        permission = permissionList.FirstOrDefault(item => item.ControllerName.ToLower() == controllerName && item.ActionMethodName.ToLower() == actionName
                            && CurrentLoggedInWorkerRoleIDs.IndexOf(item.WorkerRoleID) != -1);
                    }
                    else if (controllerName.IsNotNullOrEmpty() )
                    {
                        permission = permissionList.FirstOrDefault(item => item.ControllerName.ToLower() == controllerName
                            && CurrentLoggedInWorkerRoleIDs.IndexOf(item.WorkerRoleID) != -1);
                    }
                    if (permission != null)
                    {
                        return true;
                    }
                }
                else
                {
                    permission = GetWorkerRoleActionPermissionNew(workerRoleIds, CurrentLoggedInWorker.ID, areaName, controllerName, actionName);
                    //if (areaName.IsNotNullOrEmpty() && controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    //{
                    //    permission = context.WorkerRoleActionPermission.FirstOrDefault(item => item.AreaName.ToLower() == areaName && item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && workerRoleIds.Contains(item.WorkerRoleID.ToString()));
                    //}
                    //else if (controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    //{
                    //    permission = context.WorkerRoleActionPermission.FirstOrDefault(item => item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && workerRoleIds.Contains(item.WorkerRoleID.ToString()));
                    //}
                    if (permission != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool HasPermission(List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, int? JamatkhanaID, string areaName, string controllerName, string actionName, bool useCache = false)
        {
            //if (workerRoleIDs.Contains("1") || (workerRoleIDs.Contains(SiteConfigurationReader.RegionalManagerRoleID.ToString()) && CurrentLoggedInWorkerRegionIDs.Contains(regionID.ToString())))
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1 || (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1 && CurrentLoggedInWorkerRegionIDs.IndexOf(regionID) != -1))
            {
                return true;
            }

            workerRoleIDs = null;

            //List<WorkerInRole> workerRoleList = context.WorkerInRole.Join(context.WorkerSubProgram, left => left.ID, right => right.WorkerInRoleID, (left, right) => new { left, right }).Where(item => item.left.WorkerID == workerID && item.left.ProgramID == programID && item.left.RegionID == regionID && item.right.SubProgramID == subProgramID).Select(item => item.left).ToList();
            workerRoleIDs = GetWorkerInRoleNew(workerID, programID, regionID, subProgramID, JamatkhanaID);
            //List<WorkerInRoleNew> workerRoleList = GetWorkerInRoleNew(workerID,programID,regionID,subProgramID);
            //if (workerRoleList != null)
            //{
            //    foreach (WorkerInRoleNew workerRole in workerRoleList)
            //    {
            //        if (!workerRoleIDs.Contains(workerRole.WorkerRoleID.ToString()))
            //        {
            //            workerRoleIDs = workerRoleIDs.Concate(',', workerRole.WorkerRoleID.ToString());
            //        }
            //    }
            //}
            return HasPermission(workerRoleIDs, areaName, controllerName, actionName, useCache);
        }

        public bool HasPermission(int caseId, List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, int? JamatkhanaID, string areaName, string controllerName, string actionName, bool useCache = false)
        {
            //if (workerRoleIDs.Contains("1") || (workerRoleIDs.Contains(SiteConfigurationReader.RegionalManagerRoleID.ToString()) && CurrentLoggedInWorkerRegionIDs.Contains(regionID.ToString())))
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1 || (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1 && CurrentLoggedInWorkerRegionIDs.IndexOf(regionID) != -1))
            {
                return true;
            }

            bool isAssigned = false;

            if (HasAllCasesPermissionAction(workerRoleIDs, workerID, programID, regionID, subProgramID, JamatkhanaID, areaName, controllerName, actionName) > 0)
            {
                //return HasPermission(workerRoleIDs, workerID, programID, regionID, subProgramID, JamatkhanaID, areaName, controllerName, actionName, useCache);
                return true;
            }
            else
            {
                int count = context.CaseWorker.Where(item => item.CaseID == caseId && item.WorkerID == workerID).Count();
                if (count > 0)
                {
                    isAssigned = true;
                }
                if (isAssigned)
                {
                    return HasPermission(workerRoleIDs, workerID, programID, regionID, subProgramID, JamatkhanaID, areaName, controllerName, actionName, useCache);
                }
            }
            //int count = context.CaseWorker.Where(item => item.CaseID == caseId && item.WorkerID == workerID).Count();
            //if (count > 0)
            //{
            //    isAssigned = true;
            //}
            //if (isAssigned)
            //{
            //    return HasPermission(workerRoleIDs, workerID, programID, regionID, subProgramID, areaName, controllerName, actionName, useCache);
            //}

            return false;
        }

        public List<WorkerRoleActionPermissionNew> GetWorkerRoleActionPermissionNewList(List<int> workerRoleIDs, int workerID)
        {
            List<WorkerRoleActionPermissionNew> result = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT DISTINCT WRP.WorkerRoleID,AM.AreaName,AM.ControllerName,AM.ActionMethodName ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionAction AS PA ON P.ID = PA.PermissionID ");
            sqlQuery.Append("INNER JOIN ActionMethod AS AM ON PA.ActionMethodID = AM.ID ");
            sqlQuery.Append("WHERE WIR.WorkerID = " + workerID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");

            result = context.Database.SqlQuery<WorkerRoleActionPermissionNew>(sqlQuery.ToString()).ToList();

            return result;
        }

        public WorkerRoleActionPermissionNew GetWorkerRoleActionPermissionNew(List<int> workerRoleIDs, int workerID, string areaName, string controllerName, string actionName)
        {
            WorkerRoleActionPermissionNew result = null;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT TOP 1 WRP.WorkerRoleID,AM.AreaName,AM.ControllerName,AM.ActionMethodName ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionAction AS PA ON P.ID = PA.PermissionID ");
            sqlQuery.Append("INNER JOIN ActionMethod AS AM ON PA.ActionMethodID = AM.ID ");
            sqlQuery.Append("WHERE WIR.WorkerID = " + workerID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            if (!string.IsNullOrEmpty(areaName))
            {
                sqlQuery.Append(" AND AM.AreaName = '" + areaName + "'");
            }
            if (!string.IsNullOrEmpty(controllerName))
            {
                sqlQuery.Append(" AND AM.ControllerName = '" + controllerName + "'");
            }
            if (!string.IsNullOrEmpty(actionName))
            {
                sqlQuery.Append(" AND AM.ActionMethodName = '" + actionName + "'");
            }

            result = context.Database.SqlQuery<WorkerRoleActionPermissionNew>(sqlQuery.ToString()).FirstOrDefault();

            return result;
        }

        public int HasAllCasesPermissionAction(List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, int? JamatkhanaID, string areaName, string controllerName, string actionName)
        {
            int result = 0;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("IF EXISTS( ");
            sqlQuery.Append("SELECT TOP 1 P.ID ");
            sqlQuery.Append("FROM ");
            sqlQuery.Append("WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("INNER JOIN PermissionAction AS PA ON P.ID = PA.PermissionID ");
            sqlQuery.Append("INNER JOIN ActionMethod AS AM ON PA.ActionMethodID = AM.ID ");
            if (programID > 0 || regionID > 0 || subProgramID > 0)
            {
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
                if (JamatkhanaID.HasValue && JamatkhanaID.Value > 0)
                {
                    sqlQuery.Append("INNER JOIN PermissionJamatkhana PJK ON PR.ID = PJK.PermissionRegionID ");
                    sqlQuery.Append(" AND PJK.JamatkhanaID = " + JamatkhanaID.Value + " ");
                }
            }
            sqlQuery.Append("WHERE ");
            sqlQuery.Append("P.IsAllCases = 1 AND WIR.WorkerID = " + workerID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            if (!string.IsNullOrEmpty(areaName))
            {
                sqlQuery.Append(" AND AM.AreaName = '" + areaName + "'");
            }
            if (!string.IsNullOrEmpty(controllerName))
            {
                sqlQuery.Append(" AND AM.ControllerName = '" + controllerName + "'");
            }
            if (!string.IsNullOrEmpty(actionName))
            {
                sqlQuery.Append(" AND AM.ActionMethodName = '" + actionName + "'");
            }
            sqlQuery.Append(") ");
            sqlQuery.Append("BEGIN ");
            sqlQuery.Append("SELECT 1 AS result; ");
            sqlQuery.Append("END ");
            sqlQuery.Append("ELSE ");
            sqlQuery.Append("BEGIN ");
            sqlQuery.Append("SELECT 0 AS result; ");
            sqlQuery.Append("END ");

            result = context.Database.SqlQuery<int>(sqlQuery.ToString()).FirstOrDefault();

            return result;
        }

        public int HasAllCasesPermission(List<int> workerRoleIDs, int workerID)
        {
            int result = 0;
            string loggedinworkers = String.Join(",", CurrentLoggedInWorkerRoleIDs);

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("IF EXISTS( ");
            sqlQuery.Append("SELECT TOP 1 P.ID ");
            sqlQuery.Append("FROM ");
            sqlQuery.Append("WorkerInRoleNew AS WIR ");
            sqlQuery.Append("INNER JOIN WorkerRolePermissionNew AS WRP ON WIR.WorkerRoleID = WRP.WorkerRoleID ");
            sqlQuery.Append("INNER JOIN Permission AS P ON WRP.PermissionID = P.ID ");
            sqlQuery.Append("WHERE ");
            sqlQuery.Append("P.IsAllCases = 1 AND WIR.WorkerID = " + workerID + " AND WIR.WorkerRoleID IN (" + loggedinworkers + ") ");
            sqlQuery.Append(") ");
            sqlQuery.Append("BEGIN ");
            sqlQuery.Append("SELECT 1 AS result; ");
            sqlQuery.Append("END ");
            sqlQuery.Append("ELSE ");
            sqlQuery.Append("BEGIN ");
            sqlQuery.Append("SELECT 0 AS result; ");
            sqlQuery.Append("END ");

            result = context.Database.SqlQuery<int>(sqlQuery.ToString()).FirstOrDefault();

            return result;
        }
        public List<int> GetWorkerInRoleNew(int workerID, int programID, int regionID, int subProgramID, int? jamatKhanaID)
        {
            List<int> result = null;
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.Append("SELECT WRP.WorkerRoleID ");
            sqlQuery.Append("FROM WorkerInRoleNew AS WIR ");

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
                if (jamatKhanaID.HasValue && jamatKhanaID.Value > 0)
                {
                    sqlQuery.Append("INNER JOIN PermissionJamatkhana PJK ON PR.ID = PJK.PermissionRegionID ");
                    sqlQuery.Append(" AND PJK.JamatkhanaID = " + jamatKhanaID.Value + " ");
                }
            }
            sqlQuery.Append("WHERE WIR.WorkerID = " + workerID + " ");
            sqlQuery.Append(" AND WIR.EffectiveFrom <= '" + DateTime.Now + "' AND WIR.EffectiveTo >= '" + DateTime.Now + "' ");
            sqlQuery.Append(" GROUP BY WRP.WorkerRoleID ");

            result = context.Database.SqlQuery<int>(sqlQuery.ToString()).ToList();

            return result;
        }
    }

    public interface IWorkerRoleActionPermissionNewRepository : IBaseRepository<WorkerRoleActionPermissionNew>
    {
        bool HasPermission(List<int> workerRoleIds, string areaName, string controllerName, string actionName, bool useCache = false);
        bool HasPermission(List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, int? JamatkhanaID, string areaName, string controllerName, string actionName, bool useCache = false);
        bool HasPermission(int caseId, List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, int? JamatkhanaID, string areaName, string controllerName, string actionName, bool useCache = false);

        List<WorkerRoleActionPermissionNew> GetWorkerRoleActionPermissionNewList(List<int> workerRoleIDs, int workerID);

        WorkerRoleActionPermissionNew GetWorkerRoleActionPermissionNew(List<int> workerRoleIDs, int workerID, string areaName, string controllerName, string actionName);

        List<int> GetWorkerInRoleNew(int workerID, int programID, int regionID, int subProgramID, int? JamatkhanaID);

        int HasAllCasesPermissionAction(List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, int? JamatkhanaID, string areaName, string controllerName, string actionName);

        int HasAllCasesPermission(List<int> workerRoleIDs, int workerID);
    }
}