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
    public class CaseMemberProfile : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select family or family member")]
        [Display(Name = "Family or Family Member")]
        [ForeignKey("CaseMember")]
        //[Key]
        public Int32 CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please enter profile date")]
        [Display(Name = "Profile Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ProfileDate { get; set; }

        [Required(ErrorMessage = "Please select profile type")]
        [Display(Name = "Profile Type")]
        [ForeignKey("ProfileType")]
        public Int32 ProfileTypeID { get; set; }

        [Display(Name = "Highest Level Of Education")]
        [ForeignKey("HighestLevelOfEducation")]
        public Int32? HighestLevelOfEducationID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String HighestLevelOfEducationOther { get; set; }

        [Display(Name = "GPA")]
        [ForeignKey("GPA")]
        public Int32? GPAID { get; set; }

        [Display(Name = "Occupation")]
        [StringLength(256)]
        public String Occupation { get; set; }

       
        [Display(Name = "Annual Individual Net Income")]
        [ForeignKey("AnnualIncome")]
        public Int32? AnnualIncomeID { get; set; }

        [Display(Name = "Annual Individual Net Income")]
        
        public string Income { get; set; }


        [Display(Name = "Savings")]
        [ForeignKey("Savings")]
        public Int32? SavingsID { get; set; }

        [Display(Name = "Housing Quality")]
        [ForeignKey("HousingQuality")]
        public Int32? HousingQualityID { get; set; }

        [Display(Name = "Note")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        [System.Web.Mvc.AllowHtml]
        public String HousingQualityNote { get; set; }

        [Display(Name = "Social Assistance")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String SocialAssistance { get; set; }

        [Display(Name = "External Relationships")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String ExternalRelationship { get; set; }

        [Display(Name = "Immigration & Citizenship Status")]
        [ForeignKey("ImmigrationCitizenshipStatus")]
        public Int32? ImmigrationCitizenshipStatusID { get; set; }

        [Display(Name = "Note")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        [System.Web.Mvc.AllowHtml]
        public String ImmigrationCitizenshipStatusNote { get; set; }

        [Display(Name = "How do you feel about your self at the moment?")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String FuturePlan { get; set; }

        [Display(Name = "General Health Condition")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String HealthCondition { get; set; }

        public virtual CaseMember CaseMember { get; set; }
        public virtual ProfileType ProfileType { get; set; }
        public virtual HighestLevelOfEducation HighestLevelOfEducation { get; set; }
        public virtual GPA GPA { get; set; }
        public virtual AnnualIncome AnnualIncome { get; set; }
        public virtual Savings Savings { get; set; }
        public virtual HousingQuality HousingQuality { get; set; }
        public virtual ImmigrationCitizenshipStatus ImmigrationCitizenshipStatus { get; set; }

        [NotMapped]
        [Display(Name = "Case")]
        public Int32 CaseID { get; set; }

        [NotMapped]
        [Display(Name = "Type")]
        public String ProfileTypeName { get; set; }
        [NotMapped]
        [Display(Name = "Education")]
        public String HighestLevelOfEducationName { get; set; }
        [NotMapped]
        [Display(Name = "GPA")]
        public String GPAName { get; set; }
        [NotMapped]
        [Display(Name = "Income/Livelihood")]
        public String AnnualIncomeName { get; set; }
        [NotMapped]
        [Display(Name = "Savings")]
        public String SavingsName { get; set; }
        [NotMapped]
        [Display(Name = "Housing")]
        public String HousingQualityName { get; set; }
        [NotMapped]
        [Display(Name = "Citizenship Status")]
        public String ImmigrationCitizenshipStatusName { get; set; }
        [NotMapped]
        public bool HasPermissionToCreate { get; set; }
        [NotMapped]
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public bool HasPermissionToReadmit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
        [NotMapped]
        public bool HasPermissionToList { get; set; }
        [NotMapped]
        public string HasPermissionToRead { get; set; }
    }
}