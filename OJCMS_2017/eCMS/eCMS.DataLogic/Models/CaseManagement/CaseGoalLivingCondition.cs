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
    public class CaseGoalLivingCondition : EntityBaseModel
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int ID
        {
            get;
            set;
        }

        [ForeignKey("CaseGoal")]
        [Key, Column(Order = 0)]
        public Int32 CaseGoalID { get; set; }

        [ForeignKey("QualityOfLifeCategory")]
        [Key, Column(Order = 1)]
        public Int32 QualityOfLifeCategoryID { get; set; }

        [Required(ErrorMessage = "Please enter note for the selected quality of life category")]
        [Display(Name = "Note")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String Note { get; set; }

        public virtual CaseGoal CaseGoal { get; set; }
        public virtual QualityOfLifeCategory QualityOfLifeCategory { get; set; }

        [NotMapped]
        public string QualityOfLifeCategoryName { get; set; }

    }
}