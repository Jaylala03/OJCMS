//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace eCMS.DataLogic.Models.Lookup
{
    public class QualityOfLife : LookupBaseModel
    {
        [Display(Name = "Sub-Category")]
        [ForeignKey("QualityOfLifeSubCategory")]
        public Int32 QualityOfLifeSubCategoryID { get; set; }

        public virtual QualityOfLifeSubCategory QualityOfLifeSubCategory { get; set; }

        [NotMapped]
        [Display(Name = "Category")]
        public Int32 QualityOfLifeCategoryID { get; set; }

        [NotMapped]
        [Display(Name = "Sub-Category")]
        public String QualityOfLifeSubCategoryName { get; set; }

        [NotMapped]
        [Display(Name = "Category")]
        public String QualityOfLifeCategoryName { get; set; }
    }
}
