
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCMS.DataLogic.ViewModels
{
   public class CaseWorkerNoteVM
    {
        public int ID { get; set; }

        [Display(Name = "Date Logged")]
        public DateTime? DateLogged { get; set; }

        [Display(Name = "Family / Family Member")]
        public string FamilyMember { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Contact Method")]
        public string ContactMethod { get; set; }

        [Display(Name = "Contact Date")]
        public DateTime? ContactDate { get; set; }

        [Display(Name = "Time Spent")]
        public string TimeSpent { get; set; }

        [Display(Name = "Case Status As Date")]
        public string CaseStatusAsDate { get; set; }

        [Display(Name = "Logged By")]
        public string LoggedBy { get; set; }

        [Display(Name = "Page where Work Note was logged")]
        public string WorkNoteWasLogged { get; set; }

        [Display(Name = "ACTIONS")]
        public string Actions { get; set; }
    }
}