
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
    public class CaseGoalActionGridVM
    {
        public int ID { get; set; }
        public int CaseGoalID { get; set; }
        public int CaseID { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        [Display(Name = "Assignee Role")]
        public string AssigneeRole { get; set; }

        [Display(Name = "Status")]
        public string ActionStatus { get; set; }

        [Display(Name = "Description")]
        public string ActionDetail { get; set; }

        [Display(Name = "Created Date")]
        public  DateTime CreateDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdateDate { get; set; }

        [Display(Name = "ACTIONS")]
        public string Actions { get; set; }

    }
}