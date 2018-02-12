//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class WorkerListViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string LoginName { get; set; }
        public bool AllowLogin { get; set; }
        public int? CreatedByworkerID { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsActive { get; set; }
        public string Options { get; set; }
        public List<WorkerInRole> WorkerInRoleList { get; set; }

        public List<WorkerInRoleNew> WorkerInRoleNewList { get; set; }
        public string WorkerPrograms { get; set; }
        public string WorkerSubPrograms { get; set; }
        public string WorkerRegions { get; set; }
        public string WorkerRoles { get; set; }

        public string HasPermissionToCreate { get; set; }
        public string HasPermissionToEdit { get; set; }
        public string HasPermissionToReadmit { get; set; }
        public string HasPermissionToDelete { get; set; }
    }
}
