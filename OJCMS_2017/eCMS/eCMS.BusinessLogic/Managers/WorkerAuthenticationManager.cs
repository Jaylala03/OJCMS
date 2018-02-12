//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using System;
using System.Collections.Generic;

namespace eCMS.BusinessLogic.Helpers
{
    public class WorkerAuthenticationManager
    {
        private IWorkerRepository workerRepository;
        private IWorkerRolePermissionRepository workerRolePermissionRepository;
        private IWorkerRolePermissionNewRepository workerRolePermissionNewRepository;
        private IWorkerInRoleRepository workerinroleRepository;
        private IWorkerInRoleNewRepository workerinrolenewRepository;
        public WorkerAuthenticationManager(IWorkerRepository workerRepository, 
            IWorkerRolePermissionRepository workerRolePermissionRepository,
            IWorkerRolePermissionNewRepository workerRolePermissionNewRepository,
            IWorkerInRoleRepository workerinroleRepository, IWorkerInRoleNewRepository workerinrolenewRepository)
        {
            this.workerRepository = workerRepository;
            this.workerRolePermissionRepository = workerRolePermissionRepository;
            this.workerRolePermissionNewRepository = workerRolePermissionNewRepository;
            this.workerinroleRepository = workerinroleRepository;
            this.workerinrolenewRepository = workerinrolenewRepository;
        }

        public Worker AuthenticateWorker(string userName, string password, bool rememberMe, bool isExternalLogin=false)
        {
            try
            {
                if (userName.IsNullOrEmpty())
                {
                    throw new CustomException(CustomExceptionType.CommonArgumentNullException, "Enter user name");
                }
                if (password.IsNullOrEmpty())
                {
                    throw new CustomException(CustomExceptionType.CommonArgumentNullException, "Enter password");
                }
                string errorMessage = string.Empty;
                if (workerRepository!=null)
                {
                    string originalPassword = password;
                    password = CryptographyHelper.Encrypt(password);
                    Worker loggedInWorker = workerRepository.Find(userName, password);
                    if (loggedInWorker != null)
                    {
                        if (loggedInWorker.IsActive)
                        {
                            if (loggedInWorker.AllowLogin)
                            {
                                loggedInWorker.ConfirmPassword = loggedInWorker.Password;
                                loggedInWorker.LastLoginDate = DateTime.Now;
                                workerRepository.InsertOrUpdate(loggedInWorker);
                                workerRepository.Save();
                                if (!isExternalLogin)
                                {
                                    WebHelper.CurrentSession.Content.LoggedInWorker = loggedInWorker;
                                    List<int> roleIDs = null;
                                    List<int> regionIDs = null;

                                    roleIDs = workerinrolenewRepository.FindAllActiveWorkerInRoleByWorkerID();
                                    regionIDs = workerinrolenewRepository.FindAllActiveRegionByWorkerID();
                                    //List<WorkerInRole> workerRoles = workerinroleRepository.FindAllActiveByWorkerID(loggedInWorker.ID);

                                    //if (workerRoles != null)
                                    //{
                                    //    foreach (WorkerInRole workerRole in workerRoles)
                                    //    {
                                    //        if (!roleIDs.Contains(workerRole.WorkerRoleID.ToString()))
                                    //        {
                                    //            roleIDs = roleIDs.Concate(',', workerRole.WorkerRoleID.ToString());
                                    //        }
                                    //        if (!regionIDs.Contains(workerRole.RegionID.ToString()))
                                    //        {
                                    //            regionIDs = regionIDs.Concate(',', workerRole.RegionID.ToString());
                                    //        }
                                    //    }
                                    //}

                                    if (roleIDs == null)
                                    {
                                        throw new CustomException(CustomExceptionType.CommonArgumentNullException, "There is no role assigned to the user");
                                    }
                                    WebHelper.CurrentSession.Content.LoggedInWorkerRoleIDs = roleIDs;
                                    WebHelper.CurrentSession.Content.LoggedInWorkerRegionIDs = regionIDs;
                                    VisibilityStatus regionVisiblity = VisibilityStatus.UnDefined;
                                    VisibilityStatus programVisiblity = VisibilityStatus.UnDefined;
                                    VisibilityStatus subProgramVisiblity = VisibilityStatus.UnDefined;
                                    VisibilityStatus caseVisiblity = VisibilityStatus.UnDefined;
                                    //workerRolePermissionRepository.FindVisiblity(loggedInWorker.ID, ref regionVisiblity, ref programVisiblity, ref subProgramVisiblity, ref caseVisiblity);
                                    workerRolePermissionNewRepository.FindVisiblity(loggedInWorker.ID, ref regionVisiblity, ref programVisiblity, ref subProgramVisiblity, ref caseVisiblity);
                                    WebHelper.CurrentSession.Content.RegionVisibility = regionVisiblity;
                                    WebHelper.CurrentSession.Content.ProgramVisibility = programVisiblity;
                                    WebHelper.CurrentSession.Content.SubProgramVisibility = subProgramVisiblity;
                                    WebHelper.CurrentSession.Content.CaseVisibility = caseVisiblity;

                                    CookieHelper newCookieHelper = new CookieHelper();
                                    newCookieHelper.SetLoginCookie(userName, loggedInWorker.ID.ToString(), rememberMe);
                                    if (rememberMe)
                                    {
                                        newCookieHelper.RememberMe(userName, originalPassword);
                                    }
                                    else
                                    {
                                        newCookieHelper.ForgetMe();
                                    }
                                }
                                //loggedInWorker = setUserPermission(loggedInWorker);
                                return loggedInWorker;
                            }
                            else
                            {
                                throw new CustomException(CustomExceptionType.CommonArgumentNullException, "User access has been blocked by administrator");
                            }
                        }
                        else
                        {
                            throw new CustomException(CustomExceptionType.CommonArgumentNullException, "User has not been activated yet");
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomExceptionType.CommonArgumentNullException, "Invalid user name/password");
                    }
                }
                return null;
            }
            catch (CustomException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CustomException(CustomExceptionType.UserLoginUnknownError, Constants.Messages.UserLogin_UnknownError, ex);
            }
        }

