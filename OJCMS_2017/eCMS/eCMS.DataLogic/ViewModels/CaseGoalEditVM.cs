//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************


using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class CaseGoalEditVM : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select priority")]
        [Display(Name = "Priority set by family:")]
        [ForeignKey("RiskType")]
        public Int32 PriorityTypeID { get; set; }

        public virtual RiskType RiskType { get; set; }

        [Display(Name = "Goal Status")]
        [ForeignKey("GoalStatus")]
        public Int32? GoalStatusID { get; set; }

        public string CaseMemberName { get; set; }

        public string GoalDetail { get; set; }

        public string Indicators { get; set; }

        public virtual GoalStatus GoalStatus { get; set; }

        public GoalActionWorkNote GoalActionWorkNote { get; set; }

        public CaseActionNew CaseActionNew { get; set; }

    }
}
