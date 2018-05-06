//*********************************************************
//
//    Copyright � Organized Chaos Technologies Inc. 2015 All rights reserved.
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
    public class CaseActionNew : EntityBaseModel
    {
        [Display(Name = "Case Goal")]
        [ForeignKey("CaseGoal")]
        public Int32 CaseGoalID { get; set; }

        [Display(Name = "Assignee Role")]
        [ForeignKey("GoalAssigneeRole")]
        public Int32 GoalAssigneeRoleID { get; set; }

        [Display(Name = "Family Member")]
        [ForeignKey("CaseMember")]
        public Int32? CaseMemberID { get; set; }

        [Display(Name = "Subject Matter Expert")]
        [ForeignKey("CaseWorker")]
        public Int32? WorkerID { get; set; }

        [Display(Name = "Service Provider")]
        [ForeignKey("ServiceProvider")]
        public Int32? ServiceProviderID { get; set; }

        [Required(ErrorMessage = "Please enter action plan")]
        [Display(Name = "Action Plan")]
        [MaxLength]
        [System.Web.Mvc.AllowHtml]
        public string ActionDetail { get; set; }

        [Display(Name = "Action Status")]
        [ForeignKey("ActionStatus")]
        public Int32? ActionStatusID { get; set; }

        [Display(Name = "Other")]
        public string AssigneeOther { set; get; }

        public virtual GoalAssigneeRole GoalAssigneeRole { get; set; }
        public virtual GoalStatus ActionStatus { get; set; }

        public virtual CaseWorker CaseWorker { get; set; }
        public virtual CaseMember CaseMember { get; set; }
        public virtual CaseGoalNew CaseGoal { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }

        [NotMapped]
        [Display(Name = "Case ID")]
        public Int32 CaseID { set; get; }

        [NotMapped]
        [Display(Name = "Family Case")]
        public string CaseDisplayID { set; get; }

        [NotMapped]
        [Display(Name = "Goal Status")]
        public Int32 GoalStatusID { get; set; }

        [NotMapped]
        [Display(Name = "Priority set by family")]
        public Int32 PriorityTypeID { get; set; }

        [NotMapped]
        [Display(Name = "AssigneeRole")]
        public string AssigneeRole { set; get; }

        [NotMapped]
        [Display(Name = "AssignedTo")]
        public string AssignedTo { set; get; }

        [NotMapped]
        public bool HasPermissionToCreate { get; set; }
        [NotMapped]
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public bool HasPermissionToReadmit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
    }
}