//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class WorkerInRoleNewLVM 
    {
        [Display(Name = "Permission")]
        public string PermissionName { get; set; }

        [Display(Name = "Program")]
        public string ProgramName { get; set; }

        [Display(Name = "Region")]
        public string RegionName { get; set; }

        [Display(Name = "SubProgram")]
        public string SubProgramNames { get; set; }

        [Display(Name = "Jamatkhana")]
        public string JamatkhanaNames { get; set; }
    }
}
