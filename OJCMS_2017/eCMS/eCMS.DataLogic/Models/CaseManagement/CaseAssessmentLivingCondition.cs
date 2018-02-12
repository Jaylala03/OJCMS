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
    public class CaseAssessmentLivingCondition : EntityBaseModel
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int ID
        {
            get;
            set;
        }

        [ForeignKey("CaseAssessment")]
        public Int32 CaseAssessmentID { get; set; }

        [ForeignKey("QualityOfLife")]
        public Int32 QualityOfLifeID { get; set; }

        [Display(Name = "Note")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String Note { get; set; }

        public virtual CaseAssessment CaseAssessment { get; set; }
        public virtual QualityOfLife QualityOfLife { get; set; }

        [NotMapped]
        public int QualityOfLifeCategoryID { get; set; }

        [NotMapped]
        [Display(Name = "Quality of Life")]
        public string QualityOfLifeName { get; set; }

        [NotMapped]
        [Display(Name = "Q.O.L. Sub Category")]
        public string QualityOfLifeSubCategoryName { get; set; }

        [NotMapped]
        [Display(Name="Assessment Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AssessmentStartDate { get; set; }
    }
}