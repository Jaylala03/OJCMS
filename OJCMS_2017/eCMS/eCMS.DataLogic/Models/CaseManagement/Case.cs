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
    public class Case : EntityBaseModel
    {
        [Display(Name = "Family Case Number", Prompt = "Family Case Number")]
        [StringLength(32)]
        public String DisplayID { get; set; }


        [Display(Name = "Reference Case", Prompt = "Reference Case")]
        [StringLength(32)]
        public String CaseNumber { get; set; }

        [Required(ErrorMessage = "Please enter case start date")]
        //[Display(Name = "Case Initiation Date")]
        [Display(Name = "Case Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollDate { get; set; }

        [Display(Name = "Referral Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReferralDate { get; set; }

        //[NotMapped]
        //[Display(Name = "Contact Date")]
        ////[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime? ContactDate { get; set; }

        

        [Required(ErrorMessage = "Please select program")]
        [Display(Name = "Program")]
        [ForeignKey("Program")]
        public Int32 ProgramID { get; set; }

        [Required(ErrorMessage = "Please select primary coordinator")]
        [Display(Name = "Primary Coordinator")]
        [ForeignKey("SubProgram")]
        public Int32 SubProgramID { get; set; }

        [Required(ErrorMessage = "Please select region")]
        [Display(Name = "Region")]
        [ForeignKey("Region")]
        public Int32 RegionID { get; set; }

        //[Required(ErrorMessage = "Please select jamatkhana")]
        [Display(Name = "Jamatkhana")]
        [ForeignKey("Jamatkhana")]
        public Int32? JamatkhanaID { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select jamatkhana")]
        [Display(Name = "Jamatkhana")]
        public string JamatkhanaName { get; set; }

        [Display(Name = "Address",Prompt="Address")]
        [StringLength(256)]
        public String Address { get; set; }

        [Display(Name = "City",Prompt="City")]
        [StringLength(64)]
        public String City { get; set; }

        [Display(Name = "Postal Code", Prompt = "Postal Code")]
        [StringLength(16)]
        public String PostalCode { get; set; }

        [Required(ErrorMessage = "Please select intake method")]
        [Display(Name = "Intake Method")]
        [ForeignKey("IntakeMethod")]
        public Int32 IntakeMethodID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String IntakeMethodOther { get; set; }

        [Required(ErrorMessage = "Please select referral source")]
        [Display(Name = "Referral Source")]
        [ForeignKey("ReferralSource")]
        public Int32 ReferralSourceID { get; set; }

        //[Required(ErrorMessage = "Please enter referral source comments")]
        [Display(Name = "Referral Source Comments")]
        [MaxLength]
        public String ReferralSourceComments { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String ReferralSourceOther { get; set; }

        [Display(Name = "Hearing Source")]
        [ForeignKey("HearingSource")]
        public Int32? HearingSourceID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String HearingSourceOther { get; set; }

        [Required(ErrorMessage = "Please select case status")]
        [Display(Name = "Case Status")]
        [ForeignKey("CaseStatus")]
        public Int32 CaseStatusID { get; set; }

        [Display(Name = "Reference Case")]
        [ForeignKey("ReferenceCase")]
        public Int32? ReferenceCaseID { get; set; }

        [Display(Name = "Comments")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Comments { get; set; }

        [Required(ErrorMessage = "Please select risk type")]
        [Display(Name = "Risk Level")]
        //[ForeignKey("RiskType")]
        public Int32 RiskTypeID { get; set; }

        //[Required(ErrorMessage = "Please enter presenting problem")]
        [Display(Name = "Presenting Problem (Reason for seeking support)")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String PresentingProblem { get; set; }

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

        //public virtual RiskType RiskType { get; set; }
        public virtual Program Program { get; set; }
        public virtual SubProgram SubProgram { get; set; }
        public virtual Region Region { get; set; }
        public virtual Jamatkhana Jamatkhana { get; set; }
        public virtual IntakeMethod IntakeMethod { get; set; }
        public virtual ReferralSource ReferralSource { get; set; }
        public virtual HearingSource HearingSource { get; set; }
        public virtual CaseStatus CaseStatus { get; set; }
        public virtual Case ReferenceCase { get; set; }

        [NotMapped]
        public CaseHouseholdIncome CaseHouseholdIncome { get; set; }

        [NotMapped]
        public CaseWorkerNote CaseWorkerNote { get; set; }


        [NotMapped]
        [Display(Name="Family or Family Member First Name",Prompt="First Name")]
        public String FirstName { get; set; }
        
        [NotMapped]
        [Display(Name = "Family or Family Member Last Name", Prompt = "Last Name")]
        public String LastName { get; set; }
        
        [NotMapped]
        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        public String PhoneNumber { get; set; }

        [NotMapped]
        [Display(Name = "Worker Name", Prompt = "Worker Name")]
        public override string CreatedByWorkerName
        {
            get
            {
                return base.CreatedByWorkerName;
            }
            set
            {
                base.CreatedByWorkerName = value;
            }
        }

        [NotMapped]
        public Int32 AssignedToWorkerID { get; set; }

        [NotMapped]
        public bool IsReadmit { get; set; }

        [NotMapped]
        public bool HasPermissionToCreate { get; set; }
        [NotMapped]
        public bool HasPermissionToEdit { get; set; }
        [NotMapped]
        public bool HasPermissionToReadmit { get; set; }
        [NotMapped]
        public bool HasPermissionToDelete { get; set; }
        [NotMapped]
        public bool HasPermissionToRead { get; set; }
        
    }
}
//namespace eCMS.DataLogic
//{
//    using System;
//    using System.Collections.Generic;
    
//    public partial class case_family
//    {
//        public case_family()
//        {
//            this.case_access_level = new HashSet<case_access_level>();
//            this.case_activities = new HashSet<case_activities>();
//            this.case_call_restrictions = new HashSet<case_call_restrictions>();
//            this.case_goals = new HashSet<case_goals>();
//            this.case_issues = new HashSet<case_issues>();
//            this.case_members = new HashSet<case_members>();
//            this.CASE_SUPPORT_CIRCLE = new HashSet<CASE_SUPPORT_CIRCLE>();
//            this.status_history = new HashSet<status_history>();
//        }
    
//        public string family_initial { get; set; }
//        
//        public Nullable<short> No_Of_Members { get; set; }
//        public Nullable<short> No_Of_Members_Active { get; set; }
//        public Nullable<int> Current_Status_ID { get; set; }
//        public string City { get; set; }
//        public Nullable<int> Culture_Type_Id { get; set; }
//        public Nullable<int> Primary_worker_id { get; set; }
//        public Nullable<int> Ref_Source_Id { get; set; }
//        public Nullable<int> Client_Type_Id { get; set; }
//        public Nullable<int> Family_Signal_Id { get; set; }
//        public Nullable<int> Est_Graduation_Year { get; set; }
//        public Nullable<System.DateTime> Created_Date { get; set; }
//        public Nullable<System.DateTime> Last_Updated_Date { get; set; }
//        public Nullable<System.DateTime> Discharge_date { get; set; }
//        public Nullable<int> Client_Id { get; set; }
//        public Nullable<int> Consent_Id { get; set; }
//        public string Address { get; set; }
//        public string Ref_Number { get; set; }
//        
//        public Nullable<int> Case_Closure_Id { get; set; }
//        public Nullable<short> READMIT { get; set; }
//        public string Comments { get; set; }
//        public string Case_Closure_Comments { get; set; }
//        public string zipcode { get; set; }
//        public Nullable<int> BASE_INTAKE_ID { get; set; }
//        public Nullable<int> BASE_HEARING_ID { get; set; }
//        public Nullable<int> BASE_JK_ID { get; set; }
//        public Nullable<int> BASE_REGION_ID { get; set; }
//        public Nullable<int> BASE_REF_SOURCE_ID { get; set; }
//        public Nullable<int> intake_source_id { get; set; }
//        public Nullable<int> hearing_source_id { get; set; }
    
//        public virtual base_category base_category { get; set; }
//        public virtual base_category base_category1 { get; set; }
//        public virtual base_category base_category2 { get; set; }
//        public virtual base_category base_category3 { get; set; }
//        public virtual base_category base_category4 { get; set; }
//        public virtual base_program base_program { get; set; }
//        public virtual ICollection<case_access_level> case_access_level { get; set; }
//        public virtual ICollection<case_activities> case_activities { get; set; }
//        public virtual ICollection<case_call_restrictions> case_call_restrictions { get; set; }
//        public virtual case_worker case_worker { get; set; }
//        public virtual ICollection<case_goals> case_goals { get; set; }
//        public virtual ICollection<case_issues> case_issues { get; set; }
//        public virtual ICollection<case_members> case_members { get; set; }
//        public virtual ICollection<CASE_SUPPORT_CIRCLE> CASE_SUPPORT_CIRCLE { get; set; }
//        public virtual ICollection<status_history> status_history { get; set; }
//    }
//}
