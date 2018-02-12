//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.Shared.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models.Lookup
{
    public class City : LookupBaseModel
    {
        [Display(Name = "Country")]
        [ForeignKey("Country")]
        [IncludeInList(Sequence = 2, AllowSearch = true, Width = 30, AllowWidthInPercentage = true)]
        public Int32? CountryID { get; set; }

        [Display(Name = "State")]
        [ForeignKey("State")]
        [IncludeInList(Sequence = 3, AllowSearch = true, Width = 30, AllowWidthInPercentage = true)]
        public Int32? StateID { get; set; }

        [Display(Name = "Population")]
        public Int32? Population { get; set; }

        public virtual State State { get; set; }
        public virtual Country Country { get; set; }

        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string StateName { get; set; }
    }
}
