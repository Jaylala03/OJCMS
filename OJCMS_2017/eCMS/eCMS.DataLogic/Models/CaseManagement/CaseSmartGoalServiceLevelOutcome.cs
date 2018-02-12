//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public class CaseSmartGoalServiceLevelOutcome : EntityBaseModel
    {
        [ForeignKey("CaseSmartGoal")]
        public Int32 CaseSmartGoalID { get; set; }

        [Display(Name = "Service Level Outcome")]
        [ForeignKey("ServiceLevelOutcome")]
        public Int32 ServiceLevelOutcomeID { get; set; }

        public virtual CaseSmartGoal CaseSmartGoal { get; set; }
        public virtual ServiceLevelOutcome ServiceLevelOutcome { get; set; }

        [NotMapped]
        [Display(Name = "Service Level Outcome")]
        public String ServiceLevelOutcomeName { get; set; }
    }
}