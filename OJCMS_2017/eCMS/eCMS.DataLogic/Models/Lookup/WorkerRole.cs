//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System.ComponentModel.DataAnnotations;
namespace eCMS.DataLogic.Models.Lookup
{
    public class WorkerRole : LookupBaseModel
    {
        [Display(Name = "Is Regional Admin")]
        public bool IsRegionalAdmin { get; set; }
    }
}
