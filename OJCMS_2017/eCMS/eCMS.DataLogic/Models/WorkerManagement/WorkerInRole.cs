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
    public class WorkerInRole : EntityBaseModel
    {
        [Index("IX_Unique", 1, IsUnique = true)]
        [Display(Name = "Worker")]
        [ForeignKey("Worker")]
        public Int32 WorkerID { get; set; }

        [Index("IX_Unique", 2, IsUnique = true)]
        [Display(Name = "Role")]
        [ForeignKey("WorkerRole")]
        public Int32 WorkerRoleID { get; set; }

        [Display(Name = "Program")]
        [ForeignKey("Program")]
        [Index("IX_Unique", 3, IsUnique = true)]
        public Int32 ProgramID { get; set; }

        [Display(Name = "Region")]
        [ForeignKey("Region")]
        [Index("IX_Unique", 4, IsUnique = true)]
        public Int32 RegionID { get; set; }

        [Required(ErrorMessage="Please enter effective from date")]
        [Display(Name = "Effective From")]
        [DataType(DataType.Date)]
        public DateTime EffectiveFrom { get; set; }

        [Required(ErrorMessage = "Please enter effective to date")]
        [Display(Name = "Effective To")]
        [DataType(DataType.Date)]
        public DateTime EffectiveTo { get; set; }

        public virtual Worker Worker { get; set; }
        public virtual WorkerRole WorkerRole { get; set; }
        public virtual Program Program { get; set; }
        public virtual Region Region { get; set; }

        [NotMapped]
        [Display(Name = "Sub-Program")]
        public string[] SubProgramIDs { get; set; }

        [NotMapped]
        [Display(Name = "Worker")]
        public string WorkerName { get; set; }

        [NotMapped]
        [Display(Name = "Role")]
        public string WorkerRoleName { get; set; }

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
        public List<WorkerSubProgram> AssignedSubPrograms { get; set; }
    }
}
