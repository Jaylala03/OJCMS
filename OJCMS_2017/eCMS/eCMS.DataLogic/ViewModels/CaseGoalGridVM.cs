
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
    public class CaseGoalGridVM
    {
        public int CaseID { get; set; }
        public int SortOrder { get; set; }
        public int CaseGoalID { get; set; }

        [Display(Name = "Family / Family Member")]
        public string FamilyMember { get; set; }

        [Display(Name = "Goal Status")]
        public string GoalStatus { get; set; }

        [Display(Name = "Goal")]
        public string GoalDetail { get; set; }

        [Display(Name = "Indicators")]
        public string Indicators { get; set; }

        [Display(Name = "Priority set by family")]
        public string Priority { get; set; }

        [Display(Name = "ActionsSummary")]
        public string ActionsSummary { get; set; }

        [Display(Name = "Created Date")]
        public  DateTime CreateDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdateDate { get; set; }

        [Display(Name = "ACTIONS")]
        public string Actions { get; set; }

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