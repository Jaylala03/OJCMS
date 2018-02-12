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
    public class CaseAssessment : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select family or family member")]
        [Display(Name = "Family or Family Member")]
        [ForeignKey("CaseMember")]
        public Int32 CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please select assessment type")]
        [Display(Name = "Assessment Type")]
        [ForeignKey("AssessmentType")]
        public Int32 AssessmentTypeID { get; set; }

        [Required(ErrorMessage = "Please select individual status")]
        [Display(Name = "Individual Status(going forward)")]
        [ForeignKey("MemberStatus")]
        public Int32 MemberStatusID { get; set; }

        [Required(ErrorMessage = "Please select risk type")]
        [Display(Name = "Risk Level")]
        [ForeignKey("RiskType")]
        public Int32 RiskTypeID { get; set; }

        [Required(ErrorMessage = "Please select documented by")]
        [Display(Name = "Documented By")]
        [ForeignKey("DocumentedBy")]
        public Int32 DocumentedByID { get; set; }

        [Required(ErrorMessage = "Please enter start date")]
        [Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Please enter next assessment date")]
        [Display(Name = "Next Assessment Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NextAssessmentDate { get; set; }

        [Display(Name = "Presenting Problem (Reason for seeking support)")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String PresentingProblem { get; set; }

        [Display(Name = "Underlying problem")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String UnderlyingProblem { get; set; }

        [Display(Name = "General Comments and Risk Assessment")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String GeneralComments { get; set; }

        [Display(Name = "Participation Agreement has been signed")]
        public Boolean HasFilledForm { get; set; }

        [Display(Name = "Family has signed off on personal goal setting or family case plan has been developed")]
        public Boolean IsAgreedToConferencing { get; set; }

        [Display(Name = "Discharge Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DischargeDate { get; set; }

        [Display(Name = "Reasons For Discharge")]
        [ForeignKey("ReasonsForDischarge")]
        public Int32? ReasonsForDischargeID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String ReasonsForDischargeOther { get; set; }

        [Display(Name = "Clinical Summary")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String ClinicalSummary { get; set; }

        [Display(Name = "Recommendation")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String Recommendation { get; set; }

        public virtual CaseMember CaseMember { get; set; }
        public virtual AssessmentType AssessmentType { get; set; }
        public virtual MemberStatus MemberStatus { get; set; }
        public virtual RiskType RiskType { get; set; }
        public virtual CaseWorker DocumentedBy { get; set; }
        public virtual ReasonsForDischarge ReasonsForDischarge { get; set; }

        [NotMapped]
        [Display(Name = "Case")]
        public Int32 CaseID { get; set; }

        [NotMapped]
        [Display(Name = "Family or Family Member")]
        public String CaseMemberName { get; set; }

        [NotMapped]
        [Display(Name = "Assessment Type")]
        public String AssessmentTypeName { get; set; }

        [NotMapped]
        [Display(Name = "Individual Status")]
        public String IndividualStatusName { get; set; }

        [NotMapped]
        [Display(Name = "Risk Type")]
        public String RiskTypeName { get; set; }

        [NotMapped]
        [Display(Name = "Documented By")]
        public String DocumentedByName { get; set; }

        [NotMapped]
        [Display(Name = "Reasons For Discharge")]
        public String ReasonsForDischargeName { get; set; }

        [NotMapped]
        public String QualityOfLifeIDs { get; set; }

        [NotMapped]
        [Display(Name = "Quality of Life")]
        public String QualityOfLifeNames { get; set; }

        [NotMapped]
        public bool HasPermissionToCreate { get; set; }
        [NotMapped]
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public bool HasPermissionToReadmit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }

        [NotMapped]
        public string HasPermissionToRead { get; set; }
    }
}