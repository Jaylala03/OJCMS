
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
   public class CaseHouseholdIncomeVM
    {
        //[Display(Name = "Case")]
        //public Int32 CaseID { get; set; }

        //[Display(Name = "Case Status")]
        //public Int32 CaseStatusID { get; set; }

        [Display(Name = "Number of members in the household: ")]
        public Int32 NoOfMembers { get; set; }

        [Display(Name = "Number of children in the household: ")]
        public Int32 NoOfChild { get; set; }

        [Display(Name = "Number of seniors (+65) in the household: ")]
        public Int32 NoOfSeniors { get; set; }

        [Display(Name = "Number of physically disabled members in the household: ")]
        public Int32 NoOfPhysicallyDisabled { get; set; }

        [Display(Name = "Income Range")]
        public Int32 IncomeRangeID { get; set; }


        //public List<IncomeRange> IncomeRanges { set; get; }        

        public string IncomeRanges { set; get; }

        [Display(Name = "Is Initial Income")]
        public bool IsInitialIncome { get; set; }

        [Display(Name = "Date Entered")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Is LICO")]
        public bool IsLICO { get; set; }
    }
}