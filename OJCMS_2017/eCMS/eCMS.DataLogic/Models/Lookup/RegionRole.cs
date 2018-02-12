//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.Models.Lookup
{
    public class RegionRole : BaseModel
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int ID
        {
            get;
            set;
        }

        [Display(Name = "Region")]
        [ForeignKey("Region")]
        public Int32 RegionID { get; set; }

        [Display(Name = "Role")]
        [ForeignKey("WorkerRole")]
        public Int32 WorkerRoleID { get; set; }

        public virtual WorkerRole WorkerRole { get; set; }
        public virtual Region Region { get; set; }
    }
}
