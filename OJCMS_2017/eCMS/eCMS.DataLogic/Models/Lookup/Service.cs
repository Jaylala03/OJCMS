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
    public class Service : LookupBaseModel
    {
        [Display(Name = "Service Provider")]
        [ForeignKey("ServiceProvider")]
        public Int32 ServiceProviderID { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }

        [NotMapped]
        [Display(Name = "Service Provider")]
        public String ServiceProviderName { get; set; }

        [NotMapped]
        [Display(Name = "Region")]
        public String RegionName { get; set; }

        [NotMapped]
        [Display(Name = "Service Type")]
        public Int32 ServiceTypeID { get; set; }

        [NotMapped]
        [Display(Name = "Region")]
        public int? RegionID { get; set; }
    }
}
