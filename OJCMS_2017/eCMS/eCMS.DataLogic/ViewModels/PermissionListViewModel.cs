//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class PermissionListViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsActive { get; set; }
        public string Options { get; set; }
        public List<PermissionRegion> PermissionRegionList { get; set; }
        public string PermissionPrograms { get; set; }
        public string PermissionSubPrograms { get; set; }
        public string PermissionJamatkhanas { get; set; }
        public string PermissionRegions { get; set; }
    }
}
