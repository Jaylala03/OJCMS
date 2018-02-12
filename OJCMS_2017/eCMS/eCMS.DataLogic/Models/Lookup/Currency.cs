//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.ComponentModel.DataAnnotations;

namespace eCMS.DataLogic.Models.Lookup
{
    public class Currency : LookupBaseModel
    {
        [Display(Name = "Code")]
        [StringLength(16)]
        public String Code { get; set; }

        [Display(Name = "Symbol")]
        [StringLength(2)]
        public String Symbol { get; set; }
    }
}
