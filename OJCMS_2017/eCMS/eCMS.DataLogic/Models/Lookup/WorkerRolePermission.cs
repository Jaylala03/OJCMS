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
    public class WorkerRolePermission : BaseModel
    {
        [ForeignKey("WorkerRole")]
        [Display(Name = "Worker Role")]
        public Int32 WorkerRoleID { get; set; }

        [Display(Name = "Permission")]
        public Int32 Permission { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public virtual WorkerRole WorkerRole { get; set; }

        [NotMapped]
        public string WorkerRoleName { get; set; }
    }
}
