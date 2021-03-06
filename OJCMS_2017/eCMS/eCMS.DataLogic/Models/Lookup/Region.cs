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
    public class Region : LookupBaseModel
    {
        [Display(Name = "Code")]
        [StringLength(128)]
        public String Code { get; set; }

        [Display(Name = "Country")]
        [ForeignKey("Country")]
        public Int32 CountryID { get; set; }

        public virtual Country Country { get; set; }
    }
}
