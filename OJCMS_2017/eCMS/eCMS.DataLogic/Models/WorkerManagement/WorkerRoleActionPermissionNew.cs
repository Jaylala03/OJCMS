//*********************************************************
//
//    Copyright (c) JobSlice LLC. All rights reserved.
//	  Technical Contact: Alain Templeman, info@jobslice.com
//	  http://www.jobslice.com
//
//*********************************************************

using eCMS.DataLogic.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public class WorkerRoleActionPermissionNew : EntityBaseModel
    {
        [Display(Name = "WorkerRole")]
        [ForeignKey("WorkerRole")]
        public Int32 WorkerRoleID { get; set; }

        [Required]
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }

        [Required]
        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }

        [Required]
        [Display(Name = "Action or Method Name")]
        public string ActionMethodName { get; set; }

        public virtual WorkerInRoleNew WorkerRoleNew { get; set; }
    }
}
