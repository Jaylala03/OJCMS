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
    public class CaseWorkerNote : EntityBaseModel
    {
        public Int32 WorkerNoteActivityTypeID { get; set; }

        //[Required(ErrorMessage = "Please enter Contact Date")]
        [Display(Name = "Contact Date")]
        //[DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime NoteDate { get; set; }

        [Display(Name = "Time Spent")]
        public int TimeSpentHours { get; set; }
        [Display(Name = "Time Spent")]
        public int TimeSpentMinutes { get; set; }

        //[Display(Name = "Time Spent")]
        //public string TimeSpent { get; set; }

        [Required(ErrorMessage = "Please select contact method")]
        [Display(Name = "Contact Method")]
        [ForeignKey("ContactMethod")]
        public Int32 ContactMethodID { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        [Display(Name = "Description")]
        [MaxLength]
        public String Note { get; set; }

        public virtual ContactMethod ContactMethod { get; set; }

        [NotMapped]
        public Int32 CaseID { get; set; }

        [NotMapped]
        public Int32 CaseStatusID { get; set; }

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
        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
        [NotMapped]
        public string HasPermissionToRead { get; set; }
    }
}
