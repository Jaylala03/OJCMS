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
    public class CaseSmartGoalProgress : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select Measurable goal")]
        [Display(Name = "Measurable Goal")]
        [ForeignKey("CaseSmartGoal")]
        public Int32 CaseSmartGoalID { get; set; }

        [Display(Name = "Measurable Goal")]
        [ForeignKey("SmartGoal")]
        public Int32? SmartGoalID { get; set; }

        [Required(ErrorMessage = "Please select progress outcomes")]
        [Display(Name = "Progress Outcomes")]
        [ForeignKey("ServiceLevelOutcome")]
        public Int32 ServiceLevelOutcomeID { get; set; }

        [Required(ErrorMessage = "Please enter progress date")]
        [Display(Name = "Progress Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProgressDate { get; set; }

        [Display(Name = "Comment")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        [System.Web.Mvc.AllowHtml]
        public String Comment { get; set; }

        public virtual CaseSmartGoal CaseSmartGoal { get; set; }
        public virtual SmartGoal SmartGoal { get; set; }
        public virtual ServiceLevelOutcome ServiceLevelOutcome { get; set; }

        [NotMapped]
        [Display(Name = "Progress Outcomes")]
        public string ServiceLevelOutcomeName { get; set; }

        [NotMapped]
        [Display(Name = "Goal Statement")]
        public string SmartGoalName { get; set; }

        [NotMapped]
        public Int32 PendingActionCount { get; set; }

        [NotMapped]
        public Int32 CaseMemberID { get; set; }

        [NotMapped]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public String SmartGoalStartDate { get; set; }

        [NotMapped]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public String SmartGoalEndDate { get; set; }
    }
}