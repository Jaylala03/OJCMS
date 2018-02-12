//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class RegionSubProgramModel:BaseModel
    {
        [Display(Name = "Program")]
        public Int32 ProgramID { get; set; }

        [Display(Name = "Program")]
        public String ProgramName { get; set; }

        [Display(Name = "Sub-Program")]
        public Int32 SubProgramID { get; set; }

        [Display(Name = "Sub-Program")]
        public String SubProgramName { get; set; }

        [Display(Name="Regions")]
        public String RegionNames { get; set; }

        [NotMapped]
        public List<RegionSubProgram> AssignedRegions { get; set; }

        [NotMapped]
        public List<Region> AllRegions { get; set; }
    }
}
