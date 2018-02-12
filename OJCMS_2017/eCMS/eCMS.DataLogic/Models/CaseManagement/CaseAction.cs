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
    public class CaseAction : EntityBaseModel
    {
        [Display(Name = "Progress Note")]
        [ForeignKey("CaseProgressNote")]
        public Int32? CaseProgressNoteID { get; set; }

        [Display(Name = "Smart Goal")]
        [ForeignKey("CaseSmartGoal")]
        public Int32? CaseSmartGoalID { get; set; }

        [Display(Name = "Service Provider")]
        [ForeignKey("CaseSmartGoalServiceProvider")]
        public Int32? CaseSmartGoalServiceProviderID { get; set; }

        [Display(Name = "By Whom")]
        [ForeignKey("CaseWorker")]
        public Int32? CaseWorkerID { get; set; }


        [Display(Name = "By Whom")]
        [NotMapped]
        public string CaseMemberWorkerID { get; set; }


        [Display(Name = "Family Member")]
        [ForeignKey("CaseMember")]
        public Int32? CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please enter action plan")]
        [Display(Name = "Action Plan")]
        [MaxLength]
        [System.Web.Mvc.AllowHtml]
        public string Action { get; set; }

        [Required(ErrorMessage = "Please enter start date")]
        [Display(Name = "Target Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActionStartTime { get; set; }

        [Required(ErrorMessage = "Please enter end date")]
        [Display(Name = "Target End Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActionEndTime { get; set; }

        [Display(Name = "Is Completed")]
        public bool IsCompleted { get; set; }

        public virtual CaseProgressNote CaseProgressNote { get; set; }
        public virtual CaseSmartGoal CaseSmartGoal { get; set; }
        public virtual CaseWorker CaseWorker { get; set; }
        public virtual CaseMember CaseMember { get; set; }
        public virtual CaseSmartGoalServiceProvider CaseSmartGoalServiceProvider { get; set; }

        [NotMapped]
        [Display(Name = "By Whom")]
        public string CaseWorkerName { set; get; }

        [NotMapped]
        [Display(Name = "Family or Family Member")]
        public string CaseMemberName { set; get; }

        [NotMapped]
        [Display(Name = "Service Provider")]
        public string ServiceProviderName { set; get; }

        [NotMapped]
        [Display(Name = "Case ID")]
        public Int32 CaseID { set; get; }

        //[NotMapped]
        //[Display(Name = "Case Action ID")]
        //public Int32 CaseActionID { set; get; }

        [NotMapped]
        [Display(Name = "Family Case")]
        public string CaseDisplayID { set; get; }

        [NotMapped]
        [Display(Name = "Program")]
        public string CaseProgramName { set; get; }

        [NotMapped]
        public bool IsActionTakenByServiceProvider { get; set; }

        [NotMapped]
        [Display(Name = "Family Member")]
        [Required(ErrorMessage = "Please select family member")]
        public int CaseMemberIds { get; set; }
    }
}