        private Worker setUserPermission(Worker user)
        {
            int priorityRole = 100000000;
            List<int> userRole = workerRepository.setProgramRole(user.ID);
            foreach (int each in userRole)
            {
                if (each < priorityRole)
                    priorityRole = each; 
            }
            if (priorityRole == 1)
            {
                user.IsRegionRestricted = true;
                user.IsProgramRestricted = true;
                user.IsCaseRestricted = true;
            }
            else if (priorityRole == 2)
            {
                user.IsRegionRestricted = true;
                user.IsProgramRestricted = true;
                user.IsCaseRestricted = false;
            }
            else if (priorityRole == 3)
            {
                user.IsRegionRestricted = true;
                user.IsProgramRestricted = false;
                user.IsCaseRestricted = true;
            }
            else if (priorityRole == 4)
            {
                user.IsRegionRestricted = true;
                user.IsProgramRestricted = false;
                user.IsCaseRestricted = false;
            }
            else if (priorityRole == 5)
            {
                user.IsRegionRestricted = false;
                user.IsProgramRestricted = true;
                user.IsCaseRestricted = true;
            }
            else if (priorityRole == 6)
            {
                user.IsRegionRestricted = false;
                user.IsProgramRestricted = true;
                user.IsCaseRestricted = false;
            }
            else if (priorityRole == 7)
            {
                user.IsRegionRestricted = false;
                user.IsProgramRestricted = false;
                user.IsCaseRestricted = true;
            }
            else if (priorityRole == 8)
            {
                user.IsRegionRestricted = false;
                user.IsProgramRestricted = false;
                user.IsCaseRestricted = false;
            }
            return user;
        }

        public Worker GetSavedWorkerDetail()
        {
            CookieHelper newCookieHelper = new CookieHelper();
            string userData=newCookieHelper.GetWorkerDataFromLoginCookie();
            if (userData.IsNotNullOrEmpty())
            {
                return workerRepository.Find(userData.ToInteger(true));
            }
            return null;
        }

        public Worker GetSavedWorkerNameAndPassword()
        {
            CookieHelper newCookieHelper = new CookieHelper();
            string userName = string.Empty;
            string password = string.Empty;
            newCookieHelper.GetWorkerDataFromRememberMeCookie(ref userName, ref password);
            Worker newWorker = new Worker();
            newWorker.EmailAddress = userName;
            newWorker.Password = password;
            return newWorker;
        }
    }
}
