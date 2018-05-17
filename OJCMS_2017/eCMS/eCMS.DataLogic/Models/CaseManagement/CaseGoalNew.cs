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
using System.Web.Mvc;

namespace eCMS.DataLogic.Models
{
    public class CaseGoalNew : EntityBaseModel
    {
        public Int32 CaseID { get; set; }

        [NotMapped]
        [Display(Name = "ACTIONS")]
        public string Actions { get; set; }

        [Required(ErrorMessage = "Please enter goal details")]
        [Display(Name = "Goal Details")]
        [MaxLength]
        [System.Web.Mvc.AllowHtml]
        public string GoalDetail { get; set; }

        //[NotMapped]
        [Display(Name = "Family")]
        public bool IsFamily { set; get; }

        //[NotMapped]
        [Display(Name = "Family Member")]
        public bool IsFamilyMember { set; get; }

        [Display(Name = "")]
        [ForeignKey("CaseMember")]
        public Int32? CaseMemberID { get; set; }

        public virtual CaseMember CaseMember { get; set; }

        [NotMapped]
        public string Family { set; get; }

        [Display(Name = "Education")]
        public bool Education { get; set; }

        [Display(Name = "Family has reviewed and agreed with the Assessment")]
        public bool CaseAssessmentReviewed { get; set; }

        [Display(Name = "Income & Livelihood")]
        public bool IncomeLivelihood { get; set; }

        [Display(Name = "Assets & Life Skills")]
        public bool Assets { get; set; }

        [Display(Name = "Housing")]
        public bool Housing { get; set; }

        [Display(Name = "Social Support")]
        public bool SocialSupport { get; set; }

        [Display(Name = "Dignity & Self Respect")]
        public bool Dignity { get; set; }

        [Display(Name = "Health")]
        public bool Health { get; set; }

        [Required(ErrorMessage = "Please select priority")]
        [Display(Name = "Priority set by family")]
        //[ForeignKey("RiskType")]
        public Int32 PriorityTypeID { get; set; }

        [Required(ErrorMessage = "Please confirm family / family member agreement to the goal.")]
        [Display(Name = "Family / Family member has agreed to the Goal")]
        public bool FamilyAgreeToGoal { get; set; }

        [Display(Name = "Goal Status")]
        [ForeignKey("GoalStatus")]
        public Int32? GoalStatusID { get; set; }

        public virtual GoalStatus GoalStatus { get; set; }

        [NotMapped]
        public string CaseMemberName { get; set; }

        [NotMapped]
        public string Indicators { get; set; }

        [NotMapped]
        public CaseWorkerNote CaseWorkerNote { get; set; }

        [NotMapped]
        public GoalActionWorkNote GoalActionWorkNote { get; set; }

        [NotMapped]
        public CaseActionNew CaseActionNew { get; set; }
    }
}
