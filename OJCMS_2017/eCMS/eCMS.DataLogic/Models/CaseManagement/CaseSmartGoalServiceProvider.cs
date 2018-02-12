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
    public class CaseSmartGoalServiceProvider : EntityBaseModel
    {
        [Index("UK_CaseSmartGoalServiceProvider", 1, IsUnique = true)]
        [ForeignKey("CaseSmartGoal")]
        public Int32 CaseSmartGoalID { get; set; }

        [Required(ErrorMessage = "Please select service provider")]
        [Display(Name = "Service Provider")]
        [ForeignKey("ServiceProvider")]
        [Index("UK_CaseSmartGoalServiceProvider", 2, IsUnique = true)]
        public Int32 ServiceProviderID { get; set; }


        [Display(Name = "Other")]
        [StringLength(256)]
        public String ServiceProviderOther { get; set; }

        [Display(Name = "Service")]
        [ForeignKey("Service")]
        public Int32? ServiceID { get; set; }

        [Display(Name = "Financial Assistance")]
        [ForeignKey("FinancialAssistanceSubCategory")]
        public Int32? FinancialAssistanceSubCategoryID { get; set; }

        [Display(Name = "Amount")]
        public Double? Amount { get; set; }

        [Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Please select if its's used or proposed")]
        [Display(Name = "Is Used?")]
        public Boolean IsUsed { get; set; }

        [Display(Name = "Name")]
        [ForeignKey("Worker")]
        public Int32? WorkerID { get; set; }

        [Display(Name = "Other")]
        public String WorkerName { get; set; }

        [Display(Name = "Is Active?")]
        public Boolean IsWorkerActive { get; set; }

        [Display(Name = "Notification")]
        public Boolean IsNotificationEnabled { get; set; }

        public virtual CaseSmartGoal CaseSmartGoal { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual Service Service { get; set; }
        public virtual FinancialAssistanceSubCategory FinancialAssistanceSubCategory { get; set; }
        public virtual Worker Worker { get; set; }

        [NotMapped]
        [Display(Name="Service Provider")]
        public string ServiceProviderName { get; set; }
        [NotMapped]
        [Display(Name = "Type")]
        public string ServiceProviderType { get; set; }
        [Display(Name = "Used/Proposed")]
        public string ServiceProviderUsedProposed { get; set; }
        [NotMapped]
        public string ServiceName { get; set; }
        [NotMapped]
        public string FinancialAssistanceSubCategoryName { get; set; }
        [NotMapped]
        public int CaseID { get; set; }
        [NotMapped]
        public int CaseMemberID { get; set; }
        [NotMapped]
        [Display(Name="Provider Type")]
        public int ServiceTypeID { get; set; }
        [NotMapped]
        [Display(Name = "Region")]
        public int RegionID { get; set; }
        [NotMapped]
        [Display(Name = "Financial Assistance Category")]
       
        public Int32? FinancialAssistanceCategoryID { get; set; }
        [NotMapped]
        public Int32 QualityOfLifeCategoryID { get; set; }
        [NotMapped]
        public CaseSmartGoalServiceLevelOutcome CaseSmartGoalServiceLevelOutcome { get; set; }
        [NotMapped]
        public CaseAction CaseAction { get; set; }
        //[NotMapped]
        //public Boolean IsProposed { get; set; }
    }
}