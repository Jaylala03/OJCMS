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
    public class CaseGoalNewVM : EntityBaseModel
    {
        public List<InitialAssessmentIndicatorsVM> AssesmentIndicators { get; set; }
        public List<CaseInitialAssessmentVM> CaseInitialAssessment { get; set; }
        public CaseHouseholdIncome CaseHouseholdIncome { get; set; }
        [NotMapped]
        public CaseWorkerNote CaseWorkerNote { get; set; }
        public int CaseID { get; set; }

        //[NotMapped]
        [Display(Name = "Family")]
        public bool IsFamily { set; get; }

        //[NotMapped]
        [Display(Name = "Family Member")]
        public bool IsFamilyMember { set; get; }

        [NotMapped]
        public string Family { set; get; }

        [Display(Name = "")]
        [ForeignKey("CaseMember")]
        public Int32? CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please enter goal details")]
        [Display(Name = "Goal Details")]
        [MaxLength]
        [System.Web.Mvc.AllowHtml]
        public string GoalDetail { get; set; }

        [Display(Name = "Priority set by family:")]
        //[ForeignKey("RiskType")]
        public Int32 PriorityTypeID { get; set; }

        [Display(Name = "Goal Status:")]
        public Int32 GoalStatusID { get; set; }

        [Display(Name = "Education")]
        public bool Education { get; set; }

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

        [NotMapped]
        public GoalActionWorkNote GoalActionWorkNote { get; set; }
    }
}
