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
    public class CaseSmartGoal : EntityBaseModel
    {
        [ForeignKey("CaseGoal")]
        [Index("UK_CaseSmartGoal", 1, IsUnique = true)]
        public Int32 CaseGoalID { get; set; }

        [ForeignKey("QualityOfLifeCategory")]        
        [Index("UK_CaseSmartGoal", 2, IsUnique = true)]
        [Display(Name = "Q.O.L. Category")]
        public Int32 QualityOfLifeCategoryID { get; set; }

        [Required(ErrorMessage = "Please select progress outcomes")]
        [Display(Name = "Progress Outcomes")]
        [ForeignKey("ServiceLevelOutcome")]
        public Int32 ServiceLevelOutcomeID { get; set; }

        [Required(ErrorMessage = "Please enter start date")]
        [Display(Name = "Target Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter end date")]
        [Display(Name = "Target End Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Is Completed?")]
        public bool IsCompleted { get; set; }

        public virtual CaseGoal CaseGoal { get; set; }
        public virtual QualityOfLifeCategory QualityOfLifeCategory { get; set; }
        public virtual ServiceLevelOutcome ServiceLevelOutcome { get; set; }
        

        [NotMapped]
        [Display(Name = "Family or Family Member")]
        public string CaseMemberName { get; set; }

        [NotMapped]
        [Display(Name = "Family or Family Member")]
        public int CaseMemberID { get; set; }

        [NotMapped]
        [Display(Name = "Goal Statement")]
        public string SmartGoalName { get; set; }

        [NotMapped]
        [Display(Name = "Goal Statement")]
        public string SmartGoalIDs { get; set; }

        [NotMapped]
        [Display(Name = "Internal Service Provider")]
        public string InternalServiceProvider { get; set; }

        [NotMapped]
        [Display(Name = "External Service Provider")]
        public string ExternalServiceProvider { get; set; }

        [NotMapped]
        [Display(Name = "Q.O.L.")]
        public string QualityOfLifeCategoryName { get; set; }

        [NotMapped]
        [Display(Name = "Progress Outcomes")]
        public string ServiceLevelOutcomeName { get; set; }

        [NotMapped]
        [Display(Name = "Total Actions")]
        public int TotalActionCount { get; set; }

        [NotMapped]
        [Display(Name = "Completed Actions")]
        public int OpenActionCount { get; set; }

        [NotMapped]
        [Display(Name = "Completed Actions")]
        public int CloseActionCount { get; set; }

        [NotMapped]
        public int CaseID { get; set; }

        [NotMapped]
        [Display(Name = "Internal Service Provider")]
        public String UsedInternalServiceProviderIDs { get; set; }

        [NotMapped]
        [Display(Name = "Internal Service Provider")]
        public String ProposedInternalServiceProviderIDs { get; set; }

        [NotMapped]
        [Display(Name = "External Service Provider")]
        public String UsedExternalServiceProviderIDs { get; set; }

        [NotMapped]
        [Display(Name = "External Service Provider")]
        public String ProposedExternalServiceProviderIDs { get; set; }

        [NotMapped]
        public bool HasPermissionToCreate { get; set; }
        [NotMapped]
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
        [NotMapped]
        public string HasPermissionToTrackGoal { get; set; }
        [NotMapped]
        public string HasPermissionToRead { get; set; }
    }
}