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
    public class PermissionJamatkhana : EntityBaseModel
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int ID
        {
            get;
            set;
        }

        [Display(Name = "Permission")]
        [ForeignKey("PermissionRegion")]
        public Int32 PermissionRegionID { get; set; }

        [Display(Name = "Jamatkhana")]
        [ForeignKey("Jamatkhana")]
        public Int32 JamatkhanaID { get; set; }

        public virtual PermissionRegion PermissionRegion { get; set; }
        public virtual Jamatkhana Jamatkhana { get; set; }

        [NotMapped]
        public int[] JamatkhanaIDList { get; set; }
    }
}
