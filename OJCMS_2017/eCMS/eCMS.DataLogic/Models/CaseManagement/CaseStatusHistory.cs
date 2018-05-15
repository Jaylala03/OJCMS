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
    public class CaseStatusHistory : EntityBaseModel
    {
        public int CaseID { get; set; }

        [Display(Name = "Reason")]
        public int? ReasonID { get; set; }

        [Display(Name = "Status")]
        public int StatusID { get; set; }

        [Display(Name = "Case Status")]
        public string CaseStatus { get; set; }

        [Display(Name = "Justification")]
        public string Justification { get; set; }

        [NotMapped]
        public int CurrentStatusID { get; set; }

        [NotMapped]
        public CaseHouseholdIncome CaseHouseholdIncome { get; set; }

        [NotMapped]
        public CaseWorkerNote CaseWorkerNote { get; set; }
    }
}
