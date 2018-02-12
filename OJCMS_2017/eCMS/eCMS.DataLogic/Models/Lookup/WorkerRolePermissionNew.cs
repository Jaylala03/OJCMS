//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.Models.Lookup
{
    public class WorkerRolePermissionNew : BaseModel
    {
        [ForeignKey("WorkerRole")]
        [Display(Name = "Worker Role")]
        public Int32 WorkerRoleID { get; set; }

         [ForeignKey("Permission")]
        [Display(Name = "Permission")]
        public Int32 PermissionID { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public virtual WorkerRole WorkerRole { get; set; }
        public virtual Permission Permission { get; set; }

        [NotMapped]
        [Display(Name = "Role")]
        public string WorkerRoleName { get; set; }

        [NotMapped]
        [Display(Name = "Permission")]
        public string PermissionName { get; set; }
    }
}
