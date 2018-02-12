using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCMS.DataLogic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class CaseListViewModel
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "PROGRAM")]
        public string ProgramName { get; set; }

        [Display(Name = "SUB-PROGRAM")]
        public string SubProgramName { get; set; }

        [Display(Name = "REGION")]
        public string RegionName { get; set; }

        [Display(Name = "CASE SUMMARY")]
        public string DisplayId { get; set; }

        [Display(Name = "INITIATION")]
        public DateTime EnrollDate { get; set; }

        [Display(Name = "NAME")]
        public string CaseMember { get; set; }

        [Display(Name = "CURRENT STATUS")]
        public string StatusName { get; set; }
                                             
        [Display(Name = "WORKER NAME")]
        public string CaseWorker { get; set; }

        [Display(Name = "JK")]
        public string JamatKhanaName { get; set; }

        [Display(Name = "LAST NOTE")]
        public string CaseNote { get; set; }

        [Display(Name = "ACTIONS")]
        public string Actions { get; set; }

        [Display(Name = "NAME")]
        public string AllCaseMember { get; set; }

        [Display(Name = "NAME")]
        public string AllCaseMemberWithContact { get; set; }

        public int ProgramID { get; set; }
        public int RegionID { get; set; }
        public int SubProgramID { get; set; }
        public int? JamatkhanaID { get; set; }
        public int CaseStatusID { get; set; }
        //CaseStatusID

        public string HasPermissionToCreate { get; set; }
        public string HasPermissionToEdit { get; set; }
        public string HasPermissionToReadmit { get; set; }
        public string HasPermissionToDelete { get; set; }
        public string HasPermissionToRead { get; set; }
        public string HasPermissionToCaseProgressNoteCreate { get; set; }
        public string HasPermissionToCaseProgressNoteIndex { get; set; }
        public string LastNote { get; set; }
    }
}
