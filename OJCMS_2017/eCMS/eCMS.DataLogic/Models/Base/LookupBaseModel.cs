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

namespace eCMS.DataLogic.Models
{
    public class LookupBaseModel : BaseModel, ILookupBaseModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [StringLength(128)]
        public virtual string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(512)]
        public virtual string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [NotMapped]
        public string RegionName { get; set; }
    }
}
