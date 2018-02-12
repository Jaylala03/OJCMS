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
    public class CaseMember : EntityBaseModel
    {
        [Required(ErrorMessage = "Please select case")]
        [Display(Name = "Case")]
        [ForeignKey("Case")]
        public Int32 CaseID { get; set; }

        [Display(Name = "Family Case Number", Prompt = "Family Case Number")]
        [StringLength(32)]
        public String DisplayID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(256)]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(256)]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Please enter enroll date")]
        [Display(Name = "Enroll Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollDate { get; set; }

        [Display(Name = "Relationship to family contact")]
        [ForeignKey("RelationshipStatus")]
        public Int32? RelationshipStatusID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String RelationshipStatusOther { get; set; }

        [Display(Name = "Primary Language")]
        [ForeignKey("Language")]
        public Int32? LanguageID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String LanguageOther { get; set; }

        [Display(Name = "Language for communication")]
        [ForeignKey("CommunicationLanguage")]
        public Int32? CommunicationLanguageID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String CommunicationLanguageOther { get; set; }

        [Display(Name = "Birth Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        [ForeignKey("Gender")]
        public Int32? GenderID { get; set; }

        [Display(Name = "Ethnicity")]
        [ForeignKey("Ethnicity")]
        public Int32? EthnicityID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String EthnicityOther { get; set; }

        [Display(Name = "Marital Status")]
        [ForeignKey("MaritalStatus")]
        public Int32? MaritalStatusID { get; set; }

        [Display(Name = "Status")]
        [ForeignKey("MemberStatus")]
        
        public Int32? MemberStatusID { get; set; }

        [Display(Name = "Is Active?")]
        public Boolean IsActive { get; set; }

        [Display(Name = "Original Caller")]
        public Boolean IsPrimary { get; set; }

        [Display(Name = "Sequence")]
        public Int32? Sequence { get; set; }

        [Display(Name = "Comments")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        [System.Web.Mvc.AllowHtml]
        public String Comments { get; set; }

        [Display(Name = "Ethnicity")]
        public string[] CaseEthinicity { get; set; }

        public virtual Case Case { get; set; }
        public virtual RelationshipStatus RelationshipStatus { get; set; }
        public virtual Language Language { get; set; }
        public virtual Language CommunicationLanguage { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public virtual MaritalStatus MaritalStatus { get; set; }
        public virtual MemberStatus MemberStatus { get; set; }

        [NotMapped]
        [Display(Name = "Gender")]
        public string GenderName { get; set; }
        [NotMapped]
        [Display(Name = "Ethnicity")]
        public string EthnicityName { get; set; }

        [NotMapped]
        [Display(Name = "Relationship")]
        public string RelationshipStatusName { get; set; }

        [NotMapped]
        [Display(Name = "Marital Status")]
        public string MaritalStatusName { get; set; }

        [NotMapped]
        [Display(Name = "Language Name")]
        public string LanguageName { get; set; }

        [NotMapped]
        [Display(Name = "Program")]
        public string ProgramName { get; set; }

        [NotMapped]
        [Display(Name = "Member Status")]
        public string MemberStatusName { get; set; }

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
////------------------------------------------------------------------------------
//// <auto-generated>
////    This code was generated from a template.
////
////    Manual changes to this file may cause unexpected behavior in your application.
////    Manual changes to this file will be overwritten if the code is regenerated.
//// </auto-generated>
////------------------------------------------------------------------------------

//namespace eCMS.DataLogic
//{
//    using System;
//    using System.Collections.Generic;

//    public partial class case_members
//    {
//        public case_members()
//        {
//            this.case_access_level = new HashSet<case_access_level>();
//            this.case_activities = new HashSet<case_activities>();
//            this.case_call_restrictions = new HashSet<case_call_restrictions>();
//            this.case_goals = new HashSet<case_goals>();
//            this.case_issues = new HashSet<case_issues>();
//            this.status_history = new HashSet<status_history>();
//        }

//        public int case_mbr_id { get; set; }
//        public int case_id { get; set; }
//        public string members_initial { get; set; }
//        public Nullable<short> DOB_year { get; set; }
//        public Nullable<int> gender_id { get; set; }
//        public Nullable<int> relationship_id { get; set; }
//        public Nullable<int> status_id { get; set; }
//        public Nullable<int> working { get; set; }
//        public Nullable<int> education_level_id { get; set; }
//        public Nullable<int> occupation_id { get; set; }
//        public Nullable<bool> isleap { get; set; }
//        public Nullable<System.DateTime> enrolledleap { get; set; }
//        public Nullable<System.DateTime> created_date { get; set; }
//        public Nullable<System.DateTime> last_updated_date { get; set; }
//        public Nullable<int> language_id { get; set; }
//        public Nullable<int> education_completion_id { get; set; }
//        public string first_name { get; set; }
//        public string last_name { get; set; }
//        public Nullable<int> calling_for_id { get; set; }
//        public Nullable<int> marital_status_id { get; set; }
//        public string comments { get; set; }
//        public Nullable<int> landing_year { get; set; }
//        public string circle_support { get; set; }
//        public Nullable<int> BASE_MARTIAL_STATUS_ID { get; set; }
//        public Nullable<int> BASE_GENDER_ID { get; set; }
//        public Nullable<int> BASE_RELATIONSHIP_ID { get; set; }
//        public Nullable<int> BASE_LANGUAGE_ID { get; set; }
//        public Nullable<int> BASE_STATUS_ID { get; set; }
//        public Nullable<int> BASE_ETHNICITY_ID { get; set; }
//        public Nullable<bool> IS_ORIGINAL_CALLER { get; set; }
//        public string CASE_NUMBER { get; set; }
//        public Nullable<System.DateTime> DOB { get; set; }
//        public Nullable<bool> IS_ACTIVE { get; set; }
//        public Nullable<System.DateTime> ENROLL_DATE { get; set; }

//        public virtual BASE_ETHNICITY BASE_ETHNICITY { get; set; }
//        public virtual BASE_GENDER BASE_GENDER { get; set; }
//        public virtual BASE_LANGUAGE BASE_LANGUAGE { get; set; }
//        public virtual BASE_MARTIAL_STATUS BASE_MARTIAL_STATUS { get; set; }
//        public virtual BASE_MEMBER_STATUS BASE_MEMBER_STATUS { get; set; }
//        public virtual BASE_RELATIONSHIP_STATUS BASE_RELATIONSHIP_STATUS { get; set; }
//        public virtual ICollection<case_access_level> case_access_level { get; set; }
//        public virtual ICollection<case_activities> case_activities { get; set; }
//        public virtual ICollection<case_call_restrictions> case_call_restrictions { get; set; }
//        public virtual case_family case_family { get; set; }
//        public virtual ICollection<case_goals> case_goals { get; set; }
//        public virtual ICollection<case_issues> case_issues { get; set; }
//        public virtual ICollection<status_history> status_history { get; set; }
//    }
//}
