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
    public class WorkRolePermissionNewViewModel
    {
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public bool IsActive { get; set; }
        
        [Display(Name = "Role")]
        public String WorkerRoleName { get; set; }

      
        [Display(Name = "Permission")]
        public String PermissionName { get; set; }

        public int PermissionID { get; set; }

        public int WorkerRoleID { get; set; }

      

    }
}
