//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.Models
{
    public class Permission : LookupBaseModel
    {
        public Permission()
        {
            PermissionRegions = new List<PermissionRegion>();
            AssignedPermissionActions = new List<PermissionAction>();
        }

        [Display(Name = "All Cases")]
        public bool IsAllCases { get; set; }

        //[Display(Name = "All Regions")]
        //public bool IsAllRegions { get; set; }

        [NotMapped]
        [Display(Name = "Program")]
        public int ProgramID { get; set; }

        [NotMapped]
        [Display(Name = "Region")]
        public int RegionID { get; set; }

        [NotMapped]
        public PermissionRegion PermissionRegion { get; set; }
        [NotMapped]
        public PermissionAction PermissionAction { get; set; }
        public virtual ICollection<PermissionRegion> PermissionRegions { get; set; }
        public virtual ICollection<PermissionAction> AssignedPermissionActions { get; set; }

        [NotMapped]
        public List<ActionMethod> AllActionMethods { get; set; }

        [NotMapped]
        [Display(Name = "Action Permission")]
        public string[] ActionMethodIDs { get; set; }
    }
}
