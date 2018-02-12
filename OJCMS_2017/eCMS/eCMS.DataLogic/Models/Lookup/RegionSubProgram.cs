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
    public class RegionSubProgram : BaseModel
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

        [Display(Name = "Sub Program")]
        [ForeignKey("SubProgram")]
        public Int32 SubProgramID { get; set; }

        public virtual SubProgram SubProgram { get; set; }
        public virtual Region Region { get; set; }
    }
}
