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
    public class PermissionAction : EntityBaseModel
    {

        [Index("IX_Unique", 1, IsUnique = true)]
        [Display(Name = "Permission")]
        [ForeignKey("Permission")]
        public Int32 PermissionID { get; set; }

        [Display(Name = "ActionMethod")]
        [ForeignKey("ActionMethod")]
        [Index("IX_Unique", 2, IsUnique = true)]
        public Int32 ActionMethodID { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual ActionMethod ActionMethod { get; set; }

        [NotMapped]
        [Display(Name = "PermissionName")]
        public string PermissionName { get; set; }

        [NotMapped]
        [Display(Name = "ActionMethod")]
        public string ActionMethodName { get; set; }
       
        [NotMapped]
        public int[] ActionMethodIDList { get; set; }
    }
}
