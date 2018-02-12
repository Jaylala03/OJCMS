//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.Shared;
using System;
using System.Collections.Generic;

namespace eCMS.DataLogic.Seed
{
    public static partial class SeedData
    {
        public static List<WorkerRole> WorkerRoles
        {
            get
            {
                List<WorkerRole> items = new List<WorkerRole>();

                items.Add(new WorkerRole { Name = "ADMINISTRATOR", Description = "Administrator", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new WorkerRole { Name = "SSP", Description = "Social Services Professional", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new WorkerRole { Name = "RC/VC", Description = "Regional and Vice Chair", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new WorkerRole { Name = "SSV/FF", Description = "Case Manager", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new WorkerRole { Name = "SP", Description = "Service Provider", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new WorkerRole { Name = "INTERNAL SERVICE PROVIDER", Description = "Internal Service Provider", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new WorkerRole { Name = "REGIOINAL ADMINISTRATOR", Description = "Regional Administrator", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                return items;
            }
        }

        public static List<WorkerRoleActionPermission> WorkerRoleActionPermission
        {
            get
            {
                List<WorkerRoleActionPermission> items = new List<WorkerRoleActionPermission>();

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Readmit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Delete, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Delete, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.CreateInitialContact, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.InitialContact, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Delete, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 2, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Readmit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.InitialContact, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 3, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 4, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Read, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 4, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 4, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Readmit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 4, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.InitialContact, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 4, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 4, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 4, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Read, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.Case, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseWorker, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseMember, ActionName = Constants.Actions.Index, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Create, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });
                items.Add(new WorkerRoleActionPermission { WorkerRoleID = 5, AreaName = Constants.Areas.CaseManagement, ControllerName = Constants.Controllers.CaseProgressNote, ActionName = Constants.Actions.Edit, LastUpdateDate = DateTime.Now, IsArchived = false, LastUpdatedByWorkerID = 1, CreateDate = DateTime.Now, CreatedByWorkerID = 1 });

                return items;
            }
        }

        public static List<Worker> Workers
        {
            get
            {
                List<Worker> items = new List<Worker>();

                items.Add(new Worker { FirstName = "eCMS", LastName = "Administrator", Password = CryptographyHelper.Encrypt("admin123#@"), EmailAddress = "administrator@ecms.com", LoginName = "administrator@ecms.com", IsActive = true, AllowLogin = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                return items;
            }
        }
    }
}



























