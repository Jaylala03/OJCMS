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
    public class State : LookupBaseModel
    {
        [ForeignKey("Country")]
        [Display(Name = "Country Name")]
        public Int32 CountryID { get; set; }

        [Display(Name = "State Code")]
        [StringLength(16)]
        public String Code { get; set; }

        public virtual Country Country { get; set; }

        [NotMapped]
        public string CountryName { get; set; }
    }
}
