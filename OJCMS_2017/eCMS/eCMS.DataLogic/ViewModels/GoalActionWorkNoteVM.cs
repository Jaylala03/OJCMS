//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class GoalActionWorkNoteVM 
    {

        //[Required(ErrorMessage = "Please enter Contact Date")]
        [Display(Name = "Contact Date")]
        //[DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NoteDate { get; set; }

        //[Required(ErrorMessage = "Please enter Contact Date")]
        [Display(Name = "Date Logged")]
        //[DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateLogged { get; set; }

        [Display(Name = "Time Spent")]
        public int? TimeSpentHours { get; set; }
        [Display(Name = "Time Spent")]
        public int? TimeSpentMinutes { get; set; }

        [Display(Name = "Time Spent")]
        public string TimeSpent { get; set; }

        //[Required(ErrorMessage = "Please select contact method")]
        [Display(Name = "Contact Method")]
        public Int32? ContactMethodID { get; set; }

        //[Required(ErrorMessage = "Please enter description")]
        [Display(Name = "Notes:")]
        public String Note { get; set; }

        public String ContactMethod  { get; set; }

        //[NotMapped]
        [Display(Name = "Goal")]
        public bool IsGoal { set; get; }

        //[NotMapped]
        [Display(Name = "Action")]
        public bool IsAction { set; get; }

        //[NotMapped]
        public Int32? CaseGoalID { get; set; }
        public Int32? CaseActionID { get; set; }

        //[NotMapped]
        public Int32 StatusID { get; set; }

        [Display(Name = "Family Case")]
        public string CaseDisplayID { set; get; }

        [Display(Name = "Goal/Action detail")]
        public string Detail { get; set; }

        [Display(Name = "Goal/Action Status as at Date")]
        public string Status { set; get; }

        [Display(Name = "Logged By")]
        public string LoggedBy { set; get; }

        [Display(Name = "AssignedTo")]
        public string AssignedTo { set; get; }

        public string HasPermissionToEdit { get; set; }
        [NotMapped]
        public string HasPermissionToDelete { get; set; }
        [NotMapped]
        public string HasPermissionToRead { get; set; }
    }
}
