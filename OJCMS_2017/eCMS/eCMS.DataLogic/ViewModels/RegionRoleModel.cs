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
    public class RegionRoleModel:BaseModel
    {
        [Display(Name = "Role")]
        public Int32 WorkerRoleID { get; set; }

        [Display(Name = "Role")]
        public String WorkerRoleName { get; set; }

        [Display(Name="Regions")]
        public String RegionNames { get; set; }

        [Display(Name = "Action")]
        public String Action { get; set; }

        [NotMapped]
        public List<RegionRole> AssignedRegions { get; set; }

        [NotMapped]
        public List<Region> AllRegions { get; set; }
    }
}
