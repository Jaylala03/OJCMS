using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories.Context;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eCMS.BusinessLogic.Repositories
{
    public class WorkerRoleActionPermissionRepository : BaseRepository<WorkerRoleActionPermission>, IWorkerRoleActionPermissionRepository
    {
        public WorkerRoleActionPermissionRepository(RepositoryContext context)
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
                WorkerRoleActionPermission permission = null;
                if (useCache)
                {
                    List<WorkerRoleActionPermission> permissionList = WebHelper.CurrentSession.Content.WorkerRoleActionPermissionList;
                    if (permissionList == null || (permissionList != null && permissionList.Count == 0))
                    {
                        permissionList = context.WorkerRoleActionPermission.ToList();
                        WebHelper.CurrentSession.Content.WorkerRoleActionPermissionList = permissionList;
                    }

                    if (areaName.IsNotNullOrEmpty() && controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    {
                        permission = permissionList.FirstOrDefault(item => item.AreaName.ToLower() == areaName && item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && CurrentLoggedInWorkerRoleIDs.IndexOf(item.WorkerRoleID) != -1);
                    }
                    else if (controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    {
                        permission = permissionList.FirstOrDefault(item => item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && CurrentLoggedInWorkerRoleIDs.IndexOf(item.WorkerRoleID) != -1);
                    }
                    if (permission != null)
                    {
                        return true;
                    }
                }
                else
                {
                    if (areaName.IsNotNullOrEmpty() && controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    {
                        permission = context.WorkerRoleActionPermission.FirstOrDefault(item => item.AreaName.ToLower() == areaName && item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && CurrentLoggedInWorkerRoleIDs.IndexOf(item.WorkerRoleID) != -1);
                    }
                    else if (controllerName.IsNotNullOrEmpty() && actionName.IsNotNullOrEmpty())
                    {
                        permission = context.WorkerRoleActionPermission.FirstOrDefault(item => item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && CurrentLoggedInWorkerRoleIDs.IndexOf(item.WorkerRoleID) != -1);
                    }
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

        public bool HasPermission(List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, string areaName, string controllerName, string actionName, bool useCache = false)
        {
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1 || (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1 && CurrentLoggedInWorkerRegionIDs.IndexOf(regionID) != -1))
            {
                return true;
            }
            workerRoleIDs = null;
            List<WorkerInRole> workerRoleList = context.WorkerInRole.Join(context.WorkerSubProgram, left => left.ID, right => right.WorkerInRoleID, (left, right) => new { left, right }).Where(item => item.left.WorkerID == workerID && item.left.ProgramID == programID && item.left.RegionID == regionID && item.right.SubProgramID == subProgramID).Select(item => item.left).ToList();
            if (workerRoleList != null)
            {
                foreach (WorkerInRole workerRole in workerRoleList)
                {
                    if (workerRoleIDs.IndexOf(workerRole.WorkerRoleID) == -1)
                    {
                        workerRoleIDs.Add(workerRole.WorkerRoleID);
                    }
                }
            }
            return HasPermission(workerRoleIDs, areaName, controllerName, actionName, useCache);
        }

        public bool HasPermission(int caseId, List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, string areaName, string controllerName, string actionName, bool useCache = false)
        {
            if (CurrentLoggedInWorkerRoleIDs.IndexOf(1) != -1 || (CurrentLoggedInWorkerRoleIDs.IndexOf(SiteConfigurationReader.RegionalManagerRoleID) != -1 && CurrentLoggedInWorkerRegionIDs.IndexOf(regionID) != -1))
            {
                return true;
            }
            bool isAssigned = false;
            int count = context.CaseWorker.Where(item => item.CaseID == caseId && item.WorkerID == workerID).Count();
            if (count > 0)
            {
                isAssigned = true;
            }
            if (isAssigned)
            {
                return HasPermission(workerRoleIDs, workerID, programID, regionID, subProgramID, areaName, controllerName, actionName, useCache);
            }
            return false;
        }

        public void InsertOrUpdate(WorkerRoleActionPermission workerroleactionpermission)
        {
            if (workerroleactionpermission.ID == default(int))
            {
                // New entity
                context.WorkerRoleActionPermission.Add(workerroleactionpermission);
            }
            else
            {
                // Existing entity
                context.Entry(workerroleactionpermission).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void InsertOrUpdate(string workerroleactionpermission, List<WorkerRole> workerRoleList)
        {
            //2+AppBuilder+AppMenu+Index,3+AppBuilder+AppMenu+Index,4+AppBuilder+AppMenu+Index,2+AppBuilder+AppMenu+Search,1+OM+OrganizationAddressCategory+Create,2+OM+OrganizationAddressCategory+Create,3+OM+OrganizationAddressCategory+Create,4+OM+OrganizationAddressCategory+Create,5+OM+OrganizationAddressCategory+Create
            if (workerRoleList != null && workerroleactionpermission.IsNotNullOrEmpty())
            {
                string[] workerRolePermissions = workerroleactionpermission.ToStringArray(',');
                if (workerRolePermissions != null)
                {
                    foreach (string currentWorkerRoleActionPermission in workerRolePermissions)
                    {
                        string[] workerRolePermissionParams = currentWorkerRoleActionPermission.ToStringArray('+');
                        if (workerRolePermissionParams.Length == 4 && workerRolePermissionParams[0].ToInteger() < workerRoleList.Count)
                        {
                            WorkerRole workerRole = workerRoleList[workerRolePermissionParams[0].ToInteger()];
                            string areaName = workerRolePermissionParams[1];
                            string controllerName = workerRolePermissionParams[2];
                            string actionName = workerRolePermissionParams[3];
                            WorkerRoleActionPermission permission = context.WorkerRoleActionPermission.SingleOrDefault(item => item.AreaName.ToLower() == areaName && item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && item.WorkerRoleID == workerRole.ID);
                            if (permission == null)
                            {
                                permission = new WorkerRoleActionPermission()
                                {
                                    AreaName = areaName,
                                    ControllerName = controllerName,
                                    ActionName = actionName,
                                    WorkerRoleID = workerRole.ID
                                };
                                context.WorkerRoleActionPermission.Add(permission);
                            }
                        }
                    }
                }
            }
        }

        public void InsertOrUpdate(string menuAttributes, string deletedMenuAccess, int workerRoleID)
        {
            if (workerRoleID == 0)
            {
                throw new CustomException(CustomExceptionType.CommonInvalidArgument, "Please select a role");
            }

            //2+AppBuilder+AppMenu+Index,3+AppBuilder+AppMenu+Index,4+AppBuilder+AppMenu+Index,2+AppBuilder+AppMenu+Search,1+OM+OrganizationAddressCategory+Create,2+OM+OrganizationAddressCategory+Create,3+OM+OrganizationAddressCategory+Create,4+OM+OrganizationAddressCategory+Create,5+OM+OrganizationAddressCategory+Create
            int menuSelected = 0;
            if (menuAttributes.IsNotNullOrEmpty())
            {
                string[] workerRolePermissions = menuAttributes.ToStringArray(',');
                if (workerRolePermissions != null)
                {
                    foreach (string currentWorkerRoleActionPermission in workerRolePermissions)
                    {
                        string[] workerRolePermissionParams = currentWorkerRoleActionPermission.ToStringArray('+');
                        if (workerRolePermissionParams.Length == 3 && workerRoleID > 0)
                        {
                            string areaName = workerRolePermissionParams[0];
                            string controllerName = workerRolePermissionParams[1];
                            string actionName = workerRolePermissionParams[2];
                            if (actionName != "null")
                            {
                                menuSelected++;
                                WorkerRoleActionPermission permission = context.WorkerRoleActionPermission.SingleOrDefault(item => item.AreaName.ToLower() == areaName && item.ControllerName.ToLower() == controllerName && item.ActionName.ToLower() == actionName && item.WorkerRoleID == workerRoleID);
                                if (permission == null)
                                {
                                    permission = new WorkerRoleActionPermission()
                                    {
                                        AreaName = areaName,
                                        ControllerName = controllerName,
                                        ActionName = actionName,
                                        WorkerRoleID = workerRoleID
                                    };
                                    context.WorkerRoleActionPermission.Add(permission);
                                }
                            }
                        }
                    }
                }
            }
            if (menuSelected == 0)
            {
                throw new CustomException(CustomExceptionType.CommonInvalidArgument, "Please select a menu");
            }
        }

    }

    public interface IWorkerRoleActionPermissionRepository : IBaseRepository<WorkerRoleActionPermission>
    {
        void InsertOrUpdate(WorkerRoleActionPermission workerroleactionpermission);
        void InsertOrUpdate(string workerroleactionpermission, List<WorkerRole> workerRoleList);
        void InsertOrUpdate(string menuAttributes, string deletedMenuAccess, int workerRoleID);
        bool HasPermission(List<int> workerRoleIds, string areaName, string controllerName, string actionName, bool useCache = false);
        bool HasPermission(List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, string areaName, string controllerName, string actionName, bool useCache = false);
        bool HasPermission(int caseId, List<int> workerRoleIDs, int workerID, int programID, int regionID, int subProgramID, string areaName, string controllerName, string actionName, bool useCache = false);
        
    }
}