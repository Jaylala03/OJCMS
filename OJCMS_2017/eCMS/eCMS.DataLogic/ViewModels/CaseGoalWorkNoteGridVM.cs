
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
    public class CaseGoalWorkNoteGridVM
    {
        public int CaseID { get; set; }
        public int CaseGoalID { get; set; }

        
        public Int32? ContactMethodID { get; set; }

        [Display(Name = "Contact Method")]
        public string ContactMethod { get; set; }

        [Display(Name = "Notes")]
        [MaxLength]
        public string Note { get; set; }

        //public virtual ContactMethod ContactMethod { get; set; }

        [Display(Name = "Contact Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NoteDate { get; set; }

        [Display(Name = "Time Spent")]
        public int? TimeSpentHours { get; set; }
        [Display(Name = "Time Spent")]
        public int? TimeSpentMinutes { get; set; }

        [Display(Name = "Time Spent")]
        public string TimeSpent { get; set; }

        [Display(Name = "Goal/Action detail")]
        [MaxLength]
        public String GoalDetail { get; set; }

        [Display(Name = "Date Logged")]
        public DateTime CreateDate { get; set; }

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