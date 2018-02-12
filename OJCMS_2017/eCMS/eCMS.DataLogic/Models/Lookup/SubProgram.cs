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
    public class SubProgram : LookupBaseModel
    {
        [Display(Name = "Program Base")]
        [StringLength(1)]
        public String ProgramBase { get; set; }

        [Display(Name = "Program Email")]
        public bool ProgramEmail { get; set; }

        [Display(Name = "Program Member")]
        [StringLength(1)]
        public String ProgramMember { get; set; }

        [Display(Name = "Program")]
        [ForeignKey("Program")]
        public Int32 ProgramID { get; set; }

        public virtual Program Program { get; set; }

        [Display(Name = "Program Name")]
        [NotMapped]
        public String ProgramName { get; set; }
    }
}
