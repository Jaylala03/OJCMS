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
    public class CaseProgressNote : EntityBaseModel
    {
        //[Required(ErrorMessage = "Please select family or family member")]
        [Display(Name = "Family or Family Member")]
       // [ForeignKey("CaseMember")]
        public Int32 CaseMemberID { get; set; }

        [Required(ErrorMessage = "Please select activity")]
        [Display(Name = "Activity")]
        [ForeignKey("ActivityType")]
        public Int32 ActivityTypeID { get; set; }

        [Required(ErrorMessage = "Please enter date & time")]
        [Display(Name = "Date & Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NoteDate { get; set; }

        [Required(ErrorMessage = "Please select time spent")]
        [Display(Name = "Time Spent")]
        [ForeignKey("TimeSpent")]
        public Int32 TimeSpentID { get; set; }

        [Required(ErrorMessage = "Please select contact method")]
        [Display(Name = "Contact Method")]
        [ForeignKey("ContactMethod")]
        public Int32 ContactMethodID { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        [Display(Name = "Description")]
        [MaxLength]
        public String Note { get; set; }

        [Display(Name = "Is Initial Contact")]
        public bool IsInitialContact { get; set; }

        public virtual ActivityType ActivityType { get; set; }
        //public virtual CaseMember CaseMember { get; set; }
        public virtual TimeSpent TimeSpent { get; set; }
        public virtual ContactMethod ContactMethod { get; set; }

        [NotMapped]
        public Int32 CaseID { get; set; }

        [NotMapped]
        [Display(Name = "Family or Family Member Name")]
        public String CaseMemberName { get; set; }

        [NotMapped]
        [Display(Name = "Activity Type")]
        public String ActivityTypeName { get; set; }

        [NotMapped]
        [Display(Name = "Family Case")]
        public string CaseDisplayID { set; get; }

        [NotMapped]
        [Display(Name = "Program")]
        public string CaseProgramName { set; get; }

        [NotMapped]
        [Display(Name = "Family Members")]
        //[Required(ErrorMessage="Select atleast one family member")]
        public string[] CaseMembersIds { set; get; }

        [NotMapped]
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
        [NotMapped]
        public string HasPermissionToRead { get; set; }
    }
}
