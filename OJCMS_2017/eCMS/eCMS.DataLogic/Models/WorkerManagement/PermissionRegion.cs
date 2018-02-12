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
namespace eCMS.DataLogic.Models
{
    public class PermissionRegion : EntityBaseModel
    {
        public PermissionRegion()
        {
            AssignedSubPrograms = new List<PermissionSubProgram>();
            AssignedJamatkhanas = new List<PermissionJamatkhana>();
            SubProgramNames = string.Empty;
            JamatkhanaNames = string.Empty;
        }
        
        [Index("IX_Unique", 1, IsUnique = true)]
        [Display(Name = "Permission")]
        [ForeignKey("Permission")]
        public Int32 PermissionID { get; set; }

        [Display(Name = "Program")]
        [ForeignKey("Program")]
        [Index("IX_Unique", 2, IsUnique = true)]
        public Int32 ProgramID { get; set; }

        [Display(Name = "Region")]
        [ForeignKey("Region")]
        [Index("IX_Unique", 3, IsUnique = true)]
        public Int32 RegionID { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Program Program { get; set; }
        public virtual Region Region { get; set; }

        [NotMapped]
        [Display(Name = "Sub-Program")]
        public string[] SubProgramIDs { get; set; }

        [NotMapped]
        [Display(Name = "Jamatkhana")]
        public string[] JamatkhanaIDs { get; set; }

        [NotMapped]
        [Display(Name = "PermissionName")]
        public string PermissionName { get; set; }

        [NotMapped]
        [Display(Name = "Program")]
        public string ProgramName { get; set; }

        [NotMapped]
        [Display(Name = "Region")]
        public string RegionName { get; set; }

        [NotMapped]
        [Display(Name = "Sub-Program")]
        public string SubProgramNames { get; set; }

        [NotMapped]
        public List<SubProgram> AllSubPrograms { get; set; }

        [NotMapped]
        [Display(Name = "Jamatkhana")]
        public string JamatkhanaNames { get; set; }

        [NotMapped]
        public List<Jamatkhana> AllJamatkhanas { get; set; }

        //[NotMapped]
        //public List<PermissionSubProgram> AssignedSubPrograms { get; set; }
        public virtual ICollection<PermissionSubProgram> AssignedSubPrograms { get; set; }

        public virtual ICollection<PermissionJamatkhana> AssignedJamatkhanas { get; set; }
    }
}
