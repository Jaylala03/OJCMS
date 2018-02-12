//*********************************************************
//
//    Copyright � Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.Models.Lookup
{
    public class ServiceProvider : LookupBaseModel
    {
        [Display(Name = "Region")]
        [ForeignKey("Region")]
        public Int32? RegionID { get; set; }

        public Boolean IsExternal { get; set; }

        public virtual Region Region { get; set; }
    }
}
