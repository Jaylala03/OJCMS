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
    public class CaseSmartGoalAssignment : EntityBaseModel
    {
        [Index("UK_CaseSmartGoalServiceProvider", 1, IsUnique = true)]
        [ForeignKey("CaseSmartGoal")]
        public Int32 CaseSmartGoalID { get; set; }

        [Required(ErrorMessage = "Please select Measurable Goal")]
        [Display(Name = "Measurable Goal")]
        [ForeignKey("SmartGoal")]
        [Index("UK_CaseSmartGoalServiceProvider", 2, IsUnique = true)]
        public Int32 SmartGoalID { get; set; }

        [Display(Name = "Other")]
        [MaxLength]
        public String SmartGoalOther { get; set; }

        [Display(Name = "Target Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Target End Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Comment")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String Comment { get; set; }

        public virtual CaseSmartGoal CaseSmartGoal { get; set; }
        public virtual SmartGoal SmartGoal { get; set; }

        [NotMapped]
        [Display(Name = "Goal Statement")]
        public string SmartGoalName { get; set; }

        [NotMapped]
        public string Checked { get; set; }
    }
}