//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.Models
{
    public class WorkerSubProgram : EntityBaseModel
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int ID
        {
            get;
            set;
        }

        [Display(Name = "Worker")]
        [ForeignKey("WorkerInRole")]
        public Int32 WorkerInRoleID { get; set; }

        [Display(Name = "Sub Program")]
        [ForeignKey("SubProgram")]
        public Int32 SubProgramID { get; set; }

        public virtual WorkerInRole WorkerInRole { get; set; }
        public virtual SubProgram SubProgram { get; set; }

        [NotMapped]
        public int[] SubProgramIDList { get; set; }
    }
}
